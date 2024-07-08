using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/*
public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;

    [Header("Chat")]
    public TMP_InputField ChatInput;
    public GameObject chatPanel, chatView;

    private TextMeshProUGUI[] chatList;

    public GameObject playerSpawnPosObj;
    public Transform[] spawnPositions;

    public PhotonView PV;

    private void Awake()
    {
        // �̱��� ���� ����
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Debug.Log("���� �� ��Ʈ��ũ �Ŵ��� ����");
        chatList = chatView.GetComponentsInChildren<TextMeshProUGUI>();

        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogError("Photon�� ������� �ʾҽ��ϴ�.");
            return;
        }

        //OnJoinedRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("04. �� ���� �Ϸ�");

        // ä�� �г� on
        chatPanel.SetActive(true);

        ChatInput.text = "";
        foreach (TextMeshProUGUI chat in chatList)
            chat.text = "";


        Debug.Log("Spawn Warrior");
        PhotonNetwork.Instantiate("Warrior", playerSpawnPosObj.transform.position, Quaternion.identity);


        StartCoroutine(DelayedSetup());
    }


    private IEnumerator DelayedSetup()
    {
        yield return new WaitUntil(() => GameManager.isPlayGame);

        if (PhotonNetwork.IsMasterClient)
        {
            SpawnManager.instance.pv.RPC("CreateComputerPlayer", RpcTarget.All);
            PV.RPC("ChatRPC", RpcTarget.All, "<color=yellow>[����] " + PhotonNetwork.NickName + "���� �����ϼ̽��ϴ�</color>");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PV.RPC("ChatRPC", RpcTarget.All, "<color=yellow>" + newPlayer.NickName + "���� �����ϼ̽��ϴ�</color>");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PV.RPC("ChatRPC", RpcTarget.All, "<color=yellow>" + otherPlayer.NickName + "���� �������ϴ�</color>");
    }

    public void Send()
    {
        if (ChatInput.text.Equals(""))
            return;
        string msg = "[" + PhotonNetwork.NickName + "] " + ChatInput.text;
        PV.RPC("ChatRPC", RpcTarget.All, msg);
        ChatInput.text = "";
    }

    [PunRPC]
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < chatList.Length; i++)
            if (chatList[i].text == "")
            {
                isInput = true;
                chatList[i].text = msg;
                break;
            }
        if (!isInput)
        {
            for (int i = 1; i < chatList.Length; i++) chatList[i - 1].text = chatList[i].text;
            chatList[chatList.Length - 1].text = msg;
        }
    }

    [PunRPC]
    public void RequestSceneChange(int playerID, PhotonView pv, string sceneName)
    {

        // ��û�� �÷��̾� ID���� �� ��ȯ ����� ����
        Player targetPlayer = PhotonNetwork.CurrentRoom.GetPlayer(playerID);
        if (targetPlayer != null)
        {
            pv.RPC("ChangeSceneForLocalPlayer", targetPlayer, sceneName);
        }
        
    }

    public override void OnLeftRoom()
    {

    }

}
*/

public class NetworkManager : SingletonPhoton<NetworkManager>
{
    [Header("Loading")]
    [SerializeField]
    private GameObject loadingScreen;
    [SerializeField]
    private TextMeshProUGUI loadingText;
    [SerializeField]
    private Slider loadingSlider;

    [Header("NickName")]
    public TextMeshProUGUI StatusText;
    public TMP_InputField NickNameInput;
    public Button startButton;
    public GameObject startPanel;


    [Header("Chat")]
    public TMP_InputField ChatInput;
    public GameObject chatPanel, chatView;

    private TextMeshProUGUI[] chatList;


    // �÷��̾� ���� ��ġ
    public GameObject playerSpawnPosObj;

    // �� ���� ��ġ
    public Transform[] spawnPositions;
    private int idx;


    [Header("Photon")]
    public readonly string gameVersion = "v1.0";
    public PhotonView PV;

    void Awake()
    {
        // ������ ȥ�� ���� �ε��ϸ�, ������ ������� �ڵ����� ��ũ�� ��
        PhotonNetwork.AutomaticallySyncScene = true;

    }

