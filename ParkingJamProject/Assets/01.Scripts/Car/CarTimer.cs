using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTimer : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("Stage") == 0)
        {
            gameObject.SetActive(false);
            return;
        }


        if (PlayerPrefs.GetInt("Stage") % 3 == 0)
            gameObject.SetActive(true);
        else
        {
            gameObject.SetActive(false);

        }
    }
}
