using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public class TradePanelController : MonoBehaviourPunCallbacks
{
    public PhotonView tradePanelPhotonView;

    public static TradePanelController Instance;

    public TextMeshProUGUI initNameText;
    public TextMeshProUGUI clickedNameText;

    public TextMeshProUGUI initReadyText;
    public TextMeshProUGUI clickedReadyText;

    public GameObject tradeButtonObj;
    public GameObject nonTradeButtonObj;
    public GameObject tradeExitButtonObj;

    public GameObject slotPrefab;

    public GameObject TradeResultSuccessPanel;
    public GameObject TradeResultFailPanel;

    private Button tradeButton;         // ����ϱ� ��ư
    private Button nonTradeButton;      // ������ ��ư
    private Button tradeExitButton;     // �ŷ� �׸��ϱ� ��ư

    public Player initPlayer;
    public Player clickedPlayer;

    public List<string> itemNameList;
    public List<int> itemCountList;

    private bool initOffered = false; // ���� ��� ��ư�� �������� ����
    private bool clickedOffered = false; // ������ ��� ��ư�� �������� ����

    private const string INIT_OFFERED = "InitOffered";
    private const string CLICKED_OFFERED = "ClickedOffered";

    
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

        tradePanelPhotonView.RPC("CheckTradeStatus", initPlayer);
        tradePanelPhotonView.RPC("CheckTradeStatus", clickedPlayer);
    }

    private void OnEnable()
    {
        GameManager.isTradeChatting = true;
    }

    private void OnDisable()
    {
        GameManager.isTradeChatting = false;
    }

    public void UpdateSlot(string _itemName, int _itemCount, Vector3 _position, Vector2 _sizeDelta, byte[] _itemSpriteBytes)
    {
        Debug.Log("TT");
        if(PhotonNetwork.LocalPlayer == initPlayer)
        {
            Debug.Log("E");
            tradePanelPhotonView.RPC("UpdateTradeSlot", clickedPlayer, _itemName, _itemCount, _position, _sizeDelta, _itemSpriteBytes);
        }
        else if (PhotonNetwork.LocalPlayer == clickedPlayer)
        {
            Debug.Log("A");
            tradePanelPhotonView.RPC("UpdateTradeSlot", initPlayer, _itemName, _itemCount, _position, _sizeDelta, _itemSpriteBytes);
        }
    }

    // ������ �ű涧 ����ȭ
    [PunRPC]
    public void UpdateTradeSlot(string itemName, int itemCount, Vector3 position, Vector2 sizeDelta, byte[] itemSpriteBytes)
    {
        // �ŷ� ���� ������Ʈ ����
        // �ŷ� �г��� ������ ã�Ƽ� ������Ʈ�մϴ�.
        GameObject newSlot = Instantiate(slotPrefab, transform);
        newSlot.transform.localPosition = position;

        RectTransform rectTransform = newSlot.GetComponent<RectTransform>();
        rectTransform.sizeDelta = sizeDelta;

        Slot slot = newSlot.GetComponent<Slot>();
        slot.itemName = itemName;
        slot.itemCount = itemCount;



        //Image slotImage = newSlot.GetComponentInChildren<Image>();
        Image slotImage = newSlot.GetComponent<Image>();


        // Texture2D�� ��ȯ
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(itemSpriteBytes); // byte �迭���� �̹��� �ε�

        // Texture2D�� Sprite�� ��ȯ
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        slotImage.sprite = sprite;

        Debug.Log("T");


        TextMeshProUGUI tmp = newSlot.GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = $"{itemName}: {itemCount}";

        CanvasGroup canvasGroup = newSlot.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1.0f;
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


        tradePanelPhotonView.RPC("CheckTradeStatus", initPlayer);
        tradePanelPhotonView.RPC("CheckTradeStatus", clickedPlayer);

        //tradePanelPhotonView.RPC("CheckTradeStatus", RpcTarget.All);
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

        tradePanelPhotonView.RPC("CheckTradeStatus", initPlayer);
        tradePanelPhotonView.RPC("CheckTradeStatus", clickedPlayer);

        //tradePanelPhotonView.RPC("CheckTradeStatus", RpcTarget.All);
    }


    // �ŷ� �׸��ϱ� ��ư Ŭ�� ��
    public void OnTradeExitButtonClicked()
    {
        ResetTradeState();


        if (PhotonNetwork.LocalPlayer == initPlayer)
        {
            tradePanelPhotonView.RPC("TradeFail", clickedPlayer);
        }
        else if (PhotonNetwork.LocalPlayer == clickedPlayer)
        {
            tradePanelPhotonView.RPC("TradeFail", initPlayer);
        }

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

            boolText();

            if (initOffered && clickedOffered)
            {
                AcceptTrade();
            }
        }
    }

    // �ؽ�Ʈ ����
    private void boolText()
    {
        // �ؽ�Ʈ ����
        if (initOffered)
        {
            initReadyText.text = "�غ��";
        }
        else if (!initOffered)
        {
            initReadyText.text = "�غ���";
        }

        if (clickedOffered)
        {
            clickedReadyText.text = "�غ��";
        }
        else if (!clickedOffered)
        {
            clickedReadyText.text = "�غ���";
        }
    }

    // �ŷ� ���� ó��
    private void AcceptTrade()
    {
        ItemDataManager.Instance.TradeRemoveItem(itemNameList, itemCountList);

        if (PhotonNetwork.LocalPlayer == initPlayer)
        {
            tradePanelPhotonView.RPC("CompleteTrade", clickedPlayer, itemNameList, itemCountList);
        }
        else if (PhotonNetwork.LocalPlayer == clickedPlayer)
        {
            tradePanelPhotonView.RPC("CompleteTrade", initPlayer, itemNameList, itemCountList);
        }
    }

    // �ŷ� �Ϸ� ó��
    [PunRPC]
    public void CompleteTrade(List<string> s, List<int> i)
    {
        // ���⼭ �������� ������ �����մϴ�.
        ItemDataManager.Instance.TradeAddItem(s, i);

        TradeSuccess(); 

        gameObject.SetActive(false); // �г� �����
    }

    [PunRPC]
    public void TradeFail()
    {
        this.gameObject.SetActive(false);
        TradeResultFailPanel.SetActive(true);
    }

    [PunRPC]
    public void TradeSuccess()
    {
        TradeResultSuccessPanel.SetActive(true);
    }
}
