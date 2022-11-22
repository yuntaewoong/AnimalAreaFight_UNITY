using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/*
 * 
 * 
 * 게임 상태가 SkillSelection일때의 UI처리
 * 
 * 
 */
public enum SelectingPlayer
{
    Player1,
    Player2
}

[System.Serializable]
public class PassiveSkillOptions
{
    public PassiveSkill[] map;
}


public class SkillSelectionUI : MonoBehaviour
{

    [SerializeField] private GameObject[] mBlueRectangles;
    [SerializeField] private GameObject[] mRedRectangles;
    [SerializeField] private PassiveSkillOptions[] mPassiveSkillSelectionOptions = new PassiveSkillOptions[3];//선택할수 있는 패시브 스킬 옵션
    [SerializeField] private TextMeshProUGUI[] mSkillTexts = new TextMeshProUGUI[3];//스킬선택창 텍스트레퍼런스
    [SerializeField] private TextMeshProUGUI mSkillInfoText;
    [TextArea]
    [SerializeField] private string[] mCharacterInfoTexts;

    private AudioSource mCursorSound;

    private int mSelectingIndex = 0;//선택중인 스킬 Index
    private bool mSelected = false;//이 UI가 Enable되어있는중에 선택입력이 입력되면 true
    private SelectingPlayer mSelectingPlayer = SelectingPlayer.Player1;

    private KeyCode player1_LeftKey = KeyCode.A;
    private KeyCode player1_RightKey = KeyCode.D;

    private KeyCode player2_LeftKey = KeyCode.LeftArrow;
    private KeyCode player2_RightKey = KeyCode.RightArrow;

    private KeyCode player1_SelectionKey;
    private KeyCode player2_SelectionKey;
    public void SetPlayerKeyCode(KeyCode player1SelectionCode, KeyCode player2SelectionCode)
    {
        player1_SelectionKey = player1SelectionCode;
        player2_SelectionKey = player2SelectionCode;
    }
    private void OnEnable()//평상시엔 UnEnable되어있다가 Waiting->SkillSelection으로 상태가 전이될때 Enable되게 됨
    {
        mSelected = false;
        mSelectingIndex = 0;
        SetSelecter();//조작하는 플레이어 세팅
        mCursorSound = GetComponent<AudioSource>();
    }
    private void SetSelecter()//스킬선택UI을 조작할 Player세팅
    {
        int player1Score = GameManager.instance.GetPlayer1Score();
        int player2Score = GameManager.instance.GetPlayer2Score();
        mSelectingPlayer = (player1Score <= player2Score) ? SelectingPlayer.Player1 : SelectingPlayer.Player2;
    }
    private void Update()
    {
        UpdateText();//UI텍스트 업데이트
        ManageSelectingInput();//인풋처리
        UpdateRectangle();//선택 사각형 업데이트
    }
    private void UpdateRectangle()
    {
        foreach (var rect in mBlueRectangles)
            rect.SetActive(false);
        foreach (var rect in mRedRectangles)
            rect.SetActive(false);
        switch (mSelectingPlayer)
        {
            case SelectingPlayer.Player1:
                mRedRectangles[mSelectingIndex].SetActive(true);
                break;
            case SelectingPlayer.Player2:
                mBlueRectangles[mSelectingIndex].SetActive(true);
                break;
        }
    }
    private void ManageSelectingInput()
    {
        if(mSelectingPlayer == SelectingPlayer.Player1)//플레이어1 조작처리
        {
            if (Input.GetKeyDown(player1_LeftKey))//왼쪽
            {
                mCursorSound.Play();
                mSelectingIndex = (mSelectingIndex > 0) ? mSelectingIndex - 1 : mSelectingIndex;
            }
            if (Input.GetKeyDown(player1_RightKey))//오른쪽
            {
                mCursorSound.Play();
                mSelectingIndex = (mSelectingIndex < 2) ? mSelectingIndex + 1 : mSelectingIndex;
            }
            if (Input.GetKeyDown(player1_SelectionKey))//스킬 선택
            {
                mCursorSound.Play();
                OnSelect();
            }
        }
        else//플레이어2조작처리
        {
            if (Input.GetKeyDown(player2_LeftKey))//왼쪽
            {
                mCursorSound.Play();
                mSelectingIndex = (mSelectingIndex > 0) ? mSelectingIndex - 1 : mSelectingIndex;
            }
            if (Input.GetKeyDown(player2_RightKey))//오른쪽
            {
                mCursorSound.Play();
                mSelectingIndex = (mSelectingIndex < 2) ? mSelectingIndex + 1 : mSelectingIndex;
            }
            if (Input.GetKeyDown(player2_SelectionKey))//스킬 선택
            {
                mCursorSound.Play();
                OnSelect();
            }
        }
    }
    private void OnSelect()//스킬이 선택되었을때 실행
    {
        if (mSelected)//선택버튼은 한번만 인식
            return;
        mSelected = true;

        switch(mSelectingPlayer)
        {
            case SelectingPlayer.Player1:
                SkillHolder player1SkillHolder = GameManager.instance.GetPlayer1SkillHolder();
                GameManager.instance.AddPassiveSkillToPlayer(
                    SelectingPlayer.Player1,
                    mPassiveSkillSelectionOptions[mSelectingIndex].map[player1SkillHolder.GetSkillLevel(mSelectingIndex)]
                );//패시브 스킬을 플레이어1에게 추가
                player1SkillHolder.AddSkillLevel(mSelectingIndex);

                break;
            case SelectingPlayer.Player2:
                SkillHolder player2SkillHolder = GameManager.instance.GetPlayer2SkillHolder();
                GameManager.instance.AddPassiveSkillToPlayer(
                    SelectingPlayer.Player2,
                    mPassiveSkillSelectionOptions[mSelectingIndex].map[player2SkillHolder.GetSkillLevel(mSelectingIndex)]
                );//패시브 스킬을 플레이어2에게 추가
                player2SkillHolder.AddSkillLevel(mSelectingIndex);
                break;
        }
        


        int player1Score = GameManager.instance.GetPlayer1Score();
        int player2Score = GameManager.instance.GetPlayer2Score();
        if (player1Score == player2Score)
        {
            GameManager.instance.AddPlayer1Score(1);//다음 선택상황에서 플레이어2가 뽑도록 점수를 1늘려줌
            GameManager.instance.SkillSelectionAgain();//동점인경우 상대도 스킬뽑게 진행
        }
        else
            GameManager.instance.ChangeStateSkillSelectionToBattle();//동점이 아닌경우 다음 전투 진행
    }
    private void UpdateText()
    {
        if(mSelectingPlayer == SelectingPlayer.Player1)
        {
            SkillHolder player1SkillHolder = GameManager.instance.GetPlayer1SkillHolder();
            for (int i = 0; i < 3; i++)
                mSkillTexts[i].SetText(mPassiveSkillSelectionOptions[i].map[player1SkillHolder.GetSkillLevel(i)].mName);
        }
        else if (mSelectingPlayer == SelectingPlayer.Player2)
        {
            SkillHolder player2SkillHolder = GameManager.instance.GetPlayer2SkillHolder();
            for (int i = 0; i < 3; i++)
                mSkillTexts[i].SetText(mPassiveSkillSelectionOptions[i].map[player2SkillHolder.GetSkillLevel(i)].mName);
        }
        mSkillInfoText.SetText(mCharacterInfoTexts[mSelectingIndex]);

    }
}
