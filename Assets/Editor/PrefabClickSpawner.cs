using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabClickSpawner : EditorWindow
{
    public GameObject prefabToPlace;
    public float yOffset = 0f;
    private Plane placementPlane = new Plane(Vector3.up, Vector3.zero);

    [MenuItem("Tools/Prefab Click Spawner")]
    public static void ShowWindow()
    {
        GetWindow<PrefabClickSpawner>("Prefab Spawner");
    }

    void OnGUI()
    {
        prefabToPlace = (GameObject)EditorGUILayout.ObjectField("Prefab to Place", prefabToPlace, typeof(GameObject), false);
        yOffset = EditorGUILayout.FloatField("Y Offset", yOffset);

        EditorGUILayout.HelpBox("Click in the Scene View to place the selected prefab.", MessageType.Info);
    }

    void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (prefabToPlace == null) return;

        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (placementPlane.Raycast(ray, out float distance))
            {
                Vector3 spawnPos = ray.GetPoint(distance) + Vector3.up * yOffset;

                GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefabToPlace);
                Undo.RegisterCreatedObjectUndo(newObj, "Place Prefab");
                newObj.transform.position = spawnPos;

                e.Use(); // Consume the click event
            }
        }
    }
}
