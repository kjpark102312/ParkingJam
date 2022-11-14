using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : TriggerObstacle
{

    public override void Triggered()
    {
        base.Triggered();
        gameObject.SetActive(false);
    }
}
