using UnityEngine;

public class DontDestroyOnSceneChange : MonoBehaviour
{
    private void Awake()
    {
        // ���� ������Ʈ�� �� ��ȯ �� �ı����� �ʵ��� ����
        DontDestroyOnLoad(gameObject);
    }
}