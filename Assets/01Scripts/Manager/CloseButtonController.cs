using UnityEngine.UI;
using UnityEngine;

public class CloseButtonController : MonoBehaviour
{
    public Button closeButton; // �г��� ���� ��ư

    void Start()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(ClosePanel);
        }
    }

    void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
