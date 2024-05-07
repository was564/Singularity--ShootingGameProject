using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType
{
    public enum TYPE
    {
        NONE = -1, Ball, LASER, GUN, SNIPING, NUM
    }
}

public class EnemyControl : MonoBehaviour
{
    public GameObject bulletPrefab = null;
    private GameObject player = null;
    private GameStatus game_status = null;
    private EnemySpawn spawnInfo = null;

    public GameObject laser = null;
    private STEP step = STEP.NONE;

    public EnemyType.TYPE bulletType;

    private float shoot_timer = 0.0f;
    public float shoot_cycle_time = 2.0f;
    public static float MOVE_SPEED = 12.0f;

    private bool isStartingShoot = false;

    private BallManager ballManager = null;
    private Vector3 falledPositon = Vector3.zero;
    public float velocity = Ball.DEFAULT_BALL_VELOCITY;

    private float auto_fire_timer = 0.0f;
    public float total_auto_fire_time = 1.0f;
    private int fire_count = 0;
    public int max_fire_count = 6;

    public AudioClip fireGun = null;
    public AudioClip fireSniping = null;

    private AudioSource audio = null;

    public enum STEP
    { // 플레이어의 상태를 나타내는 열거체.
        NONE = -1, // 상태 정보 없음.
        MOVE = 0, // 이동 중.

        SHOOT, // 플레이어에게 공 쏘는 중 (추가)
        FALL, // 바다에 빠짐

        NUM, // 상태가 몇 종류 있는지 나타낸다(=3).
    };

    void Start()
    {
        this.audio = this.GetComponent<AudioSource>();
        this.game_status = GameObject.Find("GameRoot").GetComponent<GameStatus>();
        this.player = GameObject.Find("Player").gameObject;
        this.audio.volume = GameObject.Find("GameRoot").GetComponent<SceneControl>().soundSize;
        this.spawnInfo = GameObject.Find("EnemySpawn").GetComponent<EnemySpawn>();
        if (this.bulletType == EnemyType.TYPE.LASER)
        {
            this.laser = GameObject.Instantiate(bulletPrefab) as GameObject;
            this.laser.GetComponent<Laser>().shooter = this.gameObject;
            this.laser.GetComponent<Laser>().activateLaser(false);

            this.laser.GetComponent<Laser>().activate_time -= spawnInfo.level * 0.3f;
            this.laser.GetComponent<Laser>().maintain_time -= spawnInfo.level * 0.15f;
            this.laser.GetComponent<Laser>().stopping_time -= spawnInfo.level * 0.15f;
        }
        else this.ballManager = new BallManager(bulletPrefab);
        if (inCollision(player, this.gameObject)) this.step = STEP.MOVE;
        else this.step = STEP.SHOOT;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < -1.5f)
        {
            this.falledPositon = this.transform.position;
            this.step = STEP.FALL;
        }

        this.transform.LookAt(player.transform.position);

