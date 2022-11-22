using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] GameObject turretBullet; // 터렛이 쏠 총알

    private int mTurretLevel = 0;
    private GameObject mEnemy;
    private int bulletCount;        // 쏠 총알 수
    private float bulletShootCycle; // 발사주기 (몇 초마다 쏠 건지)
    private float bulletSpeed;      // 총알 속도
    private float bulletPower;      // 총알 파워
    private int mPlayer;            // 누가 설치했는지

    [SerializeField] private GameObject Capsule;
    [SerializeField] private GameObject Cylinder;

    public void SetTurret(int level, int player)
    {
        mTurretLevel = level;
        mPlayer = player;
        if(mPlayer == 1)
        {
            mEnemy = GameManager.instance.GetPlayer(2);
            Capsule.GetComponent<MeshRenderer>().material.color = new Color(255, 0, 0);
            Cylinder.GetComponent<MeshRenderer>().material.color = new Color(255, 0, 0);
        }
            
        else if (mPlayer == 2)
        {
            mEnemy = GameManager.instance.GetPlayer(1);
            Capsule.GetComponent<MeshRenderer>().material.color = new Color(0,0,255);
            Cylinder.GetComponent<MeshRenderer>().material.color = new Color(0,0,255);
        }
            


        switch(mTurretLevel)
        {
            case 1:
                bulletCount = 5;
                bulletShootCycle = 1;
                bulletPower = 5;
                bulletSpeed = 10;
                break;

            case 2:
                bulletCount = 8;
                bulletShootCycle = 0.5f;
                bulletPower = 5;
                bulletSpeed = 12.5f;
                break;

            default:
                Debug.Log("ERROR: Turret.cs");
                break;
        }

    }
    
    void Start()
    {
        StartCoroutine(Shoot());
        // Destroy(gameObject, 5f);
    }
    
    void Update()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        if (mEnemy != null)
        {
            Vector3 dir = mEnemy.transform.position - this.transform.position;
            this.transform.rotation =
                Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), 1000);
                //Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * mTurretLevel);
        }
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(1f);

        while(bulletCount > 0)
        {
            GameObject bullet = Instantiate(turretBullet, gameObject.transform.position, Quaternion.identity);
            Projectile bulletProjectile = bullet.GetComponent<Projectile>();

            if (mPlayer == 1)
                bulletProjectile.owner = Projectile.Owner.Player1;
            else if (mPlayer == 2)
                bulletProjectile.owner = Projectile.Owner.Player2;

            bulletProjectile.Power = bulletPower;

            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.AddForce(gameObject.transform.forward * bulletSpeed, ForceMode.Impulse);

            yield return new WaitForSeconds(bulletShootCycle);

            bulletCount--;
        }

        Destroy(gameObject); // 총알 다 쏘면 파괴
    }


}
