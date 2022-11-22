using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Skill_Dash : Skill
{
    public float dashVelocity;

    public override void Activate(GameObject parent)
    {
        PlayerInput playerInput = parent.GetComponent<PlayerInput>();
        Rigidbody rigidbody = parent.GetComponent<Rigidbody>();

        rigidbody.velocity = playerInput.moveDirection * dashVelocity;
    }
}
