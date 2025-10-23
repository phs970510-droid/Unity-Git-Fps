using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("����")]
    [SerializeField] private float jumpForce = 7.0f;

    [Header("�ٴ� üũ")]
    public Transform groundCheck; // �߹� ��ġ�� ��Ÿ���� Ʈ������
    [SerializeField] private float groundCheckRadius = 0.3f;    //������ ������
    [SerializeField] private LayerMask groundLayer; // ���̾� ����

    //������ ������Ʈ��
    //private Animator anim;
    private Rigidbody rb;

    private bool isGrounded;
    private bool jumpRequested;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpRequested = true;
        }
    }
    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if (jumpRequested && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            jumpRequested = false;
        }
      
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
