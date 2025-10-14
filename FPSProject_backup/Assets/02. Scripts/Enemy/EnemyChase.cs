using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyChase : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private EnemySight sight;  // 시야 스크립트 참조
    [SerializeField] private Transform player;

    [Header("이동 설정")]
    [SerializeField] private float chaseSpeed = 3.5f;
    [SerializeField] private float stopDistance = 4.0f;

    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        // Player 자동 탐색
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
            // 추격 중
            float dist = Vector3.Distance(transform.position, player.position);

            if (dist > stopDistance)
            {
                agent.isStopped = false;
                agent.speed = chaseSpeed;
                agent.SetDestination(player.position);
            }
            else
            {
                // 너무 가까우면 멈춤 (사격용 거리 확보)
                agent.isStopped = true;
            }
        }
        else
        {
            // 시야에서 사라지면 멈춤
            agent.isStopped = true;
        }
    }
}
