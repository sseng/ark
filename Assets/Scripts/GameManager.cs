using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject rightWall, leftWall;
    public GameObject block;
    public int paddleSpeed = 2;

    private int m_lives = 3;
    private InputManager m_input = new InputManager();
    private PaddleController m_paddleCon;
    private GameObject m_ball;
    private GameObject m_goPaddle;
    private GameObject m_goBrick; 
    private GameObject m_goBall;      
    private Rigidbody m_ballrb;
    private bool isBallActive;
    
    void Start()
    {
        Cursor.visible = false;

        m_goPaddle = Resources.Load("Paddle") as GameObject;
        m_goBrick = Resources.Load("Brick") as GameObject;
        m_goBall = Resources.Load("Ball") as GameObject;

        InitializePaddle();

        GenerateBlocks();
        SpawnBall();

        //Debug.Log(string.Format("Board width : {0} ", boardWidth));
    }

	void Update () 
    {
        PaddleControl();

        if (!isBallActive)
        {
            if (m_lives > 0 && Input.GetMouseButtonDown(0))
            {
                m_ballrb.AddForce(new Vector2(400, 500));
                isBallActive = true;
            }
        }

        else
        {
            if (BallOutOfBounds())
            {
                Destroy(m_ball);
                isBallActive = false;
                m_lives--;
                if (m_lives > 0)
                {
                    SpawnBall();
                }
            }
        }
        //Debug.Log(string.Format("Paddle width: {0} ", m_paddle.Width));        
	}

    void GenerateBlocks()
    {
        float blockWidth = block.transform.localScale.x;
        float blockHeight = block.transform.localScale.y;
        float wallwidth = leftWall.transform.localScale.x / 2 + rightWall.transform.localScale.x / 2;
        float boardWidth = rightWall.transform.position.x - leftWall.transform.position.x - wallwidth;
        float boardHeight = leftWall.transform.lossyScale.y - leftWall.transform.localScale.x;
        float minOffset = 0.25f;
        float maxBlocks = Mathf.Floor((boardWidth - minOffset * 2) / blockWidth);
        float offset = (boardWidth - (blockWidth + minOffset) * maxBlocks) / 2;

        float x1 = (leftWall.transform.position.x + leftWall.transform.localScale.x / 2) + offset + minOffset/2 + blockWidth / 2;
        float y1 = boardHeight - blockHeight / 2;

        float maxRows = Mathf.Floor((boardHeight / 2) / blockHeight) - 1;

        Vector2[] rowOfBlocks = new Vector2[(int)maxBlocks];
        //Debug.Log(string.Format("rows: {0}", row1.Length));

        int brickColorRange = 8;

        for (int i = 0; i < (int)maxRows; i++)
        {
            for (int j = 0; j < (int)maxBlocks; j++)
            {
                int hits = Random.Range(0, brickColorRange);
                if (hits == 0)
                    continue;

                rowOfBlocks[j] = new Vector2(x1 + j * (blockWidth + minOffset), y1 - i * (blockHeight + minOffset));
                GameObject brick;
                brick = Instantiate(m_goBrick, rowOfBlocks[j], Quaternion.identity) as GameObject;
                brick.GetComponent<Brick>().SetHits(Random.Range(0, brickColorRange));
            }
        }        
    }

    void PaddleControl()
    {
        m_paddleCon.MoveTo(m_input.MousePosition);
    }

    bool BallOutOfBounds()
    {
        return m_ball.transform.position.y < -2;
    }

    float LeftBorder()
    {
        return leftWall.transform.position.x + (leftWall.transform.localScale.x / 2) + m_paddleCon.Width/2; 
    }

    float RightBorder()
    {
        return rightWall.transform.position.x - (leftWall.transform.localScale.x / 2) - m_paddleCon.Width/2;
    }

    float PaddleXPos()
    {
        return m_paddleCon.transform.position.x;
    }

    float PaddleYPos()
    {
        return m_paddleCon.transform.position.y;
    }

    void SpawnBall()
    {
        Vector2 paddlePos = new Vector2(PaddleXPos(), PaddleYPos() + m_paddleCon.Height);
        m_ball = Instantiate(m_goBall, paddlePos, Quaternion.identity ) as GameObject;
        m_ballrb = m_ball.GetComponent<Rigidbody>();
        isBallActive = false;
    }

    void InitializePaddle()
    {
        GameObject paddle = Instantiate(m_goPaddle) as GameObject;
        m_paddleCon = paddle.GetComponent<PaddleController>();
        m_paddleCon.MaxLeft = LeftBorder();
        m_paddleCon.MaxRight = RightBorder();
        m_paddleCon.Speed = paddleSpeed;
    }
}