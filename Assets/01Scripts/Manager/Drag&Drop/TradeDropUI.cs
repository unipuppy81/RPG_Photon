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
    /// 마우스 포인터가 현재 아이템 슬롯 영역 내부로 들어갈 때 1회 호출
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Enter");
        image.color = Color.yellow;
    }


    /// <summary>
    /// 마우스 포인터가 현재 아이템 슬롯 영역을 빠져나갈때 1회호출
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer Exit");
        image.color = Color.white;
    }


    /// <summary>
    /// 현재 아이템 슬롯 영역 내부에서 드롭을 했을 때 1회 호출
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
        // pointerDrag는 현재 드래그하고있는 대상(=아이템)
        if (eventData.pointerDrag != null)
        {
            // 드래그 대상의 슬롯 데이터를 가져옴
            Slot originalSlot = eventData.pointerDrag.GetComponent<Slot>();
            if (originalSlot == null) return;

            // 드래그 대상의 원래 부모 저장
            Transform originalParent = eventData.pointerDrag.transform.parent;

            // 드래그 대상을 복제하여 현재 오브젝트로 설정하고, 위치를 현재 오브젝트와 동일하게 설정
            GameObject draggedObject = Instantiate(eventData.pointerDrag, transform);
            draggedObject.transform.localPosition = Vector3.zero;

            // 드래그 대상의 부모를 현재 오브젝트로 설정
            eventData.pointerDrag.transform.SetParent(transform);

            // 원래 부모로 복사한 드래그 대상을 다시 설정
            eventData.pointerDrag.transform.SetParent(originalParent);

            // 새로운 위치에 맞게 드롭된 오브젝트의 사이즈 조절
            RectTransform draggedRectTransform = draggedObject.GetComponent<RectTransform>();
            RectTransform targetRectTransform = GetComponent<RectTransform>();
            if (draggedRectTransform != null && targetRectTransform != null)
            {
                draggedRectTransform.sizeDelta = targetRectTransform.sizeDelta;
            }

            CanvasGroup canvasGroup = draggedObject.GetComponent<CanvasGroup>();
            TextMeshProUGUI tmp = draggedObject.GetComponentInChildren<TextMeshProUGUI>();
            Slot slot = draggedObject.GetComponent<Slot>();

            // 아이템 수량 선택 패널 활성화 및 초기화
            CountPanel.SetActive(true);
            TradeItemCountManager ticm = CountPanel.GetComponent<TradeItemCountManager>();
            ticm.itemMaxCount = originalSlot.itemCount;
            ticm.itemName = originalSlot.itemName;


            slot.enabled = false;
            tmp.text = "";
            canvasGroup.alpha = 1.0f;

            // ItemImage 오브젝트에서 Image 컴포넌트 가져오기
            Image slotImage = draggedObject.GetComponent<Image>();

            if (slotImage != null)
            {
                // 이미지를 byte[]로 변환하여 전송
                Texture2D texture = ToTexture2D(slotImage.sprite);
                if (texture != null)
                {
                    byte[] textureBytes = texture.EncodeToPNG();

                    // 상대방 클라이언트에게 슬롯 업데이트를 요청
                    TradePanelController.Instance.UpdateSlot(originalSlot.itemName, originalSlot.itemCount, draggedObject.transform.localPosition, draggedRectTransform.sizeDelta, textureBytes);
                }
            }
            else
            {
                Debug.LogError("ItemImage does not have an Image component.");
            }
        }
    }

    // Sprite를 Texture2D로 변환하는 함수
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
