using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyHammerSkill : SkillBase
{
    public HolyHammer holyHammerPrefab;
    public override void OnSkillAction(TpsController tpsController)
    {
        Vector3 v = tpsController.GetCenterTransform().position +  tpsController.GetCenterTransform().forward;
        HolyHammer hammer = Instantiate(holyHammerPrefab, v, tpsController.GetCenterTransform().rotation);
        hammer.SetUpOwner(tpsController);
    }
}
