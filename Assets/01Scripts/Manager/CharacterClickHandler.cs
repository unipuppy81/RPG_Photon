using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class CharacterClickHandler : MonoBehaviourPun
{
    public GameObject uiPanel; // ������ Ŭ�� �� ��Ÿ�� UI �г�
    public Canvas canvas; // UI ĵ����

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // ������ ���콺 ��ư Ŭ�� ����
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                PhotonView photonView = hit.collider.GetComponent<PhotonView>();
                if (photonView != null && !photonView.IsMine && hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Click Player");

                    // Ŭ���� ��ġ�� UI ĵ������ ���� ��ǥ�� ��ȯ
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        canvas.transform as RectTransform,
                        Input.mousePosition,
                        canvas.worldCamera,
                        out Vector2 localPoint
                    );

                    // UI �г� ��ġ ����
                    uiPanel.GetComponent<RectTransform>().localPosition = localPoint;
                    uiPanel.SetActive(true);
                }
            }
        }
    }
}
