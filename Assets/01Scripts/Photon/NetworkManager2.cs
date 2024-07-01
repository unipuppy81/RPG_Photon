using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NetworkManager2 : MonoBehaviourPunCallbacks
{
    [Header("Chat")]
    public TMP_InputField ChatInput;
    public GameObject chatPanel, chatView;

    private TextMeshProUGUI[] chatList;

    public GameObject playerSpawnPosObj;
    public Transform[] spawnPositions;

    public PhotonView PV;

    void Start()
    {
        Debug.Log("���� �� ��Ʈ��ũ �Ŵ��� ����");
        chatList = chatView.GetComponentsInChildren<TextMeshProUGUI>();

        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogError("Photon�� ������� �ʾҽ��ϴ�.");
            return;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            SpawnManager.instance.pv.RPC("CreateComputerPlayer", RpcTarget.All);
            PV.RPC("ChatRPC", RpcTarget.All, "<color=yellow>[����] " + PhotonNetwork.NickName + "���� �����ϼ̽��ϴ�</color>");
        }

        PhotonNetwork.Instantiate("Warrior", playerSpawnPosObj.transform.position, Quaternion.identity);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("�� ���� �Ϸ�");
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
}
