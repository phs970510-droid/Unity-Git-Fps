using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    [Header("시야 설정")]
    [SerializeField] private float viewAngle = 60f;       // 시야각
    [SerializeField] private float viewDistance = 20f;    // 시야 거리
    [SerializeField] private LayerMask targetMask;        // 플레이어 레이어
    [SerializeField] private LayerMask groundMask;      // 벽 등 장애물 레이어

    [Header("디버그")]
    public bool canSeePlayer;
    private Transform player;

    void Start()
    {
        // 플레이어 찾기 (태그로 탐색)
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
    }

    void Update()
    {
        if (player == null)
        {
            canSeePlayer = false;
            return;
        }

        CheckSight();
    }

    void CheckSight()
    {
        canSeePlayer = false;

        // 거리 검사
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > viewDistance)
            return;

        // 시야각 검사
        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        if (angle > viewAngle * 0.5f)
            return;

        // Raycast로 시야 차단 검사
        if (!Physics.Raycast(transform.position + Vector3.up * 1.5f, dirToPlayer, distanceToPlayer, groundMask))
        {
            canSeePlayer = true;
        }
    }

    // Scene 뷰에서 시야 시각화
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = canSeePlayer ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 leftDir = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward;
        Vector3 rightDir = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + leftDir * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + rightDir * viewDistance);
    }
}
