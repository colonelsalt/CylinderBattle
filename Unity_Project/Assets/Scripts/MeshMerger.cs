using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshMerger : MonoBehaviour
{
    private void Awake()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combination = new CombineInstance[meshFilters.Length];
        for (int i = 0; i < meshFilters.Length; i++)
        {
            combination[i].mesh = meshFilters[i].sharedMesh;
            combination[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].GetComponent<MeshRenderer>().enabled = false;
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combination);

        GetComponent<MeshRenderer>().enabled = true;


    }

}
