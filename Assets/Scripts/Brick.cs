using UnityEngine;

public class Brick : MonoBehaviour, IScoreable, IColorable {
    private int m_hp = 1;
    private Mesh m_mesh;
    private Vector3[] m_vertices;
    private ScoreBoard m_score;
    private int m_points = 10;
   
    private Color[] m_colors = { Color.gray, Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan };

    void Start()
    {
        if (m_hp > 0)
        {
            UpdateColor();
        }
    }

	void OnCollisionEnter()
    {
        if(m_hp > 1)
        {
            addScore();
            m_hp--;
            UpdateColor();
        }
        else
        {
            addScore();
            Destroy(this.gameObject);
        }
    }
    //Interface methods
    public void Score(ScoreBoard score)
    {
        m_score = score;
    }

    public Color Colors(int index)
    {
        if (index > m_colors.Length - 1)
        {
            return m_colors[m_colors.Length - 1];
        }
        return m_colors[index];
    }

    //class methods
    void UpdateColor()
    {
        SetColor(Colors(m_hp));
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

    void addScore()
    {
        m_score.Score += m_points;
    }

    public void SetHp(int hp)
    {
        m_hp = hp;
    }
}