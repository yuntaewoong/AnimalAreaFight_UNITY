using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PassiveSkill_RotatingBall : PassiveSkill
{
    [SerializeField] private GameObject mBallPrefab;   //주위를 도는 공 프리팹
    [SerializeField] private float mKnockBackPower;   //넉백 파워
    [SerializeField] private float mRadius;   //회전 반경값
    [SerializeField] private float mRotationSpeed;   //회전 속도
    [SerializeField] private float mBallScale;   //공 크기
    public override void PassiveAdjust(GameObject parent)
    {
        //공 생성
        for (int i = 0; i < mSkillLevel; i++)
        {//일단 스킬레벨마다 공 개수 다르게 나오나 테스트
            mGameObjects.Add(
                Instantiate(
                    mBallPrefab,
                    parent.gameObject.transform.position + new Vector3((i+1)/2.0f, 0.5f, 0.0f),
                    Quaternion.identity
                )
            );
            RotatingBall rotatingBall = mGameObjects[mGameObjects.Count - 1].GetComponent<RotatingBall>();
            rotatingBall.SetParentTransform(parent.transform);//Rotating Ball을 부모 오브젝트의 자식 오브젝트로 설정(따라다니게)
            rotatingBall.SetRotatingValue((float)i / mSkillLevel * 360.0f);
            rotatingBall.SetKnockBackPower(mKnockBackPower);
            rotatingBall.SetRadius(mRadius);
            rotatingBall.SetRotationSpeed(mRotationSpeed);
            rotatingBall.transform.localScale *= mBallScale;
        }
    }
    public override int GetPassiveSkillIdentifier()
    {
        return 2;
    }
}
