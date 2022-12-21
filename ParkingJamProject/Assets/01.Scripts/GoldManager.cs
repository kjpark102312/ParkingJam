using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    static GoldManager instance = null;

    public static GoldManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GoldManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    public int Gold
    {
        get
        {
            return _gold;
        }
        set
        {
            _gold += value;
        }
    }

    [SerializeField]
    private int _gold;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _gold = PlayerPrefs.GetInt("Gold");
    }

    void Start()
    {
        
    }

}
