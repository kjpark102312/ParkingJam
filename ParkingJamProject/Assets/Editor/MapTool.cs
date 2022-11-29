using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapTool : EditorWindow
{
    #region Attributes
    [SerializeField]
    private List<GameObject> palette = new List<GameObject>();

    [SerializeField]
    private int paletteIndex;

    private string path = "Assets/Editor Default Resources/Palette";
    private Vector2 cellSize = new Vector2(1f, 1f);

    private bool paintMode = false;
    private bool showLevelOptions = false;
    private bool showBuildableObjects = false;
    private bool carAngel = false;
    private bool carAngel2 = false;

    private GameObject curEditMap;
    #endregion

    #region UI

    [MenuItem("Window/My Map Editor")]
    private static void ShowWindow()
    {
        GetWindow(typeof(MapTool));
    }

    void OnFocus()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;

        RefreshPalette(); // 팔레트 비워주고 다시 채워주는 함수 
    }

    // 윈도우에 그려주는 함수
    private void OnGUI()
    {
        showLevelOptions = EditorGUILayout.Foldout(showLevelOptions, "Level Creator");
        if (showLevelOptions)
        {
            paintMode = GUILayout.Toggle(paintMode, "Start painting", "Button", GUILayout.Height(60f));

            EditorGUILayout.Space();

            showBuildableObjects = EditorGUILayout.Foldout(showBuildableObjects, "설치물");
            EditorGUILayout.BeginHorizontal();
            carAngel = GUILayout.Toggle(carAngel, "0, 180", "Button", GUILayout.Height(30f));
            carAngel2 = GUILayout.Toggle(carAngel2, "90, -90", "Button", GUILayout.Height(30f));
            EditorGUILayout.EndHorizontal();

            List<GUIContent> paletteIcons = new List<GUIContent>();
            foreach (GameObject prefab in palette)
            {
                Texture2D texture = AssetPreview.GetAssetPreview(prefab);
                paletteIcons.Add(new GUIContent(texture));
            }

            if (showBuildableObjects)
            {
                paletteIndex = GUILayout.SelectionGrid(paletteIndex, paletteIcons.ToArray(), 6);
            }


            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("새로운 맵 불러오기"))
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/NormalStageTemplate.prefab", typeof(GameObject)) as GameObject;
                GameObject gameObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                curEditMap = gameObject;
            }
            if(GUILayout.Button("지금 맵 저장하기"))
            {
                Debug.Log(StageManager.Instance.stages.Length);
                PrefabUtility.UnpackPrefabInstance(curEditMap, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                PrefabUtility.SaveAsPrefabAssetAndConnect(curEditMap, $"Assets/Resources/Stages/Stage{0}.prefab", InteractionMode.AutomatedAction);
            }
            EditorGUILayout.EndHorizontal();
        }
    }


    //씬 위에 그려주는 함수
    private void OnSceneGUI(SceneView sceneView)
    {
        if (paintMode)
        {
            Vector2 cellCenter = GetSelectedCell(); 

            DisplayVisualHelp(cellCenter);
            HandleSceneViewInputs(cellCenter);

            // Refresh the view
            sceneView.Repaint();
        }
    }

    private void DisplayVisualHelp(Vector2 cellOrigin)
    {

        // XZ 좌표로 변환 
        Vector3 topLeft = cellOrigin + Vector2.left * cellSize * 0.5f + Vector2.up * cellSize * 0.5f;
        Swap(out topLeft.z, ref topLeft.y);

        Vector3 topRight = cellOrigin - Vector2.left * cellSize * 0.5f + Vector2.up * cellSize * 0.5f;
        Swap(out topRight.z, ref topRight.y);

        Vector3 bottomLeft = cellOrigin + Vector2.left * cellSize * 0.5f - Vector2.up * cellSize * 0.5f;
        Swap(out bottomLeft.z, ref bottomLeft.y);

        Vector3 bottomRight = cellOrigin - Vector2.left * cellSize * 0.5f - Vector2.up * cellSize * 0.5f;
        Swap(out bottomRight.z, ref bottomRight.y);
                
        // 렌더링
        Handles.color = Color.green;
        Vector3[] lines = { topLeft, topRight, topRight, bottomRight, bottomRight, bottomLeft, bottomLeft, topLeft };
        Handles.DrawLines(lines);
    }

    private void Swap(out float z, ref float y)
    {
        //스왑 함수

        z = y;
        y = 0.588f;
    }

    #endregion

    #region MapEdition
    private void HandleSceneViewInputs(Vector2 cellCenter)
    {
        if (Event.current.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(0); 
        }

        if (paletteIndex < palette.Count && Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            GameObject prefab = palette[paletteIndex];
            GameObject gameObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            gameObject.transform.SetParent(curEditMap.transform);

            gameObject.transform.position = new Vector3(cellCenter.x, 0.588f, cellCenter.y);

            if (carAngel && carAngel2)
                gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
            else if(!carAngel && !carAngel2)
                gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
            else if(!carAngel && carAngel2)
                gameObject.transform.eulerAngles = new Vector3(0, 90, 0);
            else
                gameObject.transform.eulerAngles = new Vector3(0, -90, 0);

            // 컨트롤 z 커맨드
            Undo.RegisterCreatedObjectUndo(gameObject, "");
        }
    }
    #endregion

    void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }

    private void RefreshPalette()
    {
        palette.Clear();

        System.IO.Directory.CreateDirectory(path);
        string[] prefabFiles = System.IO.Directory.GetFiles(path, "*.prefab");
        foreach (string prefabFile in prefabFiles)
            palette.Add(AssetDatabase.LoadAssetAtPath(prefabFile, typeof(GameObject)) as GameObject);
    }

    Vector2 GetSelectedCell()
    {
        Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Vector3 mousePosition = guiRay.origin - guiRay.direction * (guiRay.origin.y / guiRay.direction.y);

        Vector2Int cell = new Vector2Int(Mathf.FloorToInt(mousePosition.x / cellSize.x), Mathf.FloorToInt(mousePosition.z / cellSize.y));
        Vector2 cellCenter = cell * cellSize;

        return cellCenter;
    }
}

