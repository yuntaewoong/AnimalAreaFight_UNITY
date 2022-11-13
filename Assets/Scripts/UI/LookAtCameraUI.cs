using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCameraUI : MonoBehaviour
{
    // 게임 상의 UI 카메라 방향으로 바라보게 하는 스크립트

    public Camera mCamera;

    void Start()
    {
        mCamera = Camera.main;    
    }
    
    void Update()
    {
        transform.LookAt(transform.position + mCamera.transform.rotation * Vector3.back, mCamera.transform.rotation * Vector3.up);
    }
}
