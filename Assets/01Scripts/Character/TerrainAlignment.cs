using UnityEngine;

public class TerrainAlignment : MonoBehaviour
{
    public Transform characterTransform;
    public LayerMask terrainLayer; // ���� ���̾ �����մϴ�.
    public float raycastHeight = 1.0f; // ����ĳ��Ʈ�� �߻��� ����
    public float groundOffset = 0.1f; // ���� ���� ĳ���� ������
    public float maxHeightDifference = 1.0f; // ĳ���Ͱ� ���� �� �ִ� �ִ� ���� ����

    private Vector3 previousPosition;

    void Start()
    {
        previousPosition = characterTransform.position;
    }

    void Update()
    {
        AlignCharacterToTerrain();
    }

    void AlignCharacterToTerrain()
    {
        Vector3 rayOrigin = characterTransform.position + Vector3.up * raycastHeight;
        Ray ray = new Ray(rayOrigin, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastHeight * 2, terrainLayer))
        {
            // ���� ���̿� ���� ��ġ�� ���� ���̸� ����մϴ�.
            float heightDifference = Mathf.Abs(hit.point.y - previousPosition.y);

            if (heightDifference <= maxHeightDifference)
            {
                // ĳ������ ��ġ�� ������ ���̿� ����ϴ�.
                Vector3 newPosition = hit.point + Vector3.up * groundOffset;
                characterTransform.position = newPosition;

                // ������ ������ ���� ĳ������ ȸ���� �����մϴ�.
                Quaternion targetRotation = Quaternion.FromToRotation(characterTransform.up, hit.normal) * characterTransform.rotation;
                characterTransform.rotation = targetRotation;

                // ���� ��ġ�� ������Ʈ�մϴ�.
                previousPosition = characterTransform.position;
            }
            else
            {
                // �̵��� ���� ���� ��ġ�� �ǵ����ϴ�.
                characterTransform.position = previousPosition;
            }
        }
    }
}
