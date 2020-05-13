using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamSkill : SkillBase
{
    public LaserBeam laserbeam;
    public override void OnSkillAction(TpsController tpsController)
    {
        base.OnSkillAction(tpsController);
        LaserBeam LaserBeam_ = Instantiate(laserbeam);
        LaserBeam_.SetUpLaserBeam(tpsController);
    }
}
