using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody playerRigid;
    [SerializeField] private float movespeed = 8.0f;

    private float horizontal;
    private float vertical;

    private void Awake()
    {
        playerRigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }
    private void FixedUpdate()
    {
        PlayerMove();
    }
    void PlayerMove()
    {
        Vector3 currentVel=playerRigid.velocity;
        Vector3 newVel = new Vector3(horizontal * movespeed, currentVel.y, vertical * movespeed);
        playerRigid.velocity = Vector3.Lerp(playerRigid.velocity, newVel, Time.deltaTime * 10.0f);
    }
}
