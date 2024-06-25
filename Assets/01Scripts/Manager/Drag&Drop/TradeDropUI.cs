using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TradeDropUI : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    private Image image;
    private RectTransform rect;

    public GameObject CountPanel;

    private void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    public void OnEnable()
    {
        CountPanel = GameObject.Find("Canvas").transform.Find("TradePanel").transform.Find("ItemCountPanel").gameObject;
    }


    /// <summary>
    /// ���콺 �����Ͱ� ���� ������ ���� ���� ���η� �� �� 1ȸ ȣ��
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Enter");
        image.color = Color.yellow;
    }


    /// <summary>
    /// ���콺 �����Ͱ� ���� ������ ���� ������ ���������� 1ȸȣ��
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer Exit");
        image.color = Color.white;
    }


    /// <summary>
    /// ���� ������ ���� ���� ���ο��� ����� ���� �� 1ȸ ȣ��
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
        // pointerDrag�� ���� �巡���ϰ��ִ� ���(=������)
        if (eventData.pointerDrag != null)
        {
            // �巡�� ����� ���� �����͸� ������
            Slot originalSlot = eventData.pointerDrag.GetComponent<Slot>();
            if (originalSlot == null) return;

            // �巡�� ����� ���� �θ� ����
            Transform originalParent = eventData.pointerDrag.transform.parent;

            // �巡�� ����� �����Ͽ� ���� ������Ʈ�� �����ϰ�, ��ġ�� ���� ������Ʈ�� �����ϰ� ����
            GameObject draggedObject = Instantiate(eventData.pointerDrag, transform);
            draggedObject.transform.localPosition = Vector3.zero;

            // �巡�� ����� �θ� ���� ������Ʈ�� ����
            eventData.pointerDrag.transform.SetParent(transform);

            // ���� �θ�� ������ �巡�� ����� �ٽ� ����
            eventData.pointerDrag.transform.SetParent(originalParent);

            // ���ο� ��ġ�� �°� ��ӵ� ������Ʈ�� ������ ����
            RectTransform draggedRectTransform = draggedObject.GetComponent<RectTransform>();
            RectTransform targetRectTransform = GetComponent<RectTransform>();
            if (draggedRectTransform != null && targetRectTransform != null)
            {
                draggedRectTransform.sizeDelta = targetRectTransform.sizeDelta;
            }

            CanvasGroup canvasGroup = draggedObject.GetComponent<CanvasGroup>();
            TextMeshProUGUI tmp = draggedObject.GetComponentInChildren<TextMeshProUGUI>();
            Slot slot = draggedObject.GetComponent<Slot>();

            // ������ ���� ���� �г� Ȱ��ȭ �� �ʱ�ȭ
            CountPanel.SetActive(true);
            TradeItemCountManager ticm = CountPanel.GetComponent<TradeItemCountManager>();
            ticm.itemMaxCount = originalSlot.itemCount;
            ticm.itemName = originalSlot.itemName;


            slot.enabled = false;
            tmp.text = "";
            canvasGroup.alpha = 1.0f;

            // ItemImage ������Ʈ���� Image ������Ʈ ��������
            Image slotImage = draggedObject.GetComponent<Image>();

            if (slotImage != null)
            {
                // �̹����� byte[]�� ��ȯ�Ͽ� ����
                Texture2D texture = ToTexture2D(slotImage.sprite);
                if (texture != null)
                {
                    byte[] textureBytes = texture.EncodeToPNG();

                    // ���� Ŭ���̾�Ʈ���� ���� ������Ʈ�� ��û
                    TradePanelController.Instance.UpdateSlot(originalSlot.itemName, originalSlot.itemCount, draggedObject.transform.localPosition, draggedRectTransform.sizeDelta, textureBytes);
                }
            }
            else
            {
                Debug.LogError("ItemImage does not have an Image component.");
            }
        }
    }

    // Sprite�� Texture2D�� ��ȯ�ϴ� �Լ�
    private Texture2D ToTexture2D(Sprite sprite)
    {
        if (sprite == null) return null;

        Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.RGBA32, false);
        texture.SetPixels(sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                  (int)sprite.textureRect.y,
                                                  (int)sprite.textureRect.width,
                                                  (int)sprite.textureRect.height));
        texture.Apply();

        return texture;
    }
}