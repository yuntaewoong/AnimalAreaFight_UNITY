using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{    
    // 플레이어 입력 값 저장

    // 움직임 입력 값
    private float moveVertical;
    private float moveHorizontal;

    private KeyCode player1_UpKey = KeyCode.W;
    private KeyCode player1_DownKey = KeyCode.S;
    private KeyCode player1_LeftKey = KeyCode.A;
    private KeyCode player1_RightKey = KeyCode.D;

    private KeyCode player2_UpKey = KeyCode.UpArrow;
    private KeyCode player2_DownKey = KeyCode.DownArrow;
    private KeyCode player2_LeftKey = KeyCode.LeftArrow;
    private KeyCode player2_RightKey = KeyCode.RightArrow;

    public Vector3 moveDirection { get; private set; }
    public bool moving { get; private set; }
    
    void Update()
    {
        if (GameManager.instance.GetGameState() != GameState.Battle)//배틀상태일때만 인풋을 받음
            return;
        if (gameObject.tag == "Player1")
        {
            moving = true;

            if (Input.GetKey(player1_UpKey))
                moveVertical = 1f;
            else if (Input.GetKey(player1_DownKey))
                moveVertical = -1f;

            if (!Input.GetKey(player1_UpKey) && !Input.GetKey(player1_DownKey))
                moveVertical = 0f;

            if (Input.GetKey(player1_RightKey))
                moveHorizontal = 1f;
            else if (Input.GetKey(player1_LeftKey))
                moveHorizontal = -1f;

            if (!Input.GetKey(player1_RightKey) && !Input.GetKey(player1_LeftKey))
                moveHorizontal = 0f;
            

            if (!Input.GetKey(player1_UpKey) && !Input.GetKey(player1_DownKey)
                 && !Input.GetKey(player1_RightKey) && !Input.GetKey(player1_LeftKey))
                moving = false;
                

        }

        if (gameObject.tag == "Player2")
        {
            moving = true;

            if (Input.GetKey(player2_UpKey))
                moveVertical = 1f;
            else if (Input.GetKey(player2_DownKey))
                moveVertical = -1f;

            if (!Input.GetKey(player2_UpKey) && !Input.GetKey(player2_DownKey))
                moveVertical = 0f;

            if (Input.GetKey(player2_RightKey))
                moveHorizontal = 1f;
            else if (Input.GetKey(player2_LeftKey))
                moveHorizontal = -1f;

            if (!Input.GetKey(player2_RightKey) && !Input.GetKey(player2_LeftKey))
                moveHorizontal = 0f;

            if (!Input.GetKey(player2_UpKey) && !Input.GetKey(player2_DownKey)
                && !Input.GetKey(player2_RightKey) && !Input.GetKey(player2_LeftKey))
                moving = false;
        }

        moveDirection = new Vector3(moveHorizontal, 0, moveVertical);
        moveDirection.Normalize();
    }
}
