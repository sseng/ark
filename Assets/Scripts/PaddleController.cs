using UnityEngine;

public class PaddleController : MonoBehaviour, IMoveable<Vector3> {
    private float m_speed = 1;

    public float MaxLeft
    {
        get;
        set;
    }

    public float MaxRight
    {
        get;
        set;
    }

    public float Speed
    {
        get
        {
            return m_speed;
        }
        set
        {
            {
                m_speed = value;
            }
        }
    }

    public float Width
    {
        get { return transform.localScale.x; }
    }

    public float Height
    {
        get { return transform.localScale.y; }
    }

    public void MoveTo(Vector3 destination)
    {
        float y = transform.position.y;
        float z = transform.position.z;
        Vector3 d = new Vector3(destination.x, y, z);

        d.x = Mathf.Clamp(d.x, MaxLeft, MaxRight);

        transform.position = Vector3.Lerp(transform.position, d, m_speed * Time.deltaTime);
    }
}