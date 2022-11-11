using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
    [SerializeField] private GameObject mWall;
    private float mTimecheck = 0;
    private float mWalltime = 0.0f;
    public void SetWallTime(float wallTime)
    {
        mWalltime = wallTime;
    }
    private void Update()
    {
        MoveStartWall();
    }
    private void MoveStartWall()
    {
        if (mTimecheck < mWalltime)
        {
            mTimecheck += Time.deltaTime;
        }
        else
        {
            Debug.Log("Destroy");
            Destroy(mWall);
        }

    }
}
