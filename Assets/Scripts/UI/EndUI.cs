using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mEndText;

    private void Update()
    {
        WinnerTextUpdate();
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.instance.ChangeStateEndToStart();
        }
    }
    private void WinnerTextUpdate()
    {
        int player1Score = GameManager.instance.GetPlayer1Score();
        int player2Score = GameManager.instance.GetPlayer2Score();
        if(player1Score > player2Score)
        {
            mEndText.color = Color.red;
            mEndText.text = "축하합니다. 1P가 이겼습니다";
        }
        else
        {
            mEndText.color = Color.blue;
            mEndText.text = "축하합니다. 2P가 이겼습니다";
        }
    }
}
