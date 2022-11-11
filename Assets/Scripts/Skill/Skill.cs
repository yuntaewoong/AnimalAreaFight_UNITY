using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : ScriptableObject
{
    public enum SkillType {Immediate, Continuous} // 스킬 타입 (즉발, 지속)

    public new string name;
    public float cooldownTime;
    public float activeTime;
    public SkillType skillType;

    public virtual void Activate(GameObject parent) {}      // 스킬 발동

    public virtual void DeActivate(GameObject parent) {}    // 스킬 해제
}
