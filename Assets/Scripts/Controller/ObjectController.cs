using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UpdateType
{
    Frame,
    Fixed
};

public enum MovementType
{
    Transform,
    Rigidbody2D
};

public class ObjectController : MonoBehaviour
{
    [SerializeField]
    private UpdateType m_UpdateType = new UpdateType();

    [SerializeField]
    private MovementType m_MovementType = new MovementType();

    [SerializeField]
    private float m_Speed;

    [SerializeField]
    private bool m_RandomSpeed;

    [SerializeField]
    private float m_MinSpeed, m_MaxSpeed;

    private Vector2 m_Direction;

    private Rigidbody2D m_Rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        if (m_RandomSpeed)
        {
            m_Speed = Random.Range(m_MinSpeed, m_MaxSpeed);
        }

        m_Direction = Vector2.zero;

        if (m_MovementType == MovementType.Rigidbody2D)
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_UpdateType == UpdateType.Frame)
        {
            Move(Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        if (m_UpdateType == UpdateType.Fixed)
        {
            Move(Time.fixedDeltaTime);
        }
    }

    public void SetDirection(Vector2 direction)
    {
        m_Direction = direction;
    }

    public void AddDirection(Vector2 direction)
    {
        m_Direction += direction;
    }

    public void NormalizeDirection()
    {
        m_Direction = m_Direction.normalized;
    }

    public void AddAndNormalizeDirection(Vector2 direction)
    {
        m_Direction += direction;
        m_Direction = m_Direction.normalized;
    }

    public void Cripple(float amount)
    {
        m_Speed -= amount;

        if (m_Speed < 1.0f)
        {
            m_Speed = 1.0f;
        }
    }

    private void Move(float time)
    {
        if (m_MovementType == MovementType.Transform)
        {
            transform.position = new Vector3(transform.position.x + m_Speed * m_Direction.x * time,
                                             transform.position.y + m_Speed * m_Direction.y * time, 0.0f);
        }
        else if (m_MovementType == MovementType.Rigidbody2D)
        {
            m_Rigidbody2D.position += m_Speed * m_Direction * time;
        }
    }
}
