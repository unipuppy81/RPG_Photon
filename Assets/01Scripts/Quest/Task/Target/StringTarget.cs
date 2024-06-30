using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="Quest/Task/Target/String", fileName = "Target_")]
public class StringTarget : TaskTarget
{
    [SerializeField]
    private string value;

    public override object Value => value;

    // ���� ������ value�� Slime �̶�� ���ڿ��̰� 
    // ���� target�� Slime�̶�� ���ڿ��̶�� true�� ���ϵǾ� 
    // �� Task�� ��ǥ�� �ϴ� Target�� �´ٴ� ���� �˷��ִ� ��
    public override bool IsEqual(object target)
    {
        string targetAsString = target as string;
        if (targetAsString == null)
            return false;
        
        return value == targetAsString;
    }
}
