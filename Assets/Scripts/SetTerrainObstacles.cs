using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class SetTerrainObstacles : MonoBehaviour
{
    // Start is called before the first frame update
    TreeInstance[] Obstacle;
    public Terrain terrain;
    public NavMeshSurface surface;
    float width;
    float lenght; 
    float hight;
    bool isError;
    public void Bake()
    {
        surface.RemoveData();
        Obstacle = terrain.terrainData.treeInstances;

        lenght = terrain.terrainData.size.z;
        width = terrain.terrainData.size.x;
        hight = terrain.terrainData.size.y;
        Debug.Log("Terrain Size is :" + width + " , " + hight + " , " + lenght);

        int i = 0;
        GameObject parent = new GameObject("Tree_Obstacles");
        parent.transform.tag = "obstacles";

        Debug.Log("Adding "+Obstacle.Length+" navMeshObstacle Components for Trees");
        foreach (TreeInstance tree in Obstacle)
        {
            Vector3 tempPos = new Vector3(tree.position.x * width, tree.position.y * hight, tree.position.z * lenght);
            Quaternion tempRot = Quaternion.AngleAxis(tree.rotation * Mathf.Rad2Deg, Vector3.up);

            GameObject obs = new GameObject("Obstacle" + i);
            obs.transform.SetParent(parent.transform);
            obs.transform.position = tempPos;
            obs.transform.rotation = tempRot;

            obs.AddComponent<NavMeshObstacle>();
            NavMeshObstacle obsElement = obs.GetComponent<NavMeshObstacle>();
            obsElement.carving = true;
            obsElement.carveOnlyStationary = true;

            if (terrain.terrainData.treePrototypes[tree.prototypeIndex].prefab.GetComponent<NavMeshObstacle>() == null)
            {
                isError = true;
                Debug.LogError("ERROR  There is no NavMeshObstacle attached to ''" + terrain.terrainData.treePrototypes[tree.prototypeIndex].prefab.name + "'' please add one.");
                break;
            }
            NavMeshObstacle treeObs = terrain.terrainData.treePrototypes[tree.prototypeIndex].prefab.GetComponent<NavMeshObstacle>();
            obsElement.shape = treeObs.shape;
            obsElement.center = treeObs.center;
            obsElement.radius = treeObs.radius;
            obsElement.height = treeObs.height;


            i++;
        }
        parent.transform.position = terrain.GetPosition();
        if(!isError) Debug.Log("All " + Obstacle.Length + " NavMeshObstacles were succesfully added to your Scene, Horray !");
        if (surface) { 
            surface.BuildNavMesh();
        }
    }

    public void Clear()
    {
        if (surface)
        {
            surface.RemoveData();
            GameObject.FindGameObjectsWithTag("obstacles").ToList().ForEach(x => DestroyImmediate(x));
        }
    }
}
