using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    [Header("�þ� ����")]
    [SerializeField] private float viewAngle = 60f;       // �þ߰�
    [SerializeField] private float viewDistance = 20f;    // �þ� �Ÿ�
    [SerializeField] private LayerMask targetMask;        // �÷��̾� ���̾�
    [SerializeField] private LayerMask groundMask;      // �� �� ��ֹ� ���̾�

    [Header("�����")]
    public bool canSeePlayer;
    private Transform player;

    void Start()
    {
        // �÷��̾� ã�� (�±׷� Ž��)
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

        // �Ÿ� �˻�
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > viewDistance)
            return;

        // �þ߰� �˻�
        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        if (angle > viewAngle * 0.5f)
            return;

        // Raycast�� �þ� ���� �˻�
        if (!Physics.Raycast(transform.position + Vector3.up * 1.5f, dirToPlayer, distanceToPlayer, groundMask))
        {
            canSeePlayer = true;
        }
    }

    // Scene �信�� �þ� �ð�ȭ
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
