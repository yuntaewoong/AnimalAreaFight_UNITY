using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMaker : MonoBehaviour
{
    
    
    [SerializeField] private int mMapSize = 10;  //한 줄 사이즈
    [SerializeField] private int mPlatformCount = 5; //한 줄에 몇 개 플랫폼 넣을 건지
    [SerializeField] private float mCubeY = 1.0f; 
    
    [SerializeField] private GameObject mCube;
    [SerializeField] private Transform mCubeT;

    private PlatformOwner[,] mBingoMap;
    private GameObject[,] mPlatformList;

    public void ClearAllBoard()
    {
        for (int i =0; i< mPlatformCount; i++)
        {
            for(int j =0; j< mPlatformCount; j++)
            {
                mBingoMap[i, j] = PlatformOwner.Neutral;
                mPlatformList[i, j].GetComponentInChildren<Platform>().ClearPlatform();
            }
        }
    }

    public enum PlatformOwner//주인.
    {
        Neutral,
        Temp1,//임시로 플레이어 1이 주인
        Temp2,
        Perm1,//영구적인 플레이어 1이 주인
        Perm2
    }
    private void MakeBingoMap()
    {
        mBingoMap = new PlatformOwner[mPlatformCount, mPlatformCount];
        mPlatformList = new GameObject[mPlatformCount, mPlatformCount];
    }
    public void SetBingoMap(int x, int z, PlatformOwner owner) 
    {
        mBingoMap[x, z] = owner;
    }
    public PlatformOwner GetBingoMap(int x, int z)
    {
        return mBingoMap[x, z];
    }

    public int GetBingoMapSize()
    {
        return mPlatformCount;
    }

    public GameObject GetPlatformList(int x, int z)
    {
        return mPlatformList[x, z];
    }


    private void SummonCube(int mPlatformSize, int x, int z)
    {
        
        GameObject mInstance = Instantiate(mCube);
        
        mInstance.name = x + "," + z;
        SetBingoMap(x, z, PlatformOwner.Neutral);
        mPlatformList[x, z] = mInstance;
        mInstance.transform.position = new Vector3(mCubeT.position.x + x*mPlatformSize, 0 , mCubeT.position.z + z* mPlatformSize);
        mInstance.transform.localScale = new Vector3(mPlatformSize, mCubeY, mPlatformSize);
        mInstance.SetActive(true);
        
    }

    private void StartSummon()
    {
        int mPlatformSize = mMapSize / mPlatformCount;

        for (int x =0; x<mPlatformCount; x++)
        {
            for(int z =0; z<mPlatformCount; z++)
            {
                SummonCube(mPlatformSize, x, z);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MakeBingoMap();
        StartSummon();  

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
