using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * 
 * 
 * ĳ���� ���� UI
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

    private int mSelectingIndex = 0;//�������� ĳ���� Index
    private bool mSelected = false;//�� UI�� Enable�Ǿ��ִ��߿� �����Է��� �ԷµǸ� true
    private SelectingPlayer mSelectingPlayer = SelectingPlayer.Player1;//player1���� ����
    private int mSelectedNum = 0;//���ÿϷ��� Player��
    private int mPlayer1SelectIndex = 0;//player1�� ������ �÷��̾��ε���
    private int mPlayer2SelectIndex = 0;//player2�� ������ �÷��̾��ε���

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
        SetSelecter();//�����ϴ� �÷��̾� ����
    }
    private void SetSelecter()//ĳ���ͼ���UI�� ������ Player����
    {
        if (mSelectedNum == 0)
            mSelectingPlayer = SelectingPlayer.Player1;
        else if(mSelectedNum == 1)
            mSelectingPlayer = SelectingPlayer.Player2;
    }
    private void Update()
    {
        ManageSelectingInput();//��ǲó��
        ChangeCharacterAnimation();//ĳ���ͼ���â �𵨸� �ִϸ��̼� ������Ʈ
        UpdateCharacterInfo();//ĳ���� ���� ������Ʈ
        UpdateRectangle();//���� �簢�� ������Ʈ
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
    private void ChangeCharacterAnimation()//���ÿ��� idle�̴ٰ�, ���õɽ� run�ִϸ��̼� ����ϵ���
    {
        foreach (var animator in mSelectionCharacterAnimators)
            animator.SetBool("IsSelected", false);
        mSelectionCharacterAnimators[mSelectingIndex].SetBool("IsSelected", true);//�������� ģ���� true
    }
    private void ManageSelectingInput()
    {
        if (mSelectingPlayer == SelectingPlayer.Player1)//�÷��̾�1 ����ó��
        {
            if (Input.GetKeyDown(player1_LeftKey))//����
                mSelectingIndex = (mSelectingIndex > 0) ? mSelectingIndex - 1 : mSelectingIndex;
            if (Input.GetKeyDown(player1_RightKey))//������
                mSelectingIndex = (mSelectingIndex < 2) ? mSelectingIndex + 1 : mSelectingIndex;
            if (Input.GetKeyDown(player1_SelectionKey))//��ų ����
                OnSelect();
        }
        else//�÷��̾�2����ó��
        {
            if (Input.GetKeyDown(player2_LeftKey))//����
                mSelectingIndex = (mSelectingIndex > 0) ? mSelectingIndex - 1 : mSelectingIndex;
            if (Input.GetKeyDown(player2_RightKey))//������
                mSelectingIndex = (mSelectingIndex < 2) ? mSelectingIndex + 1 : mSelectingIndex;
            if (Input.GetKeyDown(player2_SelectionKey))//��ų ����
                OnSelect();
        }
    }
    private void OnSelect()//��ų�� ���õǾ����� ����
    {
        if (mSelected)//���ù�ư�� �ѹ��� �ν�
            return;
        mSelected = true;
        mSelectedNum++;
        if (mSelectedNum == 1)//Player1�� ó�� ���ý�
        {
            mPlayer1SelectIndex = mSelectingIndex;
            GameManager.instance.CharacterSelectionAgain();//�÷��̾�2 ���ý���
        }
        else//��� �÷��̰� ���ÿϷ�������
        {
            mPlayer2SelectIndex = mSelectingIndex;
            GameManager.instance.ChangeStateCharacterSelectionToBattle(
                mPlayer1SelectIndex,
                mPlayer2SelectIndex
            );//��Ʋ����
        }
    }
    private void UpdateCharacterInfo()//ĳ���� ����â ������Ʈ
    {
        mCharacterInfoTextMesh.SetText(mCharacterInfoTexts[mSelectingIndex]);
    }
}
