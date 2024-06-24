using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;
using TMPro;
using System;

[System.Serializable]
public class Item
{
    public Item(string _Type, string _Name, string _Explain, string _Number, bool _isUsing, string _Index)
    {
        Type = _Type;
        Name = _Name;
        Explain = _Explain;
        Number = _Number;
        isUsing = _isUsing;
        Index = _Index;
    }

    public string Type, Name, Explain, Number, Index;
    public bool isUsing;
}

public class ItemDataManager : MonoBehaviour
{
    private static ItemDataManager instance;

    public TextAsset ItemDatabase;
    public List<Item> AllItemList, MyItemList, curItemList;
    public string curType = "Equipment";
    public GameObject[] slots, UsingImage;
    public Image[] TabImage, ItemImage;
    public Sprite TabIdleSprite, TabSelectSprite;
    public Sprite[] ItemSprite;
    public GameObject ExplainPanel;
    public RectTransform CanvasRect;
    public TMP_InputField ItemNameInput, ItemNumberInput;


    // Trade System 
    public TMP_InputField TradeItemCount;

    IEnumerator PointerCoroutine;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    void Start()
    {
        // ��ü ������ ����Ʈ �ҷ�����
        string[] line = ItemDatabase.text.Substring(0, ItemDatabase.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            AllItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4] == "TRUE", row[5]));
        }

        Load();
    }

    public void GetItem(string itemName)
    {
        Item curItem = MyItemList.Find(x => x.Name == itemName);

        if (curItem != null)
        {
            // �����ϸ� ���� ����
            curItem.Number = (int.Parse(curItem.Number) + 1).ToString();

            // �����ϸ� ��� ����
        }
        else
        {
            // ��ü���� ���� �������� ã�� �� �����ۿ� �߰�
            Item curAllItem = AllItemList.Find(x => x.Name == itemName);
            if (curAllItem != null)
            {
                curAllItem.Number = int.Parse("1").ToString();
                MyItemList.Add(curAllItem);
            }
        }
    }


    // ������ ȹ��
    public void GetItemClick()
    {
        Item curItem = MyItemList.Find(x => x.Name == ItemNameInput.text);

        if (curItem != null)
        {
            curItem.Number = (int.Parse(curItem.Number) + int.Parse(ItemNumberInput.text)).ToString();
        }
        else
        {
            // ��ü���� ���� �������� ã�� �� �����ۿ� �߰�
            Item curAllItem = AllItemList.Find(x => x.Name == ItemNameInput.text);
            if (curAllItem != null)
            {
                curAllItem.Number = ItemNumberInput.text;
                MyItemList.Add(curAllItem);
            }
        }

        MyItemList.Sort((p1, p2) =>
        {
            try
            {
                int index1 = int.Parse(p1.Index);
                int index2 = int.Parse(p2.Index);
                return index1.CompareTo(index2);
            }
            catch (FormatException)
            {
                // ��ȯ�� �� ���� ��� ���ڿ� ��ü�� ���մϴ�.
                return p1.Index.CompareTo(p2.Index);
            }
        });

        Save();
    }


    // ������ ����
    public void RemoveItemClick()
    {
        Item curItem = MyItemList.Find(x => x.Name == ItemNameInput.text);

        if (curItem != null)
        {
            int curNumber = int.Parse(curItem.Number) - int.Parse(ItemNumberInput.text);

            if (curNumber <= 0) MyItemList.Remove(curItem);
            else curItem.Number = curNumber.ToString();
        }

        MyItemList.Sort((p1, p2) =>
        {
            try
            {
                int index1 = int.Parse(p1.Index);
                int index2 = int.Parse(p2.Index);
                return index1.CompareTo(index2);
            }
            catch (FormatException)
            {
                // ��ȯ�� �� ���� ��� ���ڿ� ��ü�� ���մϴ�.
                return p1.Index.CompareTo(p2.Index);
            }
        });
        Save();
    }

    public void ResetItemClick()
    {
        Item BasicItem = AllItemList.Find(x => x.Name == "sword_A");
        MyItemList = new List<Item>() { BasicItem };
        Save();
    }

    
    public void SlotClick(int slotNum)
    {
        Item curItem = curItemList[slotNum];
        Item UsingItem = curItemList.Find(x => x.isUsing == true);

        if (curType == "Equipment")
        {
            if (UsingItem != null) UsingItem.isUsing = false;
            curItem.isUsing = true;
        }
        else
        {
            curItem.isUsing = !curItem.isUsing;
            if (UsingItem != null) UsingItem.isUsing = false;
        }
        Save();
    }

    public void TabClick(string tabName)
    {
        // ���� ������ ����Ʈ�� Ŭ���� Ÿ�Ը� �߰�
        curType = tabName;
        curItemList = MyItemList.FindAll(x => x.Type == tabName);


        for (int i = 0; i < slots.Length; i++)
        {        // ���԰� �ؽ�Ʈ ���̱�
            bool isExist = i < curItemList.Count;
            slots[i].SetActive(isExist);
            slots[i].GetComponentInChildren<TextMeshProUGUI>().text = isExist ? curItemList[i].Name + "/" + curItemList[i].isUsing : "";

            if (isExist)
            {
                ItemImage[i].sprite = ItemSprite[AllItemList.FindIndex(x => x.Name == curItemList[i].Name)];
            }
        }

        // �� �̹���
        int tabNum = 0;
        switch (tabName)
        {
            case "Equipment": tabNum = 0; break;
            case "Consume": tabNum = 1; break;
        }
        for (int i = 0; i < TabImage.Length; i++)
        {
            TabImage[i].sprite = i == tabNum ? TabSelectSprite : TabIdleSprite;
        }
    }

    public void PointerEnter(int slotNum)
    {
        // ���Կ� ���콺�� �ø��� 0.5���Ŀ� ����â ���
        PointerCoroutine = PointerEnterDelay(slotNum);
        StartCoroutine(PointerCoroutine);

        // ����â�� �̸�, �̹���, ����, ���� ��Ÿ����
        ExplainPanel.GetComponentInChildren<TextMeshProUGUI>().text = curItemList[slotNum].Name;
        ExplainPanel.transform.GetChild(2).GetComponentInChildren<Image>().sprite = slots[slotNum].transform.GetComponent<Image>().sprite;
        ExplainPanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = curItemList[slotNum].Number + "��";
        ExplainPanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = curItemList[slotNum].Explain;
        // �г��� ��ġ�� ������ ���콺 ��ġ�� ����
        //ExplainPanel.GetComponent<RectTransform>().anchoredPosition3D = Input.mousePosition;

    }
    IEnumerator PointerEnterDelay(int slotNum)
    {
        yield return new WaitForSeconds(0.5f);
        ExplainPanel.SetActive(true);

    }

    public void PointerExit(int slotNum)
    {
        StopCoroutine(PointerCoroutine);
        ExplainPanel.SetActive(false);
    }


    /// <summary>
    /// �ŷ��� ������ ����
    /// </summary>
    public void TradeItemRemove()
    {
        Item curItem = MyItemList.Find(x => x.Name == ItemNameInput.text);

        if (curItem != null)
        {
            int curNumber = int.Parse(curItem.Number) - int.Parse(ItemNumberInput.text);

            if (curNumber <= 0) MyItemList.Remove(curItem);
            else curItem.Number = curNumber.ToString();
        }

        MyItemList.Sort((p1, p2) =>
        {
            try
            {
                int index1 = int.Parse(p1.Index);
                int index2 = int.Parse(p2.Index);
                return index1.CompareTo(index2);
            }
            catch (FormatException)
            {
                // ��ȯ�� �� ���� ��� ���ڿ� ��ü�� ���մϴ�.
                return p1.Index.CompareTo(p2.Index);
            }
        });
        Save();
    }







    /// <summary>
    ///  ������ �ҷ����� & �����ϱ�
    /// </summary>
    void Save()
    {
        string jdata = JsonConvert.SerializeObject(MyItemList);
        File.WriteAllText(Application.dataPath + "/Resources/MyItemText.txt", jdata);

        TabClick(curType);
    }

    void Load()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/Resources/MyItemText.txt");
        MyItemList = JsonConvert.DeserializeObject<List<Item>>(jdata);

        TabClick(curType);
    }

}
