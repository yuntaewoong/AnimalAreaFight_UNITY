using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Skill_Running : Skill
{
    // 토끼 스킬
    // 특정 시간동안 엄청 빨라짐
    
    public float runningSpeed;

    private float originalSpeed;
    private Vector3 originalSize;

    public override void Activate(GameObject parent)
    {
        if (GameManager.instance.GetGameState() != GameState.Battle)//Battle상태일때만 유효
            return;
        if (GameManager.instance.IsWallTime())//벽이 다 내려올때까지 스킬밴
            return;
        PlayerMovement playerMovement = parent.GetComponent<PlayerMovement>();

        originalSpeed = playerMovement.moveSpeed;
        playerMovement.moveSpeed = runningSpeed;

        originalSize = parent.transform.localScale;
        parent.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

        parent.GetComponent<Rigidbody>().mass = 5;

        PlayOccupyParticle(Color.white, parent);
    }


    public override void DeActivate(GameObject parent)
    {
        if (originalSize == Vector3.zero)
            originalSize = new Vector3(0.25f, 0.25f, 0.25f);
        if (originalSpeed == 0)
            originalSpeed = 3;
        
        PlayerMovement playerMovement = parent.GetComponent<PlayerMovement>();

        playerMovement.moveSpeed = originalSpeed;
        parent.transform.localScale = originalSize;

        parent.GetComponent<Rigidbody>().mass = 1;
    }

    private void PlayOccupyParticle(Color color, GameObject parent)
    {
        ParticleSystem ps = parent.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule ma = ps.main;
        ma.startColor = color;
        ps.Play();
    }
}
