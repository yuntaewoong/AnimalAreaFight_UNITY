using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RotatingBall : MonoBehaviour
{
    [SerializeField] private float mRotationSpeed;//회전 속도
    [SerializeField] private Vector3 mRadiusVector;//회전 반경
    [SerializeField] private float mBallHeight;//ball의 y높이
    private Transform mParentTransform;//부모 트랜스폼
    private float mRotatingValue = 0.0f;//회전값(360:1바퀴)
    private float mKnockBackPower;
    public void SetParentTransform(Transform transform)
    {
        mParentTransform = transform;
    }
    public void SetRotatingValue(float rotatingValue)
    {
        mRotatingValue = rotatingValue;
    }
    public void SetKnockBackPower(float knockBackPower)
    {
        mKnockBackPower = knockBackPower;
    }
    public float GetKnockBackPower()
    {
        return mKnockBackPower;
    }
    private void RotateAroundParent()
    {
        mRotatingValue += mRotationSpeed * Time.deltaTime;
        Vector3 offset = Quaternion.AngleAxis(mRotatingValue, Vector3.up) * mRadiusVector;
        transform.position = mParentTransform.position + offset + new Vector3(0, mBallHeight,0);
    }
    private void Update()
    {
        RotateAroundParent();
    }
}
