using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DroppableUI : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    private Image image;
    private RectTransform rect;

    private void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
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
            // �巡�� ����� ���� �θ� ����
            Transform originalParent = eventData.pointerDrag.transform.parent;

            // �巡�� ����� �����Ͽ� ���� ������Ʈ�� �����ϰ�, ��ġ�� ���� ������Ʈ�� �����ϰ� ����
            GameObject draggedObject = Instantiate(eventData.pointerDrag, transform);
            draggedObject.transform.localPosition = Vector3.zero;

            // �巡�� ����� �θ� ���� ������Ʈ�� ����
            eventData.pointerDrag.transform.SetParent(transform);

            // ���� �θ�� ������ �巡�� ����� �ٽ� ����
            eventData.pointerDrag.transform.SetParent(originalParent);

            CanvasGroup canvasGroup = draggedObject.GetComponent<CanvasGroup>();
            TextMeshProUGUI tmp = draggedObject.GetComponentInChildren<TextMeshProUGUI>();
            Slot slot = draggedObject.GetComponent<Slot>();
            //Image _image = draggedObject.GetComponentInChildren<Image>();

            //image = _image;
            slot.enabled = false;
            tmp.text = "";
            canvasGroup.alpha = 1.0f;
        }

    }

}
