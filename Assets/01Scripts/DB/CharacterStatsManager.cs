using UnityEngine;
using Photon.Pun;

public class CharacterStatsManager : MonoBehaviourPunCallbacks
{
    public static CharacterStatsManager instance { get; private set; }


    [SerializeField]
    public DataBase statsDB;

    private void Awake()
    {
        // �ν��Ͻ��� �̹� �����ϸ� ���� �������� �ʰ� ���� �ν��Ͻ��� ����մϴ�.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� �����ǵ��� �����մϴ�.
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �����ϸ� �ߺ� ������ �����մϴ�.
            return;
        }
    }
}