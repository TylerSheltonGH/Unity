using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    [SerializeField]
    private Joystick m_MovementJoystick;

    [SerializeField]
    private Joystick m_ProjectileLauncherJoystick;

    [SerializeField]
    private float m_JoystickSensitivity;

    [SerializeField]
    private GameObject m_ProjectileLauncher;

    private Vector2 m_ProjectileLauncherDirection;

    private ProjectileLauncherObject m_ProjectileLauncherObject;

    private ObjectController m_ObjectController;

    private float m_Currency;

    [SerializeField]
    private float m_MaxHealth;

    private float m_Health;

    // Start is called before the first frame update
    void Start()
    {
        m_JoystickSensitivity = .01f;

        m_Currency = 100000.0f;

        m_ProjectileLauncherObject = m_ProjectileLauncher.GetComponent<ProjectileLauncherObject>();

        m_ObjectController = GetComponent<ObjectController>();

        m_MaxHealth = 100000.0f;// DEFAULT: 10

        m_Health = m_MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Health > m_MaxHealth)
        {
            m_Health = m_MaxHealth;
        }

        if (m_Health <= 0.0f)
        {
            //RIP
        }

        GetInput();
    }

    public void AddHealth(float health)
    {
        m_Health += health;
    }

    public void AddMaxHealth(float maxHealth)
    {
        m_MaxHealth += maxHealth;
    }

    public void TakeDamage(float damage)
    {
        m_Health -= damage;
    }

    public void AddCurrency(float amount)
    {
        m_Currency += amount;
    }

    public void SubtractCurrency(float amount)
    {
        m_Currency -= amount;
    }

    public float GetCurrency()
    {
        return m_Currency;
    }

    public float GetHealth()
    {
        return m_Health;
    }

    public float GetMaxHealth()
    {
        return m_MaxHealth;
    }

    private void GetInput()
    {
        Vector2 movementDirection = Vector2.zero;
        Vector2 projectileLauncherDirection = Vector2.zero;
        m_ProjectileLauncherDirection = Vector2.zero;

        m_ObjectController.SetDirection(movementDirection);

        if (m_MovementJoystick.Horizontal >= m_JoystickSensitivity || m_MovementJoystick.Horizontal <= -m_JoystickSensitivity)
        {
            movementDirection.x = m_MovementJoystick.Horizontal;
        }
        if (m_MovementJoystick.Vertical >= m_JoystickSensitivity || m_MovementJoystick.Vertical <= -m_JoystickSensitivity)
        {
            movementDirection.y = m_MovementJoystick.Vertical;
        }

        if (m_ProjectileLauncherJoystick.Horizontal >= m_JoystickSensitivity || m_ProjectileLauncherJoystick.Horizontal <= -m_JoystickSensitivity)
        {
            m_ProjectileLauncherDirection.x = m_ProjectileLauncherJoystick.Horizontal;
        }
        if (m_ProjectileLauncherJoystick.Vertical >= m_JoystickSensitivity || m_ProjectileLauncherJoystick.Vertical <= -m_JoystickSensitivity)
        {
            m_ProjectileLauncherDirection.y = m_ProjectileLauncherJoystick.Vertical;
        }

        if (Input.GetKey("w"))
        {
            movementDirection += Vector2.up;
        }
        if (Input.GetKey("a"))
        {
            movementDirection += Vector2.left;
        }
        if (Input.GetKey("s"))
        {
            movementDirection += Vector2.down;
        }
        if (Input.GetKey("d"))
        {
            movementDirection += Vector2.right;
        }

        m_ObjectController.AddAndNormalizeDirection(movementDirection);
        
        //projectileLauncherDirection = projectileLauncherDirection.normalized;
        m_ProjectileLauncher.GetComponent<ProjectileLauncherObject>().Rotate(m_ProjectileLauncherDirection);


        if (m_ProjectileLauncherDirection.x >= m_JoystickSensitivity || 
            m_ProjectileLauncherDirection.x <= -m_JoystickSensitivity ||
            m_ProjectileLauncherDirection.y >= m_JoystickSensitivity ||
            m_ProjectileLauncherDirection.y <= -m_JoystickSensitivity)
        {
            m_ProjectileLauncherObject.Launch();
        }
    }

    public Vector2 GetProjectileLauncherDirection()
    {
        return m_ProjectileLauncherDirection;
    }
}
