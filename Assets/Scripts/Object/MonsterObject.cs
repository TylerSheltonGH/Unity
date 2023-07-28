using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MonsterType
{
    Melee,
    Ranged,
    Boss
};

public class MonsterObject : MonoBehaviour
{
    [SerializeField]
    private MonsterType m_MonsterType = new MonsterType();

    [SerializeField]
    private GameObject m_ProjectileLauncher;

    [SerializeField]
    private float m_MaxHealth;

    [SerializeField]
    private float m_Health;

    [SerializeField]
    private float m_CurrencyDrop;

    [SerializeField]
    private float m_Damage;

    [SerializeField]
    private GameObject m_DeathEffect;

    private ObjectController m_ObjectController;

    private GameObject m_Player;

    private bool m_Alive;

    private float m_DestroyTime;

    private float m_StoppingDistance;

    private float m_AttackDistance;

    private float m_TimeBetweenAttacks;

    private float m_AttackTimer;

    // Start is called before the first frame update
    void Start()
    {
        //m_Health = m_MaxHealth;

        if (m_Health <= 0.0f)
        {
            m_Health = 1.0f;
        }

        m_ObjectController = GetComponent<ObjectController>();

        m_Player = GameObject.FindGameObjectWithTag("Player");

        m_Damage = 1.0f;

        m_Alive = true;

        m_DestroyTime = 0.1f;

        if (m_MonsterType == MonsterType.Ranged)
        {
            m_StoppingDistance = Random.Range(2.0f, 5.0f);
            m_AttackDistance = Random.Range(5.0f, 6.0f);
            m_TimeBetweenAttacks = Random.Range(0.5f, 1.0f);
        } 
        else if (m_MonsterType == MonsterType.Melee)
        {
            m_StoppingDistance = 0.2f;
            m_AttackDistance = 1.0f;
            m_TimeBetweenAttacks = Random.Range(0.5f, 1.0f);
        } 
        else if (m_MonsterType == MonsterType.Boss)
        {
            m_StoppingDistance = Random.Range(5.0f, 8.0f);
            m_AttackDistance = Random.Range(8.0f, 10.0f);
            m_TimeBetweenAttacks = Random.Range(0.5f, 1.0f);
        }

        m_AttackTimer = m_TimeBetweenAttacks;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Alive)
        {
            FindTarget();
        }

        if (m_AttackTimer > 0.0f)
        {
            m_AttackTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (m_Health <= 0.0f && m_Alive)
        {
            m_Alive = false;

            m_Player.GetComponent<PlayerObject>().AddCurrency(m_CurrencyDrop);

            m_ObjectController.SetDirection(Vector2.zero);
        }

        /*    GameObject smokePuff = Instantiate(smoke, transform.position, transform.rotation) as GameObject;
    ParticleSystem parts = smokePuff.GetComponent<ParticleSystem>();
    float totalDuration = parts.duration + parts.startLifetime;
    Destroy(smokePuff, totalDuration);
*/

        if (!m_Alive)
        {
            m_DestroyTime -= Time.fixedDeltaTime;

            if (m_DestroyTime <= 0.0f)
            {
                Instantiate(m_DeathEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    private void Attack()
    {
        if (m_AttackTimer <= 0.0f)
        {
            if (m_MonsterType == MonsterType.Ranged)
            {
                m_ProjectileLauncher.GetComponent<ProjectileLauncherObject>().Launch();
            }
            else if (m_MonsterType == MonsterType.Melee)
            {
                m_Player.GetComponent<PlayerObject>().TakeDamage(m_Damage);
            } 
            else if (m_MonsterType == MonsterType.Boss)
            {
                m_ProjectileLauncher.GetComponent<ProjectileLauncherObject>().LaunchMultiple(16);
            }

            m_AttackTimer = m_TimeBetweenAttacks;
        }
    }

    public void SetHealth(float health)
    {
        m_Health = health;
    }

    public void TakeDamage(float damage)
    {
        m_Health -= damage;
    }

    private void FindTarget()
    {
        Transform playerTransform = m_Player.transform;

        Vector2 direction = Vector2.zero;

        m_ObjectController.SetDirection(direction);

        if (Vector2.Distance(playerTransform.position, transform.position) > m_StoppingDistance)
        {
            direction = playerTransform.position - transform.position;
        } 
        else if (Vector2.Distance(playerTransform.position, transform.position) < m_StoppingDistance)
        {
            direction = transform.position - playerTransform.position;
        }

        if (Vector2.Distance(playerTransform.position, transform.position) <= m_AttackDistance)
        {
            Attack();
        }

        m_ObjectController.AddAndNormalizeDirection(direction);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if  (collision.gameObject.tag == "Player")
        {
            //collision.gameObject.GetComponent<PlayerObject>().TakeDamage(m_Damage);
        }
    }
}