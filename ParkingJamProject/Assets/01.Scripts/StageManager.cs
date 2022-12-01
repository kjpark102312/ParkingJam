using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum Hardmode
{
    limitCountMode,
    limitTimeMode,
}

public class StageManager : MonoBehaviour
{
   

    static StageManager instance = null;

    public static StageManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StageManager>();
            }
            return instance;
        }
    }

    private Dictionary<string, GameObject> stagesDic = new Dictionary<string, GameObject>();

    public GameObject[] stages;

    public int curStageIndex;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        curStageIndex = PlayerPrefs.GetInt("Stage");
    }

    void Start()
    {
        stages = Resources.LoadAll<GameObject>("Stages");

        PlayerPrefs.SetInt("Stage", 8);
        if (PlayerPrefs.GetInt("Stage") >= stages.Length)
        {
            PlayerPrefs.SetInt("Stage", 6);
            curStageIndex = 0;
        }

        for (int i = 0; i < stages.Length; i++)
        {
            stagesDic[stages[i].name.ToString()] = stages[i];
        }

        LoadStage();

        //SceneManager.sceneLoaded += (scene, loadSceneMode) =>
        //{
        //    LoadStage();
        //};
    }

    public void LoadStage()
    {
        if (PlayerPrefs.GetInt("Stage") == 0)
        {
            Instantiate(stagesDic["Stage"], Vector3.zero, Quaternion.identity);
        }
        else if (PlayerPrefs.GetInt("Stage") % 3 == 0)
        {
            if (PlayerPrefs.GetInt("Stage") / 3 > 1)
            {
                Debug.Log("asd");
                if (!stagesDic.ContainsKey("HardStage" + PlayerPrefs.GetInt("Stage") / 3))
                {
                    Instantiate(stagesDic["Stage" + (PlayerPrefs.GetInt("Stage") + 1)], Vector3.zero, Quaternion.identity);
                    return;
                }
            }

            Instantiate(stagesDic["HardStage" + PlayerPrefs.GetInt("Stage") / 3], Vector3.zero, Quaternion.identity);

            PlayerPrefs.SetInt("Stage", PlayerPrefs.GetInt("Stage") - 1);
        }
        else
        {
            Instantiate(stagesDic["Stage" + PlayerPrefs.GetInt("Stage")], Vector3.zero, Quaternion.identity);
        }
    }   
}
