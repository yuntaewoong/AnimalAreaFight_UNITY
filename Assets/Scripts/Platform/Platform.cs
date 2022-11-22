using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayersOnBlock//���ǿ� �ö���ִ� Player��
{ 
    Nothing,
    Player1,
    Player2,
    BothPlayer
}

public class Platform : MonoBehaviour
{
    [SerializeField] private int mFeverMultiplyer = 10;
    [SerializeField] private float mFeverTime = 15.0f;
    [SerializeField] private float mColorTime=1.5f;
    [SerializeField] private float mPermPenaltyTime = 1.5f; //������ ���� �� ���
    [SerializeField] private GameObject mPlatform;
    [SerializeField] private PlatformMaker mPlatformMaker;
    [SerializeField] private float mFeverBingoPenalty = 6.0f;
    private Platform mTempPlatform;
    
    private float mPlayer1Point = 0f;
    private float mPlayer2Point = 0f;

    private PlayersOnBlock mPlayersOnBlock = PlayersOnBlock.Nothing;
    //���� �÷����� ���� �ö�� �ִ°�. 4���� ���: player1,player2,player1 �� player2,nothing 

    private bool mHasPlayer1 = false;
    private bool mHasPlayer2 = false;


    private int mPlatformX; //�÷��� ��ġ
    private int mPlatformZ;
    private int mPlatformSize;

    [SerializeField] private Color NeutralColor = new Color(0.0f, 0.0f, 0.0f);
    [SerializeField] private Color Temp1Color = new Color(0.8f, 0.3f, 0.0f);
    [SerializeField] private Color Temp2Color = new Color(0.0f, 0.3f, 0.8f);
    [SerializeField] private Color Perm1Color = new Color(1.0f, 0.0f, 0.0f);
    [SerializeField] private Color Perm2Color = new Color (0.0f, 0.0f, 1.0f);


    public void SetHasPlayer(bool bHasPlayer)
    {
        mHasPlayer1 = bHasPlayer;
        mHasPlayer2 = bHasPlayer;
    }

    private void PlayPlatformSound()
    {
        mPlatform.GetComponent<AudioSource>().Play();
    }

    private void  PlatformInitialization()
    {
        mPlatformX = int.Parse(mPlatform.name[0].ToString());
        mPlatformZ = int.Parse(mPlatform.name[2].ToString());
        mPlatformSize = mPlatformMaker.GetBingoMapSize();
    }

