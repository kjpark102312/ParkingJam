using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Challenges
{
    CarOut,
    CarCrash,
    LevelClear,
    Crash
}

public class ChallengeUI : MonoBehaviour
{
    void Start()
    {
        int _index = Random.Range(0, 4);


    }
}
