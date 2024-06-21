using Photon.Pun;
using UnityEngine;

public class PlayerAnimatorSync : MonoBehaviourPun, IPunObservable
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // �����͸� ������
            stream.SendNext(animator.GetBool("UseSkill"));
        }
        else
        {
            // �����͸� �ޱ�
            bool isAttacking = (bool)stream.ReceiveNext();
            animator.SetBool("UseSkill", isAttacking);
        }
    }
}