        if (this.step == STEP.MOVE)
        {
            if (isStartingShoot) isStartingShoot = false;
            Vector3 move_direction = -(player.transform.position - this.transform.position);
            move_direction.Normalize();

            if(this.bulletType == EnemyType.TYPE.LASER) 
                laser.GetComponent<Laser>().activateLaser(false);

            move_direction *= MOVE_SPEED * Time.deltaTime; // 속도×시간＝거리.
            this.transform.position += move_direction;
        }
        else if(this.step == STEP.SHOOT)
        {
            
            if(this.bulletType != EnemyType.TYPE.LASER) this.ballManager.update();

            
            shoot_timer += Time.deltaTime;
            if (this.bulletType == EnemyType.TYPE.GUN) auto_fire_timer += Time.deltaTime;

            if(!isStartingShoot)
            {
                isStartingShoot = true;
                this.shoot_timer = (shoot_cycle_time / 2);
            }
            if(shoot_timer > shoot_cycle_time)
            {
                if (this.bulletType == EnemyType.TYPE.LASER)
                    //&& Vector3.Distance(this.transform.position, player.transform.position) < 8.0f)
                {
                    laser.GetComponent<Laser>().activateLaser(true);
                    this.shoot_timer = 0.0f;
                }
                else if(this.bulletType == EnemyType.TYPE.Ball)
                {
                    ballManager.position = this.transform.position;


                    this.transform.Rotate(Vector3.up * Random.Range(-15.0f, 15.0f));

                    if (!player.GetComponent<PlayerControl>().invincible)
                    {
                        audio.clip = fireGun;
                        audio.Play();
                    }
                    ballManager.activateBall(this.transform.forward, velocity);
                    this.shoot_timer = 0.0f;
                }
                else if(this.bulletType == EnemyType.TYPE.GUN)
                {
                    ballManager.position = this.transform.position;

                    if (fire_count >= max_fire_count)
                    {
                        this.shoot_timer = 0.0f;
                        this.auto_fire_timer = 0.0f;
                        fire_count = 0;
                    }
                   
                    if (auto_fire_timer > total_auto_fire_time / max_fire_count)
                    {
                        auto_fire_timer = 0.0f;
                        fire_count++;
                        if (!player.GetComponent<PlayerControl>().invincible)
                        {
                            audio.clip = fireGun;
                            audio.Play();
                        }
                        ballManager.activateBall(this.transform.forward, velocity);
                    }
                }
                else if(this.bulletType == EnemyType.TYPE.SNIPING)
                {
                    ballManager.position = this.transform.position;
                    float ballVelocity = Vector3.Distance(player.transform.position, this.transform.position) * velocity / 7;
                    if (!player.GetComponent<PlayerControl>().invincible)
                    {
                        audio.clip = fireSniping;
                        audio.Play();
                    }
                    ballManager.activateBall(this.transform.forward, ballVelocity);
                    this.shoot_timer = 0.0f;
                }
            }
        }
        else if(this.step == STEP.FALL)
        {
            game_status.Score += 100;
            if (bulletType != EnemyType.TYPE.LASER)
            {
                GameObject.Find("EnemySpawn").GetComponent<EnemySpawn>().removeEnemy(this.gameObject);
            }
            else if(bulletType == EnemyType.TYPE.LASER)
            {
                GameObject.Find("EnemySpawn").GetComponent<EnemySpawn>().removeEnemy(this.gameObject);
                Destroy(this.laser.gameObject);
                Destroy(this.gameObject);
            }
            /*
            Vector3 position = -(falledPositon * Random.Range(0.0f, 0.7f));
            position.y = 0;
            this.transform.position = position;
            this.step = STEP.SHOOT;
            */
        }
    }

    void OnCollisionStay(Collision other)
    {
        
    }
    
    void OnTriggerStay(Collider other)
    {
        GameObject other_go = other.gameObject;
        // 트리거의 GameObject 레이어 설정이 Player이라면.
        if (other_go.layer == LayerMask.NameToLayer("Player"))
        {
            if (inCollision(other_go, this.gameObject)) this.step = STEP.MOVE;
            else this.step = STEP.SHOOT;
        }
    }

    void OnTriggerExit(Collider other)
    {
        this.step = STEP.SHOOT;
    }

    private bool inCollision(GameObject one, GameObject trigger)
    {
        float distance = Vector3.Magnitude(one.transform.position - trigger.transform.position);
        
        if(distance < trigger.GetComponent<SphereCollider>().radius - 0.5f)
        {
            return true;
        }
        return false;
    }

    private void rotateSmooth(Vector3 inputDirection)
    {
        Vector3 direction = inputDirection.normalized;
        Quaternion q = Quaternion.LookRotation(direction);
        this.transform.rotation =
            Quaternion.Lerp(this.transform.rotation, q, 0.05f);
    }

    private GameObject createBall()
    {
        return GameObject.Instantiate(bulletPrefab) as GameObject;
    }

    public BallManager getBallManager()
    {
        return this.ballManager;
    }
}

