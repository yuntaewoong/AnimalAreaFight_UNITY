using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/*
 * 
 * 
 * ���� ���°� SkillSelection�϶��� UIó��
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
    [SerializeField] private PassiveSkillOptions[] mPassiveSkillSelectionOptions = new PassiveSkillOptions[3];//�����Ҽ� �ִ� �нú� ��ų �ɼ�
    [SerializeField] private TextMeshProUGUI[] mSkillTexts = new TextMeshProUGUI[3];//��ų����â �ؽ�Ʈ���۷���
    [SerializeField] private TextMeshProUGUI mSkillInfoText;
    [TextArea]
    [SerializeField] private string[] mCharacterInfoTexts;

    private AudioSource mCursorSound;

    private int mSelectingIndex = 0;//�������� ��ų Index
    private bool mSelected = false;//�� UI�� Enable�Ǿ��ִ��߿� �����Է��� �ԷµǸ� true
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
    private void OnEnable()//���ÿ� UnEnable�Ǿ��ִٰ� Waiting->SkillSelection���� ���°� ���̵ɶ� Enable�ǰ� ��
    {
        mSelected = false;
        mSelectingIndex = 0;
        SetSelecter();//�����ϴ� �÷��̾� ����
        mCursorSound = GetComponent<AudioSource>();
    }
    private void SetSelecter()//��ų����UI�� ������ Player����
    {
        int player1Score = GameManager.instance.GetPlayer1Score();
        int player2Score = GameManager.instance.GetPlayer2Score();
        mSelectingPlayer = (player1Score <= player2Score) ? SelectingPlayer.Player1 : SelectingPlayer.Player2;
    }
    private void Update()
    {
        UpdateText();//UI�ؽ�Ʈ ������Ʈ
        ManageSelectingInput();//��ǲó��
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
    private void ManageSelectingInput()
    {
        if(mSelectingPlayer == SelectingPlayer.Player1)//�÷��̾�1 ����ó��
        {
            if (Input.GetKeyDown(player1_LeftKey))//����
            {
                mCursorSound.Play();
                mSelectingIndex = (mSelectingIndex > 0) ? mSelectingIndex - 1 : mSelectingIndex;
            }
            if (Input.GetKeyDown(player1_RightKey))//������
            {
                mCursorSound.Play();
                mSelectingIndex = (mSelectingIndex < 2) ? mSelectingIndex + 1 : mSelectingIndex;
            }
            if (Input.GetKeyDown(player1_SelectionKey))//��ų ����
            {
                mCursorSound.Play();
                OnSelect();
            }
        }
        else//�÷��̾�2����ó��
        {
            if (Input.GetKeyDown(player2_LeftKey))//����
            {
                mCursorSound.Play();
                mSelectingIndex = (mSelectingIndex > 0) ? mSelectingIndex - 1 : mSelectingIndex;
            }
            if (Input.GetKeyDown(player2_RightKey))//������
            {
                mCursorSound.Play();
                mSelectingIndex = (mSelectingIndex < 2) ? mSelectingIndex + 1 : mSelectingIndex;
            }
            if (Input.GetKeyDown(player2_SelectionKey))//��ų ����
            {
                mCursorSound.Play();
                OnSelect();
            }
        }
    }
    private void OnSelect()//��ų�� ���õǾ����� ����
    {
        if (mSelected)//���ù�ư�� �ѹ��� �ν�
            return;
        mSelected = true;

        switch(mSelectingPlayer)
        {
            case SelectingPlayer.Player1:
                SkillHolder player1SkillHolder = GameManager.instance.GetPlayer1SkillHolder();
                GameManager.instance.AddPassiveSkillToPlayer(
                    SelectingPlayer.Player1,
                    mPassiveSkillSelectionOptions[mSelectingIndex].map[player1SkillHolder.GetSkillLevel(mSelectingIndex)]
                );//�нú� ��ų�� �÷��̾�1���� �߰�
                player1SkillHolder.AddSkillLevel(mSelectingIndex);

                break;
            case SelectingPlayer.Player2:
                SkillHolder player2SkillHolder = GameManager.instance.GetPlayer2SkillHolder();
                GameManager.instance.AddPassiveSkillToPlayer(
                    SelectingPlayer.Player2,
                    mPassiveSkillSelectionOptions[mSelectingIndex].map[player2SkillHolder.GetSkillLevel(mSelectingIndex)]
                );//�нú� ��ų�� �÷��̾�2���� �߰�
                player2SkillHolder.AddSkillLevel(mSelectingIndex);
                break;
        }
        


        int player1Score = GameManager.instance.GetPlayer1Score();
        int player2Score = GameManager.instance.GetPlayer2Score();
        if (player1Score == player2Score)
        {
            GameManager.instance.AddPlayer1Score(1);//���� ���û�Ȳ���� �÷��̾�2�� �̵��� ������ 1�÷���
            GameManager.instance.SkillSelectionAgain();//�����ΰ�� ��뵵 ��ų�̰� ����
        }
        else
            GameManager.instance.ChangeStateSkillSelectionToBattle();//������ �ƴѰ�� ���� ���� ����
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
