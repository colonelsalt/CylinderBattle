using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // --------------------------------------------------------------
    [SerializeField] private GameObject m_objectToSpawn;
    [SerializeField] private float m_timeBetweenSpawns;
    // --------------------------------------------------------------
    private Transform[] m_spawnPositions;
    private float m_timeSinceLastSpawn = 0f;
    private Transform m_lastSpawnPosition;
    // --------------------------------------------------------------

    private void Awake()
    {
        if (transform.childCount <= 0)
        {
            Debug.LogError("No Spawn positions set for " + gameObject + "!");
        }

        GameManager.OnGameOver += OnGameOver;

        m_spawnPositions = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            m_spawnPositions[i] = transform.GetChild(i);
        }
    }

    private void Update()
    {
        m_timeSinceLastSpawn += Time.deltaTime;
        if (m_timeSinceLastSpawn >= m_timeBetweenSpawns)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        Transform spawnPoint;
        do
        {
            spawnPoint = m_spawnPositions[Random.Range(0, m_spawnPositions.Length)];
        } while (spawnPoint == m_lastSpawnPosition);

        Instantiate(m_objectToSpawn, spawnPoint.position, Quaternion.identity);
        m_timeSinceLastSpawn = 0f;
        m_lastSpawnPosition = spawnPoint;
    }

    private void OnGameOver(int numOfWinner)
    {
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        GameManager.OnGameOver -= OnGameOver;
    }


}
