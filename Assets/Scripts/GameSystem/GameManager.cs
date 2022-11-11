using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState//������ ���¸� ��Ÿ���� Enum
{
    CharacterSelection,//���ӿ��� ����� ĳ���͸� ���ϴ� ����
    Battle,//������ �÷����ϴ� ����(�̶����� mPerGameTimer�� �۵���)
    Waiting,//Waiting����, �÷��̾��� input�� �����ʰ� ���ӻ��¸� ó���ϴ� ����(���� �ִϸ��̼��̶����)
    SkillSelection,//Skill�� ���� ����
    End//�ºΰ� �����Ǿ� ���� ȭ���� �����ִ� ����
}


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private int mNumMaxGame = 5;//?�� 
    [SerializeField] private int mNumToWinGame = 3;//?������
    [SerializeField] private double mPerGamePlayTime = 10.0;//���Ӵ� ??��
    [SerializeField] private GameObject mCharacterSelectionUI;//ĳ���� ����UI Prefab�� ���۷���
    [SerializeField] private GameObject mWaitingUI;//WaitingUI Prefab�� ���۷���
    [SerializeField] private GameObject mBattleUI;//BattleUI Prefab�� ���۷���
    [SerializeField] private GameObject mSkillSelectionUI;//SkillSelectionUI Prefab�� ���۷���
    [SerializeField] private PlatformMaker mPlatformMaker;//PlatformMaker�� ���۷���
    [SerializeField] private GameObject[] mPlayerPrefabs;//�÷��̾� ���ӿ�����Ʈ ������ ���۷�����
    [SerializeField] private Vector3 mPlayer1InitPosition;//�÷��̾�1 ������ġ
    [SerializeField] private Vector3 mPlayer2InitPosition;//�÷��̾�2 ������ġ
    [SerializeField] private GameObject mStartWall; // �����Ҷ� �����Ǵ� �� prefab�� ���۷���
    [SerializeField] private float mWallTime;//���� �������� �ð�
    [SerializeField] private GameObject mInvisibleWall; // �����Ҷ� �����Ǵ� ���� prefab�� ���۷���

    private int mPlayer1Score=0;//�÷��̾�1 ����
    private int mPlayer2Score=0;//�÷��̾�2 ����
    private int mNumPlayer1Win = 0;//�÷��̾�1 �̱�Ƚ��
    private int mNumPlayer2Win = 0;//�÷��̾�2 �̱�Ƚ��
    private double mPerGameTimer = 0.0;//�� ���ӿ� ���Ǵ� Ÿ�̸� ����(�� ������ ���۵ɶ����� mPerGamePlayTime���� �ʱ�ȭ��)
    private GameState mGameState = GameState.CharacterSelection;//������ ����
    private SkillHolder mPlayer1SkillHolder;//�÷��̾�1 ��ųȦ��
    private SkillHolder mPlayer2SkillHolder;//�÷��̾�2 ��ųȦ��

    public KeyCode mPlayer1ActiveSkillKey; // �÷��̾�1 ��Ƽ�꽺ų Ű
    public KeyCode mPlayer2ActiveSKillKey; // �÷��̾�2 ��Ƽ�꽺ų Ű

    private GameObject player1;
    private GameObject player2;
    private GameObject mStartWall1;
    private GameObject mStartWall2;
    private GameObject mInvisibleWall1;
    private GameObject mInvisibleWall2;

    private int mOccupyCount1 = 0;
    private int mOccupyCount2 = 0;


    public GameObject GetPlayer(int player)
    {
        if (player == 1)
        {
            return player1;
        }
        else if (player == 2)
        {
            return player2;
        }
        else
            return null;
    }

    public void CheckTurret(int player)
    {
        if(player == 1)
        {
            if (player1.GetComponent<Player>().GetPassiveTurretLevel()!=0)
            {
                mOccupyCount1++;
                if (mOccupyCount1 == 5)
                {
                    mOccupyCount1 = 0;
                    player1.GetComponent<Player>().SpawnTurret();

                }
            }
        }
        else if (player == 2)
        {
            if (player2.GetComponent<Player>().GetPassiveTurretLevel() != 0)
            {
                mOccupyCount2++;
                if (mOccupyCount2 == 5)
                {
                    mOccupyCount2 = 0;
                    player2.GetComponent<Player>().SpawnTurret();
                }
            }
        }
    }
    public SkillHolder GetPlayer1SkillHolder()
    {
        return mPlayer1SkillHolder;
    }
    public SkillHolder GetPlayer2SkillHolder()
    {
        return mPlayer2SkillHolder;
    }
    public void AddPlayer1Score(int valueToAdd)
    {
        mPlayer1Score += valueToAdd;
    }
    public void AddPlayer2Score(int valueToAdd)
    {
        mPlayer2Score += valueToAdd;
    }

    public int GetPlayer1Score()
    {
        return mPlayer1Score;
    }
    public int GetPlayer2Score()
    {
        return mPlayer2Score;
    }
    public int GetPlayer1WinCount()
    {
        return mNumPlayer1Win;
    }
    public int GetPlayer2WinCount()
    {
        return mNumPlayer2Win;
    }
    public double GetRemainingTime()
    {
        return mPerGameTimer;
    }
    public GameState GetGameState()
    {
        return mGameState;
    }
    public bool IsWallTime()//���� �����ϸ� true, �� �������� false
    {
        return mPerGameTimer > mPerGamePlayTime - mWallTime;
    }

    public void CharacterSelectionAgain()//������ �÷��̾� ����
    {
        if (mGameState != GameState.CharacterSelection)
        {
            Debug.LogError("�߸��� ���¿��� ����");
            return;
        }
        mCharacterSelectionUI.SetActive(false);
        mCharacterSelectionUI.SetActive(true);//���ٰ� Ű�鼭 OnEnable�Լ� ����
    }
    public void ChangeStateCharacterSelectionToBattle(int player1Index,int player2Index)//Character���� ���°� ������ Battle���·� �����ϴ� �ڵ�
    {
        if (mGameState != GameState.CharacterSelection)
        {
            Debug.LogError("�߸��� ���º���õ�");
            return;
        }
        {//�÷��̾�1 ����
            player1 = Instantiate(mPlayerPrefabs[player1Index], mPlayer1InitPosition, Quaternion.identity);
            player1.GetComponent<Player>().SetPlayerNumber(1);
            
            player1.transform.Rotate(new Vector3(0, 90, 0));
            player1.tag = "Player1";
            mPlayer1SkillHolder = player1.GetComponent<SkillHolder>();//��ųȦ�� ���۷��� ��������
            mPlayer1SkillHolder.mActiveSkillKey = mPlayer1ActiveSkillKey; // �÷��̾�1 ��ų Ű ����
            mPlayer1SkillHolder.animalType = player1Index; // ���� Ÿ�� ����
        }
        {//�÷��̾�2 ����
            player2 = Instantiate(mPlayerPrefabs[player2Index], mPlayer2InitPosition, Quaternion.identity);
            player2.GetComponent<Player>().SetPlayerNumber(2);

            player2.transform.Rotate(new Vector3(0, -90, 0));
            player2.tag = "Player2";
            mPlayer2SkillHolder = player2.GetComponent<SkillHolder>();//��ųȦ�� ���۷��� ��������
            mPlayer2SkillHolder.mActiveSkillKey = mPlayer2ActiveSKillKey; // �÷��̾�2 ��ų Ű ����
            mPlayer2SkillHolder.animalType = player2Index;
        }
        {//�ʹ� �� ����
            mStartWall1 = Instantiate(mStartWall, mPlayer1InitPosition, Quaternion.identity);
            mStartWall1.GetComponent<StartWall>().SetWallTime(mWallTime);
            mStartWall2 = Instantiate(mStartWall, mPlayer2InitPosition, Quaternion.identity);
            mStartWall2.GetComponent<StartWall>().SetWallTime(mWallTime);
            mInvisibleWall1 = Instantiate(mInvisibleWall, mPlayer1InitPosition, Quaternion.identity);
            mInvisibleWall1.GetComponent<InvisibleWall>().SetWallTime(mWallTime);
            mInvisibleWall2 = Instantiate(mInvisibleWall, mPlayer2InitPosition, Quaternion.identity);
            mInvisibleWall2.GetComponent<InvisibleWall>().SetWallTime(mWallTime);
        }
        mCharacterSelectionUI.SetActive(false);
        mBattleUI.SetActive(true);
        mGameState = GameState.Battle;
    }



    public void ChangeStateWaitingToSkillSelection()//waiting���°� ������ SkillSelection���� ���¸� �����ϴ� �ڵ�
    {
        if (mGameState != GameState.Waiting)
        {
            Debug.LogError("�߸��� ���º���õ�");
            return;
        }
        mGameState = GameState.SkillSelection;
        mSkillSelectionUI.SetActive(true);
        mWaitingUI.SetActive(false);
    }
    public void SkillSelectionAgain()//���º��� ��� �ٽ� ��ų���� ����
    {
        if (mGameState != GameState.SkillSelection)
        {
            Debug.LogError("�߸��� ���¿��� ����");
            return;
        }
        mSkillSelectionUI.SetActive(false);
        mSkillSelectionUI.SetActive(true);//���ٰ� Ű�鼭 OnEnable�Լ� ����
    }
    public void ChangeStateSkillSelectionToBattle()//��ų���� ���°� ������ Battle���·� ���¸� �����ϴ� �ڵ�
    {
        if (mGameState != GameState.SkillSelection)
        {
            Debug.LogError("�߸��� ���º���õ�");
            return;
        }
        mGameState = GameState.Battle;
        mPlatformMaker.ClearAllBoard();
        ResetTimer();
        ResetScore();
        mBattleUI.SetActive(true);
        mSkillSelectionUI.SetActive(false);
        {//�� �÷��̾��� �нú� ��ų ����
            mPlayer1SkillHolder.PassiveAdjust();
            mPlayer2SkillHolder.PassiveAdjust();
        }
        player1.transform.position = mPlayer1InitPosition;
        player2.transform.position = mPlayer2InitPosition;
        player1.transform.Rotate(new Vector3(0, 90, 0));
        player2.transform.Rotate(new Vector3(0, -90, 0));
        {
            mStartWall1 = Instantiate(mStartWall, mPlayer1InitPosition, Quaternion.identity);
            mStartWall1.GetComponent<StartWall>().SetWallTime(mWallTime);
            mStartWall2 = Instantiate(mStartWall, mPlayer2InitPosition, Quaternion.identity);
            mStartWall2.GetComponent<StartWall>().SetWallTime(mWallTime);
            mInvisibleWall1 = Instantiate(mInvisibleWall, mPlayer1InitPosition, Quaternion.identity);
            mInvisibleWall1.GetComponent<InvisibleWall>().SetWallTime(mWallTime);
            mInvisibleWall2 = Instantiate(mInvisibleWall, mPlayer2InitPosition, Quaternion.identity);
            mInvisibleWall2.GetComponent<InvisibleWall>().SetWallTime(mWallTime);
        }
    }
    public void ChangeStateBattleToWaiting()//��Ʋ���°� ������ Waiting���·� ��ȯ
    {
        if (mGameState != GameState.Battle)
        {
            Debug.LogError("�߸��� ���º���õ�");
            return;
        }
        mPerGameTimer = mPerGamePlayTime;//�ð��ʱ�ȭ
        mGameState = GameState.Waiting;//Waiting���·� ����
        JudgeWinner();//���� ����(������ ��count 1�ø���)
        mBattleUI.SetActive(false);
        mWaitingUI.SetActive(true);
        {//�� �÷��̾��� �нú� ��ų�� ���� ���ܳ� �λ깰 ����
            mPlayer1SkillHolder.DestroyPassiveObject();
            mPlayer2SkillHolder.DestroyPassiveObject();
        }


    }
    public void AddPassiveSkillToPlayer(SelectingPlayer passiveSkillOwner, PassiveSkill passiveSkill)
    {
        switch (passiveSkillOwner)
        {
            case SelectingPlayer.Player1:
                mPlayer1SkillHolder.AddPassiveSkill(passiveSkill);
                break;
            case SelectingPlayer.Player2:
                mPlayer2SkillHolder.AddPassiveSkill(passiveSkill);
                break;
        }
    }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        StartFiniteStateMachine();
        ResetTimer();
        ResetScore();
        mCharacterSelectionUI.GetComponent<CharacterSelectionUI>().SetPlayerKeyCode(
            mPlayer1ActiveSkillKey,
            mPlayer2ActiveSKillKey
        );//ĳ���� ����Ű=��Ƽ�꼱��Ű ����ȭ�۾�
        mSkillSelectionUI.GetComponent<SkillSelectionUI>().SetPlayerKeyCode(
            mPlayer1ActiveSkillKey,
            mPlayer2ActiveSKillKey
        );//��ų ����Ű=��Ƽ�꼱��Ű ����ȭ�۾�
    }
    private void StartFiniteStateMachine()
    {
        mCharacterSelectionUI.SetActive(true);//ĳ���ͼ��� UI�� Ȱ��ȭ�ϸ� ���¸ӽ� ����
    }
    private void Update()
    {
        if (mGameState == GameState.Battle)
        {
            mPerGameTimer -= Time.deltaTime;
            FinishCheckPerEachGame();
        }
    }
    private void ResetTimer()
    {
        mPerGameTimer = mPerGamePlayTime;
    }
    private void ResetScore()
    {
        mPlayer1Score = 0;
        mPlayer2Score = 0;
        mOccupyCount1 = 0;
        mOccupyCount2 = 0;
    }
    private void FinishCheckPerEachGame()
    {
        if (mPerGameTimer <= 0.0)
        {
            ChangeStateBattleToWaiting();
        }
    }
    private void JudgeWinner()
    {
        if (mPlayer1Score > mPlayer2Score)//player1�켼
        {
            mNumPlayer1Win++;
        }
        else if (mPlayer2Score > mPlayer1Score)//player2�켼
        {
            mNumPlayer2Win++;
        }
        else//����
        {
            mNumPlayer1Win++;
            mNumPlayer2Win++;
        }
    }
}
