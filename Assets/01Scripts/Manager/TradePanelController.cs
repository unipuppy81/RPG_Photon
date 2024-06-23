using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class TradePanelController : MonoBehaviourPunCallbacks
{
    public PhotonView tradePanelPhotonView;

    public static TradePanelController Instance;

    public TextMeshProUGUI initNameText;
    public TextMeshProUGUI clickedNameText;

    public GameObject tradeButtonObj;
    public GameObject nonTradeButtonObj;
    public GameObject tradeExitButtonObj;

    public GameObject TradeResultSuccessPanel;
    public GameObject TradeResultFailPanel;

    private Button tradeButton;         // ����ϱ� ��ư
    private Button nonTradeButton;      // ������ ��ư
    private Button tradeExitButton;     // �ŷ� �׸��ϱ� ��ư

    private Player initPlayer;
    private Player clickedPlayer;

    private bool initOffered = false; // ���� ��� ��ư�� �������� ����
    private bool clickedOffered = false; // ������ ��� ��ư�� �������� ����

    private const string INIT_OFFERED = "InitOffered";
    private const string CLICKED_OFFERED = "ClickedOffered";

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

    private void Start()
    {
        tradeButton = tradeButtonObj.GetComponent<Button>();
        nonTradeButton = nonTradeButtonObj.GetComponent<Button>();
        tradeExitButton = tradeExitButtonObj.GetComponent<Button>();

        tradePanelPhotonView = GetComponent<PhotonView>();
    }

    // ����ϱ� �г� �ʱ�ȭ
    public void Initialize(Player _initiator, Player _clickedPlayer)
    {
        initPlayer = _initiator;
        clickedPlayer = _clickedPlayer;

        initNameText.text = initPlayer.NickName;
        clickedNameText.text = clickedPlayer.NickName;

        // �ʱ�ȭ �� �� �÷��̾��� ��� ���¸� �ʱ�ȭ
        Hashtable initProps = new Hashtable { { INIT_OFFERED, false } };
        initPlayer.SetCustomProperties(initProps);

        Hashtable clickedProps = new Hashtable { { CLICKED_OFFERED, false } };
        clickedPlayer.SetCustomProperties(clickedProps);
    }

    private void OnEnable()
    {
        GameManager.isTradeChatting = true;
    }

    private void OnDisable()
    {
        GameManager.isTradeChatting = false;
    }
    // ��� ��ư Ŭ�� ��, ������ bool �� �������� ó��
    public void OnTradeButtonClicked()
    {
        if (tradeButtonObj.activeSelf)
        {
            tradeButtonObj.SetActive(false);
            nonTradeButtonObj.SetActive(true);
        }

        if (PhotonNetwork.LocalPlayer == initPlayer)
        {
            Hashtable props = new Hashtable { { INIT_OFFERED, true } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
        else if (PhotonNetwork.LocalPlayer == clickedPlayer)
        {
            Hashtable props = new Hashtable { { CLICKED_OFFERED, true } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }

        tradePanelPhotonView.RPC("CheckTradeStatus", RpcTarget.All);
    }

    // ��� ��� ��ư Ŭ�� ��
    public void OnTradeButtonNonClicked()
    {
        if (nonTradeButtonObj.activeSelf)
        {
            tradeButtonObj.SetActive(true);
            nonTradeButtonObj.SetActive(false);
        }

        if (PhotonNetwork.LocalPlayer == initPlayer)
        {
            Hashtable props = new Hashtable { { INIT_OFFERED, false } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
        else if (PhotonNetwork.LocalPlayer == clickedPlayer)
        {
            Hashtable props = new Hashtable { { CLICKED_OFFERED, false } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }

        tradePanelPhotonView.RPC("CheckTradeStatus", RpcTarget.All);
    }


    // �ŷ� �׸��ϱ� ��ư Ŭ�� ��
    public void OnTradeExitButtonClicked()
    {
        ResetTradeState();

        tradePanelPhotonView.RPC("TradeFail", RpcTarget.All);

        gameObject.SetActive(false); // �г� �����
    }

    // �ŷ� ���� �ʱ�ȭ - Ŭ���̾�Ʈ
    private void ResetTradeState()
    {
        if (PhotonNetwork.LocalPlayer == initPlayer)
        {
            Hashtable props = new Hashtable { { INIT_OFFERED, false } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
        else if (PhotonNetwork.LocalPlayer == clickedPlayer)
        {
            Hashtable props = new Hashtable { { CLICKED_OFFERED, false } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }

        tradePanelPhotonView.RPC("ResetPartnerTradeState", RpcTarget.Others);
    }

    // �ŷ� ���� �ʱ�ȭ - ���� �� ����ȭ
    [PunRPC]
    public void ResetPartnerTradeState()
    {
        if (PhotonNetwork.LocalPlayer == initPlayer)
        {
            Hashtable props = new Hashtable { { CLICKED_OFFERED, false } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
        else if (PhotonNetwork.LocalPlayer == clickedPlayer)
        {
            Hashtable props = new Hashtable { { INIT_OFFERED, false } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
    }

    // �ŷ� ���� Ȯ�� �� ó��
    [PunRPC]
    public void CheckTradeStatus()
    {
        if (initPlayer.CustomProperties.ContainsKey(INIT_OFFERED) &&
            clickedPlayer.CustomProperties.ContainsKey(CLICKED_OFFERED))
        {
            initOffered = (bool)initPlayer.CustomProperties[INIT_OFFERED];
            clickedOffered = (bool)clickedPlayer.CustomProperties[CLICKED_OFFERED];

            if (initOffered && clickedOffered)
            {
                AcceptTrade();
            }
        }
    }

    // �ŷ� ���� ó��
    private void AcceptTrade()
    {
        tradePanelPhotonView.RPC("CompleteTrade", RpcTarget.All);
    }

    // �ŷ� �Ϸ� ó��
    [PunRPC]
    public void CompleteTrade()
    {
        Debug.Log("Trade completed");
        // ���⼭ �������� ������ �����մϴ�.

        tradePanelPhotonView.RPC("TradeSuccess", RpcTarget.All);
        gameObject.SetActive(false); // �г� �����
    }

    [PunRPC]
    public void TradeFail()
    {
        TradeResultFailPanel.SetActive(true);
    }

    [PunRPC]
    public void TradeSuccess()
    {
        TradeResultSuccessPanel.SetActive(true);
    }
}
