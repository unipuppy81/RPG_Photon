using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using EnemyState;



public class Enemy : Monster
{
    public enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Die
    }

    [Header("Animator")]
    public Animator _animator;
    public ParticleSystem deathEffect;

    [Header("FSM")]
    public EnemyState _curState;
    private FSMEnemy _fsmEnemy;

    [Header("Photon")]
    public PhotonView enemyPhotonview;
    public int photonviewID;

    [Header("Enemy Stats")]
    public float curhealth; // ���� ü��
    public float maxHealth; // �ִ� ü��

    private float healthRegenInterval = 1f; // ü�� ȸ�� ���� (1��)
    private float lastHealthRegenTime = 1000; // ������ ü�� ȸ�� �ð�

    public float Atk;
    public float Def;

    public float moveSpeed;
    public float healMount;

    public float detectionRange = 5f;
    public float chaseRange = 7f;
    public LayerMask targetLayer;
    public Transform target;

    private Vector3 spawnPosition; // �ʱ� ���� ��ġ
    private bool isReturn;


    private void Start()
    {
        _animator = GetComponent<Animator>();
        _curState = EnemyState.Idle;
        _fsmEnemy = new FSMEnemy(new IdleState(this));



        isReturn = false;

        spawnPosition = transform.position; // ���� ��ġ ����
    }

    private void Update()
    {
        switch (_curState)
        {
            case EnemyState.Idle:
                if (DetectTargets())
                {
                    if (AttackPlayer())
                        ChangeState(EnemyState.Attack);
                    else
                        ChangeState(EnemyState.Move);
                }
                break;

            case EnemyState.Move:
                if (DetectTargets())
                {
                    if (AttackPlayer())
                        ChangeState(EnemyState.Attack);
                }
                else
                    ChangeState(EnemyState.Idle);

                break;

            case EnemyState.Attack:
                if (DetectTargets())
                {
                    if (!AttackPlayer())
                    {
                        ChangeState(EnemyState.Move);
                    }
                }
                else
                    ChangeState(EnemyState.Idle);

                break;

            case EnemyState.Return:
                ReturnToSpawn();

                if (spawnPosition.x == transform.position.x && spawnPosition.z == transform.position.z && Mathf.Approximately(spawnPosition.z, transform.position.z))
                    ChangeState(EnemyState.Idle);

                break;

            case EnemyState.Die:
                if (isDie())
                    ChangeState(EnemyState.Die);

                break;
        }

        _fsmEnemy.UpdateState();
    }

    private void OnEnable()
    {
        SetStatsEnemy();
        //photonView.RPC("SetStatsEnemy", RpcTarget.All);
    }
    void OnEnabled()
    {
        enemyPhotonview = GetComponent<PhotonView>();
        photonviewID = enemyPhotonview.ViewID;
    }


    private void ChangeState(EnemyState nextState)
    {
        _curState = nextState;

        switch (_curState)
        {
            case EnemyState.Idle:
                _fsmEnemy.ChangeState(new IdleState(this));
                break;

            case EnemyState.Move:
                _fsmEnemy.ChangeState(new MoveState(this));
                break;

            case EnemyState.Attack:
                _fsmEnemy.ChangeState(new AttackState(this));
                break;

            case EnemyState.Return:
                _fsmEnemy.ChangeState(new ReturnState(this));
                break;

            case EnemyState.Die:
                _fsmEnemy.ChangeState(new DieState(this));
                break;
        }
    }

    [PunRPC]
    public void SetStatsEnemy() // ü��, ���ݷ�, ���, �̵��ӵ� + 
    {
        for (int i = 0; i < StatsDBManager.instance.statsDB.Enemy.Count; ++i)
        {
            if (StatsDBManager.instance.statsDB.Enemy[i].Name == "Ghost")
            {
                maxHealth = StatsDBManager.instance.statsDB.Enemy[i].Maxhp;
                Atk = StatsDBManager.instance.statsDB.Enemy[i].Atk;
                Def = StatsDBManager.instance.statsDB.Enemy[i].Def;
                moveSpeed = StatsDBManager.instance.statsDB.Enemy[i].Speed;
                healMount = StatsDBManager.instance.statsDB.Enemy[i].Healamount;
                detectionRange = StatsDBManager.instance.statsDB.Enemy[i].Radius;
                return;
            }
        }
    }


    /// <summary>
    /// �÷��̾� ����
    /// </summary>
    /// <returns></returns>
    public bool DetectTargets()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange, targetLayer);
        if (hits.Length > 0)
        {
            // ������ ù ��° ���� Ÿ������ ����
            target = hits[0].transform;
            return true;
        }
        target = null;
        return false;
    }

    /// <summary>
    /// �÷��̾� ������ �̵�
    /// </summary>
    public void MoveTowardsPlayer()
    {
        Character_Warrior cw = target.GetComponent<Character_Warrior>();

        if (cw.isSafe)
        {
            ChangeState(EnemyState.Return);
            target = null;
        }


        if (target == null) return;
        
        // �÷��̾ ���� �̵��ϴ� ����
        Vector3 direction = (target.position - transform.position).normalized;

        // ȸ�� ����
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // ȸ�� �ӵ� ���� (5f�� ������ ��)

        transform.position += direction * Time.deltaTime * moveSpeed;
    }

    /// <summary>
    /// �÷��̾� ����
    /// </summary>
    /// <returns></returns>
    public bool AttackPlayer()
    {
        if (target == null) return false;

        // �÷��̾ ������ �� �ִ� ���� (��: �����Ÿ� �ȿ� �ִ���)
        float attackRange = 2f; // ���÷� ���� �����Ÿ��� 2�� ����
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        return distanceToTarget <= attackRange;
    }

    public void PerformAttack()
    {
        if (target == null) return;

        // �÷��̾ �����ϴ� ����
    }

    /// <summary>
    /// Idle ���¿� ������ ü���� �ڵ����� ȸ��
    /// </summary>
    public void RegenerateHealth()
    {
        if (Time.time >= lastHealthRegenTime + healthRegenInterval)
        {
            curhealth += healMount; // ü�� 50 ȸ��
            curhealth = Mathf.Clamp(curhealth, 0, maxHealth); // �ִ� ü���� ���� �ʵ��� ����
            lastHealthRegenTime = Time.time; // ������ ü�� ȸ�� �ð� ����
        }
    }


    [PunRPC]
    public void TakeDamage(float attack)
    {
        float damage = CombatCalculator.CalculateDamage(attack, Def);
        curhealth -= damage;
        if (isDie())
        {
            ChangeState(EnemyState.Die);
        }
    }

    public void Die()
    {
        _animator.SetTrigger("isDie");

        Vector3 hitPoint = transform.position;
        hitPoint.y += 0.5f;

        Destroy(Instantiate(deathEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.up, hitPoint)), deathEffect.main.startLifetimeMultiplier);

    }

    // �ִϸ��̼� �̺�Ʈ�� ȣ��� �޼���
    public void DieComplete()
    {
        // ��ü �ı�
        Destroy(gameObject);

        // ���� ����ϴ°� ������ Ŭ���̾�Ʈ������ ó��
        SpawnManager.instance.RemoveGhost();

        if (PhotonNetwork.IsMasterClient)
        {
            SpawnManager.instance.CreateComputerPlayer();
        }
    }

    public bool isDie()
    {
        return curhealth <= 0;
    }

    public void ReturnToSpawn()
    {
        isReturn = true;
        Vector3 direction = (new Vector3(spawnPosition.x, transform.position.y, spawnPosition.z) - transform.position).normalized;
        transform.position += direction * Time.deltaTime * moveSpeed;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // y�� ȸ���� �����ϰ� ȸ��
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("IdlePoint") && isReturn)
        {
            ChangeState(EnemyState.Idle);
            isReturn = false;
        }
    }


}
