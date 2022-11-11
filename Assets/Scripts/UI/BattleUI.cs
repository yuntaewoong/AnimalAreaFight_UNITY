using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/*
 * 
 * 
 * Gane 상태가 Battle일때의 UI처리
 * 
 * 
 */
public class BattleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mTimeText;
    [SerializeField] private TextMeshProUGUI mPlayer1ScoreText;
    [SerializeField] private TextMeshProUGUI mPlayer2ScoreText;
    [SerializeField] private TextMeshProUGUI mPlayer1WincountText;
    [SerializeField] private TextMeshProUGUI mPlayer2WincountText;

    void Update()
    {
        TimeSpan remainingTime = TimeSpan.FromSeconds(GameManager.instance.GetRemainingTime());
        mTimeText.SetText(remainingTime.Minutes + ":" + remainingTime.Seconds);
        mPlayer1ScoreText.SetText(GameManager.instance.GetPlayer1Score().ToString());
        mPlayer2ScoreText.SetText(GameManager.instance.GetPlayer2Score().ToString());

        mPlayer1WincountText.SetText(GameManager.instance.GetPlayer1WinCount().ToString());
        mPlayer2WincountText.SetText(GameManager.instance.GetPlayer2WinCount().ToString());
    }
}
