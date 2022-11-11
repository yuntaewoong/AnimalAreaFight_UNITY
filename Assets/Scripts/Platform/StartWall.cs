using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWall : MonoBehaviour
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
            transform.Translate(new Vector3(0, -1, 0)/ mWalltime * Time.deltaTime);
            mTimecheck += Time.deltaTime;
        }
        else
        {
            Destroy(mWall);
        }

    }
}
