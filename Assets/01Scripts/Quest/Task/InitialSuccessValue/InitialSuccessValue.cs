using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Level40 ������ �Ѵٰų�, �� ������ ���� ������ �Ǿ���ϴ�
/// Task�� �������ִ� ��
/// </summary>
public abstract class InitialSuccessValue : ScriptableObject
{
    public abstract int GetValue(Task task);
}
