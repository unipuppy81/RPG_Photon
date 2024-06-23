using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerTrade : MonoBehaviourPun
{
    void Update()
    {
        
    }

    // Ÿ�� �÷��̾ ã�� �ӽ� �޼���
    Player FindTargetPlayer()
    {
        // ���� ���ӿ����� Ÿ�� �÷��̾ ã�� ������ �����ؾ� �մϴ�.
        // ���⼭�� ���÷� ù ��° �ٸ� �÷��̾ ��ȯ�մϴ�.
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player != PhotonNetwork.LocalPlayer)
            {
                return player;
            }
        }
        return null;
    }
}
