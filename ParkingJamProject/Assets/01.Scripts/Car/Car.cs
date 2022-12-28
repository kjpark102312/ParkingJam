using DG.Tweening;
using System;
using System.Collections;
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

    private bool _isFirst = false;
    private bool _isCol = false;

    private float _sightAngle = 90f;
    private float _speed = 15f;

    public int cornerIndex = 0;


    public Vector3 curMoveDir;

    [SerializeField]
    private People[] _people;

    private Rigidbody _rb;

    private Stage _curstageInfo;
    private Action _carPass = () => { };
    private TimeLimitCar _timeLimitCar;
    private CoinEffect _effect;
    private CrashEffect _crashEffect;

    private IEnumerator _moveCo;
    private IEnumerator _passCo;

    #endregion


    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _curstageInfo = GetComponentInParent<Stage>();
        _people = FindObjectsOfType<People>();
        _timeLimitCar = FindObjectOfType<TimeLimitCar>();
        _effect = FindObjectOfType<CoinEffect>();
        _crashEffect = FindObjectOfType<CrashEffect>();

        if (_timeLimitCar!=null)
        {
            _carPass += _timeLimitCar.CarPass;
        }

        for (int i = 0; i < _people.Length; i++)
        {
            _people[i].onCollisionCar += () =>
            {
                if (isGameOver)
                {
                    Debug.Log(this.gameObject.name);

                    CrashPeople();
                    Invoke("GameOver", 1f);
                }
            };
        }
    }

    void CrashPeople()
    {
        StopCoroutine(_moveCo);

        _rb.velocity = Vector3.zero;
    }

    protected virtual void Update()
    {
        // 자동차객체 로테이션 제어문
        if (!isMove)
        {
            if (isGameOver)
            {
                _rb.constraints = RigidbodyConstraints.None;
                return;
            }
            if(!_isCol)
            {
                _rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            }
        }
        else
        {
            if (!isPassing)
            {
                if (transform.localEulerAngles.y == 270 || transform.localEulerAngles.y == 90)
                {
                    _rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX;
                }
                else
                {
                    _rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
                }
            }
            else
            {
                _rb.constraints = RigidbodyConstraints.FreezePositionY;
            }
        }

        if (targetCorner != null && !isPassing)
        {
            if (!_isFirst)
            {
                if (Mathf.Floor(transform.position.x) == Mathf.Floor(targetCorner.localPosition.x)
                || Mathf.Floor(transform.position.z) == Mathf.Floor(targetCorner.localPosition.z) )
                {
                    StopCoroutine(_moveCo);
                    Pass();

                    _rb.velocity = Vector3.zero;

                    isPassing = true;
                    isPass = false;
                    _isFirst = true;

                    _carPass();
                    _effect.PlayEffect(transform.position);
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

                        _passCo = PassCo();
                        StartCoroutine(_passCo);
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
                        StopCoroutine(_moveCo);
                        Debug.Log("sdf");
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
                _rb.velocity = curMoveDir * _speed;
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

        distance = Mathf.Abs(distance);


        if (Physics.Raycast(transform.position, curMoveDir, out hit, distance, layerMask))
        {
            if (hit.transform.CompareTag("Car") && hit.transform.CompareTag("Obstacle") && hit.transform.CompareTag("Wall"))
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

        _rb.constraints = RigidbodyConstraints.FreezePositionY;

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

            _rb.velocity = -transform.right.normalized * _speed;

            yield return null;
        }
    }


    // 스테이지 안에서 MoveCo 코루틴을 제어하는 함수
    public virtual void Move(Vector3 dir)
    {
        

        if(_curstageInfo != null)
        {
            if (_curstageInfo.mode == Stage.StageMode.limitMove)
            {
                if (_curstageInfo.moveCount <= 0)
                {
                    return;
                }

                _curstageInfo.UpdateMovecount();
            }
        }

        float dot = Vector3.Dot(dir.normalized, -transform.right.normalized);

        float theta = Mathf.Acos(dot) * (180 / Mathf.PI);

        if (theta <= _sightAngle)
        {
            if (!isMove)
            {
                curMoveDir = -transform.right;

                _moveCo = MoveCo(curMoveDir);
                StartCoroutine(_moveCo);

            }
        }
        else
        {
            if (!isMove)
            {
                curMoveDir = transform.right;
                _moveCo = MoveCo(curMoveDir);
                StartCoroutine(_moveCo);

            }
        }
    }

    // 실질적인 움직임과 코너부분 제어하는 함수
    IEnumerator MoveCo(Vector3 dir)
    {
        isMove = true;
        isPassing = false;

        _rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

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
            CheckOtherCar(dir);
            if (!isOtherCar)
                _rb.velocity = dir * _speed;


            yield return null;
        }
    }

    bool isOtherCar;
    public void CheckOtherCar(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, 3f))
        {
            if (hit.transform.CompareTag("Car"))
            {
                if(hit.transform.GetComponent<Car>().isPassing)
                {
                    isOtherCar = true;
                    _rb.velocity = Vector3.zero;
                }
            }
            else
            {
                isOtherCar = false;
                isMove = true;
            }
        }
        else
        {
            isOtherCar = false;
            isMove = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Car") || collision.gameObject.CompareTag("Obstacle"))
        {
            isPass = false;
            _isCol = true;
            if (isMove)
                StopCoroutine(_moveCo);

            _rb.velocity = Vector3.zero;

            if (collision.gameObject.CompareTag("Car"))
            {
                //CrashAnim(collision.gameObject);
            }
            Invoke("ColKnockBack", 0.05f);

            if(GameManager.Instance.IsVibrate)
            {
                Handheld.Vibrate();
            }

            if(isMove)
            {

                for (int i = 0; i < _crashEffect._crashEffect.Length; i++)
                {
                    if (_crashEffect._crashEffect[i].gameObject.activeSelf)
                        continue;

                    _crashEffect._crashEffect[i].gameObject.SetActive(true);
                    _crashEffect._crashEffect[i].Play();
                    _crashEffect._crashEffect[i].transform.position = collision.contacts[0].point;
                    Debug.Log("ASDASD");


                    if (transform.localEulerAngles.y == 270 || transform.localEulerAngles.y == 90)
                    {
                        _crashEffect._crashEffect[i].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                    }
                    else
                    {
                        _crashEffect._crashEffect[i].transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                    }
                    break;
                }
            }
            _isCol = true;
        }
    }

    // 부딪혔을때 연출
    void CrashAnim(GameObject crashObj)
    {
        crashObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;

        Debug.Log(curMoveDir);
        if(curMoveDir.z != 0)
        {
            Debug.Log(curMoveDir.z);

            crashObj.transform.DORotate(new Vector3(curMoveDir.z * 10, crashObj.transform.localEulerAngles.y, 0), 0.2f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
            {
                crashObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                _isCol = false;
            });
        }
        else
        {
            Debug.Log(curMoveDir.x);

            crashObj.transform.DORotate(new Vector3(curMoveDir.x * 10f, crashObj.transform.localEulerAngles.y,0), 0.2f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
            {
                crashObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                _isCol = false;
            });
        }
    }

    // 게임오버 연출
    void GameOver()
    {
        _rb.AddForce(-curMoveDir * 5f, ForceMode.Impulse);
        _rb.AddForce(transform.up * 10f, ForceMode.Impulse);
        _rb.AddTorque(transform.right * 10f, ForceMode.Impulse);

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
        float _dot = Vector3.Dot(dir.normalized, -transform.right.normalized);

        float _theta = Mathf.Acos(_dot) * (180 / Mathf.PI);

        if (_theta <= 90)
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

            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;

            _rb.constraints = RigidbodyConstraints.FreezeRotation;

            _rb.AddForce(-curMoveDir * 2.5f, ForceMode.Impulse);

            Invoke("FreezePos", 0.1f);
        }
    }

    // Pass했을때 옵션들 제어해주는 함수
    public void Pass()
    {
        this.gameObject.GetComponent<BoxCollider>().isTrigger = true;

    }

    //isMove 제어하는 함수
    void FreezePos()
    {
        isMove = false;
    }
}
