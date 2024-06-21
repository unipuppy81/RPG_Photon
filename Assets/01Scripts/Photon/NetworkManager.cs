using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public Text statusText;
    public InputField nickNameInput;

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
        Debug.Log("00. ���� �Ŵ��� ����");

        PhotonNetwork.GameVersion = this.gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("01. ���� ������ ����");

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

        if (PhotonNetwork.IsMasterClient)
        {
            SpawnManager.instance.pv.RPC("CreateComputerPlayer", RpcTarget.All);
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
    }
}
