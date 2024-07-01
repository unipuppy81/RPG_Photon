using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TradeManager : MonoBehaviourPunCallbacks
{
    public static TradeManager Instance;
    public GameObject yesornoPanel;
    public GameObject tradePanel;
    public TextMeshProUGUI nameText;
    public Button acceptButton;
    public Button rejectButton;

    private Player tradeInitiator; // �ŷ��� ������ �÷��̾�
    private Player clickedPlayer; // �ŷ��� ���� ���� �÷��̾�

    public PhotonView pv;

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

    void Start()
    {
        pv = GetComponent<PhotonView>();
        acceptButton.onClick.AddListener(OnAcceptTrade);
        rejectButton.onClick.AddListener(OnRejectTrade);
    }

    // �ŷ� ��û �ޱ�
    [PunRPC]
    public void ReceiveTradeRequest(Player initPlayer, Player _clickedPlayer)
    {
        tradeInitiator = initPlayer; // �ŷ��� ������ �÷��̾� ����
        clickedPlayer = _clickedPlayer; // �ŷ��� ���ϴ� �÷��̾� ����

        if (PhotonNetwork.LocalPlayer == null)
        {
            Debug.LogError("PhotonNetwork.LocalPlayer is null");
        }

        if (clickedPlayer == null)
        {
            Debug.LogError("clickedPlayer is null");
        }

        // ������ ȭ�鿡 �ŷ� ��û UI ǥ��
        if (PhotonNetwork.LocalPlayer != null && PhotonNetwork.LocalPlayer == clickedPlayer)
        {
            ShowTradeRequest();
        }
    }

    // �ŷ� ��û UI ǥ��
    public void ShowTradeRequest()
    {
        yesornoPanel.SetActive(true);
        nameText.text = tradeInitiator.NickName + " ���� �ŷ��� ��û�߽��ϴ�.";
    }

    // �ŷ� ��û ����
    public void OnAcceptTrade()
    {
        yesornoPanel.SetActive(false);

        if(PhotonNetwork.LocalPlayer == clickedPlayer)
        {
            // �ŷ� �г� ����
            TradePanelActive(tradeInitiator, clickedPlayer); 
        }

        // ��Ʈ��ũ�� ���� ���濡�Ե� UI�� ǥ���ϵ��� RPC ȣ��
        pv.RPC("TradePanelActive", tradeInitiator, tradeInitiator, clickedPlayer);
    }

    [PunRPC]
    public void TradePanelActive(Player initPlayer, Player _clickedPlayer)
    {
        tradePanel.SetActive(true);
        TradePanelController.Instance.Initialize(initPlayer, _clickedPlayer);
    }

    // �ŷ� ��û �ź�
    public void OnRejectTrade()
    {
        RejectTradeRequest();
        yesornoPanel.SetActive(false);
    }

    // �ŷ� ��û �ź� ó��
    public void RejectTradeRequest()
    {
        Debug.Log("Trade request rejected");
    }

    // �ŷ� �Ϸ� ó��
    [PunRPC]
    public void CompleteTrade()
    {
        Debug.Log("Trade completed");
        // ���⼭ �������� ������ �����մϴ�.
    }

    public override void OnLeftRoom()
    {
        Debug.Log("On Left Room");

        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            Destroy(gameObject);
            SceneManager.LoadScene("TownScene");
        }
        else if (SceneManager.GetActiveScene().name == "TownScene")
        {
            Destroy(gameObject);
            SceneManager.LoadScene("GameScene");
        }
    }
}
