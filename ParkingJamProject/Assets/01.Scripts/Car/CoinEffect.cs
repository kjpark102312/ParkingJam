using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem[] effects;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            effects[i] = transform.GetChild(i).GetComponent<ParticleSystem>();
        }
    }

    private void Update()
    {
        for (int i = 0; i < effects.Length; i++)
        {
            if(effects[i].gameObject.activeSelf)
            {
                if (effects[i].isStopped)
                {
                    effects[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void PlayEffect(Vector3 position)
    {
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].gameObject.activeSelf)
            {
                continue;
            }


            effects[i].gameObject.SetActive(true);
            effects[i].Play();
            effects[i].gameObject.transform.position = position;
            break;
        }
    }
}
