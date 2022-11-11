using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * 
 * 
 * 캐릭터 선택 UI
 * 
 */
public class CharacterSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject[] mBlueRectangles;
    [SerializeField] private GameObject[] mRedRectangles;
    [SerializeField] private Animator[] mSelectionCharacterAnimators;
    [SerializeField] private TextMeshProUGUI mCharacterInfoTextMesh;
    [TextArea]
    [SerializeField] private string[] mCharacterInfoTexts;

    private int mSelectingIndex = 0;//선택중인 캐릭터 Index
    private bool mSelected = false;//이 UI가 Enable되어있는중에 선택입력이 입력되면 true
    private SelectingPlayer mSelectingPlayer = SelectingPlayer.Player1;//player1먼저 선택
    private int mSelectedNum = 0;//선택완료한 Player수
    private int mPlayer1SelectIndex = 0;//player1이 선택한 플레이어인덱스
    private int mPlayer2SelectIndex = 0;//player2가 선택한 플레이어인덱스

    private KeyCode player1_LeftKey = KeyCode.A;
    private KeyCode player1_RightKey = KeyCode.D;
    private KeyCode player1_SelectionKey;

    private KeyCode player2_LeftKey = KeyCode.LeftArrow;
    private KeyCode player2_RightKey = KeyCode.RightArrow;
    private KeyCode player2_SelectionKey;
    public void SetPlayerKeyCode(KeyCode player1SelectionCode, KeyCode player2SelectionCode)
    {
        player1_SelectionKey = player1SelectionCode;
        player2_SelectionKey = player2SelectionCode;
    }

    private void OnEnable()
    {
        mSelected = false;
        mSelectingIndex = 0;
        SetSelecter();//조작하는 플레이어 세팅
    }
    private void SetSelecter()//캐릭터선택UI를 조작할 Player세팅
    {
        if (mSelectedNum == 0)
            mSelectingPlayer = SelectingPlayer.Player1;
        else if(mSelectedNum == 1)
            mSelectingPlayer = SelectingPlayer.Player2;
    }
    private void Update()
    {
        ManageSelectingInput();//인풋처리
        ChangeCharacterAnimation();//캐릭터선택창 모델링 애니메이션 업데이트
        UpdateCharacterInfo();//캐릭터 설명 업데이트
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
    private void ChangeCharacterAnimation()//평상시에는 idle이다가, 선택될시 run애니메이션 재생하도록
    {
        foreach (var animator in mSelectionCharacterAnimators)
            animator.SetBool("IsSelected", false);
        mSelectionCharacterAnimators[mSelectingIndex].SetBool("IsSelected", true);//선택중인 친구만 true
    }
    private void ManageSelectingInput()
    {
        if (mSelectingPlayer == SelectingPlayer.Player1)//플레이어1 조작처리
        {
            if (Input.GetKeyDown(player1_LeftKey))//왼쪽
                mSelectingIndex = (mSelectingIndex > 0) ? mSelectingIndex - 1 : mSelectingIndex;
            if (Input.GetKeyDown(player1_RightKey))//오른쪽
                mSelectingIndex = (mSelectingIndex < 2) ? mSelectingIndex + 1 : mSelectingIndex;
            if (Input.GetKeyDown(player1_SelectionKey))//스킬 선택
                OnSelect();
        }
        else//플레이어2조작처리
        {
            if (Input.GetKeyDown(player2_LeftKey))//왼쪽
                mSelectingIndex = (mSelectingIndex > 0) ? mSelectingIndex - 1 : mSelectingIndex;
            if (Input.GetKeyDown(player2_RightKey))//오른쪽
                mSelectingIndex = (mSelectingIndex < 2) ? mSelectingIndex + 1 : mSelectingIndex;
            if (Input.GetKeyDown(player2_SelectionKey))//스킬 선택
                OnSelect();
        }
    }
    private void OnSelect()//스킬이 선택되었을때 실행
    {
        if (mSelected)//선택버튼은 한번만 인식
            return;
        mSelected = true;
        mSelectedNum++;
        if (mSelectedNum == 1)//Player1이 처음 선택시
        {
            mPlayer1SelectIndex = mSelectingIndex;
            GameManager.instance.CharacterSelectionAgain();//플레이어2 선택시작
        }
        else//모든 플레이가 선택완료했을시
        {
            mPlayer2SelectIndex = mSelectingIndex;
            GameManager.instance.ChangeStateCharacterSelectionToBattle(
                mPlayer1SelectIndex,
                mPlayer2SelectIndex
            );//배틀시작
        }
    }
    private void UpdateCharacterInfo()//캐릭터 정보창 업데이트
    {
        mCharacterInfoTextMesh.SetText(mCharacterInfoTexts[mSelectingIndex]);
    }
}
