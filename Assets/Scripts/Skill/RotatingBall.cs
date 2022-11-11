using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RotatingBall : MonoBehaviour
{
    [SerializeField] private float mRotationSpeed;//ȸ�� �ӵ�
    [SerializeField] private Vector3 mRadiusVector;//ȸ�� �ݰ�
    [SerializeField] private float mBallHeight;//ball�� y����
    private Transform mParentTransform;//�θ� Ʈ������
    private float mRotatingValue = 0.0f;//ȸ����(360:1����)
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