using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UILabel;

public class CrashEffect : MonoBehaviour
{
    public ParticleSystem[] crashEffect;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < crashEffect.Length; i++)
        {
            if (crashEffect[i].gameObject.activeSelf)
            {
                if (crashEffect[i].isStopped)
                {
                    crashEffect[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
