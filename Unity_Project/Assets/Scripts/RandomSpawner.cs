using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns given prefab in a random selection of Transform children (Spawn positions)
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

    // Index in above array where last object was spawned
    private int m_LastSpawnIndex;

    // 
    private int m_NumVacantPositions = 0;

    // How many seconds until next spawn
    private float m_NextSpawnTime;

    private float m_TimeSinceLastSpawn = 0f;

    // --------------------------------------------------------------

    private void Awake()
    {
        if (transform.childCount <= 0)
        {
            Debug.LogError("RandomSpawner: No Spawn positions set for " + name + "!");
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
        UpdateVacancyCount();

        // Do not spawn if already occupied all allowed positions
        if (m_NumVacantPositions <= 0) return;

        // Ensure another object isn't currently at position to spawn, and avoid spawning at same place as last time if possible
        Transform spawnPoint;
        int spawnIndex;
        do
        {
            spawnIndex = Random.Range(0, m_SpawnPositions.Length);
            spawnPoint = m_SpawnPositions[spawnIndex];
        }
        while (IsOccupied(spawnPoint) || (spawnIndex == m_LastSpawnIndex && m_NumVacantPositions > 1));

        m_LastSpawnIndex = spawnIndex;
        GameObject spawnedObject = Instantiate(m_ObjectToSpawn, spawnPoint) as GameObject;

        if (m_UsePriorityRendering)
        {
            SetPriorityRendering(spawnedObject);
        }
    }

    // Whether a given spawn position is already in use by another object of this type
    private bool IsOccupied(Transform t) 
    {
        return (t.childCount != 0);
    }

    // Check each spawn position (in case spawned object was destroyed), and update count of vacant positions
    private void UpdateVacancyCount()
    {
        int numOccupied = 0;
        foreach (Transform child in transform)
        {
            if (IsOccupied(child)) numOccupied++;
        }
        m_NumVacantPositions = m_MaxNumObjects - numOccupied;
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
