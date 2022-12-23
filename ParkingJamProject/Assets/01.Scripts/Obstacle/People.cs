using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class People : MonoBehaviour
{
    public Transform[] points;

    public int index = 0;

    NavMeshAgent agent;

    Animator anim;
    Rigidbody rb;

    public UnityAction onCollisionCar = () => { };
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();

        StartCoroutine(Patrol());
    }

    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit,1f))
        {
            if(hit.collider.CompareTag("Car"))
            {
                agent.isStopped = true;
                anim.SetBool("IsWalk", false);
            }
            else
            {
                agent.isStopped = false;
                anim.SetBool("IsWalk", true);
            }    
        }
        else
        {
            agent.isStopped = false;
            anim.SetBool("IsWalk", true);
        }
        Debug.DrawRay(transform.position, transform.forward, Color.red, 1f);

        
    }

    IEnumerator Patrol()
    {
        Vector3 dir;
        Vector3 dir2;
        while (!agent.isStopped)
        {
            dir = new Vector3((points[index].position - transform.position).x, transform.position.y, (points[index].position - transform.position).z);
            dir2 = new Vector3(points[index].position.x, transform.position.y, points[index].position.z);
            agent.destination = points[index].position;

            Debug.Log(dir);

            transform.LookAt(dir2);

            if (!agent.pathPending && agent.remainingDistance < 1f)
            {
                if (points.Length == 0)
                    continue;
                agent.destination = points[index].position;

                index = index + 1;
                if (index >= points.Length)
                {
                    index = 0;
                }
            }

            transform.DOMove(transform.position + dir.normalized, 0.2f);
            Debug.Log("sad");
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            anim.SetBool("IsWalk", false);

            if (collision.gameObject.GetComponent<Car>().isMove == true)
            {
                rb.constraints = RigidbodyConstraints.FreezePosition;

                transform.LookAt(collision.transform);

                anim.SetTrigger("ColCar");

                collision.gameObject.GetComponent<Car>().isMove = false;
                collision.gameObject.GetComponent<Car>().isGameOver = true;

                UIManager.Instance.GameOverTween();

                onCollisionCar();
            }
            else
            {

            }
        }
    }
}
