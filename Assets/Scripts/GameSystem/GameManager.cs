using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState//게임의 상태를 나타내는 Enum
{
    Start,//게임 시작 UI나오는 상태
    CharacterSelection,//게임에서 사용할 캐릭터를 정하는 상태
    Battle,//게임을 플레이하는 상태(이때에만 mPerGameTimer가 작동함)
    Waiting,//Waiting상태, 플레이어의 input을 받지않고 게임상태를 처리하는 과정(점수 애니메이션이라던가)
    SkillSelection,//Skill을 고르는 상태
    End//승부가 결정되어 최종 화면을 보여주는 상태
}


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private int mNumMaxGame = 5;//?판 
    [SerializeField] private int mNumToWinGame = 3;//?선승제
    [SerializeField] private double mPerGamePlayTime = 10.0;//게임당 ??초
    [SerializeField] private GameObject mStartUI;//시작화면 UI레퍼런스
    [SerializeField] private GameObject mCharacterSelectionUI;//캐릭터 선택UI Prefab의 레퍼런스
    [SerializeField] private GameObject mWaitingUI;//WaitingUI Prefab의 레퍼런스
    [SerializeField] private GameObject mBattleUI;//BattleUI Prefab의 레퍼런스
    [SerializeField] private GameObject mSkillSelectionUI;//SkillSelectionUI Prefab의 레퍼런스
    [SerializeField] private GameObject mCooldownUI;// 스킬 쿨타임 표시 UI
    [SerializeField] private GameObject mEndUI;//종료화면 UI레퍼런스
    [SerializeField] private GameObject mFeverUI;//피버 UI레퍼런스
    [SerializeField] private PlatformMaker mPlatformMaker;//PlatformMaker의 레퍼런스
    [SerializeField] private GameObject[] mPlayerPrefabs;//플레이어 게임오브젝트 프리팹 레퍼런스들
    [SerializeField] private Vector3 mPlayer1InitPosition;//플레이어1 시작위치
    [SerializeField] private Vector3 mPlayer2InitPosition;//플레이어2 시작위치
    [SerializeField] private GameObject mStartWall; // 시작할때 생성되는 벽 prefab의 레퍼런스
    [SerializeField] private float mWallTime;//벽이 내려가는 시간
    [SerializeField] private GameObject mInvisibleWall; // 시작할때 생성되는 투명벽 prefab의 레퍼런스
    [SerializeField] private AudioClip mMainBGM;
    [SerializeField] private AudioClip mFeverBGM;
    

    private int mPlayer1Score=0;//플레이어1 점수
    private int mPlayer2Score=0;//플레이어2 점수
    private int mNumPlayer1Win = 0;//플레이어1 이긴횟수
    private int mNumPlayer2Win = 0;//플레이어2 이긴횟수
    private double mPerGameTimer = 0.0;//각 게임에 사용되는 타이머 변수(각 게임이 시작될때마다 mPerGamePlayTime으로 초기화됨)
    private GameState mGameState = GameState.Start;//게임의 상태
    private SkillHolder mPlayer1SkillHolder;//플레이어1 스킬홀더
    private SkillHolder mPlayer2SkillHolder;//플레이어2 스킬홀더

    public KeyCode mPlayer1ActiveSkillKey; // 플레이어1 액티브스킬 키
    public KeyCode mPlayer2ActiveSKillKey; // 플레이어2 액티브스킬 키

    private GameObject player1;
    private GameObject player2;
    private GameObject mStartWall1;
    private GameObject mStartWall2;
    private GameObject mInvisibleWall1;
    private GameObject mInvisibleWall2;

    private int mOccupyCount1 = 0;
    private int mOccupyCount2 = 0;

    private AudioSource mAudioSource;

    private bool bFeverAudioPlay;


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
    public bool IsWallTime()//벽이 존재하면 true, 다 내려가면 false
    {
        return mPerGameTimer > mPerGamePlayTime - mWallTime;
    }
    public bool IsGameEnd()//mNumToWinGame보다 많이 승리한 플레이어가 있는가?
    {
        return mNumPlayer1Win >= mNumToWinGame || mNumPlayer2Win >= mNumToWinGame;
    }
    public void CharacterSelectionAgain()//나머지 플레이어 선택
    {
        if (mGameState != GameState.CharacterSelection)
        {
            Debug.LogError("잘못된 상태에서 실행");
            return;
        }
        mCharacterSelectionUI.SetActive(false);
        mCharacterSelectionUI.SetActive(true);//껏다가 키면서 OnEnable함수 실행
    }
    public void ChangeStateStartToCharacterSelection()//초기 UI에서 게임시작버튼 클릭시 실행되는 함수
    {
        if (mGameState != GameState.Start)
        {
            Debug.LogError("잘못된 상태변경시도");
            return;
        }
        mGameState = GameState.CharacterSelection;
        mStartUI.SetActive(false);
        mCharacterSelectionUI.SetActive(true);
        
    }



    public void ChangeStateCharacterSelectionToBattle(int player1Index,int player2Index)//Character선택 상태가 끝나고 Battle상태로 변경하는 코드
    {
        if (mGameState != GameState.CharacterSelection)
        {
            Debug.LogError("잘못된 상태변경시도");
            return;
        }
        mCooldownUI.gameObject.SetActive(true);

        {//플레이어1 생성
            player1 = Instantiate(mPlayerPrefabs[player1Index], mPlayer1InitPosition, Quaternion.identity);
            player1.GetComponent<Player>().SetPlayerNumber(1);
            
            player1.transform.Rotate(new Vector3(0, 90, 0));
            player1.tag = "Player1";
            mPlayer1SkillHolder = player1.GetComponent<SkillHolder>();//스킬홀더 레퍼런스 가져오기
            mPlayer1SkillHolder.mActiveSkillKey = mPlayer1ActiveSkillKey; // 플레이어1 스킬 키 지정
            mPlayer1SkillHolder.animalType = player1Index; // 동물 타입 지정
            player1.transform.GetChild(0).GetChild(0).gameObject.SetActive(true); // 1P text 띄우기
            mCooldownUI.transform.GetChild(0).GetChild(player1Index).gameObject.SetActive(true); // 쿨타임 UI 캐릭터 표시
            mPlayer1SkillHolder.mCooldownUI = mCooldownUI.transform.GetChild(0).GetChild(player1Index);
        }
        {//플레이어2 생성
            player2 = Instantiate(mPlayerPrefabs[player2Index], mPlayer2InitPosition, Quaternion.identity);
            player2.GetComponent<Player>().SetPlayerNumber(2);

            player2.transform.Rotate(new Vector3(0, -90, 0));
            player2.tag = "Player2";
            mPlayer2SkillHolder = player2.GetComponent<SkillHolder>();//스킬홀더 레퍼런스 가져오기
            mPlayer2SkillHolder.mActiveSkillKey = mPlayer2ActiveSKillKey; // 플레이어2 스킬 키 지정
            mPlayer2SkillHolder.animalType = player2Index;
            player2.transform.GetChild(0).GetChild(1).gameObject.SetActive(true); // 2P text 띄우기
            mCooldownUI.transform.GetChild(1).GetChild(player2Index).gameObject.SetActive(true); // 쿨타임 UI 캐릭터 표시
            mPlayer2SkillHolder.mCooldownUI = mCooldownUI.transform.GetChild(1).GetChild(player2Index);
        }
        {//초반 벽 생성
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

    public void ChangeStateBattleToWaiting()//배틀상태가 끝나고 Waiting상태로 전환
    {
        if (mGameState != GameState.Battle)
        {
            Debug.LogError("잘못된 상태변경시도");
            return;
        }
        mPerGameTimer = mPerGamePlayTime;//시간초기화
        mGameState = GameState.Waiting;//Waiting상태로 변경
        JudgeWinner();//승자 판정(승자의 승count 1늘리기)
        mBattleUI.SetActive(false);
        mWaitingUI.SetActive(true);
        {//양 플레이어의 패시브 스킬로 인해 생겨난 부산물 삭제
            mPlayer1SkillHolder.DestroyPassiveObject();
            mPlayer2SkillHolder.DestroyPassiveObject();
        }


    }

    public void ChangeStateWaitingToSkillSelection()//waiting상태가 끝나고 SkillSelection으로 상태를 변경하는 코드
    {
        if (mGameState != GameState.Waiting)
        {
            Debug.LogError("잘못된 상태변경시도");
            return;
        }
        mGameState = GameState.SkillSelection;
        mSkillSelectionUI.SetActive(true);
        mWaitingUI.SetActive(false);
    }
    public void ChangeStateWaitingToEnd()//waiting상태가 끝나고 End로 상태를 변경하는 코드
    {
        if (mGameState != GameState.Waiting)
        {
            Debug.LogError("잘못된 상태변경시도");
            return;
        }
        mGameState = GameState.End;
        mEndUI.SetActive(true);
        mWaitingUI.SetActive(false);
    }

    public void SkillSelectionAgain()//무승부인 경우 다시 스킬선택 진행
    {
        if (mGameState != GameState.SkillSelection)
        {
            Debug.LogError("잘못된 상태에서 실행");
            return;
        }
        mSkillSelectionUI.SetActive(false);
        mSkillSelectionUI.SetActive(true);//껏다가 키면서 OnEnable함수 실행
    }
    public void ChangeStateSkillSelectionToBattle()//스킬선택 상태가 끝나고 Battle상태로 상태를 변경하는 코드
    {
        if (mGameState != GameState.SkillSelection)
        {
            Debug.LogError("잘못된 상태변경시도");
            return;
        }
        mGameState = GameState.Battle;
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
        mPlatformMaker.ClearAllBoard();
        ResetTimer();
        ResetScore();
        mBattleUI.SetActive(true);
        mSkillSelectionUI.SetActive(false);
        {//양 플레이어의 패시브 스킬 적용
            mPlayer1SkillHolder.PassiveAdjust();
            mPlayer2SkillHolder.PassiveAdjust();
        }
        mPlatformMaker.ClearAllBoard();

        mAudioSource.Stop();
        mAudioSource.clip = mMainBGM;
        mAudioSource.Play();
    }
    public void ChangeStateEndToStart()//마무리 UI에서 시작화면 버튼 눌렀을때 실행되는 함수
    {
        if (mGameState != GameState.End)
        {
            Debug.LogError("잘못된 상태변경시도");
            return;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

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
        );//캐릭터 선택키=액티브선택키 동기화작업
        mSkillSelectionUI.GetComponent<SkillSelectionUI>().SetPlayerKeyCode(
            mPlayer1ActiveSkillKey,
            mPlayer2ActiveSKillKey
        );//스킬 선택키=액티브선택키 동기화작업

        mAudioSource = GetComponent<AudioSource>();
        mAudioSource.clip = mMainBGM;
        mAudioSource.Play();
    }
    private void StartFiniteStateMachine()
    {
        mStartUI.SetActive(true);//시작 UI를 활성화하며 상태머신 시작
    }
    private void Update()
    {
        if (mGameState == GameState.Battle)
        {
            mPerGameTimer -= Time.deltaTime;
            FinishCheckPerEachGame();
        }

        if (mAudioSource.clip.name == mMainBGM.name && mPerGameTimer <= 15) // 15초 이하(피버타임)
        {
            mAudioSource.Stop();
            mAudioSource.clip = mFeverBGM;
            mAudioSource.Play();
        }


        if(mPerGameTimer <= 15 && mPerGameTimer >= 13)
        {
            mFeverUI.SetActive(true);
        }
        else
        {
            mFeverUI.SetActive(false);
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
        if (mPlayer1Score > mPlayer2Score)//player1우세
        {
            mNumPlayer1Win++;
        }
        else if (mPlayer2Score > mPlayer1Score)//player2우세
        {
            mNumPlayer2Win++;
        }
        else//동점
        {
            mNumPlayer1Win++;
            mNumPlayer2Win++;
        }
    }
}
