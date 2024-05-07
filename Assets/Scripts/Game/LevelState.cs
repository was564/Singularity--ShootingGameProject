using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelState
{
    public int ballTypeEnemyProbability;
    public int snipeTypeEnemyProbability;
    public int gunTypeEnemyProbability;
    public int laserTypeEnemyProbability;

    public float enemySpawnTime;

    public LevelState(int ball, int snipe, int gun, int laser, float spawnTime)
    {
        this.ballTypeEnemyProbability = ball;
        this.snipeTypeEnemyProbability = snipe;
        this.gunTypeEnemyProbability = gun;
        this.laserTypeEnemyProbability = laser;
        this.enemySpawnTime = spawnTime;
    }
    /*
    public float ballShootCycle;
    public float snipeShootCycle;
    public float gunShootCycle;
    public float laserShootCycle;

    public float bulletVelocity;

    public float laserActivateTime;
    public float laserMaintainTime;
    public float laserStoppingTime;
    */
}
