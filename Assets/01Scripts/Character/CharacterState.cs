public abstract class CharacterState<T> where T : class
{
    /// <summary>
    /// �ش� ���¸� ������ �� 1ȸ ȣ��
    /// </summary>
    /// <param name="sender"></param>
    public abstract void Enter(T sender);


    /// <summary>
    /// �ش� ���¸� ������Ʈ�� �� �� ������ ȣ��
    /// </summary>
    /// <param name="sender"></param>
    public abstract void Execute(T sender);


    /// <summary>
    /// �ش� ���¸� ������ �� 1ȸ ȣ��
    /// </summary>
    /// <param name="sender"></param>
    public abstract void Exit(T sender);
}