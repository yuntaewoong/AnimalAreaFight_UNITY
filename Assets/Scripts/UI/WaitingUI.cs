using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
/*
 * 
 * Game���°� Waiting�϶��� UIó��
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
    private Vector2 mPlayer1BarOffsetMaxInitialValue;//�ʱ� offsetMax�� �����
    private Vector2 mPlayer2BarOffsetMinInitialValue;//�ʱ� offsetMin�� �����


    IEnumerator WaitingUICorutine()//Waiting���� UI������Ʈ �ڷ�ƾ
    {
        float iteratorScore = 0.0f;//UI�ִϸ��̼��� �����ϸ鼭 ������Ʈ�Ǵ� Score��
        int player1Score = GameManager.instance.GetPlayer1Score();
        int player2Score = GameManager.instance.GetPlayer2Score();
        int sumOfPlayer1ScorePlayer2Score = player1Score + player2Score;
        int loserScore = player1Score <= player2Score ? player1Score : player2Score;
        int winnerScore = player1Score >= player2Score ? player1Score : player2Score;
        float bar1Width = mPlayer1Bar.rect.width;
        float bar2Width = mPlayer2Bar.rect.width;
        float timeInterval = 0.016f;//�ð�����(60�����ӱ���)
        float gageSpeed = 3.0f;//������ �������� ���ǵ�

        while (true)
        {
            if((int)iteratorScore < loserScore)//�й����� ���������� ���� ����
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
                     * ���º� ����ó��
                     */
                    if (winnerScore == loserScore)//���ºθ�
                    {
                        mWinnerText.gameObject.SetActive(true);
                        mWinnerText.SetText("Draw");
                        yield return new WaitForSeconds(3.0f);//3.0�� ������
                        GameManager.instance.ChangeStateWaitingToSkillSelection();//��ų ���û��·� ��ȯ
                        yield break;
                    }
                }
                yield return new WaitForSeconds(timeInterval);
            }
            else if((int)iteratorScore >= loserScore && (int)iteratorScore < winnerScore)//�¸����� ������ ����
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
            else if((int)iteratorScore == winnerScore)//�¸��� ȭ�� �����ϸ� �ڷ�ƾ ������
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
                yield return new WaitForSeconds(3.0f);//3.0�� ������

                GameManager.instance.ChangeStateWaitingToSkillSelection();//��ų ���û��·� ��ȯ
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
    private void OnEnable()//���ÿ� UnEnable�Ǿ��ִٰ� Battle->Waiting���� ���� ���°� ���̵ɶ� ȣ��
    {
        mPlayer1ScoreText.SetText("0");
        mPlayer2ScoreText.SetText("0");
        mPlayer1ScoreText.fontSize = mScoreFontSize;
        mPlayer2ScoreText.fontSize = mScoreFontSize;
        mPlayer1Bar.offsetMax = new Vector2(mPlayer1BarOffsetMaxInitialValue.x, mPlayer1BarOffsetMaxInitialValue.y);
        mPlayer2Bar.offsetMin = new Vector2(mPlayer2BarOffsetMinInitialValue.x, mPlayer2BarOffsetMinInitialValue.y);
        mWinnerText.gameObject.SetActive(false);
        StartCoroutine(WaitingUICorutine());//�ڷ�ƾ ����
    }
}
