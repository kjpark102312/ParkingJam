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

    private Dictionary<string, GameObject> _stagesDic = new Dictionary<string, GameObject>();

    public GameObject[] stages;

    public int curStageIndex;
    public int getGoldCount;

    GameObject cam;
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

        curStageIndex = PlayerPrefs.GetInt("Stage");
    }

    void Start()
    {
        stages = Resources.LoadAll<GameObject>("Stages");

        if (PlayerPrefs.GetInt("Stage") >= stages.Length-1)
        {
            PlayerPrefs.SetInt("Stage", 0);
            curStageIndex = 0;
        }

        for (int i = 0; i < stages.Length; i++)
        {
            _stagesDic[stages[i].name.ToString()] = stages[i];
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
            Instantiate(_stagesDic["Stage"], Vector3.zero, Quaternion.identity);
            Camera.main.transform.position = new Vector3(42.7f, 27.8f, -67.2f);
        }
        else if (PlayerPrefs.GetInt("Stage") % 3 == 0)
        {
            if (PlayerPrefs.GetInt("Stage") / 3 > 1)
            {
                if (!_stagesDic.ContainsKey("HardStage" + PlayerPrefs.GetInt("Stage") / 3))
                {
                    Instantiate(_stagesDic["Stage" + (PlayerPrefs.GetInt("Stage"))], Vector3.zero, Quaternion.identity);
                    Camera.main.transform.position = new Vector3(42.7f, 33.6f, -67.2f);
                    return;
                }
            }
            Instantiate(_stagesDic["HardStage" + PlayerPrefs.GetInt("Stage") / 3], Vector3.zero, Quaternion.identity);
        }
        else
        {
            Instantiate(_stagesDic["Stage" + PlayerPrefs.GetInt("Stage")], Vector3.zero, Quaternion.identity);
            Camera.main.transform.position = new Vector3(42.7f, 33.6f, -67.2f);
        }
    }   
}
