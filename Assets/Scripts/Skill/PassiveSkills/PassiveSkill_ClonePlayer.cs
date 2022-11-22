using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PassiveSkill_ClonePlayer : PassiveSkill
{
    [SerializeField] private GameObject[] mClonePlayerPrefabs;//�н� �÷��̾� �����շ��۷��� �迭 
    [SerializeField] private float mDistanceBetweenClone = 3.0f;//clone������ �Ÿ�
    public override void PassiveAdjust(GameObject parent)
    {
        int animalType = parent.GetComponent<SkillHolder>().animalType;
        for (int i = 0; i < mSkillLevel; i++)
        {
            if(i == 0)
            {
                mGameObjects.Add(
                Instantiate(
                        mClonePlayerPrefabs[animalType],
                        parent.gameObject.transform.position + new Vector3(0.0f, 0.0f, (4) * mDistanceBetweenClone),
                        Quaternion.identity
                    )
                );
            }
            else if(i == 1)
            {
                mGameObjects.Add(
                Instantiate(
                        mClonePlayerPrefabs[animalType],
                        parent.gameObject.transform.position + new Vector3(0.0f, 0.0f, (2) * mDistanceBetweenClone),
                        Quaternion.identity
                    )
                );
            }
            else
            {
                mGameObjects.Add(
                Instantiate(
                        mClonePlayerPrefabs[animalType],
                        parent.gameObject.transform.position + new Vector3(0.0f, 0.0f, (i + 1) * mDistanceBetweenClone),
                        Quaternion.identity
                    )
                );
            }
            
            mGameObjects[mGameObjects.Count - 1].GetComponent<PlayerClone>().SetActualPlayer(parent);//���� ����
            mGameObjects[mGameObjects.Count - 1].tag = parent.tag;//�±� ī��
        }
    }
    public override int GetPassiveSkillIdentifier()
    {
        return 1;
    }
}
