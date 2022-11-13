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

    public void GoAway(Vector3 otherPosition, float power, bool goUp = true)
    // 상대로부터 멀리 플레이어 날리는 함수 (상대 위치, 파워, 좀 더 위쪽으로 날릴건지)
    {
        Vector3 d = gameObject.transform.position - otherPosition; // 정규방향벡터 설정
        
        if (goUp)
            d = new Vector3(d.x, 0.5f, d.z).normalized;
        else if (!goUp)
            d = new Vector3(d.x, 0.1f, d.z).normalized;

        Debug.Log("d: " + d);
        
        rb.AddForce(d * power, ForceMode.Impulse);

        Debug.Log("boom!!: " + gameObject.tag);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball") // 접촉한 오브젝트의 tag가 Ball이라면
        {
            Projectile ball = other.GetComponent<Projectile>();
            
            if (ball.owner.ToString() != this.tag) // 접촉한 플레이어가 Ball 던진 사람이 아니라면
            {
                GoAway(other.transform.position, ball.Power, false);
            }
        }

        if (other.tag == "RotatingBall") // 접촉한 오브젝트의 tag가 RotatingBall이라면
        {
            RotatingBall rotatingBall = other.GetComponent<RotatingBall>();
            // 정규방향벡터 설정 (넉백방향)

            GoAway(other.transform.position, rotatingBall.GetKnockBackPower(), false);
        }

    }

    
    

    
}
