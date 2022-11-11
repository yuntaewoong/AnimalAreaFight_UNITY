using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PassiveSkill_BuildTurret : PassiveSkill
{
    [SerializeField] private GameObject mTurretPrefab;
    public override void PassiveAdjust(GameObject parent)
    {
        parent.GetComponent<Player>().SetTurret(mTurretPrefab);
        parent.GetComponent<Player>().SetPassiveTurretLevel(mSkillLevel);
    }

    public override int GetPassiveSkillIdentifier()
    {
        return 0;
    }
}
