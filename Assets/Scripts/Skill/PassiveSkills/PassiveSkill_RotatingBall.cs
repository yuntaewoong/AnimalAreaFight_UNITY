using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PassiveSkill_RotatingBall : PassiveSkill
{
    [SerializeField] private GameObject mBallPrefab;   //������ ���� �� ������
    [SerializeField] private float mKnockBackPower;   //�˹� �Ŀ�
    public override void PassiveAdjust(GameObject parent)
    {
        //�� ����
        for (int i = 0; i < mSkillLevel; i++)
        {//�ϴ� ��ų�������� �� ���� �ٸ��� ������ �׽�Ʈ
            mGameObjects.Add(
                Instantiate(
                    mBallPrefab,
                    parent.gameObject.transform.position + new Vector3((i+1)/2.0f, 0.5f, 0.0f),
                    Quaternion.identity
                )
            );
            RotatingBall rotatingBall = mGameObjects[mGameObjects.Count - 1].GetComponent<RotatingBall>();
            rotatingBall.SetParentTransform(parent.transform);//Rotating Ball�� �θ� ������Ʈ�� �ڽ� ������Ʈ�� ����(����ٴϰ�)
            rotatingBall.SetRotatingValue((float)i / mSkillLevel * 360.0f);
            rotatingBall.SetKnockBackPower(mKnockBackPower);
            Debug.Log(mKnockBackPower);
        }
    }
    public override int GetPassiveSkillIdentifier()
    {
        return 2;
    }
}
