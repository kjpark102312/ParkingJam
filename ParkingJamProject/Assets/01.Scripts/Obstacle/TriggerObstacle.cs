using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObstacle : MonoBehaviour, ITriggerObj
{
    public int maxTriggerCount = 1;

    public int spawnObstacleCount = 8;

    int curTriggerCount;

    public virtual void Triggered()
    {

    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Car"))
        {
            curTriggerCount++;
            if (maxTriggerCount == curTriggerCount)
                Triggered();

        }
    }

    
}
