using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashEffect : MonoBehaviour
{
    public ParticleSystem[] _crashEffect;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _crashEffect.Length; i++)
        {
            if (_crashEffect[i].gameObject.activeSelf)
            {
                if (_crashEffect[i].isStopped)
                {
                    _crashEffect[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