    private void PlayOccupyParticle(Color color)
    {
        ParticleSystem ps = mPlatform.GetComponentInChildren<ParticleSystem>();
        ParticleSystem.MainModule ma = ps.main;
        ma.startColor = color;
        ps.Play();
        PlayPlatformSound();
    }
    private void CheckPlayer()
    {
        
        if (mPlayersOnBlock == PlayersOnBlock.Player1)
        {// ���ǿ� �ö�� ����� �÷��̾�1
            if (mPlatformMaker.GetBingoMap(mPlatformX, mPlatformZ) == PlatformMaker.PlatformOwner.Neutral)
            //  ���� ���� ������ �߸��̸�
            {
                InteractWithPlayer1(0.2f);
            }
            else if (mPlatformMaker.GetBingoMap(mPlatformX, mPlatformZ) == PlatformMaker.PlatformOwner.Temp2)
            //���� ���� ������ �ӽ� �÷��̾�2
            {
                InteractWithPlayer1(1.0f);
            }
            else if (mPlatformMaker.GetBingoMap(mPlatformX, mPlatformZ) == PlatformMaker.PlatformOwner.Perm2)
            //  ���� ���� ������ ���� �÷��̾�2��
            {
                if (GameManager.instance.GetRemainingTime() < mFeverTime)
                {
                    InteractWithPlayer1(mFeverBingoPenalty);
                }
                else
                {
                    InteractWithPlayer1(mPermPenaltyTime);
                }
                    
               
            }
        }
        else if (mPlayersOnBlock == PlayersOnBlock.Player2)
        {// ���ǿ� �ö�� ����� �÷��̾�2
            if (mPlatformMaker.GetBingoMap(mPlatformX, mPlatformZ) == PlatformMaker.PlatformOwner.Neutral)
            //  ���� ���� ������ �߸��̸�
            {
                InteractWithPlayer2(0.2f);
            }
            else if (mPlatformMaker.GetBingoMap(mPlatformX, mPlatformZ) == PlatformMaker.PlatformOwner.Temp1)
            //  ���� ���� ������ �ӽ� �÷��̾�1
            {
                InteractWithPlayer2(1.0f);
            }
            else if (mPlatformMaker.GetBingoMap(mPlatformX, mPlatformZ) == PlatformMaker.PlatformOwner.Perm1)
            //  ���� ���� ������ ���� �÷��̾�1��
            {
                if (GameManager.instance.GetRemainingTime() < mFeverTime)
                {
                    InteractWithPlayer2(mFeverBingoPenalty);
                }
                else
                {
                    InteractWithPlayer2(mPermPenaltyTime);
                }
            }
        }
    }
    private bool CheckBingo(int player)
    {
        PlatformMaker.PlatformOwner case1 =  PlatformMaker.PlatformOwner.Neutral;
        PlatformMaker.PlatformOwner case2 =  PlatformMaker.PlatformOwner.Neutral;
        if (player == 1)
        {
            case1 = PlatformMaker.PlatformOwner.Temp1;
            case2 = PlatformMaker.PlatformOwner.Perm1;
        }
        else  if (player== 2)
        {
            case1 = PlatformMaker.PlatformOwner.Temp2;
            case2 = PlatformMaker.PlatformOwner.Perm2;
        }
        bool ReturnBingo = false;
        bool Bingo = true;

        for (int i =0; i<mPlatformSize; i++) //���� ���� Ȯ��
        {
            if (mPlatformMaker.GetBingoMap(mPlatformX, i)!= case1 &&
                mPlatformMaker.GetBingoMap(mPlatformX, i) != case2)
            {
                Bingo = false;
                break;
                
            }
            
        }
        if (Bingo) //���� ����!
        {
            for (int i = 0; i < mPlatformSize; i++)
            {
                mPlatformMaker.SetBingoMap(mPlatformX, i, case2);
                mTempPlatform = mPlatformMaker.GetPlatformList(mPlatformX, i).GetComponentInChildren<Platform>();

                mTempPlatform.ClearPlatform();
                if(player == 1)
                    mTempPlatform.PlayOccupyParticle(Perm1Color);
                else if (player == 2)
                    mTempPlatform.PlayOccupyParticle(Perm2Color);
            }
            
            ReturnBingo = true;
        }

        Bingo = true;
        for (int i = 0; i < mPlatformSize; i++) //���� ���� Ȯ��
        {
            if (mPlatformMaker.GetBingoMap(i, mPlatformZ) != case1 &&
                mPlatformMaker.GetBingoMap(i, mPlatformZ) != case2)
            {
                Bingo = false;
                break;
            }

        }
        if (Bingo) //���� ����!
        {
            for (int i = 0; i < mPlatformSize; i++)
            {
                mPlatformMaker.SetBingoMap(i, mPlatformZ, case2);
                mTempPlatform = mPlatformMaker.GetPlatformList(i, mPlatformZ).GetComponentInChildren<Platform>();
                mTempPlatform.ClearPlatform();
                if (player == 1)
                    mTempPlatform.PlayOccupyParticle(Perm1Color);
                else if (player == 2)
                    mTempPlatform.PlayOccupyParticle(Perm2Color);
            }
            
            ReturnBingo = true;
        }

        Bingo = true;
        if(mPlatformX == mPlatformZ) //������ ���� ���� �밢�� ���� Ȯ��.
        {
            for (int i = 0; i < mPlatformSize; i++)
            {
                if (mPlatformMaker.GetBingoMap(i, i) != case1 &&
                    mPlatformMaker.GetBingoMap(i, i) != case2)
                {
                    Bingo = false;
                    break;
                }
            }
            if (Bingo)
            {
                for (int i = 0; i < mPlatformSize; i++)
                {
                    mPlatformMaker.SetBingoMap(i, i, case2);
                    mTempPlatform = mPlatformMaker.GetPlatformList(i, i).GetComponentInChildren<Platform>();
                    mTempPlatform.ClearPlatform();
                    if (player == 1)
                        mTempPlatform.PlayOccupyParticle(Perm1Color);
                    else if (player == 2)
                        mTempPlatform.PlayOccupyParticle(Perm2Color);
                }
                
                ReturnBingo = true;
            }
        }
        Bingo = true;
        if (mPlatformX + mPlatformZ == mPlatformSize - 1) //���� ���� ���� �밢�� Ȯ��
        {
            for (int i = 0; i < mPlatformSize; i++)
            {
                if (mPlatformMaker.GetBingoMap(i, mPlatformSize-1-i) != case1 &&
                    mPlatformMaker.GetBingoMap(i, mPlatformSize-1-i) != case2)
                {
                    Bingo = false;
                    break;
                }
            }
            if (Bingo)
            {
                for (int i = 0; i < mPlatformSize; i++)
                {
                    mPlatformMaker.SetBingoMap(i, mPlatformSize - 1 - i, case2);
                    mTempPlatform = mPlatformMaker.GetPlatformList(i, mPlatformSize - 1 - i).GetComponentInChildren<Platform>();
                    mTempPlatform.ClearPlatform();
                    if (player == 1)
                        mTempPlatform.PlayOccupyParticle(Perm1Color);
                    else if (player == 2)
                        mTempPlatform.PlayOccupyParticle(Perm2Color);
                }
                
                ReturnBingo = true;
            }

        }

        return ReturnBingo;
    }

