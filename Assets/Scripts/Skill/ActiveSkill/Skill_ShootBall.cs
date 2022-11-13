using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Skill_ShootBall : Skill
{
    // 상대 넉백용 공 발사 스킬

    public GameObject ballPrefab;   // 발사할 공 프리팹
    public float ballSpeed;         // 공 스피드
    public float ballPower;         // 공 파워

    public override void Activate(GameObject parent)
    {
        if (GameManager.instance.GetGameState() != GameState.Battle)//Battle상태일때만 유효
            return;
        if (GameManager.instance.IsWallTime())//벽이 다 내려올때까지 스킬밴
            return;
        // 바라보는 방향으로 공 생성
        
        GameObject ball = Instantiate(ballPrefab, parent.gameObject.transform.position - new Vector3(0, 0.35f, 0), Quaternion.identity);

        // ball 파워 정보 넘기기
        ball.GetComponent<Projectile>().Power = ballPower;
        
        // 발사한 플레이어 정보 넘기기
        if (parent.gameObject.tag == "Player1")
            ball.GetComponent<Projectile>().owner = Projectile.Owner.Player1;
        else if (parent.gameObject.tag == "Player2")
            ball.GetComponent<Projectile>().owner = Projectile.Owner.Player2;


        // 공 발사
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.AddForce(parent.gameObject.transform.forward * ballSpeed, ForceMode.Impulse);

    }
}
