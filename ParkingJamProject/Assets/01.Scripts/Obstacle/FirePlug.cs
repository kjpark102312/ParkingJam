using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePlug : TriggerObstacle
{
    public GameObject curActiveObj;
    public GameObject afterActionObj;
    void ActionFirePlug()
    {
        curActiveObj.SetActive(false);
        afterActionObj.SetActive(true);
    }

    public override void Triggered()
    {
        base.Triggered();
        ActionFirePlug();
    }
}