    private void InteractWithPlayer1(float penaltyTime)
    {
        if (GameManager.instance.GetGameState() != GameState.Battle)
            return;

        if (mPlayer1Point >= 1.0f)
        {
            if(mPlatformMaker.GetBingoMap(mPlatformX, mPlatformZ) == PlatformMaker.PlatformOwner.Neutral)
            {
                
                GameManager.instance.AddPlayer1Score(1);
                GameManager.instance.CheckTurret(1);

            }
            else if(mPlatformMaker.GetBingoMap(mPlatformX, mPlatformZ) == PlatformMaker.PlatformOwner.Temp2
                || mPlatformMaker.GetBingoMap(mPlatformX, mPlatformZ) == PlatformMaker.PlatformOwner.Perm2)
            {
                GameManager.instance.AddPlayer1Score(1);
                GameManager.instance.AddPlayer2Score(-1);
                GameManager.instance.CheckTurret(1);
            }
            mPlatformMaker.SetBingoMap(mPlatformX, mPlatformZ, PlatformMaker.PlatformOwner.Temp1);

            if (!CheckBingo(1))
                PlayOccupyParticle(Temp1Color);

            mPlayer1Point = 1.0f;
            return;
        }
        
        if(GameManager.instance.GetRemainingTime() < mFeverTime)
        {
            mPlayer1Point += Time.deltaTime  / (mColorTime * penaltyTime) *mFeverMultiplyer;
        }
        else
        {
            mPlayer1Point += Time.deltaTime / (mColorTime * penaltyTime) ;
        }
        
        
        if (mPlatformMaker.GetBingoMap(mPlatformX, mPlatformZ) == PlatformMaker.PlatformOwner.Neutral)//���� �߸� �÷����̸�
        {
            mPlatform.GetComponent<MeshRenderer>().material.color = Color.Lerp(NeutralColor, Temp1Color, mPlayer1Point);

        }
        else if (mPlatformMaker.GetBingoMap(mPlatformX, mPlatformZ) == PlatformMaker.PlatformOwner.Temp2)  //���� ��.
        {
            mPlatform.GetComponent<MeshRenderer>().material.color = Color.Lerp(Temp2Color, Temp1Color, mPlayer1Point);
        }
        else if (mPlatformMaker.GetBingoMap(mPlatformX, mPlatformZ) == PlatformMaker.PlatformOwner.Perm2)  //���� ��.
        {
            mPlatform.GetComponent<MeshRenderer>().material.color = Color.Lerp(Perm2Color, Temp1Color, mPlayer1Point);
        }
    }
    private void InteractWithPlayer2(float penaltyTime)
    {
        if (GameManager.instance.GetGameState() != GameState.Battle)
            return;
        if (mPlayer2Point >= 1.0f)
        {
            if (mPlatformMaker.GetBingoMap(mPlatformX, mPlatformZ) == PlatformMaker.PlatformOwner.Neutral)
            {
                GameManager.instance.AddPlayer2Score(1);
                GameManager.instance.CheckTurret(2);

            }
            else if (mPlatformMaker.GetBingoMap(mPlatformX, mPlatformZ) == PlatformMaker.PlatformOwner.Temp1
                || mPlatformMaker.GetBingoMap(mPlatformX, mPlatformZ) == PlatformMaker.PlatformOwner.Perm1)
            {
                GameManager.instance.AddPlayer1Score(-1);
                GameManager.instance.AddPlayer2Score(1);
                GameManager.instance.CheckTurret(2);
            }
            mPlatformMaker.SetBingoMap(mPlatformX, mPlatformZ, PlatformMaker.PlatformOwner.Temp2);
            
            if (!CheckBingo(2))
                PlayOccupyParticle(Temp2Color);

            mPlayer2Point = 1.0f;
            return;
        }
        if (GameManager.instance.GetRemainingTime() < mFeverTime)
        {
            mPlayer2Point += Time.deltaTime / (mColorTime * penaltyTime) * mFeverMultiplyer;
        }
        else
        {
            mPlayer2Point += Time.deltaTime / (mColorTime * penaltyTime);
        }



        if (mPlatformMaker.GetBingoMap(mPlatformX, mPlatformZ) == PlatformMaker.PlatformOwner.Neutral)//���� �߸� �÷����̸�
        {
            mPlatform.GetComponent<MeshRenderer>().material.color = Color.Lerp(NeutralColor, Temp2Color, mPlayer2Point);
        }
        else if (mPlatformMaker.GetBingoMap(mPlatformX, mPlatformZ) == PlatformMaker.PlatformOwner.Temp1)  //���� ��.
        {
            mPlatform.GetComponent<MeshRenderer>().material.color = Color.Lerp(Temp1Color, Temp2Color, mPlayer2Point);
        }
        else if (mPlatformMaker.GetBingoMap(mPlatformX, mPlatformZ) == PlatformMaker.PlatformOwner.Perm1)  //���� ��.
        {
            mPlatform.GetComponent<MeshRenderer>().material.color = Color.Lerp(Perm1Color, Temp2Color, mPlayer2Point);
        }

        return;
    }
    public void ClearPlatform()
    {
        
        switch (mPlatformMaker.GetBingoMap(mPlatformX, mPlatformZ))
        {
            case PlatformMaker.PlatformOwner.Neutral:
                mPlatform.GetComponent<MeshRenderer>().material.color = NeutralColor;
                break;
            case PlatformMaker.PlatformOwner.Temp1:
                mPlatform.GetComponent<MeshRenderer>().material.color = Temp1Color;
                break;
            case PlatformMaker.PlatformOwner.Perm1:
                mPlatform.GetComponent<MeshRenderer>().material.color = Perm1Color;
                break;
            case PlatformMaker.PlatformOwner.Temp2:
                mPlatform.GetComponent<MeshRenderer>().material.color = Temp2Color;
                break;
            case PlatformMaker.PlatformOwner.Perm2:
                mPlatform.GetComponent<MeshRenderer>().material.color = Perm2Color;
                break;
        }
        
        return;
    }

