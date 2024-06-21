using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class SpawnManager : MonoBehaviourPunCallbacks
{
    private List<Transform> positionsList = new List<Transform>();

    public static SpawnManager instance;
    public PhotonView pv;
    public Transform[] spawnPositons;

    public int currentGhostCount = 0;
    public int maxGhostCount = 2;

    private bool isCreatingPlayer = false;

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
        GameObject spawnParent = GameObject.Find("EnemySpawnPos");

        if (spawnParent != null)
        {
            // EnemySpawnPos�� ��� �ڽ� Transform�� �����ɴϴ�.
            spawnPositons = spawnParent.GetComponentsInChildren<Transform>();

            // ù ��° ��Ҵ� �θ� ��ü�� Transform�̹Ƿ� �̸� �����մϴ�.
            spawnPositons = spawnPositons.Skip(1).ToArray();

            foreach (Transform pos in spawnPositons)
                positionsList.Add(pos);
        }
        else
        {
            Debug.LogError("EnemySpawnPos ���� ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    [PunRPC]
    public void CreateComputerPlayer()
    {
        // ���� ���̸� �� �̻� �������� ����
        if (isCreatingPlayer)
            return;

        isCreatingPlayer = true;

        int count = maxGhostCount - currentGhostCount;

        for (int i = 0; i < count; i++)
        {
            int idx = Random.Range(0, spawnPositons.Length);

            PhotonNetwork.InstantiateRoomObject("Ghost", spawnPositons[idx].position, Quaternion.identity);

            currentGhostCount++;
        }

        // ������ ���� �� �÷��׸� ����
        isCreatingPlayer = false;
    }

    [PunRPC]
    public void RemoveGhost()
    {
        currentGhostCount--;
        if (currentGhostCount <= 0)
        {
            currentGhostCount = 0;
        }
    }
}