    void Start()
    {
        ShowLoadingScreen();

        Debug.Log("00. ���� �Ŵ��� ����");

        chatList = chatView.GetComponentsInChildren<TextMeshProUGUI>();

        //startButton.onClick.AddListener(JoinRoom);
        //OnLogin();

        PhotonNetwork.GameVersion = this.gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }
    void OnLogin()
    {
        PhotonNetwork.ConnectUsingSettings();
        startButton.interactable = false;
        StatusText.text = "������ ������ ������...";
    }

    void JoinRoom()
    {
        if (NickNameInput.text.Equals(""))
            PhotonNetwork.LocalPlayer.NickName = "unknown";
        else
            PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;

        PhotonNetwork.JoinRandomRoom();
        Debug.Log("01. ���� ������ ����");

    }

    void Connect() => PhotonNetwork.ConnectUsingSettings();



    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("01. ���� ������ ����");

        //StatusText.text = "Online: ������ ������ ���� ��";
        //startButton.interactable = true;

        //PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("02. ���� �� ���� ����");

        // �� �Ӽ� ����
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 5;

        // �� ���� -> �ڵ� ����
        PhotonNetwork.CreateRoom("room_1", ro);

    }


    public override void OnCreatedRoom()
    {
        Debug.Log("03. �� ���� �Ϸ�");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("04. �� ���� �Ϸ�");


        // �г��� �г� off
        //startPanel.SetActive(false);


        // ä�� �г� on
        chatPanel.SetActive(true);

        ChatInput.text = "";
        foreach (TextMeshProUGUI chat in chatList)
            chat.text = "";

        if (PhotonNetwork.IsMasterClient)
        {
            SpawnManager.instance.pv.RPC("CreateComputerPlayer", RpcTarget.All);
            PV.RPC("ChatRPC", RpcTarget.All, "<color=yellow>[����] " + PhotonNetwork.NickName + "���� �����ϼ̽��ϴ�</color>");
        }

        PhotonNetwork.Instantiate("Warrior", playerSpawnPosObj.transform.position, Quaternion.identity);

        
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        this.CreateRoom();
    }

    void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.CleanupCacheOnLeave = true; // ������ ������ ������Ʈ ����

        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    // ������ ����Ǿ��� �� ȣ��Ǵ� �ݹ� �޼���
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("New master client: " + newMasterClient.NickName);
        // �ʿ��� �ʱ�ȭ �۾��� �߰��� �� �ֽ��ϴ�.
    }

    // �÷��̾ ������ �� ȣ��Ǵ� �ݹ� �޼���
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " has left the room.");
        PV.RPC("ChatRPC", RpcTarget.All, "<color=yellow>" + otherPlayer.NickName + "���� �������ϴ�</color>");

    }

    private void ShowLoadingScreen()
    {
        // �ε� ȭ���� �����ִ� �޼���
        loadingScreen.SetActive(true);
        StartCoroutine(IncreaseSliderValue());
        loadingText.text = "���� ���� ��...";
    }
    IEnumerator IncreaseSliderValue()
    {
        float timer = 0f;
        float startValue = loadingSlider.value;
        float endValue = 1f;

        while (timer < 4.0f)
        {
            timer += Time.deltaTime;
            float progress = timer / 4.0f;
            loadingSlider.value = Mathf.Lerp(startValue, endValue, progress);
            yield return null;
        }

        // duration�� ���� �� �ִ� ������ ����
        loadingSlider.value = 1f;
    }
    public void HideLoadingScreen()
    {
        // �ε� ȭ���� ����� �޼���
        loadingScreen.SetActive(false);
    }

    // �÷��̾� ���� �� ä�� â ���
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PV.RPC("ChatRPC", RpcTarget.All, "<color=yellow>" + newPlayer.NickName + "���� �����ϼ̽��ϴ�</color>");
    }

    // ä�� ���� �Լ�
    public void Send()
    {
        if (ChatInput.text.Equals(""))
            return;
        string msg = "[" + PhotonNetwork.NickName + "] " + ChatInput.text;
        PV.RPC("ChatRPC", RpcTarget.All, msg);
        ChatInput.text = "";
    }

    [PunRPC]
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < chatList.Length; i++)
            if (chatList[i].text == "")
            {
                isInput = true;
                chatList[i].text = msg;
                break;
            }
        if (!isInput)
        {
            for (int i = 1; i < chatList.Length; i++) chatList[i - 1].text = chatList[i].text;
            chatList[chatList.Length - 1].text = msg;
        }
    }
}
