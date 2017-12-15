using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // --------------------------------------------------------------
    [SerializeField]
    private GameObject m_objectToSpawn;

    [SerializeField]
    private float m_timeBetweenSpawns;
    // --------------------------------------------------------------

    private Transform[] m_spawnPositions;
    private float m_timeSinceLastSpawn = 0f;
    // --------------------------------------------------------------

    private void Awake()
    {
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
            m_timeSinceLastSpawn = 0f;
        }
    }

    private void Spawn()
    {
        Transform spawnPoint = m_spawnPositions[Random.Range(0, m_spawnPositions.Length)];
        Instantiate(m_objectToSpawn, spawnPoint.position, Quaternion.identity);
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
