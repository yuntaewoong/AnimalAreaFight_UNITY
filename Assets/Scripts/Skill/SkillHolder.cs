using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHolder : MonoBehaviour
{
    [SerializeField] private Skill mActiveSkill;
    [SerializeField] private List<PassiveSkill> mPassiveSkills;//패시브 스킬배열
    private int[] mSkillLevels = new int[3];
    private float mCooldownTime;
    private float mActiveTime;
    enum ActiveSkillState { ready, active, cooldown } // 액티브 스킬 상태 (사용가능, 사용중, 쿨타임)
    private ActiveSkillState mActiveSkillState = ActiveSkillState.ready;


  
    public int animalType; // 동물 타입 (0:토끼, 1:거북이, 2:소)
    public KeyCode mActiveSkillKey; // 액티브 스킬 시전 키
    public bool isActive; // 현재 active 상태인지
    public float Power { get; set; }
    
    public int GetSkillLevel(int skillIndex)
    {
        return mSkillLevels[skillIndex];
    }
    public void AddSkillLevel(int skillIndex)
    {
        mSkillLevels[skillIndex] += 1; 
    }
    public void AddPassiveSkill(PassiveSkill pSkill)
    {
        mPassiveSkills.Add(pSkill);
    }
    public void PassiveAdjust()//mPassiveSkills의 PassiveAdjust함수들 호출
    {
        foreach (PassiveSkill pSkill in mPassiveSkills)
        {
            Debug.Log(pSkill.GetPassiveSkillIdentifier());
            if (pSkill.mSkillLevel == mSkillLevels[pSkill.GetPassiveSkillIdentifier()])//현재 레벨에 해당하는 스킬만 발동함(예를 들어, 같은이름의 lv1 lv2 lv3스킬이 있으면 lv3만 발동
                pSkill.PassiveAdjust(gameObject);
        }
    }
    public void DestroyPassiveObject()
    {
        foreach (PassiveSkill pSkill in mPassiveSkills)
            pSkill.DestroyPassiveObject();
    }
    private void Start()
    {
        for (int i = 0; i < 3; i++)
            mSkillLevels[i] = 0;

        isActive = false;
    }
    private void Update()
    {
        switch(mActiveSkillState)
        {
            case ActiveSkillState.ready: // ready 상태일때
                if (Input.GetKeyDown(mActiveSkillKey))
                {
                    mActiveSkill.Activate(gameObject);              // 액티브 스킬 발동
                    mActiveSkillState = ActiveSkillState.active;    // 스킬 시전 상태로 전환
                    mActiveTime = mActiveSkill.activeTime;          // 액티브 스킬 시전 시간 정보 갖고오기
                }

                break;
            case ActiveSkillState.active:
                // 스킬 시전 시간 지나면 쿨다운 타임으로 전환
                isActive = true;

                if (mActiveTime > 0)
                {
                    mActiveTime -= Time.deltaTime;
                }
                else
                {
                    isActive = false;
                    mActiveSkillState = ActiveSkillState.cooldown;           // 쿨타운 상태로 전환
                    mCooldownTime = mActiveSkill.cooldownTime;               // 쿨다운 시간 정보 갖고오기
                    
                    if(mActiveSkill.skillType == Skill.SkillType.Continuous) // 지속성 스킬이라면
                        mActiveSkill.DeActivate(gameObject);                 // 스킬 해제
                }

                break;
            case ActiveSkillState.cooldown:
                // 스킬 쿨타임 지나면 ready 상태로 전환
                if (mCooldownTime > 0)
                {
                    mCooldownTime -= Time.deltaTime;
                }
                else
                {
                    mActiveSkillState = ActiveSkillState.ready;
                    if(gameObject.tag == "Player1")
                        Debug.Log("player1 skill ready");
                    else if (gameObject.tag == "Player2")
                        Debug.Log("player2 skill ready");
                }
                break;
        }
    
    }
}
