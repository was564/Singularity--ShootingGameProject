using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager
{
    public GameObject ballPrefab = null;
    public static int INIT_BALL_AMOUNT = 20;
    public Vector3 position { get; set; } = Vector3.zero;

    // priority queue는 .net.core 6부터만 지원 (유니티 .net FrameWork)
    private Queue<GameObject> usingBallQueue = null;
    private Queue<GameObject> waitingBallQueue = null;

    public BallManager(GameObject ballPrefab)
    {
        this.ballPrefab = ballPrefab;
        usingBallQueue = new Queue<GameObject>();
        waitingBallQueue = new Queue<GameObject>();
        for (int i = 0; i < INIT_BALL_AMOUNT; i++)
        {
            GameObject go = GameObject.Instantiate(ballPrefab) as GameObject;
            if (go == null) continue;
            waitingBallQueue.Enqueue(go);
        }
    }

    public void update()
    {
        if (usingBallQueue.Count == 0) return;
        GameObject go = usingBallQueue.Peek();
        if(Vector3.Magnitude(go.gameObject.transform.position) > PlayerControl.MOVE_AREA_RADIUS + 10.0f)
        {
            go.GetComponent<Ball>().isUsed = false;
        }
        if(!go.GetComponent<Ball>().isUsed)
        {
            waitingBallQueue.Enqueue(usingBallQueue.Dequeue());
        }
    }

    public void addBall()
    {
        waitingBallQueue.Enqueue(GameObject.Instantiate(ballPrefab) as GameObject);
    }

    public void activateBall(Vector3 direction, float velocity)
    {
        GameObject go = waitingBallQueue.Dequeue();
        Ball ball_info = go.GetComponent<Ball>();
        go.transform.position = this.position + (Vector3.up * 0.5f);
        ball_info.isUsed = true;
        ball_info.setDirection(direction);
        ball_info.ball_velocity = velocity;
        usingBallQueue.Enqueue(go);
    }

    public GameObject[] getUsingBallArray()
    {
        return usingBallQueue.ToArray();
    }

    public void allStopBall()
    {
        while(usingBallQueue.Count > 0)
        {
            GameObject ball = usingBallQueue.Dequeue();
            ball.GetComponent<Ball>().isUsed = false;
            waitingBallQueue.Enqueue(ball);
        }
    }
}
