using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private GameObject m_ObjectToSpawn;

    [SerializeField] private float m_MinTimeBetweenSpawns;

    [SerializeField] private float m_MaxTimeBetweenSpawns;
    
    // Max. number of allowed objects of this type in this level
    [SerializeField] private int m_MaxNumObjects;

    // --------------------------------------------------------------

    private Transform[] m_SpawnPositions;

    // How many seconds until next spawn
    private float m_NextSpawnTime;

    private float m_TimeSinceLastSpawn = 0f;
    
    // --------------------------------------------------------------

    private void Awake()
    {
        if (transform.childCount <= 0)
        {
            Debug.LogError("No Spawn positions set for " + gameObject + "!");
        }
        if (m_MaxNumObjects > transform.childCount)
        {
            Debug.LogError("Cannot spawn more objects than spawn positions available!");
        }

        Collector.OnAllPisCollected += OnGameOver;

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

        Instantiate(m_ObjectToSpawn, spawnPoint);
    }

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

    private void OnGameOver(int numOfWinner)
    {
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        Collector.OnAllPisCollected -= OnGameOver;
    }


}
