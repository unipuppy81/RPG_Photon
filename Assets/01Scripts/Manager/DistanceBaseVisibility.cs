using System.Collections;
using UnityEngine;
using Photon.Pun;

public class DistanceBaseVisibility : MonoBehaviourPunCallbacks
{
    public float maxDistance; // �ִ� �Ÿ�
    private Transform player;
    private LODGroup lodGroup;

    void Start()
    {
        maxDistance = 5.0f;

        lodGroup = GetComponent<LODGroup>();
        StartCoroutine(AssignPlayer());
    }

    private IEnumerator AssignPlayer()
    {
        while (player == null)
        {
            player = FindObjectOfType<Character_Warrior>()?.transform;
            if (player == null)
            {
                yield return new WaitForSeconds(0.5f); // �÷��̾ ã�� ������ ���
            }
        }
    }

    void Update()
    {
        if (player == null || lodGroup == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > maxDistance)
        {
            if (lodGroup.enabled)
            {
                lodGroup.enabled = false; // maxDistance �̻��̸� LOD �׷� ��Ȱ��ȭ
            }
        }
        else
        {
            if (!lodGroup.enabled)
            {
                lodGroup.enabled = true; // maxDistance �̳��� LOD �׷� Ȱ��ȭ
            }
        }
    }
}
