using UnityEngine;
using Photon.Pun;
public class LookAtCamera : MonoBehaviourPunCallbacks
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main; // �� ī�޶� ã��
    }

    void Update()
    {
        // �ؽ�Ʈ�� �׻� ī�޶� �ٶ󺸰� ��
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }
}
