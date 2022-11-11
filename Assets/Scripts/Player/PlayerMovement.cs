using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;                // 움직임 속도
    
    private float rotationSpeed = 100000f;      // 회전 속도

    private PlayerInput playerInput;
    private Rigidbody rb;
    private Animator anim;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // 물리 갱신 주기에 맞춰 실행
    void FixedUpdate()
    {
        Move();
        Rotate();
    }

    // 플레이어 이동
    private void Move()
    {
        rb.MovePosition(rb.position + playerInput.moveDirection * moveSpeed * Time.fixedDeltaTime);
        anim.SetBool("run", playerInput.moving);
    }

    // 플레이어 회전
    private void Rotate()
    {
        if (playerInput.moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(playerInput.moveDirection);

            targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            rb.MoveRotation(targetRotation);
        }
        else
        {
            rb.angularVelocity = new Vector3(0, 0, 0);
        }
    }
}
