using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyHammerSkill : SkillBase
{
    public ProjectileBase holyHammerPrefab;
    public override void OnSkillAction(TpsController tpsController)
    {
        Vector3 v = tpsController.GetCenterTransform().position +  tpsController.GetCenterTransform().forward;
        ProjectileBase hammer = Instantiate(holyHammerPrefab, v, tpsController.GetCenterTransform().rotation);
        hammer.SetUpOwner(tpsController);
    }
}
