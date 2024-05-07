using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool isUsed { get; set; } = false;
    // normalizing하고 넣기
    private Vector3 direction = Vector3.zero;

    public static float DEFAULT_BALL_VELOCITY = 7.0f;
    public float ball_velocity;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = Vector3.up * (-10);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isUsed)
        {
            setShowBall(false);
            this.transform.position = Vector3.up * (-10);
            return;
        }
        this.transform.position += direction * ball_velocity * Time.deltaTime;
    }

    public void setShowBall(bool show)
    {
        this.GetComponent<MeshRenderer>().enabled = show;
        //this.GetComponent<SphereCollider>().enabled = show;
    }

    private Vector3 getDirection()
    {
        return this.direction;
    }

    public void setDirection(Vector3 direction)
    {
        direction.y = 0;
        this.direction = direction;
    }
}
