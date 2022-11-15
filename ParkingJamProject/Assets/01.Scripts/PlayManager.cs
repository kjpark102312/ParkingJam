using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    Car[] cars;
    Goal goal;
    void Start()
    {
        cars = FindObjectsOfType<Car>();
        goal = FindObjectOfType<Goal>();
    }

    void Update()
    {
        
    }

    public void CheckClear()
    {
        int count = 0;

        for (int i = 0; i < cars.Length; i++)
        {
            if (!cars[i].gameObject.activeSelf)
            {
                count++;
            }
        }

        if (count == cars.Length)
        {
            Debug.Log("Clear");

            //�� �Լ� ����ɶ� üũ�ϱ�
        }
    }
}
