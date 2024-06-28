using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public abstract class TaskTarget : ScriptableObject
{
    /// <summary>
    /// ���� Target�� �ڷ����� �� class�� ��ӹ޴� �ڽĿ��� ������ ���̹Ƿ� object Ÿ��
    /// </summary>
    public abstract object Value { get; }

    /// <summary>
    /// QuestSystem�� ����� Target�� Task�� ������ Target�� ������ Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public abstract bool isEqual(object target);
}
