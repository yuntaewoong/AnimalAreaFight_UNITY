using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Skill_Dash : Skill
{
    public float dashVelocity;
    public float dashPower;

    public override void Activate(GameObject parent)
    {
        if (GameManager.instance.GetGameState() != GameState.Battle)//Battle상태일때만 유효
            return;
        if (GameManager.instance.IsWallTime())//벽이 다 내려올때까지 스킬밴
            return;

        SkillHolder playerSkillHolder = parent.GetComponent<SkillHolder>();
        playerSkillHolder.Power = dashPower; // dash power 정보 넘기기

        PlayerInput playerInput = parent.GetComponent<PlayerInput>();
        Rigidbody rigidbody = parent.GetComponent<Rigidbody>();

        rigidbody.velocity = playerInput.moveDirection * dashVelocity;
    }
}
