using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkill : ScriptableObject
{
    public string mName;//스킬이름
    public int mSkillLevel;//패시브 스킬 레벨
    protected List<GameObject> mGameObjects;//패시브에 의해 생성된 오브젝트들
    public virtual void PassiveAdjust(GameObject parent) { }//부모 오브젝트의 레퍼런스를 받아와 패시브 스킬적용(레벨초기화 이후 실행됨)
    public void DestroyPassiveObject() //패시브 효과에 의해 생성된 오브젝트를 삭제(레벨초기화할때 실행됨)
    {
        foreach (var i in mGameObjects)
            Destroy(i);
    }
    public virtual int GetPassiveSkillIdentifier() { throw new NotImplementedException(); }//각 패시브스킬별로 고유한 Int값 반환
}
