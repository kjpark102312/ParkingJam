using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapTool : EditorWindow
{
    private Object floorTile = null;
    public Object[] cars = null;

    [MenuItem("Window/My MapTool")]
    public static void ShowMapTool()
    {
        EditorWindow.GetWindow(typeof(MapTool));
    }

    private void OnGUI()
    {
        floorTile = EditorGUILayout.ObjectField("¹Ù´Ú Å¸ÀÏ", floorTile, typeof(GameObject), true);

        //cars = EditorGUILayout.ObjectField("Â÷", floorTile, typeof(GameObject), true);
    }
}
