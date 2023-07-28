using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_WaveText;

    [SerializeField]
    private TMP_Text m_GameOverWaveText;

    private GameObject[] m_GameObjects;

    private int m_CurrentWave;

    private int m_SpawnAmount;

    private int m_RemainingSpawnAmount;

    private int m_MaxAlive;

    private float m_CheckTime;
    private float m_CheckTimer;


    // Start is called before the first frame update
    void Start()
    {
        m_CurrentWave = 0;

        m_WaveText.text = "Wave: " + m_CurrentWave.ToString();

        m_SpawnAmount = 4 + (m_CurrentWave * 2);

        m_RemainingSpawnAmount = m_SpawnAmount;

        m_MaxAlive = 100;

        m_CheckTime = 1.0f;
        m_CheckTimer = m_CheckTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_CheckTimer <= 0.0f)
        {
            if (m_CurrentWave != -1)
            {
                CheckWave();
                SpawnWave();
            }
            m_CheckTimer = m_CheckTime;
        }
        else
        {
            m_CheckTimer -= Time.deltaTime;
        }
    }

    private void SpawnWave()
    {
        if (m_RemainingSpawnAmount > 0 && m_GameObjects.Length < m_MaxAlive)
        {
            SpawnManager spawnManager = GetComponent<SpawnManager>();

            for (int i = 0; i < m_MaxAlive; i++)
            {
                //spawnManager.SpawnPrefab();

                //spawnManager.SpawnAndReturnPrefab().GetComponent<MonsterObject>().SetHealth(100.0f);
                if (m_CurrentWave % 10 == 0)
                {
                    float health = 100.0f + (m_CurrentWave * 10);
                    spawnManager.SpawnAndReturnBoss().gameObject.GetComponent<MonsterObject>().SetHealth(health);
                }
                else
                {
                    float health = 1.0f + (m_CurrentWave * 2);
                    spawnManager.SpawnAndReturnPrefab().gameObject.GetComponent<MonsterObject>().SetHealth(health);
                }

                m_RemainingSpawnAmount--;

                if (m_RemainingSpawnAmount <= 0)
                {
                    break;
                }
            }
        }
    }

    private void CheckWave()
    {
        m_GameObjects = GameObject.FindGameObjectsWithTag("Monster");

        GameObject boss = GameObject.FindGameObjectWithTag("Boss");

        if (m_GameObjects.Length == 0 && boss == null)
        {
            NextWave();
        }
    }

    private void NextWave()
    {
        m_CurrentWave++;

        m_WaveText.text = "Wave: " + m_CurrentWave.ToString();
        m_GameOverWaveText.text = "Wave: " + m_CurrentWave.ToString();

        if (m_CurrentWave % 10 == 0)
        {
            //SPAWN BOSS
            m_SpawnAmount = 1;
        }
        else
        {
            m_SpawnAmount = 4 + (m_CurrentWave * 2);
        }
        m_RemainingSpawnAmount = m_SpawnAmount;
    }
}
