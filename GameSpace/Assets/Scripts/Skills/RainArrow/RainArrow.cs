using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainArrow : SkillBase
{
    public ArrowGroup arrowGroup;
    private TpsController playerController;
    private Vector3 spawnArrowPosition;

    public override void OnSkillAction(TpsController tpsControllertpsController)
    {
        base.OnSkillAction(tpsControllertpsController);
        print(playerController);
        playerController = tpsControllertpsController;
        spawnArrowPosition = (playerController.GetCenterTransform().position + playerController.GetCenterTransform().forward);
        spawnArrowPosition.y += 8;
        ArrowGroup arrowSkill = Instantiate(arrowGroup, spawnArrowPosition,Quaternion.Euler(90,0,0));
        arrowSkill.SetUp(playerController, playerController.centerTransform);


    }
}
