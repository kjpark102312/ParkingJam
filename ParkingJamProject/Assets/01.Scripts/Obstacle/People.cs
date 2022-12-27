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

    Animator anim = null;
    Rigidbody rb = null;

    public UnityAction onCollisionCar = () => { };

    private bool isPatrol = true;

    IEnumerator patrolCo;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();

        patrolCo = Patrol();
        StartCoroutine(Patrol());
    }

    void Update()
    {
        if(isPatrol)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
            {
                if (hit.collider.CompareTag("Car"))
                {
                    StopAllCoroutines();
                    anim.SetBool("IsWalk", false);
                    isPatrol = false;
                }
                else
                {
                    if(isPatrol == false)
                    {
                        StartCoroutine(Patrol());
                       
                    }
                    anim.SetBool("IsWalk", true);
                    isPatrol = true;

                }
            }
            else
            {
                if(isPatrol == false)
                {
                    StartCoroutine(Patrol());
                }
                anim.SetBool("IsWalk", true);
                isPatrol = true;
            }
        }
    }

    IEnumerator Patrol()
    {
        Vector3 dir;
        Vector3 dir2;
        index = 0;
        Debug.Log(points.Length);

        while (isPatrol)
        {
            dir = new Vector3((points[index].position - transform.position).x, 0 , (points[index].position - transform.position).z);
            dir2 = new Vector3(points[index].position.x, transform.position.y, points[index].position.z);

            transform.LookAt(dir2);

            if (Vector3.Distance(transform.position, points[index].position) <= 1f)
            {
                if (points.Length == 0)
                    continue;

                index = index + 1;
                if (index >= points.Length)
                {
                    index = 0;
                }
            }
            Debug.DrawRay(transform.position, dir.normalized, Color.red, 1f);
            transform.DOMove(transform.position + dir.normalized, 0.2f);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            StopAllCoroutines();
            isPatrol = false;
            anim.SetBool("IsWalk", false);

            if (collision.gameObject.GetComponent<Car>().isMove == true)
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;

                transform.LookAt(new Vector3(collision.transform.position.x, transform.position.y, collision.transform.position.z));

                anim.SetTrigger("ColCar");

                collision.gameObject.GetComponent<Car>().isMove = false;
                collision.gameObject.GetComponent<Car>().isGameOver = true;
                collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

                UIManager.Instance.GameOverTween();

                onCollisionCar();
            }
            else
            {

            }
        }
    }
}
