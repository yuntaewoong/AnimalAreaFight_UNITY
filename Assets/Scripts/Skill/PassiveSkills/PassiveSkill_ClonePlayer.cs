using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PassiveSkill_ClonePlayer : PassiveSkill
{
    [SerializeField] private GameObject[] mClonePlayerPrefabs;//분신 플레이어 프리팹레퍼런스 배열 
    [SerializeField] private float mDistanceBetweenClone = 3.0f;//clone사이의 거리
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
            
            mGameObjects[mGameObjects.Count - 1].GetComponent<PlayerClone>().SetActualPlayer(parent);//원본 설정
            mGameObjects[mGameObjects.Count - 1].tag = parent.tag;//태그 카피
        }
    }
    public override int GetPassiveSkillIdentifier()
    {
        return 1;
    }
}
