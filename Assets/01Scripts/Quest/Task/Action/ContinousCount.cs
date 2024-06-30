using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Action/ContinousCount", fileName = "Continous Count")]
public class ContinousCount : TaskAction
{
    /// <summary>
    /// ����� ������ ���� �� ���ϰ�, ������ ������ �ʱ�ȭ
    /// </summary>
    /// <param name="task"></param>
    /// <param name="currentSuccess"></param>
    /// <param name="successCount"></param>
    /// <returns></returns>
    public override int Run(Task task, int currentSuccess, int successCount)
    {
        return successCount > 0 ? currentSuccess + successCount : 0;
    }
}
