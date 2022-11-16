using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class People : MonoBehaviour
{
    public Transform[] points;

    public int index = 0;

    NavMeshAgent agent;

    UIManager uIManager;

    public Action onCollisionCar = () => { };
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        uIManager = FindObjectOfType<UIManager>();
    }

    void Update()
    {
        RaycastHit hit;

        Debug.DrawRay(transform.position, transform.forward + transform.right, Color.red , 3f);

        if(Physics.Raycast(transform.position, transform.forward, out hit, 1f, 1 << 6))
        {
            agent.isStopped = true;

        }
        else
        {
            agent.isStopped = false;
        }
        Patrol();
    }

    void Patrol()
    {
        agent.destination = points[index].position;

        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            if (points.Length == 0)
                return;
            agent.destination = points[index].position;

            index = index + 1;
            if(index >= points.Length)
            {
                index = 0;
            }
        }

        Vector3 dir = points[index].localPosition - transform.localPosition;

        transform.LookAt(dir);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Car"))
        {
            agent.isStopped = true;


            if (collision.gameObject.GetComponent<Car>().isMove == true)
            {
                collision.gameObject.GetComponent<Car>().isMove = false;

                uIManager.GameOverTween();

                onCollisionCar();
                
            }
        }
    }
}
