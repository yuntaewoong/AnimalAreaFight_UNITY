using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkill : ScriptableObject
{
    public string mName;//��ų�̸�
    public int mSkillLevel;//�нú� ��ų ����
    protected List<GameObject> mGameObjects;//�нú꿡 ���� ������ ������Ʈ��
    public virtual void PassiveAdjust(GameObject parent) { }//�θ� ������Ʈ�� ���۷����� �޾ƿ� �нú� ��ų����(�����ʱ�ȭ ���� �����)
    public void DestroyPassiveObject() //�нú� ȿ���� ���� ������ ������Ʈ�� ����(�����ʱ�ȭ�Ҷ� �����)
    {
        foreach (var i in mGameObjects)
            Destroy(i);
    }
    public virtual int GetPassiveSkillIdentifier() { throw new NotImplementedException(); }//�� �нú꽺ų���� ������ Int�� ��ȯ
}
