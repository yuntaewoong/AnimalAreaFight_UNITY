using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Skill_Boom : Skill
{
    public float skillPower;

    public override void Activate(GameObject parent)
    {
        if (GameManager.instance.GetGameState() != GameState.Battle)//Battle상태일때만 유효
            return;
        if (GameManager.instance.IsWallTime())//벽이 다 내려올때까지 스킬밴
            return;

        Collider[] colliderArray = Physics.OverlapSphere(parent.transform.position, 0.8f);
        foreach(Collider c in colliderArray)
        {
            if ((c.tag == "Player1" || c.tag == "Player2") && c.tag != parent.tag)
            {
                c.GetComponent<Player>().GoAway(parent.transform.position, skillPower, true);

                ParticleSystem ps = parent.GetComponentInChildren<ParticleSystem>();
                ParticleSystem.MainModule ma = ps.main;
                ma.startColor = Color.gray;
                ps.Play();
            }
        }
    }
}
