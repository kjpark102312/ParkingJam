using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Car : MonoBehaviour
{
    
    public Transform passPos;
    public Transform[] corners;
    public Transform targetCorner;

    public bool isMove = false;
    bool isPassing = false;

    float sightAngle = 90f;
    float speed = 15f;

    public int cornerIndex = 0;

    Rigidbody rb;

    Vector3 curMoveDir;

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

        
        if(targetCorner != null && !isPassing)
        {
            Debug.Log(Mathf.Floor(transform.localPosition.y));
            Debug.Log(Mathf.Floor(targetCorner.localPosition.z));

            if (Mathf.Floor(transform.localPosition.x) == Mathf.Floor(targetCorner.localPosition.x) 
                || Mathf.Floor(transform.localPosition.z) == Mathf.Floor(targetCorner.localPosition.z))
            {
                Debug.Log("Stop");
                StopAllCoroutines();

                rb.velocity = Vector3.zero;

                isPassing = true;

                float angle;

                if (curMoveDir == -transform.right.normalized)
                    angle = transform.localEulerAngles.y + 90f;
                else
                    angle = transform.localEulerAngles.y - 90f;

                if (Mathf.Abs(transform.localEulerAngles.y) == 90)
                {
                    Debug.LogError("dwddw");
                    transform.position = new Vector3(transform.position.x, transform.position.y, targetCorner.position.z);
                }
                else
                    transform.position = new Vector3(targetCorner.position.x, transform.position.y, transform.position.z);

                transform.DORotate(new Vector3(0f, angle, 0), 0.2f).OnComplete(() =>
                    {
                        if (CheckAngle(targetCorner.position - transform.position))
                        {

                        }
                        else
                            targetCorner = corners[cornerIndex + 1];
                        cornerIndex = cornerIndex + 1;



                        
                        StartCoroutine(PassCo());

                        Debug.Log("asd");

                    });
            }
        }
    }

    IEnumerator PassCo()
    {

        isMove = true;

        rb.constraints = RigidbodyConstraints.None;

        while (true)
        {
            if (Vector3.Distance(transform.position, targetCorner.position) <= 1)
            {
                Debug.Log(transform.localEulerAngles.y);
                transform.DORotate(new Vector3(0f, transform.localEulerAngles.y + 90, 0), 0.2f);

                if (cornerIndex < corners.Length)
                {
                    targetCorner = corners[cornerIndex + 1];
                    cornerIndex = cornerIndex + 1;
                }
                Debug.Log(cornerIndex);
            }

            rb.velocity = -transform.right.normalized * speed;

            yield return null;
        }
    }

    public void Move(Vector3 dir)
    {
        float dot = Vector3.Dot(dir.normalized, -transform.right.normalized);

        float theta = Mathf.Acos(dot) * (180 / Mathf.PI);

        if (theta <= sightAngle)
        {
            if (!isMove)
            {
                StartCoroutine(MoveCo(-transform.right));

                curMoveDir = -transform.right;
            }

        }
        else
        {
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

        for (int i = 0; i < corners.Length-1; i++)
        {
            if (CheckAngle(corners[i].transform.position - transform.position))
            {
                if(dir == -transform.right.normalized)
                {
                    targetCorner = corners[i];
                    cornerIndex = i-1;
                }
            }
            else
            {
                if (dir == transform.right.normalized)
                {
                    targetCorner = corners[i];
                    cornerIndex = i-1;

                }
            }
        }

        while (true)
        {
            rb.velocity = dir * speed;

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
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            rb.AddForce(-curMoveDir * 2.5f, ForceMode.Impulse);

            Invoke("FreezePos", 0.3f);
        }
    }



    public void Pass()
    {
        this.gameObject.layer = 0;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    void FreezePos()
    {
        isMove = false;
    }
}

