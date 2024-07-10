using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class CharacterClickHandler : MonoBehaviourPun
{
    public GameObject uiPanel; // ������ Ŭ�� �� ��Ÿ�� UI �г�
    public Canvas canvas; // UI ĵ����
    public PhotonView myPV;

    private PhotonView clickedPhotonView; // Ŭ���� �÷��̾��� PhotonView

    [Header("Information")]
    [SerializeField]
    private GameObject clickHandlerPanel;
    [SerializeField]
    private GameObject informationPanel;

    [SerializeField]
    private Button informationBtn;
    [SerializeField]
    private Button exitInformationBtn;

    [SerializeField]
    private TextMeshProUGUI _nameText;
    [SerializeField]
    private TextMeshProUGUI _hpText;
    [SerializeField]
    private TextMeshProUGUI _atkText;
    [SerializeField]
    private TextMeshProUGUI _defText;
    [SerializeField]
    private TextMeshProUGUI _speedText;
    


    private string informationName;
    private float informationMaxHp;
    private float informationAtk;
    private float informationDef;
    private float informationSpeed;


    private void Start()
    {
        informationBtn.onClick.AddListener(InformationButton);
        exitInformationBtn.onClick.AddListener(ExitInformationPanel);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // ������ ���콺 ��ư Ŭ�� ����
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                PhotonView photonView = hit.collider.GetComponent<PhotonView>();
                if (photonView != null && !photonView.IsMine && hit.collider.CompareTag("Player"))
                {
                    clickedPhotonView = photonView;

                    // Ŭ���� ��ġ�� UI ĵ������ ���� ��ǥ�� ��ȯ
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        canvas.transform as RectTransform,
                        Input.mousePosition,
                        canvas.worldCamera,
                        out Vector2 localPoint
                    );

                    // UI �г� ��ġ ����
                    uiPanel.GetComponent<RectTransform>().localPosition = localPoint;
                    uiPanel.SetActive(true);


                    Character_Warrior cw = hit.collider.gameObject.GetComponent<Character_Warrior>();
                    informationMaxHp = cw.MaxHp;
                    informationAtk= cw.Atk; 
                    informationDef= cw.Def;
                    informationSpeed = cw.WalkSpeed;
                    informationName = clickedPhotonView.Owner.NickName;

                    InformationSet(informationName, informationMaxHp, informationAtk, informationDef, informationSpeed);
                }
            }
        }
    }

    private void InformationButton()
    {
        informationPanel.SetActive(true);
        clickHandlerPanel.SetActive(false);
    }

    private void ExitInformationPanel()
    {
        informationPanel.SetActive(false);
    }

    private void InformationSet(string _name, float _maxHp, float _atk, float _def, float _speed)
    {
        _nameText.text = "�г��� : " + _name.ToString();
        _hpText.text = "HP : " + _maxHp.ToString();
        _atkText.text = "Atk : " + _atk.ToString();
        _defText.text = "Def : " + _def.ToString();
        _speedText.text = "Speed : " + _speed.ToString();
    }

    // �ŷ� ���� ��ư Ŭ�� �� ȣ��� �޼���
    public void OnTradeProposalButtonClicked()
    {
        // clickedPhotonView�� ����� �÷��̾�� �ŷ� ���� �޽����� ����
        if (clickedPhotonView != null && clickedPhotonView.Owner != null)
        {
            // �ŷ� ��û ������
            TradeManager.Instance.pv.RPC("ReceiveTradeRequest", clickedPhotonView.Owner, PhotonNetwork.LocalPlayer, clickedPhotonView.Owner);
            uiPanel.SetActive(false); // UI �г� �����
        }
        else
        {
            Debug.LogError("Clicked PhotonView or its owner is null.");
        }
    }

    // Photon RPC�� ���� �ŷ� ���� �޽����� ����
    [PunRPC]
    private void ReceiveTradeProposal(Player initPlayer)
    {
        //TradeManager.Instance.SendTradeRequest(initPlayer, PhotonNetwork.LocalPlayer);
    }

}
