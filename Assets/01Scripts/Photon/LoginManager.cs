using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class LoginManager : MonoBehaviourPunCallbacks
{
    [Header("Login UI")]
    public TMP_InputField NickNameInput;
    public TextMeshProUGUI StatusText;
    public Button startButton;

    void Start()
    {
        startButton.onClick.AddListener(OnLoginButtonClicked);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "v1.0";
    }

    void OnLoginButtonClicked()
    {
        if (string.IsNullOrEmpty(NickNameInput.text))
        {
            StatusText.text = "�г����� �Է��ϼ���.";
            return;
        }

        PhotonNetwork.NickName = NickNameInput.text;
        StatusText.text = "������ ������ ������...";
        PhotonNetwork.ConnectUsingSettings();
        startButton.interactable = false;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");
        StatusText.text = "������ ������ ���� ��";
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        StatusText.text = "�κ� ���� �Ϸ�. ������ �����մϴ�...";
        PhotonNetwork.JoinRandomRoom(); // �濡 �������� ���� �õ�
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected: {cause}");
        StatusText.text = $"������ ���������ϴ�: {cause}";
        startButton.interactable = true;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join random room, creating a new room...");
        CreateRoom();
    }

    void CreateRoom()
    {
        string roomName = "Room_" + Random.Range(0, 1000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.CleanupCacheOnLeave = true;

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room created successfully");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room successfully");
        StatusText.text = "�濡 ���� �Ϸ�. ������ �����մϴ�...";
        PhotonNetwork.LoadLevel("GameScene"); // ���� ������ ��ȯ
    }
}
