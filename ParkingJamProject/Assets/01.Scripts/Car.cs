using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Car : MonoBehaviour
{
    float sightAngle = 90f;

    Rigidbody rb;

    public bool isMove = false;

    float speed = 15f;

    public Transform passPos;

    Vector3 curMoveDir;

    public Transform[] corners;

    public Transform targetCorner;


    int cornerIndex = 0;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isMove)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }

        for (int i = 0; i < corners.Length; i++)
        {
            if (Mathf.Round(transform.position.x) == Mathf.Round(corners[i].position.x) || Mathf.Round(transform.position.y) == Mathf.Round(corners[i].position.y))
            {
                Debug.Log("Stop");
                StopAllCoroutines();

                rb.velocity = Vector3.zero;

                transform.DORotate(new Vector3(0f, -90f, 0), 0.2f).OnComplete(() =>
                {
                    if (CheckAngle(targetCorner.position - transform.position))
                        return;
                    else
                        targetCorner = corners[cornerIndex + 1];
                });

            }
        }
    }

    public void Move(Vector3 dir)
    {
        float dot = Vector3.Dot(dir.normalized, -transform.right.normalized);

        float theta = Mathf.Acos(dot) * (180 / Mathf.PI);

        if (theta <= sightAngle)
        {
            Debug.Log("¾Õ");
            if (!isMove)
            {
                StartCoroutine(MoveCo(-transform.right));

                curMoveDir = -transform.right;
            }

        }
        else
        {
            Debug.Log("µÚ");
            if (!isMove)
            {
                StartCoroutine(MoveCo(transform.right));

                curMoveDir = transform.right;
            }


        }
    }

    public void PassCheck(Vector3 dir)
    {
        RaycastHit hit;

        Debug.DrawRay(transform.position, dir, Color.red, 20f);
        Debug.DrawRay(transform.position + new Vector3(0, 0, -1f), dir, Color.red, 20f);
        Debug.DrawRay(transform.position + new Vector3(0, 0, +1f), dir, Color.red, 20f);

        if (Physics.Raycast(transform.position + new Vector3(0, 0, -1f), dir, out hit, 20f, 1 << 6))
        {
            return;
        }
        if (Physics.Raycast(transform.position + new Vector3(0, 0, +1f), dir, out hit, 20f, 1 << 6))
        {
            return;
        }
        if (Physics.Raycast(transform.position, dir, out hit, 20f, 1 << 6))
        {
            return;
        }

        Pass();
    }


    IEnumerator MoveCo(Vector3 dir)
    {
        isMove = true;

        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

        PassCheck(dir);
        GetTargetCorner();

        while (true)
        {
            rb.velocity = dir * speed;

            if (targetCorner != null)
            {
                
            }

            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car") || collision.gameObject.CompareTag("Wall"))
        {
            StopAllCoroutines();

            ColKnockBack();

            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    void GetTargetCorner()
    {
        targetCorner = corners[0];

        for (int i = 0; i < corners.Length; i++)
        {
            if (Vector3.Distance(transform.position, targetCorner.position) > Vector3.Distance(transform.position, corners[i].position))
            {
                targetCorner = corners[i];
                cornerIndex = i;

                Debug.Log("¾Æ¾Ç");
            }
        }
    }


    bool CheckAngle(Vector3 dir)
    {
        float dot = Vector3.Dot(dir.normalized, -transform.right.normalized);

        float theta = Mathf.Acos(dot) * (180 / Mathf.PI);

        if(theta <= 90)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    void ColKnockBack()
    {
        if (isMove)
        {
            Debug.Log("³Ë¹é");

            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            rb.AddForce(-curMoveDir * 2.5f, ForceMode.Impulse);

            Invoke("FreezePos", 0.3f);
        }
    }



    public void Pass()
    {
        this.gameObject.layer = 0;


        Debug.Log("ÆÐ½º");
    }

    void FreezePos()
    {
        isMove = false;
    }
}

