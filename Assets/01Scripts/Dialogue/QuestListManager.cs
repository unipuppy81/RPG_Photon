using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestListManager : MonoBehaviour
{
    public GameObject questUIObj;
    [SerializeField] Transform panelTransform;



    [SerializeField] QuestManager qManager;


    [Header("Town")]
    [SerializeField] GameObject townList;
    public GameObject[] townChildObjects;

    [Header("Dungeon")]
    [SerializeField] GameObject dungeonList;
    public GameObject[] dungeonChildObjects;

    [Header("Shop")]
    [SerializeField] GameObject shopList;
    public GameObject[] shopChildObjects;




    void Start()
    {
        qManager = GetComponent<QuestManager>();


        // �θ� GameObject�� ���� GameObject�� �迭�� ����
        GetChildObjects(townList, out townChildObjects);
        GetChildObjects(dungeonList, out dungeonChildObjects);
        GetChildObjects(shopList, out shopChildObjects);

        btnSet();
    }



    void GetChildObjects(GameObject parentObject, out GameObject[] childObjects)
    {

        // �θ� GameObject�� ��� ���� GameObject�� �迭�� �޾ƿ�
        int childCount = parentObject.transform.childCount;
        Debug.Log(parentObject.name + childCount);
        childObjects = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            childObjects[i] = parentObject.transform.GetChild(i).gameObject;
            Debug.Log(childObjects[i].name);
        }

        // �迭�� ����� ���� GameObject�鿡 ���� ���ϴ� �۾� ����
        foreach (GameObject childObject in childObjects)
        {
            // ���ϴ� �۾� ����
            // ��: childObject.GetComponent<YourComponent>().YourMethod();
        }
    }

    void btnSet()
    {
        townList.SetActive(true);
        dungeonList.SetActive(false);
        shopList.SetActive(false);
    }

    public void townButtonClick()
    {
        //GameObject questInstance = Instantiate(questUIObj, panelTransform);
        townList.SetActive(true);
        dungeonList.SetActive(false);
        shopList.SetActive(false);
    }

    public void dungeonButtonClick()
    {
        //GameObject questInstance = Instantiate(questUIObj, panelTransform);
        townList.SetActive(false);
        dungeonList.SetActive(true);
        shopList.SetActive(false);
    }

    public void shopButtonClick()
    {
        //GameObject questInstance = Instantiate(questUIObj, panelTransform);
        townList.SetActive(false);
        dungeonList.SetActive(false);
        shopList.SetActive(true);
    }
}
