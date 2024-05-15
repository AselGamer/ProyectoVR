using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CustomEditor(typeof(SetTerrainObstacles))]
public class SetTerrainObstaclesEditor : Editor
{
    public Terrain terrain = null;
    public NavMeshSurface navMeshSurface = null;

    public override void OnInspectorGUI()
    {
        SetTerrainObstacles setTerrainObstacles = (SetTerrainObstacles)target;

        EditorGUILayout.BeginHorizontal();
        setTerrainObstacles.terrain = (Terrain)EditorGUILayout.ObjectField("Surface terrain", setTerrainObstacles.terrain, typeof(Terrain), true);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        setTerrainObstacles.surface = (NavMeshSurface)EditorGUILayout.ObjectField("Surface navmesh", setTerrainObstacles.surface, typeof(NavMeshSurface), true);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Bake"))
        {
            setTerrainObstacles.Bake();
        }

        if (GUILayout.Button("Clear"))
        {
            setTerrainObstacles.Clear();
        }
        serializedObject.Update();
    }
}
