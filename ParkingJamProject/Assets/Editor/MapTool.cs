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
    #endregion

    #region UI
    // The window is selected if it already exists, else it's created.
    [MenuItem("Window/My Map Editor")]
    private static void ShowWindow()
    {
        GetWindow(typeof(MapTool));
    }

    void OnFocus()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI; // Don't add twice
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;

        RefreshPalette(); // �ȷ�Ʈ ����ְ� �ٽ� ä���ִ� �Լ� 
    }

    // �����쿡 �׷��ִ� �Լ�
    private void OnGUI()
    {
        showLevelOptions = EditorGUILayout.Foldout(showLevelOptions, "Level Creator");
        if (showLevelOptions)
        {
            paintMode = GUILayout.Toggle(paintMode, "Start painting", "Button", GUILayout.Height(60f));

            EditorGUILayout.Space();

            showBuildableObjects = EditorGUILayout.Foldout(showBuildableObjects, "��ġ��");

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

            if (GUILayout.Button("���ο� �� �ҷ�����"))
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/NormalStageTemplate.prefab", typeof(GameObject)) as GameObject;
                GameObject gameObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

            }
            GUILayout.Button("���� �� �����ϱ�");
            EditorGUILayout.EndHorizontal();
        }
    }


    //�� ���� �׷��ִ� �Լ�
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

        // XZ ��ǥ�� ��ȯ 
        Vector3 topLeft = cellOrigin + Vector2.left * cellSize * 0.5f + Vector2.up * cellSize * 0.5f;
        Swap(out topLeft.z, ref topLeft.y);

        Vector3 topRight = cellOrigin - Vector2.left * cellSize * 0.5f + Vector2.up * cellSize * 0.5f;
        Swap(out topRight.z, ref topRight.y);

        Vector3 bottomLeft = cellOrigin + Vector2.left * cellSize * 0.5f - Vector2.up * cellSize * 0.5f;
        Swap(out bottomLeft.z, ref bottomLeft.y);

        Vector3 bottomRight = cellOrigin - Vector2.left * cellSize * 0.5f - Vector2.up * cellSize * 0.5f;
        Swap(out bottomRight.z, ref bottomRight.y);
                
        // ������
        Handles.color = Color.green;
        Vector3[] lines = { topLeft, topRight, topRight, bottomRight, bottomRight, bottomLeft, bottomLeft, topLeft };
        Handles.DrawLines(lines);
    }

    private void Swap(out float z, ref float y)
    {
        //���� �Լ�

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


            gameObject.transform.position = new Vector3(cellCenter.x, 0.588f, cellCenter.y);

            // ��Ʈ�� z Ŀ�ǵ�
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

