using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CharacterClickHandler : MonoBehaviourPun
{
    public GameObject uiPanel; // ������ Ŭ�� �� ��Ÿ�� UI �г�
    public Canvas canvas; // UI ĵ����
    public PhotonView myPV;

    private PhotonView clickedPhotonView; // Ŭ���� �÷��̾��� PhotonView

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
                    clickedPhotonView = photonView;

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

    // �ŷ� ���� ��ư Ŭ�� �� ȣ��� �޼���
    public void OnTradeProposalButtonClicked()
    {
        // clickedPhotonView�� ����� �÷��̾�� �ŷ� ���� �޽����� ����
        if (clickedPhotonView != null && clickedPhotonView.Owner != null)
        {
            // �ŷ� ��û ������
            TradeManager.Instance.pv.RPC("ReceiveTradeRequest", clickedPhotonView.Owner, PhotonNetwork.LocalPlayer, clickedPhotonView.Owner);
            uiPanel.SetActive(false); // UI �г� �����
        }
        else
        {
            Debug.LogError("Clicked PhotonView or its owner is null.");
        }
    }

    // Photon RPC�� ���� �ŷ� ���� �޽����� ����
    [PunRPC]
    private void ReceiveTradeProposal(Player initPlayer)
    {
        //TradeManager.Instance.SendTradeRequest(initPlayer, PhotonNetwork.LocalPlayer);
    }

}
