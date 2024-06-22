using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform canvas;           // UI�� �ҼӵǾ��ִ� �ֻ���� canvas Transform
    private Transform previousParent;   // �ش� ������Ʈ�� ������ �ҼӵǾ� �־��� �θ� Transform
    private RectTransform rect;         // UI ��ġ ��� ���� ����
    private CanvasGroup canvasGroup;    // UI�� ���İ��� ��ȣ�ۿ� ��� ���� CanvasGroup


    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }



    /// <summary>
    /// �巡�� �� 1ȸ ȣ��
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // �巡�� ������ �ҼӵǾ� �ִ� �θ� Transform ���� ����
        previousParent = transform.parent;

        // ���� �巡������ UI�� ȭ���� �ֻ�ܿ� ��µǵ��� �ϱ� ����
        transform.SetParent(canvas);        // �θ� ������Ʈ�� Canvas�� ����
        transform.SetAsLastSibling();       // ���� �տ� ���̵��� ������ �ڽ����� ����

        // �巡�� ������ ������Ʈ�� �ϳ��� �ƴ� �ڽĵ��� ������ �������� �ֱ� ������ CanvasGroup���� ����
        // ���İ��� .6���� �����ϰ�, ���� �浹ó���� ���� �ʵ��� �Ѵ�
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// �巡�� �� �� ������ ȣ��
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        // ���� ��ũ������ ���콺 ��ġ�� UI��ġ�� ����
        rect.position = eventData.position;
    }

    /// <summary>
    /// �巡�� ���� 1ȸ ȣ��
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        // �巡�� �����ϸ� �θ� canvas�� �����Ǳ� ������ 
        // �巡�� ����� �θ� canvas�̸� �����۽����� �ƴ� �����Ѱ��� 
        // ����� �ߴٴ� ���̱� ������ �巡�� ������ �ҼӵǾ� �ִ� ������ �������� ������ �̵�
        if (transform.parent == canvas)
        {
            // �������� �ҼӵǾ��־��� previousParent�� �ڽ����� �����ϰ�, �ش� ��ġ�� ����
            transform.SetParent(previousParent);
            rect.position = previousParent.GetComponent<RectTransform>().position;
        }

        // ���İ� 1, ���� �浹ó�� ok
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
    }
}
