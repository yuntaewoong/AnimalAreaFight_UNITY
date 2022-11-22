using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClone : MonoBehaviour
{
    private GameObject mActualPlayer;//�� �н��� ���ӵǴ� �÷��̾� ���۷���                                     
    private Vector3 mVectorToPlayer;//�÷��̾�� �����Ǿ�� �ϴ� �Ÿ�����
    private Animator mAnimator;
    private Animator mOriginalAnimator;
    public void SetActualPlayer(GameObject player)
    {
        mActualPlayer = player;
        mVectorToPlayer = mActualPlayer.transform.position - transform.position;
        mOriginalAnimator = mActualPlayer.GetComponent<Animator>();
    }

    private void UpdateCloneTransform()//�н��� ������ mVectorToPlayer��ŭ �������� �����ϰ� ������
    {
        transform.position = mActualPlayer.transform.position - mVectorToPlayer;//position ī��
        transform.rotation = mActualPlayer.transform.rotation;//rotation ī��
        mAnimator.SetBool("run", mOriginalAnimator.GetBool("run"));//�ִϸ��̼� ī��
    }
    private void Start()
    {
        mAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        UpdateCloneTransform();
        if(GameManager.instance.GetGameState() == GameState.Battle && !GameManager.instance.IsWallTime())
        {
            this.enabled = false;
        }
    }

}
