using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_Prefabs;

    [SerializeField]
    private GameObject m_Boss;

    [SerializeField]
    private GameObject m_BossSpawnPoint;

    /*private int m_Amount;

    private int m_Alive;*/

    [SerializeField]
    private GameObject[] m_SpawnPointObjects;

    // Start is called before the first frame update
    void Start()
    {
        m_SpawnPointObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private void CheckAlive()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Monster");

        m_Alive = gameObjects.Length;
    }*/

    public void SpawnPrefab()
    {
        int spawnPoint = Random.Range(0, m_SpawnPointObjects.Length);
        Vector2 spawnPosition = m_SpawnPointObjects[spawnPoint].transform.position;

        Instantiate(m_Prefabs[Random.Range(0, m_Prefabs.Length)], spawnPosition, Quaternion.identity);
    }

    public GameObject SpawnAndReturnPrefab()
    {
        int spawnPoint = Random.Range(0, m_SpawnPointObjects.Length);
        Vector2 spawnPosition = m_SpawnPointObjects[spawnPoint].transform.position;

        return Instantiate(m_Prefabs[Random.Range(0, m_Prefabs.Length)], spawnPosition, Quaternion.identity);
    }

    public GameObject SpawnAndReturnBoss()
    {
        Vector2 spawnPosition = m_BossSpawnPoint.transform.position;

        return Instantiate(m_Boss, spawnPosition, Quaternion.identity);
    }

    /*private void SpawnPrefab()
    {
        CheckAlive();

        if (m_Alive < m_Amount)
        {
            for (int i = m_Alive; i < m_Amount; i++)
            {
                int spawnPoint = Random.Range(0, m_SpawnPointObjects.Length);
                Vector2 spawnPosition = m_SpawnPointObjects[spawnPoint].transform.position;

                Instantiate(m_Prefab, spawnPosition, Quaternion.identity);
            }
        }
    }*/
}
