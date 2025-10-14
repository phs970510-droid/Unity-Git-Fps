using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("점프")]
    [SerializeField] private float jumpForce = 7.0f;

    [Header("바닥 체크")]
    public Transform groundCheck; // 발밑 위치를 나타내는 트랜스폼
    [SerializeField] private float groundCheckRadius = 0.3f;    //감지용 반지름
    [SerializeField] private LayerMask groundLayer; // 레이어 설정

    //참조용 컴포넌트들
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
