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
        if (GameManager.instance.GetGameState() != GameState.Battle)//Battle�����϶��� ��ȿ
            return;
        if (GameManager.instance.IsWallTime())//���� �� �����ö����� ��ų��
            return;

        SkillHolder playerSkillHolder = parent.GetComponent<SkillHolder>();
        playerSkillHolder.Power = dashPower; // dash power ���� �ѱ��

        PlayerInput playerInput = parent.GetComponent<PlayerInput>();
        Rigidbody rigidbody = parent.GetComponent<Rigidbody>();

        rigidbody.velocity = playerInput.moveDirection * dashVelocity;
    }
}
