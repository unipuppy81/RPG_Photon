using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class NpcDistance : MonoBehaviour
{
    public List<GameObject> playerArr = new List<GameObject>();       // �÷��̾� ������Ʈ

    [SerializeField]
    private TextMeshPro displayText;        // ǥ���� �ؽ�Ʈ UI ���
    [SerializeField]
    private TextMeshPro nameText;

    public float detectRadius = 2.0f; // �ؽ�Ʈ�� ǥ���� �Ÿ�

    public LayerMask playerLayer;

    private ObjData objData;

    
    private void Awake()
    {
        StartCoroutine(DelayedSetup());
    }

    void Update()
    {
        // �ֺ��� �÷��̾� Ž��
        Collider[] hitPlayers = Physics.OverlapSphere(transform.position, detectRadius, playerLayer);

        // ���� �������� �÷��̾� ���
        List<GameObject> currentPlayers = new List<GameObject>();

        foreach (Collider player in hitPlayers)
        {
            currentPlayers.Add(player.gameObject);
        }

        // playerArr�� ���� �÷��̾ �߰�
        foreach (GameObject player in currentPlayers)
        {
            if (!playerArr.Contains(player))
            {
                Character_Warrior cw = player.GetComponent<Character_Warrior>();
                cw.isCommunicate = true;
                playerArr.Add(player);
            }
        }

        // currentPlayers�� ���� �÷��̾ playerArr���� ����
        for (int i = playerArr.Count - 1; i >= 0; i--)
        {
            if (!currentPlayers.Contains(playerArr[i]))
            {
                playerArr.RemoveAt(i);
            }
        }

        // �ؽ�Ʈ ǥ�� ����
        if (playerArr.Count > 0)
        {
            displayText.gameObject.SetActive(true);
        }
        else
        {
            displayText.gameObject.SetActive(false);
        }
    }

    void LateUpdate()
    {
        // �ؽ�Ʈ�� �׻� ī�޶� �ٶ󺸰� ����
        //displayText.transform.LookAt(displayText.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    private IEnumerator DelayedSetup()
    {
        yield return new WaitUntil(() => GameManager.isPlayGame);


        objData = GetComponent<ObjData>();

        if (objData != null)
        {
            nameText.text = objData.objName;
        }

    }
}
