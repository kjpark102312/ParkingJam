using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Car : MonoBehaviour
{

    #region Attributes
    public Transform passPos;
    public Transform[] corners;
    public Transform targetCorner;

    public bool isMove = false;
    public bool isPass = false;
    public bool isGameOver = false;
    public bool isPassing = false;
    private bool isFirst = false;
    bool isCol = false;

    float sightAngle = 90f;
    float speed = 15f;

    public int cornerIndex = 0;

    Rigidbody rb;

    public Vector3 curMoveDir;

    People[] people;

    Stage curstageInfo;

    private Action carPass = () => { };
    private TimeLimitCar timeLimitCar;

    private CoinEffect effect;

    
    private CrashEffect crashEffect;
    #endregion


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        people = FindObjectsOfType<People>();
        curstageInfo = FindObjectOfType<Stage>();
        timeLimitCar = FindObjectOfType<TimeLimitCar>();
        effect = FindObjectOfType<CoinEffect>();
        crashEffect = FindObjectOfType<CrashEffect>();

        if (timeLimitCar!=null)
        {
            carPass += timeLimitCar.CarPass;
        }

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

    protected virtual void Update()
    {
        // �ڵ�����ü �����̼� ���
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

        if (targetCorner != null && !isPassing)
        {
            if (!isFirst)
            {
                if (Mathf.Floor(transform.position.x) == Mathf.Floor(targetCorner.localPosition.x)
                || Mathf.Floor(transform.position.z) == Mathf.Floor(targetCorner.localPosition.z) )
                {
                    StopAllCoroutines();
                    Pass();

                    rb.velocity = Vector3.zero;

                    isPassing = true;
                    isPass = false;
                    isFirst = true;

                    carPass();
                    effect.PlayEffect(transform.position);
                    StageManager.Instance.getGoldCount += 2;

                    float angle;

                    if (curMoveDir == -transform.right.normalized)
                        angle = transform.localEulerAngles.y + 90f;
                    else
                        angle = transform.localEulerAngles.y - 90f;


                    if (transform.localEulerAngles.y == 270 || transform.localEulerAngles.y == 90)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, targetCorner.position.z);
                    }
                    else
                    {
                        transform.position = new Vector3(targetCorner.position.x, transform.position.y, transform.position.z);
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

    // �ڵ����� ���������� Pass�� �������� üũ�ϴ� �Լ�
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
        }
    }


    // Pass������ �������� ���� �ڵ����� ����Ǵ� �ڷ�ƾ
    IEnumerator PassCo()
    {
        isMove = true;
        isPass = false;

        rb.constraints = RigidbodyConstraints.FreezePositionY;

        while (true)
        {
            if (Vector3.Distance(transform.position, targetCorner.position) <= 0.9f)
            {
                cornerIndex = cornerIndex + 1;

                if (cornerIndex < corners.Length)
                {
                    transform.DORotate(new Vector3(0f, transform.localEulerAngles.y + 90, 0), 0.15f);
                    targetCorner = corners[cornerIndex];
                }
            }

            rb.velocity = -transform.right.normalized * speed;

            yield return null;
        }
    }


    // �������� �ȿ��� MoveCo �ڷ�ƾ�� �����ϴ� �Լ�
    public virtual void Move(Vector3 dir)
    {
        if(curstageInfo != null)
        {
            if (curstageInfo.mode == Stage.StageMode.limitMove)
            {

                if (curstageInfo.moveCount <= 0)
                {
                    return;
                }

                curstageInfo.UpdateMovecount();
            }
        }

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

    // �������� �����Ӱ� �ڳʺκ� �����ϴ� �Լ�
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
        if(collision.gameObject.CompareTag("People"))
        {
            Debug.Log("asd");
            rb.velocity = Vector3.zero;
        }

        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Car") || collision.gameObject.CompareTag("Obstacle"))
        {
            isPass = false;
            StopAllCoroutines();

            rb.velocity = Vector3.zero;

            Invoke("ColKnockBack", 0.05f);

            if(GameManager.Instance.IsVibrate)
            {
                Handheld.Vibrate();
            }
            //ColKnockBack();

            if(isMove)
            {
                for (int i = 0; i < crashEffect.crashEffect.Length; i++)
                {
                    if (crashEffect.crashEffect[i].gameObject.activeSelf)
                        continue;

                    crashEffect.crashEffect[i].gameObject.SetActive(true);
                    crashEffect.crashEffect[i].Play();
                    crashEffect.crashEffect[i].transform.position = collision.contacts[0].point;

                    if (transform.localEulerAngles.y == 270 || transform.localEulerAngles.y == 90)
                    {
                        crashEffect.crashEffect[i].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                    }
                    else
                    {
                        crashEffect.crashEffect[i].transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                    }
                    break;
                }
            }
            

            Debug.Log("�浹!!"+this.gameObject + this.gameObject.transform.position);
                
            isCol = true;
        }
    }

    // �ε������� ����
    void CrashAnim(GameObject crashObj)
    {
        crashObj.transform.DORotate(new Vector3(curMoveDir.x * 10f, crashObj.transform.localEulerAngles.y, crashObj.transform.localEulerAngles.z), 0.2f).SetLoops(2, LoopType.Yoyo);
    }

    // ���ӿ��� ����
    void GameOver()
    {
        rb.AddForce(-curMoveDir * 5f, ForceMode.Impulse);
        rb.AddForce(transform.up * 10f, ForceMode.Impulse);
        rb.AddTorque(transform.right * 10f, ForceMode.Impulse);

        Debug.Log("���� ����");
    }

    // targetCorner������ Null���̸� �ȵǱ� ������ ���Ƿ� �־��ִ� �Լ�
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

    // ��ü ����, �ĸ� üũ���ִ� �Լ�
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

    // �ε������� ����2
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

    // Pass������ �ɼǵ� �������ִ� �Լ�
    public void Pass()
    {
        this.gameObject.layer = 0;
        this.gameObject.GetComponent<BoxCollider>().isTrigger = true;

    }

    //isMove �����ϴ� �Լ�
    void FreezePos()
    {
        isMove = false;
    }
}
