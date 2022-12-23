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

    Animator anim;
    Rigidbody rb;

    public Action onCollisionCar = () => { };
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        Debug.DrawRay(transform.position, transform.forward, Color.red, 1f);

        Patrol();
    }

    void Patrol()
    {
        if(!agent.isStopped)
        {
            agent.destination = points[index].position;

            if (!agent.pathPending && agent.remainingDistance < 0.1f)
            {
                if (points.Length == 0)
                    return;
                agent.destination = points[index].position;

                index = index + 1;
                if (index >= points.Length)
                {
                    index = 0;
                }
            }

            Vector3 dir = points[index].localPosition - transform.localPosition;

            anim.SetBool("IsWalk", true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            rb.constraints = RigidbodyConstraints.FreezePosition;

            transform.LookAt(collision.transform);

            anim.SetBool("IsWalk", false);
            anim.SetTrigger("ColCar");

            if (collision.gameObject.GetComponent<Car>().isMove == true)
            {
                collision.gameObject.GetComponent<Car>().isMove = false;
                collision.gameObject.GetComponent<Car>().isGameOver = true;

                UIManager.Instance.GameOverTween();

                onCollisionCar();
            }
        }
    }
}
