using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private GameObject m_ObjectToSpawn;

    [SerializeField] private float m_MinTimeBetweenSpawns;

    [SerializeField] private float m_MaxTimeBetweenSpawns;

    // Max. number of allowed objects of this type for this set of Spawn points
    [SerializeField] private int m_MaxNumObjects;

    // Set if object will be on top of a transparent platform (and needs priority rendering to be visible)
    [SerializeField] private bool m_UsePriorityRendering = false;

    // --------------------------------------------------------------

    private Transform[] m_SpawnPositions;

    private int m_NumSpawned = 0;

    // How many seconds until next spawn
    private float m_NextSpawnTime;

    private float m_TimeSinceLastSpawn = 0f;

    // --------------------------------------------------------------

    private void Awake()
    {
        if (transform.childCount <= 0)
        {
            Debug.LogError("RandomSpawner: No Spawn positions set for " + gameObject + "!");
        }
        if (m_MaxNumObjects > transform.childCount)
        {
            Debug.LogError("RandomSpawner: Cannot spawn more objects than spawn positions available!");
            gameObject.SetActive(false);
        }

        GameManager.OnGameOver += OnGameOver;

        m_SpawnPositions = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            m_SpawnPositions[i] = transform.GetChild(i);
        }

        m_NextSpawnTime = Random.Range(m_MinTimeBetweenSpawns, m_MaxTimeBetweenSpawns);
    }

    private void Update()
    {
        m_TimeSinceLastSpawn += Time.deltaTime;
        if (m_TimeSinceLastSpawn >= m_NextSpawnTime)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        m_TimeSinceLastSpawn = 0f;
        m_NextSpawnTime = Random.Range(m_MinTimeBetweenSpawns, m_MaxTimeBetweenSpawns);

        // Do not spawn if exceeded number of allowed objects
        if (NumSpawnedObjects() >= m_MaxNumObjects) return;

        // Ensure another object isn't currently at position to spawn
        Transform spawnPoint;
        do
        {
            spawnPoint = m_SpawnPositions[Random.Range(0, m_SpawnPositions.Length)];
        }
        while (IsOccupied(spawnPoint));

        GameObject spawnedObject = Instantiate(m_ObjectToSpawn, spawnPoint) as GameObject;

        if (m_UsePriorityRendering)
        {
            SetPriorityRendering(spawnedObject);
        }
    }

    // Whether a given spawn position already in use by another object of this type
    private bool IsOccupied(Transform t) 
    {
        return (t.childCount != 0);
    }

    private int NumSpawnedObjects()
    {
        int sum = 0;
        foreach (Transform child in transform)
        {
            if (IsOccupied(child)) sum++;
        }
        return sum;
    }

    // Increase rendering priority for object
    private void SetPriorityRendering(GameObject g)
    {
        foreach (Renderer rend in g.GetComponentsInChildren<Renderer>())
        {
            rend.material.renderQueue = 3100;
        }
    }

    private void OnGameOver(int numOfWinner)
    {
        m_NextSpawnTime = float.MaxValue;
    }

    private void OnDisable()
    {
        GameManager.OnGameOver -= OnGameOver;
    }


}
