using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncherObject : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Projectile;

    //[SerializeField]
    //private GameObject[] m_Projectiles;

    [SerializeField]
    private GameObject m_ProjectilePoint;

    [SerializeField]
    private float m_TimeBetweenLaunches;

    private float m_TimeBetweenLaunchesTimer;

    // Start is called before the first frame update
    void Start()
    {
        m_TimeBetweenLaunchesTimer = m_TimeBetweenLaunches;
    }

    // Update is called once per frame
    void Update()
    {
        //Rotate();

        if (m_TimeBetweenLaunchesTimer >= 0.0f)
        {
            m_TimeBetweenLaunchesTimer -= Time.deltaTime;
        }
    }

    public void SetTimeBetweenLaunches(float timeBetweenLaunches)
    {
        m_TimeBetweenLaunches = timeBetweenLaunches;
    }

    public void SubractTimeBetweenLaunches(float amount)
    {
        m_TimeBetweenLaunches -= amount;
    }

    public void SetExplodingProjeciltes(bool explodingProjectiles)
    {
        m_Projectile.GetComponent<ProjectileObject>().SetExplodes(explodingProjectiles);
    }

    public void AddLifeStealAmount(float lifeSteal)
    {
        m_Projectile.GetComponent<ProjectileObject>().AddLifeStealAmount(lifeSteal);
    }

    public void AddDamage(float damage)
    {
        m_Projectile.GetComponent<ProjectileObject>().AddDamage(damage);
    }

    public void Rotate(Vector2 direction)
    {
        //Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        /*Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - m_ProjectilePoint.transform.position;
        direction = direction.normalized;
        Vector3 point = new Vector3(direction.x * .2f, direction.y * .2f, 0);
        Vector3 axis = new Vector3(0, 0, 1);
        transform.RotateAround(point, axis, Time.deltaTime * 10);*/

        /*Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - m_ProjectilePoint.transform.position;
        direction = direction.normalized;

        Quaternion rot = Quaternion.AngleAxis(angle, rotationAxis);
        transform.position = m_ProjectilePoint.transform.position + rot * direction;
        transform.localRotation = rot;*/

        //Vector2 dir = Vector2.zero;

        //direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        /*else if (m_Type == 1)
        {
            dir = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
        }*/

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = rotation;
    }

    public void Launch()
    {
        if (m_TimeBetweenLaunchesTimer <= 0.0f)
        {
            Instantiate(m_Projectile, m_ProjectilePoint.transform.position, Quaternion.identity);

            m_TimeBetweenLaunchesTimer = m_TimeBetweenLaunches;
        }
    }

    public void LaunchMultiple(int count)
    {
        if (m_TimeBetweenLaunchesTimer <= 0.0f)
        {
            for (int i = 0; i < count; i++)
            {
                Instantiate(m_Projectile, m_ProjectilePoint.transform.position, Quaternion.identity);
            }

            m_TimeBetweenLaunchesTimer = m_TimeBetweenLaunches;
        }
    }
}
