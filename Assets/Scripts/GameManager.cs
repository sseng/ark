using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject rightWall, leftWall;
    public GameObject block;
    public GameObject score;
    public int paddleSpeed = 2;
    
    private int m_lives = 3;
    private int m_blockHpRange = 4;
    private InputManager m_input = new InputManager();
    private PaddleController m_paddleCon;
    private GameObject m_ball;
    private GameObject m_goPaddle;
    private GameObject m_goBrick; 
    private GameObject m_goBall;      
    private Rigidbody m_ballrb;
    private bool isBallActive;
    private ScoreBoard m_scoreboard;
    
    void Start()
    {
        Cursor.visible = false;

        m_goPaddle = Resources.Load("Paddle") as GameObject;
        m_goBrick = Resources.Load("Brick") as GameObject;
        m_goBall = Resources.Load("Ball") as GameObject;

        m_scoreboard = new ScoreBoard();

        InitializePaddle();
        GenerateBlocks(m_blockHpRange);
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
            UpdateScore();

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

    void GenerateBlocks(int blocksHp)
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

        for (int i = 0; i < (int)maxRows; i++)
        {
            for (int j = 0; j < (int)maxBlocks; j++)
            {
                int hits = Random.Range(0, blocksHp);
                if (hits == 0)
                    continue;

                rowOfBlocks[j] = new Vector2(x1 + j * (blockWidth + minOffset), y1 - i * (blockHeight + minOffset));
                GameObject brick = Instantiate(m_goBrick, rowOfBlocks[j], Quaternion.identity) as GameObject;
                Brick brickScript = brick.GetComponent<Brick>();
                brickScript.SetHp(hits);
                brickScript.Score(m_scoreboard);
            }
        }        
    }

    void UpdateScore()
    {
        if (score != null)
        {
            score.GetComponent<Text>().text = m_scoreboard.Score.ToString();
        }
        else
        {
            Debug.LogError("GameManager Score GameObject is null or not assigned");
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

    void SpawnBall()
    {
        float xPos = m_paddleCon.transform.position.x;
        float yPos = m_paddleCon.transform.position.y;
        Vector2 paddlePos = new Vector2(xPos, yPos + m_paddleCon.Height);

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