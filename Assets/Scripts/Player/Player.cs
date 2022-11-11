using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int mPlayerNumber; //내가 1번 플레이언지 2번 플레이언지
    private int mPassiveTurretLevel = 0;
    private GameObject mMyTurret;

    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public int GetPassiveTurretLevel()
    {
        return mPassiveTurretLevel; 
    }

    public void SpawnTurret()
    {
        GameObject myTurret = Instantiate(mMyTurret, this.transform.position, Quaternion.identity);
        myTurret.GetComponent<Turret>().SetTurret(mPassiveTurretLevel, mPlayerNumber);


    }
    public void SetTurret(GameObject turret)
    {
        mMyTurret = turret;
    }
    public void SetPassiveTurretLevel(int passivelevel)
    {
        mPassiveTurretLevel = passivelevel;
    }

    public void SetPlayerNumber(int number)
    {
        mPlayerNumber = number; 
    }
    public int GetPlayerNumber()
    {
        return mPlayerNumber;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball") // 접촉한 오브젝트의 tag가 Ball이라면
        {
            Projectile ball = other.GetComponent<Projectile>();
            
            if (ball.owner.ToString() != this.tag) // 접촉한 플레이어가 Ball 던진 사람이 아니라면
            {
                // 정규방향벡터 설정 (넉백방향)
                Vector3 d = (gameObject.transform.position - other.transform.position).normalized;

                rb.AddForce(d * ball.Power);
            }
        }

        if (other.tag == "RotatingBall") // 접촉한 오브젝트의 tag가 RotatingBall이라면
        {
            RotatingBall rotatingBall = other.GetComponent<RotatingBall>();
            // 정규방향벡터 설정 (넉백방향)
            Vector3 d = (gameObject.transform.position - other.transform.position).normalized;
            rb.AddForce(d * rotatingBall.GetKnockBackPower());
        }

        if (other.tag == "Player1" || other.tag == "Player2") // 접촉한 오브젝트가 플레이어라면
        {
            var otherPlayerSkillHolder = other.GetComponent<SkillHolder>();
            if (otherPlayerSkillHolder.animalType == 2 && otherPlayerSkillHolder.isActive)
            // 접촉한 상대가 cow이고 스킬 사용중이라면
            {
                // 정규방향벡터 설정 (넉백방향)
                Vector3 d = (gameObject.transform.position - other.transform.position).normalized;

                rb.AddForce(d * otherPlayerSkillHolder.Power, ForceMode.Impulse);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        
    }
}
