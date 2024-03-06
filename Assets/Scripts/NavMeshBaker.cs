using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshBaker : MonoBehaviour
{
    public NavMeshSurface surface;
    void Start()
    {
        BuildNavMesh();
    }
    void BuildNavMesh()
    {
        surface.BuildNavMesh();
        Debug.Log("NM baked");
    }
}
