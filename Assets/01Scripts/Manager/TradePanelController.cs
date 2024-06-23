using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class TradePanelController : MonoBehaviourPun
{
    public static TradePanelController Instance;

    public TextMeshProUGUI initNameText;
    public TextMeshProUGUI clickedNameText;

    public Button tradeButton;
    public Button tradeExitButton; // �ŷ� �׸��ϱ� ��ư

    private Player initPlayer;
    private Player clickedPlayer;

    private bool initOffered = false; // ���� �ŷ� ��ư�� �������� ����
    private bool clickedOffered = false; // ������ �ŷ� ��ư�� �������� ����

    private PhotonView tradePhotonView;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // �ŷ��ϱ� �г� �ʱ�ȭ
    public void Initialize(Player _initiator, Player _clickedPlayer)
    {
        initPlayer = _initiator;
        clickedPlayer = _clickedPlayer;

        initNameText.text = initPlayer.NickName;
        clickedNameText.text = clickedPlayer.NickName;
    }

    // �ŷ��ϱ� ���� ��ư Ŭ�� ��
    public void OnOfferButtonClicked()
    {
        if (!initOffered) // ���� �ŷ� ��ư�� �̹� ���� ���¶�� �ٽ� ���� �� ����
        {
            Debug.Log("Offer button clicked");
            initOffered = true;
            photonView.RPC("ReceiveTradeOffer", RpcTarget.Others);
            CheckTradeStatus();
        }
    }

    // ���濡�� �ŷ��ϱ� ���� ����
    [PunRPC]
    private void ReceiveTradeOffer()
    {
        clickedOffered = true;
        CheckTradeStatus();
    }

    // ��� ��ư Ŭ�� ��
    public void OnCancelButtonClicked()
    {
        if (initOffered) // ���� �ŷ��ϱ� ��ư�� ���� ���¶��
        {
            Debug.Log("Cancel button clicked");
            initOffered = false;
            clickedOffered = false; // ������ �ŷ� ���µ� �ʱ�ȭ
            photonView.RPC("CancelTradeOffer", RpcTarget.Others);
            gameObject.SetActive(false); // �г� �����
        }
    }

    // �ŷ��ϱ� �׸��ϱ� ��ư Ŭ�� ��
    private void OnStopTradeButtonClicked()
    {
        Debug.Log("Stop trade button clicked");
        initOffered = false;
        clickedOffered = false; // �ŷ��ϱ� �غ� ���� �ʱ�ȭ
        gameObject.SetActive(false); // �г� �����
    }

    // �ŷ��ϱ� ��� �޽��� ����
    [PunRPC]
    private void CancelTradeOffer()
    {
        clickedOffered = false; // ������ �ŷ� ���� �ʱ�ȭ
    }

    // �ŷ� ���� Ȯ�� �� ó��
    private void CheckTradeStatus()
    {
        if (initOffered && clickedOffered)
        {
            AcceptTrade();
        }
    }

    // �ŷ��ϱ� ���� ó��
    private void AcceptTrade()
    {
        Debug.Log("Trade accepted by both players");
        photonView.RPC("CompleteTrade", RpcTarget.Others);
        gameObject.SetActive(false); // �г� �����
    }

    // �ŷ��ϱ� �Ϸ� ó��
    [PunRPC]
    private void CompleteTrade()
    {
        Debug.Log("Trade completed");
        // ���⼭ �������� ������ �����մϴ�.
    }
}
