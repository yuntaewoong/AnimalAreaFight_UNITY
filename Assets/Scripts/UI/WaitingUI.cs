using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
/*
 * 
 * Game상태가 Waiting일때의 UI처리
 * 
 * 
 */
public class WaitingUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mWinnerText;
    [SerializeField] private TextMeshProUGUI mPlayer1ScoreText;
    [SerializeField] private TextMeshProUGUI mPlayer2ScoreText;
    [SerializeField] private RectTransform mPlayer1Bar;
    [SerializeField] private RectTransform mPlayer2Bar;
    [SerializeField] private float mScoreFontSize = 160;
    private Vector2 mPlayer1BarOffsetMaxInitialValue;//초기 offsetMax값 저장용
    private Vector2 mPlayer2BarOffsetMinInitialValue;//초기 offsetMin값 저장용


    IEnumerator WaitingUICorutine()//Waiting상태 UI업데이트 코루틴
    {
        float iteratorScore = 0.0f;//UI애니메이션이 동작하면서 업데이트되는 Score값
        int player1Score = GameManager.instance.GetPlayer1Score();
        int player2Score = GameManager.instance.GetPlayer2Score();
        int sumOfPlayer1ScorePlayer2Score = player1Score + player2Score;
        int loserScore = player1Score <= player2Score ? player1Score : player2Score;
        int winnerScore = player1Score >= player2Score ? player1Score : player2Score;
        float bar1Width = mPlayer1Bar.rect.width;
        float bar2Width = mPlayer2Bar.rect.width;
        float timeInterval = 0.016f;//시간간격(60프레임기준)
        float gageSpeed = 3.0f;//게이지 차오르는 스피드

        while (true)
        {
            if((int)iteratorScore < loserScore)//패배자의 점수까지는 같이 증가
            {
                mPlayer1ScoreText.SetText(((int)iteratorScore).ToString());
                mPlayer2ScoreText.SetText(((int)iteratorScore).ToString());
                mPlayer1Bar.offsetMax = new Vector2(-bar1Width + iteratorScore  / (float)sumOfPlayer1ScorePlayer2Score * bar1Width, 0);
                mPlayer2Bar.offsetMin = new Vector2(bar2Width - iteratorScore  / (float)sumOfPlayer1ScorePlayer2Score * bar2Width, 0);
                if ((int)(iteratorScore + timeInterval * gageSpeed) == loserScore)
                {
                    mPlayer1ScoreText.SetText(((int)iteratorScore + 1).ToString());
                    mPlayer2ScoreText.SetText(((int)iteratorScore + 1).ToString());
                    yield return new WaitForSeconds(3.0f);
                    /*
                     * 무승부 예외처리
                     */
                    if (winnerScore == loserScore)//무승부면
                    {
                        mWinnerText.gameObject.SetActive(true);
                        mWinnerText.SetText("Draw");
                        yield return new WaitForSeconds(3.0f);//3.0초 딜레이
                        GameManager.instance.ChangeStateWaitingToSkillSelection();//스킬 선택상태로 전환
                        yield break;
                    }
                }
                yield return new WaitForSeconds(timeInterval);
            }
            else if((int)iteratorScore >= loserScore && (int)iteratorScore < winnerScore)//승리자의 점수만 증가
            {
                if(player1Score > player2Score)
                {
                    mPlayer1ScoreText.SetText(((int)iteratorScore).ToString());
                    mPlayer1Bar.offsetMax = new Vector2(-bar1Width + iteratorScore / (float)sumOfPlayer1ScorePlayer2Score * bar1Width, 0);
                }
                else
                {
                    mPlayer2ScoreText.SetText(((int)iteratorScore).ToString());
                    mPlayer2Bar.offsetMin = new Vector2(bar2Width - iteratorScore / (float)sumOfPlayer1ScorePlayer2Score * bar2Width, 0);
                }
                yield return new WaitForSeconds(timeInterval);
            }
            else if((int)iteratorScore == winnerScore)//승리자 화면 강조하며 코루틴 마무리
            {
                mWinnerText.gameObject.SetActive(true);
                if (player1Score > player2Score)
                {
                    mPlayer1ScoreText.SetText(((int)iteratorScore).ToString());
                    mPlayer1ScoreText.fontSize = mPlayer1ScoreText.fontSize * (float)1.5;
                    mPlayer1Bar.offsetMax = new Vector2(-bar1Width + iteratorScore / (float)sumOfPlayer1ScorePlayer2Score * bar1Width, 0);
                    mWinnerText.SetText("Winner Is Red");
                }
                else
                {
                    mPlayer2ScoreText.SetText(((int)iteratorScore).ToString());
                    mPlayer2ScoreText.fontSize = mPlayer1ScoreText.fontSize * (float)1.5;
                    mPlayer2Bar.offsetMin = new Vector2(bar2Width - iteratorScore / (float)sumOfPlayer1ScorePlayer2Score * bar2Width, 0);
                    mWinnerText.SetText("Winner Is Blue");
                }
                yield return new WaitForSeconds(3.0f);//3.0초 딜레이

                GameManager.instance.ChangeStateWaitingToSkillSelection();//스킬 선택상태로 전환
                yield break;
            }
            iteratorScore = iteratorScore + timeInterval* gageSpeed;
        }
    }

    private void Awake()
    {
        mPlayer1BarOffsetMaxInitialValue = new Vector2(mPlayer1Bar.offsetMax.x, mPlayer1Bar.offsetMax.y);
        mPlayer2BarOffsetMinInitialValue = new Vector2(mPlayer2Bar.offsetMin.x, mPlayer2Bar.offsetMin.y);

    }
    private void OnEnable()//평상시엔 UnEnable되어있다가 Battle->Waiting으로 게임 상태가 전이될때 호출
    {
        mPlayer1ScoreText.SetText("0");
        mPlayer2ScoreText.SetText("0");
        mPlayer1ScoreText.fontSize = mScoreFontSize;
        mPlayer2ScoreText.fontSize = mScoreFontSize;
        mPlayer1Bar.offsetMax = new Vector2(mPlayer1BarOffsetMaxInitialValue.x, mPlayer1BarOffsetMaxInitialValue.y);
        mPlayer2Bar.offsetMin = new Vector2(mPlayer2BarOffsetMinInitialValue.x, mPlayer2BarOffsetMinInitialValue.y);
        mWinnerText.gameObject.SetActive(false);
        StartCoroutine(WaitingUICorutine());//코루틴 시작
    }
}
