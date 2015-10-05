using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour {
    private int m_hits = 2;
    private Mesh m_mesh;
    private Vector3[] m_vertices;
   
    private Color[] m_colors = { Color.clear, Color.blue, Color.yellow, Color.green, Color.gray, Color.red };

    void Start()
    {
        UpdateColor();

        //if (m_hits <= 0)
        //{
        //    Destroy(this.gameObject);
        //}        
    }

	void OnCollisionEnter()
    {        
        m_hits--;
        if (m_hits <= 0)
        {
            Destroy(this.gameObject);
        }
        UpdateColor();
    }

    void UpdateColor()
    {
        SetColor(Colors(m_hits));
    }

    void SetColor(Color color)
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Color[] colors = new Color[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            colors[i] = color;
        }
        mesh.colors = colors;
    }

    public void SetHits(int number)
    {
        m_hits = number;
    }

    Color Colors(int color)
    {
        if (color > m_colors.Length - 1 )
        {
            //Debug.Log(m_colors.Length - 1);
            return m_colors[m_colors.Length - 1];
        }
        return m_colors[color];
    }
}
