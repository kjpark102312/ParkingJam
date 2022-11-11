using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{

    public Transform passPos;
    public Transform[] corners;
    public Transform targetCorner;

    public bool isMove = false;
    bool isPassing = false;
    bool isPass = false;

    float sightAngle = 90f;
    float speed = 15f;

    public int cornerIndex = 0;

    Rigidbody rb;

    public Vector3 curMoveDir;

    bool isGameOver = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isMove)
        {
            if (isGameOver)
            {
                rb.constraints = RigidbodyConstraints.None;
                return;
            }
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }


        if (isPass)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, curMoveDir, out hit, 5f, 1 << 6))
            {
                if (hit.transform.CompareTag("Car"))
                {
                    if(hit.transform.GetComponent<Car>().isPassing == true)
                    {
                        StopAllCoroutines();
                        rb.velocity = Vector3.zero;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                rb.velocity = curMoveDir * speed;
                Debug.Log("sad");
            }
        }

        if (targetCorner != null && !isPassing)
        {
            if (Mathf.Floor(transform.localPosition.x) == Mathf.Floor(targetCorner.localPosition.x)
                || Mathf.Floor(transform.localPosition.z) == Mathf.Floor(targetCorner.localPosition.z))
            {
                StopAllCoroutines();
                Pass();

                rb.velocity = Vector3.zero;

                isPassing = true;
                isPass = false;

                float angle;

                if (curMoveDir == -transform.right.normalized)
                    angle = transform.localEulerAngles.y + 90f;
                else
                    angle = transform.localEulerAngles.y - 90f;


                if (transform.localEulerAngles.y == 270 || transform.localEulerAngles.y == 90)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, targetCorner.localPosition.z);
                }
                else
                {
                    transform.localPosition = new Vector3(targetCorner.localPosition.x, transform.localPosition.y, transform.localPosition.z);
                }

                transform.DORotate(new Vector3(0f, angle, 0), 0.2f).OnComplete(() =>
                    {
                        if (CheckAngle(targetCorner.position - transform.position))
                        {

                        }
                        else
                            targetCorner = corners[cornerIndex + 1];
                        cornerIndex = cornerIndex + 1;

                        StartCoroutine(PassCo());
                    });
            }
        }
    }


    void CheckPass()
    {
        RaycastHit hit;

        float distance = targetCorner.position.x - transform.position.x;

        if (Physics.Raycast(transform.position, curMoveDir, out hit, distance - 2f, 1 << 6))
        {
            if (hit.transform.CompareTag("Car"))
            {
                isPass = false;
            }
        }
        else
        {
            isPass = true;
        }
    }

    IEnumerator PassCo()
    {
        isMove = true;
        isPass = false;

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


    IEnumerator MoveCo(Vector3 dir)
    {
        isMove = true;
        isPassing = false;

        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

        GetTargetCorner();

        for (int i = 0; i < corners.Length - 1; i++)
        {
            if (CheckAngle(corners[i].transform.position - transform.position))
            {
                if (dir == -transform.right.normalized)
                {
                    targetCorner = corners[i];
                    cornerIndex = i - 1;
                }
            }
            else
            {
                if (dir == transform.right.normalized)
                {
                    targetCorner = corners[i];
                    cornerIndex = i - 1;

                }
            }
        }
        CheckPass();
        while (true)
        {
            rb.velocity = dir * speed;

            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Obstacle"))
        {
            StopAllCoroutines();

            ColKnockBack();
            CrashAnim(collision.gameObject);

            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        if(collision.gameObject.CompareTag("People"))
        {
            isGameOver = true;
            isMove = false;

            StopAllCoroutines();

            Invoke("GameOver", 1f);
        }
    }

    void CrashAnim(GameObject crashObj)
    {
        crashObj.transform.DORotate(new Vector3(curMoveDir.x* 10f, crashObj.transform.localEulerAngles.y, crashObj.transform.localEulerAngles.z), 0.2f).SetLoops(2,LoopType.Yoyo);
    }

    void GameOver()
    {
        rb.AddForce(-curMoveDir * 5f, ForceMode.Impulse);
        rb.AddForce(transform.up * 12f, ForceMode.Impulse);
        rb.AddTorque(transform.right * 12f, ForceMode.Impulse);

        Debug.Log("게임 오버");
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

        if (theta <= 90)
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
            if (isGameOver)
                return;

            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            rb.AddForce(-curMoveDir * 2.5f, ForceMode.Impulse);

            Invoke("FreezePos", 0.3f);
        }
    }

    public void Pass()
    {
        this.gameObject.layer = 0;
        this.gameObject.GetComponent<BoxCollider>().isTrigger = true;

        Debug.Log("패스");
    }

    void FreezePos()
    {
        isMove = false;
    }
}

