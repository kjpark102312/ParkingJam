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
    public bool isPass = false;
    public bool isGameOver = false;
    public bool isPassing = false;
    bool isCol = false;

    float sightAngle = 90f;
    float speed = 10f;

    public int cornerIndex = 0;

    Rigidbody rb;

    public Vector3 curMoveDir;

    People[] people;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        people = FindObjectsOfType<People>();

        for (int i = 0; i < people.Length; i++)
        {
            people[i].onCollisionCar += () =>
            {
                if (isGameOver)
                {
                    StopAllCoroutines();

                    rb.velocity = Vector3.zero;

                    Invoke("GameOver", 1f);
                }
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
        else
        {
            if (!isPassing)
            {
                if (transform.localEulerAngles.y == 270 || transform.localEulerAngles.y == 90)
                {
                    rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX;
                }
                else
                {
                    rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
                }
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezePositionY;
            }
        }
        
        
        // 중복 이동 제한 함수
        

        //if (isCol)
        //{
        //    rb.velocity = Vector3.zero;
        //}

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

    private void FixedUpdate()
    {
        if (isPass)
        {
            RaycastHit hit;
            int layerMask = 1 << 6 | 1 << 7 | 1 << 9;
            if (Physics.Raycast(transform.position, curMoveDir, out hit, 10f, layerMask))
            {
                if (hit.transform.CompareTag("Car"))
                {
                    if (hit.transform.GetComponent<Car>().isPassing == true)
                    {
                        StopAllCoroutines();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                rb.velocity = curMoveDir * speed;
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
            distance = targetCorner.position.z - transform.position.z;
        }
        else
        {
            distance = targetCorner.position.x - transform.position.x;
        }

        int layerMask = 1 << 6 | 1 << 7 | 1 << 9;

        Debug.DrawLine(transform.position, curMoveDir * (distance - 2f), Color.red);

        distance = Mathf.Abs(distance);

        Debug.Log(distance);
        Debug.Log(curMoveDir);

        if (Physics.Raycast(transform.position, curMoveDir, out hit, distance, layerMask))
        {
            if (hit.transform.CompareTag("Car") && hit.transform.CompareTag("Obstacle") && hit.transform.CompareTag("Wall"))
            {
                isPass = false;

                Debug.Log(hit.transform.tag);
            }
        }
        else
        {
            isPass = true;
            Debug.Log("아무것도없음");
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
                transform.DORotate(new Vector3(0f, transform.localEulerAngles.y + 90, 0), 0.15f);

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
                curMoveDir = -transform.right;

                StartCoroutine(MoveCo(-transform.right));

            }
        }
        else
        {
            if (!isMove)
            {
                curMoveDir = transform.right;

                StartCoroutine(MoveCo(transform.right));

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

            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Car") || collision.gameObject.CompareTag("Obstacle"))
        {
            isPass = false;
            StopAllCoroutines();

            rb.velocity = Vector3.zero;

            Invoke("ColKnockBack", 0.05f);
            //ColKnockBack();

            Debug.Log("충돌!!"+this.gameObject + this.gameObject.transform.position);
                
            isCol = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Obstacle"))
        {
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
            rb.angularVelocity = Vector3.zero;

            rb.constraints = RigidbodyConstraints.FreezeRotation;

            rb.AddForce(-curMoveDir * 2.5f, ForceMode.Impulse);

            Debug.Log(-curMoveDir);

            Invoke("FreezePos", 0.1f);
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

//2022-11-18
/*
 * 오늘은 기분이 좋은 날이다.
 * 왜냐 버그를 고쳤기 때문이다.
 * 그리고 금요일이기 때문
 * 냐하 그리고 나는 집에가서 게임을 할거에요
 * 무엇?
 * 피퐈
 * 롤
 * 롤체
 * 로아
 * 오버웣취
 */