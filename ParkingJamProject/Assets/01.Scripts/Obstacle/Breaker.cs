using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breaker : MonoBehaviour
{
    RaycastHit[] raycastHits;

    void Start()
    {
        
    }

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.red, 100f);

        raycastHits = Physics.RaycastAll(transform.position, transform.forward, 100f, 1 << 6);

        if(raycastHits.Length > 1)
        {
            Debug.Log("dkdk");
            int index = 0;
            for (int i = 0; i <raycastHits.Length; i++)
            {
                if (raycastHits[i].transform.GetComponent<Car>().isPass)
                {
                    Debug.Log(raycastHits[i]);
                    index++;
                    if (index == raycastHits.Length)
                    {
                        Debug.Log("OPEN");
                        OpenBreaker();
                    }
                }
            }
        }
    }

    void OpenBreaker()
    {
        gameObject.SetActive(false);
    }

}
