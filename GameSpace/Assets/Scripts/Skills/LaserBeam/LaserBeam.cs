using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : DamageBase
{
    LineRenderer laser;
    private bool onAcitve;
    private bool canDoDamage;
    private TpsController playerController;
    private float counter;
    private float delayDamageCounter;
    private void Start()
    {
        laser = GetComponent<LineRenderer>();
        laser.enabled = false;   
    }
    private void Update()
    {
        if (onAcitve)
        {
            counter += Time.deltaTime;
            if (counter > 5)
            {
                onAcitve = false;
                Destroy(gameObject);  
            }
            if (canDoDamage==false)
            {
                delayDamageCounter += Time.deltaTime;
                if (delayDamageCounter > 0.3f)
                {
                    delayDamageCounter = 0;
                    canDoDamage = true;
                }
             
            }
            laser.enabled = true;
            laser.SetPosition(0, playerController.GetCenterTransform().position);
            laser.SetPosition(1, playerController.GetCenterTransform().position+(playerController.GetCenterTransform().forward * 6));

            RaycastHit hit;
      
            if (Physics.Raycast(playerController.GetCenterTransform().position, playerController.GetCenterTransform().forward, out hit, 6))
            {
                TpsController target = hit.transform.GetComponent<TpsController>();
                if (target != null)
                {
                    if (GetOwner() != target)
                    {
                        if (canDoDamage)
                        {
                            DoDamage(target);
                            canDoDamage = false;
                        }
                       
                    }
                }
               
             
            }
        }
       
    }

    public void SetUpLaserBeam(TpsController playerController_)
    {
        playerController = playerController_;
        SetUpOwner(playerController);
        onAcitve = true;
     
    }

}
