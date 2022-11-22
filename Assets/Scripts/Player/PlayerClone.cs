using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClone : MonoBehaviour
{
    private GameObject mActualPlayer;//이 분신이 종속되는 플레이어 레퍼런스                                     
    private Vector3 mVectorToPlayer;//플레이어와 유지되어야 하는 거리벡터
    private Animator mAnimator;
    private Animator mOriginalAnimator;
    public void SetActualPlayer(GameObject player)
    {
        mActualPlayer = player;
        mVectorToPlayer = mActualPlayer.transform.position - transform.position;
        mOriginalAnimator = mActualPlayer.GetComponent<Animator>();
    }

    private void UpdateCloneTransform()//분신은 원본과 mVectorToPlayer만큼 떨어질뿐 동일하게 움직임
    {
        transform.position = mActualPlayer.transform.position - mVectorToPlayer;//position 카피
        transform.rotation = mActualPlayer.transform.rotation;//rotation 카피
        mAnimator.SetBool("run", mOriginalAnimator.GetBool("run"));//애니메이션 카피
    }
    private void Start()
    {
        mAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        UpdateCloneTransform();
        if(GameManager.instance.GetGameState() == GameState.Battle && !GameManager.instance.IsWallTime())
        {
            this.enabled = false;
        }
    }

}
