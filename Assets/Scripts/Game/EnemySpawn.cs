using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject player = null;

    public AudioSource effectSoundSource = null;

    public GameObject ballEnemyPrefab = null;
    public GameObject laserEnemyPrefab = null;
    public GameObject machineGunEnemyPrefab = null;
    public GameObject sniperEnemyPrefab = null;

    public float init_amount_enemy = 4.0f;
    private float enemy_spawn_Time = 8.0f;
    public float nextLevel_time = 20.0f;

    private List<LevelState> levelStates;

    public List<GameObject> enemies = null;

    private GameStatus game_status = null;

    private float timer = 0.0f;
    private float set_timer = 0.0f;
    public int set = 1;
    public float level = 0.0f;

    public AudioClip enemyDeadeffectSound = null;

    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.Find("Player");
        this.game_status = GameObject.Find("GameRoot").GetComponent<GameStatus>();
        enemies = new List<GameObject>();
        this.GetComponent<MeshRenderer>().enabled = false;
        this.effectSoundSource = GameObject.Find("EnemyDeadEffectSound").GetComponent<AudioSource>();
        effectSoundSource.clip = enemyDeadeffectSound;

        levelStates = new List<LevelState>();
        initLevelStates();

        enemy_spawn_Time = levelStates[1].enemySpawnTime;

        for (int i = 0; i < init_amount_enemy; i++)
        {
            createEnemy(set);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        set_timer += Time.deltaTime;

        if(timer > enemy_spawn_Time)
        {
            timer = 0.0f;
            createEnemy(set);
            createEnemy(set);
        }
        if(set_timer > nextLevel_time)
        {
            set_timer = 0.0f;
            set++;
            if (set % levelStates.Count == 0)
            {
                level++;
                set = 1;
            }
            enemy_spawn_Time = levelStates[set].enemySpawnTime;
            
        }
    }

    private void createEnemy(int set)
    {
        GameObject enemy;
        EnemyType.TYPE enemyType = EnemyType.TYPE.Ball;
        // int randomNum = Random.Range(level/4, Mathf.Clamp(level, 1, 4));
        int randomNum = Random.Range(1, 100 + 1);

        LevelState currentLevel = levelStates[set];
        if (randomNum <= currentLevel.ballTypeEnemyProbability) enemyType = EnemyType.TYPE.Ball;
        else if (randomNum <= currentLevel.snipeTypeEnemyProbability) enemyType = EnemyType.TYPE.SNIPING;
        else if (randomNum <= currentLevel.gunTypeEnemyProbability) enemyType = EnemyType.TYPE.GUN;
        else if (randomNum <= currentLevel.laserTypeEnemyProbability) enemyType = EnemyType.TYPE.LASER;
        else enemyType = EnemyType.TYPE.Ball;
        float min_enemy_shoot_cycle;
        switch (enemyType)
        {
            case EnemyType.TYPE.LASER:
                enemy = GameObject.Instantiate(laserEnemyPrefab) as GameObject;
                min_enemy_shoot_cycle = 5.0f - level * 0.6f;
                enemy.GetComponent<EnemyControl>().shoot_cycle_time = Random.Range(min_enemy_shoot_cycle, min_enemy_shoot_cycle + 1.0f);
                enemy.GetComponent<EnemyControl>().bulletType = EnemyType.TYPE.LASER;
                break;
            case EnemyType.TYPE.GUN:
                enemy = GameObject.Instantiate(machineGunEnemyPrefab) as GameObject;
                min_enemy_shoot_cycle = 5.0f - level * 0.75f;
                enemy.GetComponent<EnemyControl>().shoot_cycle_time = Random.Range(min_enemy_shoot_cycle, min_enemy_shoot_cycle + 1.0f);
                enemy.GetComponent<EnemyControl>().bulletType = EnemyType.TYPE.GUN;
                enemy.GetComponent<EnemyControl>().velocity = Ball.DEFAULT_BALL_VELOCITY + (1.0f * (level - 1));
                break;
            case EnemyType.TYPE.SNIPING:
                enemy = GameObject.Instantiate(sniperEnemyPrefab) as GameObject;
                min_enemy_shoot_cycle = 6.0f - level * 0.75f;
                enemy.GetComponent<EnemyControl>().shoot_cycle_time = Random.Range(min_enemy_shoot_cycle, min_enemy_shoot_cycle + 1.0f);
                enemy.GetComponent<EnemyControl>().bulletType = EnemyType.TYPE.SNIPING;
                enemy.GetComponent<EnemyControl>().velocity = Ball.DEFAULT_BALL_VELOCITY + (1.0f * (level - 1));
                break;
            default:
                enemy = GameObject.Instantiate(ballEnemyPrefab) as GameObject;
                min_enemy_shoot_cycle = 3.5f - level * 0.75f;
                enemy.GetComponent<EnemyControl>().shoot_cycle_time = Random.Range(min_enemy_shoot_cycle, min_enemy_shoot_cycle + 1.0f);
                enemy.GetComponent<EnemyControl>().bulletType = EnemyType.TYPE.Ball;
                enemy.GetComponent<EnemyControl>().velocity = Ball.DEFAULT_BALL_VELOCITY + (1.0f * (level - 1));
                break;
        }


        enemy.transform.position = Vector3.zero;
        do
        {
            enemy.transform.position =
                new Vector3(Random.Range(-13.0f, 13.0f), 1, Random.Range(-13.0f, 13.0f));
        } while (Vector3.Distance(enemy.transform.position, player.transform.position) < 4.0f);

        if (enemy.GetComponent<EnemyControl>().bulletType != EnemyType.TYPE.LASER)
        {
            enemies.Add(enemy);
        }
    }

    public void removeEnemy(GameObject enemy)
    {
        EnemyControl enemyScript = enemy.GetComponent<EnemyControl>();
        if (enemyScript.bulletType != EnemyType.TYPE.LASER)
        {
            enemies.Remove(enemy);
            enemyScript.getBallManager().allStopBall();
            Destroy(enemy);
        }
        this.effectSoundSource.Play();
    }

    /*
    private void assignBulletPrefab(GameObject enemy, EnemyType.TYPE type)
    {
        switch (type)
        {
            case EnemyType.TYPE.Ball:
                enemy.GetComponent<EnemyControl>().bulletPrefab = ballBulletPrefab;
                enemy.GetComponent<EnemyControl>().shoot_cycle_time = Random.Range(1.5f, 2.5f);
                enemy.GetComponent<EnemyControl>().bulletType = EnemyType.TYPE.Ball;
                break;
            case EnemyType.TYPE.LASER:
                enemy.GetComponent<EnemyControl>().bulletPrefab = laserBulletPrefab;
                enemy.GetComponent<EnemyControl>().shoot_cycle_time = Random.Range(5.0f, 8.0f);
                enemy.GetComponent<EnemyControl>().bulletType = EnemyType.TYPE.LASER;
                break;
        }
    }
    */

    
    private void initLevelStates()
    {
        levelStates.Add(null);
        levelStates.Add(new LevelState(100, 0, 0, 0, 7.0f));
        levelStates.Add(new LevelState(50, 100, 0, 0, 7.0f));
        levelStates.Add(new LevelState(33, 66, 99, 0, 6.5f));
        levelStates.Add(new LevelState(25, 50, 75, 100, 6.5f));
        levelStates.Add(new LevelState(10, 30, 65, 100, 6.0f));
        levelStates.Add(new LevelState(0, 10, 55, 100, 6.0f));
    }
    
}
