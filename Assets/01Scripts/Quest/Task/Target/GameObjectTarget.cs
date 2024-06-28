using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Target/GameObject", fileName = "Target_")]
public class GameObjectTarget : TaskTarget
{
    [SerializeField]
    private GameObject value;

    public override object Value => value;


    // value�� �� GameObject�� prefab�ε� 
    // isEqual�� ���ڷ� ������ ���� �������ϼ��� �ְ� ���Ӿ��� �����ϴ� ������Ʈ�ϼ��� ����
    // Equal ����� ���� ������Ʈ���� prefab ������ false ��ȯ�Ҽ��� ����
    public override bool IsEqual(object target)
    {
        var targetAsGameObject = target as GameObject;
        if (targetAsGameObject == null)
            return false;

        return targetAsGameObject.name.Contains(value.name);
    }
}
