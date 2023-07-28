using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType
{
    Player,
    Monster,
    Boss
};

public class ProjectileObject : MonoBehaviour
{
    [SerializeField]
    private ProjectileType m_ProjectileType = new ProjectileType();

    [SerializeField]
    private float m_Damage;

    [SerializeField]
    private float m_LifeTime;

    [SerializeField]
    private bool m_Cripples;

    [SerializeField]
    private float m_CrippleAmount;

    [SerializeField]
    private bool m_Explodes;

    [SerializeField]
    private float m_ExplosionRadius, m_ExplosionForce, m_ExplosionDamage;

    [SerializeField]
    private GameObject m_DestroyEffect;

    [SerializeField]
    private bool m_LifeSteal;

    [SerializeField]
    private float m_LifeStealAmount;

    private bool m_Collided;

    private ObjectController m_ObjectController;

    private GameObject m_Player;

    [SerializeField]
    private GameObject m_ExplosionEffect;

    // Start is called before the first frame update
    void Start()
    {
        m_ObjectController = GetComponent<ObjectController>();

        m_Player = GameObject.FindGameObjectWithTag("Player");

        Vector2 direction = Vector2.zero;

        if (m_ProjectileType == ProjectileType.Player)
        {
            //direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), m_Player.GetComponent<Collider2D>());

            direction = m_Player.GetComponent<PlayerObject>().GetProjectileLauncherDirection();

            //CameraController.Instance.Shake(5.0f, 0.1f);
            CameraController.Instance.RandomShake(3.0f, 5.0f, 0.05f, 0.1f);
        }
        else if (m_ProjectileType == ProjectileType.Monster)
        {
            direction = m_Player.transform.position - transform.position;

            GameObject boss = GameObject.FindGameObjectWithTag("Boss");

            if (boss != null)
            {
                Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), boss.GetComponent<Collider2D>());
            }

            GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

            foreach(GameObject monster in monsters)
            {
                Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), monster.GetComponent<Collider2D>());
            }
        }
        else if (m_ProjectileType == ProjectileType.Boss)
        {
            direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            direction = direction.normalized;

            GameObject boss = GameObject.FindGameObjectWithTag("Boss");

            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), boss.GetComponent<Collider2D>());

            GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

            foreach (GameObject monster in monsters)
            {
                Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), monster.GetComponent<Collider2D>());
            }
        }

        m_ObjectController.AddAndNormalizeDirection(direction);

        m_Collided = false;

        m_LifeSteal = true;
    }

    // Update is called once per frame
    void Update()
    {
        m_LifeTime -= Time.deltaTime;

        if (m_LifeTime <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    public void AddLifeStealAmount(float lifeSteal)
    {
        m_LifeStealAmount += lifeSteal;
    }

    public void AddDamage(float damage)
    {
        m_Damage += damage;
    }

    public void SetExplodes(bool explodes)
    {
        m_Explodes = explodes;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*if (m_ProjectileType == ProjectileType.Monster && collision.collider.tag == "Monster")
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), collision.collider.GetComponent<Collider2D>());
            return;
        }*/

        if (!m_Collided)
        {
            m_Collided = true;

            if (m_Explodes)
            {
                Knockback();
            }

            string tag = collision.collider.tag;

            if (tag == "Player")
            {
                collision.collider.GetComponent<PlayerObject>().TakeDamage(m_Damage);
            }
            else if (tag == "Monster")
            {
                if (m_LifeSteal)
                {
                    m_Player.GetComponent<PlayerObject>().AddHealth(m_LifeStealAmount);
                }
                collision.collider.GetComponent<MonsterObject>().TakeDamage(m_Damage);
            }
            else if (tag == "Boss")
            {
                if (m_LifeSteal)
                {
                    m_Player.GetComponent<PlayerObject>().AddHealth(m_LifeStealAmount);
                }
                collision.collider.GetComponent<MonsterObject>().TakeDamage(m_Damage);
            }

            Instantiate(m_DestroyEffect, transform.position, Quaternion.identity);

            if (m_Explodes && m_ExplosionEffect != null)
            {
                Instantiate(m_ExplosionEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
    private void Knockback()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, m_ExplosionRadius);

        foreach (Collider2D nearby in collider2Ds)
        {
            Rigidbody2D rigidbody2D = nearby.GetComponent<Rigidbody2D>();
            if (rigidbody2D != null)
            {
                rigidbody2D.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

                float distance = Vector2.Distance(transform.position, rigidbody2D.transform.position);

                string tag = nearby.tag;

                if (tag == "Player")
                {
                    //nearby.GetComponent<PlayerObject>().TakeDamage(m_ExplosionDamage);
                }
                else if (tag == "Monster" || tag == "Boss")
                {
                    nearby.GetComponent<MonsterObject>().TakeDamage(m_ExplosionDamage - distance);

                    if (m_Cripples)
                    {
                        nearby.GetComponent<ObjectController>().Cripple(m_CrippleAmount / distance);
                    }
                }
            }
        }
    }
}
