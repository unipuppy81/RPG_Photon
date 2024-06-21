using UnityEngine;
using Photon.Pun;
using CharacterOwnedStates;

public class StateMachine<T> where T : Character_Warrior // class
{
    public T ownerEntity; // stateMachine�� ������
    private CharacterState<T> currentState; // ���� ���¸� ���� ������Ƽ
    private CharacterState<T> previousState; // ���� ���¸� ���� ������Ƽ


    // FSM���� ����� ������
    public float m_turnSpeed = 200;
    public float currentSpeed { get; private set; }
    //�⺻ ���¸� �����ÿ� �����ϰ� ������ ����
    [PunRPC]
    public void StateMachineSetup(int ownerPhotonViewId, int stateId)
    {
        PhotonView targetView = PhotonView.Find(ownerPhotonViewId);
        if (targetView == null)
        {
            Debug.LogError("PhotonView not found");
            return;
        }

        T owner = targetView.GetComponent<T>();
        if (owner == null)
        {
            Debug.LogError("Component of type T not found on the target view");
            return;
        }

        // WarriorStates ���������� ���� ��ȯ
        State entryState = (State)stateId;

        // ��ü���� ���� Ŭ���� ����
        CharacterState<T> state = CreateState(entryState);

        SetupStateMachine(owner, state);
    }

    private CharacterState<T> CreateState(State state)
    {
        switch (state)
        {
            case State.Idle:
                return new WarriorIdle() as CharacterState<T>;
            case State.Walk:
                return new WarriorWalk() as CharacterState<T>;
            case State.Run:
                return new WarriorRun() as CharacterState<T>;
            case State.Warrior_Skill:
                return new WarriorAttack() as CharacterState<T>;
            case State.Die:
                return new WarriorDie() as CharacterState<T>;

            // �ٸ� ���µ� �߰�
            default:
                return null;
        }
    }

    public void SetupStateMachine(T owner, CharacterState<T> entryState)
    {
        ownerEntity = owner;
        currentState = null;
        previousState = null;

        // entryState ���·� ���� ����
        ChangeState(owner, entryState);
    }

    public void Execute()
    {
        if (currentState != null)
        {
            currentState.Execute(ownerEntity);
        }
    }
    public void ChangeState(T state, CharacterState<T> newState)
    {
        ownerEntity = state;

        // ���� �ٲٷ��� ���°� ������ ���¸� �ٲ��� �ʴ´�
        if (newState == null) return;



        // ���� ������� ���°� ������ Exit() �޼��� ȣ��
        if (currentState != null)
        {
            // ���°� ����Ǹ� ���� ���´� ���� ���°� �Ǳ� ������ previousState�� ����
            previousState = currentState;
            currentState.Exit(ownerEntity);
        }

        // ���ο� ���·� �����ϰ� ���� �ٲ� ������ enter �޼��� ȣ��
        currentState = newState;
        currentState.Enter(ownerEntity);

    }

    public void RevertToPreviousState()
    {
        //ChangeState(previousState);
    }
}
