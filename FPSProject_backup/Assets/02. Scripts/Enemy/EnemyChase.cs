using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyChase : MonoBehaviour
{
    [Header("����")]
    [SerializeField] private EnemySight sight;  // �þ� ��ũ��Ʈ ����
    [SerializeField] private Transform player;

    [Header("�̵� ����")]
    [SerializeField] private float chaseSpeed = 3.5f;
    [SerializeField] private float stopDistance = 4.0f;

    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        // Player �ڵ� Ž��
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (sight == null || player == null) return;

        if (sight.canSeePlayer)
        {
            // �߰� ��
            float dist = Vector3.Distance(transform.position, player.position);

            if (dist > stopDistance)
            {
                agent.isStopped = false;
                agent.speed = chaseSpeed;
                agent.SetDestination(player.position);
            }
            else
            {
                // �ʹ� ������ ���� (��ݿ� �Ÿ� Ȯ��)
                agent.isStopped = true;
            }
        }
        else
        {
            // �þ߿��� ������� ����
            agent.isStopped = true;
        }
    }
}
