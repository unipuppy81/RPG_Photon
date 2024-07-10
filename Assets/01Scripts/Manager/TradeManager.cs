using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TradeManager : MonoBehaviourPunCallbacks
{
    public static TradeManager Instance;
    public GameObject yesornoPanel;
    public GameObject tradePanel;
    public TextMeshProUGUI nameText;

    [Header("PlayerSelect")]
    [SerializeField]
    private GameObject totalPlayerSelectPanel;
    [SerializeField]
    private Button informationButton;
    [SerializeField]
    private Button tradeButton;
    [SerializeField]
    private Button exitButton;

    [Header("Request")]
    [SerializeField]
    private Button acceptButton;
    [SerializeField]
    private Button rejectButton;



    private Player tradeInitiator; // �ŷ��� ������ �÷��̾�
    private Player clickedPlayer; // �ŷ��� ���� ���� �÷��̾�

    public PhotonView pv;


    [Header("Reset")]
    public List<GameObject> resetGameObject = new List<GameObject>();


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


        exitButton.onClick.AddListener(ExitButton);
    }

    // ������
    private void ExitButton()
    {
        totalPlayerSelectPanel.SetActive(false);
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

    // �ŷ� �Ϸ� ���� ó��
    [PunRPC]
    public void ResetTradePanel()
    {

    }

}
