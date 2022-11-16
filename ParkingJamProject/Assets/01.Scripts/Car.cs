using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Car : MonoBehaviour
{
    public Transform passPos;
    public Transform[] corners;
    public Transform targetCorner;

    public bool isMove = false;
    bool isPassing = false;
    public bool isPass = false;

    float sightAngle = 90f;
    float speed = 15f;

    public int cornerIndex = 0;

    Rigidbody rb;

    public Vector3 curMoveDir;

    bool isGameOver = false;

    People[] people;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        people = FindObjectsOfType<People>();
        
        for (int i = 0; i < people.Length; i++)
        {
            people[i].onCollisionCar += () =>
            {
                isGameOver = true;

                StopAllCoroutines();

                rb.velocity = Vector3.zero;

                Invoke("GameOver", 1f);
            };
        }
    }

    private void Update()
    {
        // 자동차객체 로테이션 제어문
        if (!isMove)
        {
            if (isGameOver)
            {
                rb.constraints = RigidbodyConstraints.None;
                return;
            }
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
        }

        // 중복 이동 제한 함수
        if (isPass)
        {
            RaycastHit hit;
            int layerMask = 1 << 6 | 1 << 7;
            if (Physics.Raycast(transform.position, curMoveDir, out hit, 10f, layerMask))
            {
                if (hit.transform.CompareTag("Car"))
                {
                    if (hit.transform.GetComponent<Car>().isPassing == true)
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
            }
        }

        // Pass 했을때 자동차객체 위치 및 로테이션 제어문
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

    // 자동차를 움직였을때 Pass가 가능한지 체크하는 함수
    void CheckPass()
    {
        RaycastHit hit;
        float distance;
        if (transform.localEulerAngles.y == 270 || transform.localEulerAngles.y == 90)
        {
            distance = targetCorner.position.x - transform.position.x;
        }
        else
        {
            distance = targetCorner.position.z - transform.position.z;
        }

        int layerMask = 1 << 6 | 1 << 7 | 1 << 8;

        Debug.DrawLine(transform.position, curMoveDir * (distance - 2f), Color.red);

        if (Physics.Raycast(transform.position, curMoveDir, out hit, distance - 2f, layerMask))
        {
            if (hit.transform.CompareTag("Car") && hit.transform.CompareTag("Obstacle"))
            {
                isPass = false;
            }
        }
        else
        {
            isPass = true;
        }
    }


    // Pass했을때 움직임을 위해 자동으로 진행되는 코루틴
    IEnumerator PassCo()
    {
        isMove = true;
        isPass = false;

        rb.constraints = RigidbodyConstraints.FreezePositionY;

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


    // 스테이지 안에서 MoveCo 코루틴을 제어하는 함수
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



    // 실질적인 움직임과 코너부분 제어하는 함수
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

            Debug.Log("asd");

                yield return null;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log(this.gameObject.name +','+ collision.gameObject.tag);

            StopAllCoroutines();
            rb.velocity = Vector3.zero;
            
            ColKnockBack();
            //CrashAnim(collision.gameObject);
        }
    }


    // 부딪혔을때 연출
    void CrashAnim(GameObject crashObj)
    {
        crashObj.transform.DORotate(new Vector3(curMoveDir.x * 10f, crashObj.transform.localEulerAngles.y, crashObj.transform.localEulerAngles.z), 0.2f).SetLoops(2, LoopType.Yoyo);
    }

    // 게임오버 연출
    void GameOver()
    {
        rb.AddForce(-curMoveDir * 5f, ForceMode.Impulse);
        rb.AddForce(transform.up * 10f, ForceMode.Impulse);
        rb.AddTorque(transform.right * 10f, ForceMode.Impulse);

        Debug.Log("게임 오버");
    }

    // targetCorner변수가 Null값이면 안되기 때문에 임의로 넣어주는 함수
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

    // 객체 정면, 후면 체크해주는 함수
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

    // 부딪혔을때 연출2
    void ColKnockBack()
    {
        if (isMove)
        {
            if (isGameOver)
                return;

            StopAllCoroutines();

            rb.velocity = Vector3.zero;

            rb.constraints = RigidbodyConstraints.FreezeRotation;

            rb.AddForce(-curMoveDir * 2.5f, ForceMode.Impulse);

            Debug.Log(-curMoveDir);

            Invoke("FreezePos", 0.3f);
        }
    }

    // Pass했을때 옵션들 제어해주는 함수
    public void Pass()
    {
        this.gameObject.layer = 0;
        this.gameObject.GetComponent<BoxCollider>().isTrigger = true;

        Debug.Log("패스");
    }

    //isMove 제어하는 함수
    void FreezePos()
    {
        isMove = false;
    }
}

