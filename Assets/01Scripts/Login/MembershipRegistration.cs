using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class MembershipManager : MonoBehaviourPunCallbacks
{
    [Header("Login")]
    [SerializeField]
    private TextMeshProUGUI StatusText;
    [SerializeField]
    private TMP_InputField loginIdInput;
    [SerializeField]
    private TMP_InputField loginPasswordInput;

    [Header("Sign Up")]
    [SerializeField]
    private TMP_InputField idInput;   // ���̵� �Է� �ʵ�
    [SerializeField]
    private TMP_InputField passwordInput;   // ��й�ȣ �Է� �ʵ�
    [SerializeField]
    private TMP_InputField nicknameInput;   // �г��� �Է� �ʵ� (ȸ�����Կ�)

    [SerializeField]
    private Button loginButton;         // �α��� ��ư
    [SerializeField]
    private Button SignupPanelButton;
    [SerializeField]
    private Button registerButton;      // ȸ�� ���� ��ư
    [SerializeField]
    private Button ExitButton;

    [SerializeField]
    private GameObject SignUpPanel;

    private void Start()
    {
        registerButton.onClick.AddListener(RegisterUser);
        loginButton.onClick.AddListener(LoginUser);
        SignupPanelButton.onClick.AddListener(SignupPanel);
        ExitButton.onClick.AddListener(ExitSignupPanel);

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "v1.0";
    }

    // �α���
    private void LoginUser()
    {
        string userId = loginIdInput.text;    // �Էµ� ����ڸ�
        string password = loginPasswordInput.text;    // �Էµ� ��й�ȣ

        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("����ڸ�� ��й�ȣ�� ������� �� �����ϴ�.");
            return;
        }

        // ����� ����� ������ ��������
        if (PlayerPrefs.HasKey(userId))
        {
            string savedPassword = PlayerPrefs.GetString(userId);

            if (savedPassword == password)
            {
                // �α��� ����, �г��� ��������
                string nickname = PlayerPrefs.GetString(userId + "_nickname");

                // PhotonNetwork�� �г��� ���� �� ����
                PhotonNetwork.NickName = nickname;

                PhotonNetwork.LoadLevel("GameScene"); // ���� ������ ��ȯ
                //PhotonNetwork.ConnectUsingSettings();

                loginButton.interactable = false;
            }
            else
            {
                Debug.LogWarning("��й�ȣ�� ��ġ���� �ʽ��ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("����ڰ� �������� �ʽ��ϴ�.");
        }
    }

    // ȸ������
    private void RegisterUser()
    {
        string username = idInput.text;    // �Էµ� ����ڸ�
        string password = passwordInput.text;    // �Էµ� ��й�ȣ
        string nickname = nicknameInput.text;    // �Էµ� �г���

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(nickname))
        {
            Debug.LogWarning("����ڸ�, ��й�ȣ �� �г����� ������� �� �����ϴ�.");
            return;
        }

        // ����ڰ� �̹� �����ϴ��� Ȯ���մϴ�.
        if (PlayerPrefs.HasKey(username))
        {
            Debug.LogWarning("�̹� ���̵� �����մϴ�.");
            return;
        }

        // PlayerPrefs�� ����Ͽ� ����� �����͸� �����մϴ�.
        PlayerPrefs.SetString(username, password);
        PlayerPrefs.SetString(username + "_nickname", nickname);
        PlayerPrefs.Save();

        Debug.Log("����� ��� �Ϸ�: " + username);

        ExitSignupPanel();
    }

    // Event
    private void SignupPanel()
    {
        SignUpPanel.SetActive(true);

        loginIdInput.text = "";
        loginPasswordInput.text = "";
    }

    private void ExitSignupPanel()
    {
        SignUpPanel.SetActive(false);

        idInput.text = "";
        passwordInput.text = "";
        nicknameInput.text = "";
    }

    /*
    public override void OnConnectedToMaster()
    {
        StatusText.text = "������ ���� ��";
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        StatusText.text = "���ӿ� ������ �����ϰڽ��ϴ�...";

        PhotonNetwork.JoinRandomRoom(); // �濡 �������� ���� �õ�
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        StatusText.text = $"������ ���������ϴ�: {cause}";
        loginButton.interactable = true;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
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
        StatusText.text = "���� �Ϸ�";

        //LoadingSceneController.LoadScene("GameScene");
        PhotonNetwork.LoadLevel("GameScene"); // ���� ������ ��ȯ
    }
    */
}
