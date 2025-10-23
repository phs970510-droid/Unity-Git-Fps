using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBase : MonoBehaviour
{
    [SerializeField] private EnemyData data;

    private NavMeshAgent agent;
    private Transform player;
    private int currentHp;
    private bool isDead = false;
    private bool canSeePlayer = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHp = data.baseHp;
    }

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag(data.playerTag);
            if (p != null) player = p.transform;
        }

        agent.speed = data.baseMoveSpeed;
    }

    void Update()
    {
        if (isDead || player == null) return;

        UpdateSight();
        HandleChase();
    }

    // -------------------------------
    // �þ� ����
    void UpdateSight()
    {
        canSeePlayer = false;

        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer > data.findRange)
            return;

        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        if (angle > data.fieldOfView * 0.5f)
            return;

        if (!Physics.Raycast(transform.position + Vector3.up * 1.5f, dirToPlayer, distToPlayer, data.obstacleMask))
        {
            canSeePlayer = true;
        }
    }

    // -------------------------------
    // �߰� / ���� �Ÿ� ����
    void HandleChase()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if (canSeePlayer)
        {
            if (dist > data.attackRange)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }
            else
            {
                agent.isStopped = true;

                if (data.isMelee)
                    AttackMelee();
                else if (data.isRanged)
                    AttackRanged();
            }
        }
        else
        {
            agent.isStopped = true;
        }
    }

    // -------------------------------
    // ���� ó��
    void AttackMelee()
    {
        // �ӽ�: ���� ������
        Debug.Log($"{data.enemyName} ���� ����! {data.meleeDamage} ����");
    }

    void AttackRanged()
    {
        // �ӽ�: �ѱ��� ����
        Debug.Log($"{data.enemyName} ���Ÿ� ����!");
    }

    // -------------------------------
    // ü�� ����
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHp -= amount;
        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        agent.isStopped = true;
        Debug.Log($"{data.enemyName} ���");
        gameObject.SetActive(false);
    }

    // -------------------------------
    // �þ� ����� ǥ��
    private void OnDrawGizmosSelected()
    {
        if (data == null) return;
        Gizmos.color = canSeePlayer ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, data.findRange);

        Vector3 leftDir = Quaternion.Euler(0, -data.fieldOfView / 2, 0) * transform.forward;
        Vector3 rightDir = Quaternion.Euler(0, data.fieldOfView / 2, 0) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + leftDir * data.findRange);
        Gizmos.DrawLine(transform.position, transform.position + rightDir * data.findRange);
    }
}