    private void Awake()
    {
        mPlatformMaker = GameObject.Find("CubeMaker").GetComponent<PlatformMaker>();
    }


    private void Start()
    {
        PlatformInitialization();
        ClearPlatform();
    }
    private void Update()
    {
        CheckPlayer();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Player1"))
        {
            mHasPlayer1 = true;
            if (mPlayersOnBlock == PlayersOnBlock.Nothing)
                mPlayersOnBlock = PlayersOnBlock.Player1;
            //else if (mPlayersOnBlock == PlayersOnBlock.Player2)
                //mPlayersOnBlock = PlayersOnBlock.BothPlayer;
        }
        if (collision.collider.gameObject.CompareTag("Player2"))
        {
            mHasPlayer2 = true;
            if (mPlayersOnBlock == PlayersOnBlock.Nothing)
                mPlayersOnBlock = PlayersOnBlock.Player2;
            //else if (mPlayersOnBlock == PlayersOnBlock.Player1)
                //mPlayersOnBlock = PlayersOnBlock.BothPlayer;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Player1"))
        {
            //if (mPlayersOnBlock == PlayersOnBlock.Player1)
            //mPlayersOnBlock = PlayersOnBlock.Nothing;
            //else if (mPlayersOnBlock == PlayersOnBlock.BothPlayer)
            //mPlayersOnBlock = PlayersOnBlock.Player2;
            mHasPlayer1 = false;
            if (mHasPlayer2)
                mPlayersOnBlock = PlayersOnBlock.Player2;
            else
                mPlayersOnBlock = PlayersOnBlock.Nothing;
            mPlayer1Point = 0.0f;
            if (mPlayersOnBlock == PlayersOnBlock.Nothing ||
                mPlatformMaker.GetBingoMap(mPlatformX, mPlatformZ) == PlatformMaker.PlatformOwner.Temp2||
                mPlatformMaker.GetBingoMap(mPlatformX, mPlatformZ) == PlatformMaker.PlatformOwner.Perm2)
            {
                ClearPlatform();
            }
        }
        if (collision.collider.gameObject.CompareTag("Player2"))
        {
            mHasPlayer2 = false;
            /*
            if (mPlayersOnBlock == PlayersOnBlock.Player2)
                mPlayersOnBlock = PlayersOnBlock.Nothing;
            else if (mPlayersOnBlock == PlayersOnBlock.BothPlayer)
                mPlayersOnBlock = PlayersOnBlock.Player1;
            */
            if (mHasPlayer1)
                mPlayersOnBlock = PlayersOnBlock.Player1;
            else
                mPlayersOnBlock = PlayersOnBlock.Nothing;
            mPlayer2Point = 0.0f;
            if (mPlayersOnBlock == PlayersOnBlock.Nothing || 
                mPlatformMaker.GetBingoMap(mPlatformX, mPlatformZ) == PlatformMaker.PlatformOwner.Temp1||
                mPlatformMaker.GetBingoMap(mPlatformX, mPlatformZ) == PlatformMaker.PlatformOwner.Perm1)
            {
                ClearPlatform();
            }
        }
    }
}
