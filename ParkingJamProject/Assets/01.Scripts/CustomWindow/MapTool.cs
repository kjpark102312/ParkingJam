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
        floorTile = EditorGUILayout.ObjectField("�ٴ� Ÿ��", floorTile, typeof(GameObject), true);

        //cars = EditorGUILayout.ObjectField("��", floorTile, typeof(GameObject), true);
    }
}
