using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : TriggerObstacle
{

    public override void Triggered()
    {
        base.Triggered();
        Invoke("BreakObj", 0.5f);
    }

    void BreakObj()
    {
        gameObject.SetActive(false);
    }
}
