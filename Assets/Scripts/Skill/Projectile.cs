using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // 발사체 스크립트
    // 생성되면 발사하고 어딘가에 닿거나 시간 지나면 사라짐

    public enum Owner { Player1, Player2 }; // 발사체 발사한 사람

    public Owner owner;

    public float Power { get; set; } // 투사체 파워


    void Start()
    {
        Destroy(gameObject, 2f);
    }

    void Update()
    {
    }


}
