using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Vector2 spawnArea;
    public float spawnRadius;

    public GameObject normalBall;
    public GameObject halfBall;
    public GameObject spinningBall;
    public GameObject turretBall;
    public GameObject spikeBall;


    [Range(0f, 1f)]
    public float normalBallChance = 0.5f;
    [Range(0f, 1f)]
    public float halfBallChance = 0.3f;
    [Range(0f, 1f)]
    public float spinningBallChance = 0.2f;
    [Range(0f, 1f)]
    public float turretBallChance = 0.1f;
    [Range(0f, 1f)]
    public float spikeBallChance = 0.1f;

    public Transform player;
    public int enemyAmount;
    private int enemiesRemaining;

    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.y > transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 500f, transform.position.z);
            Spawn();
        }
    }

    public void Spawn()
    {
        enemiesRemaining = enemyAmount;
        while (enemiesRemaining > 0)
        {
            Vector2 spawnPos = GetRandomSpawnPosition();
            GameObject enemyToSpawn = ChooseEnemyToSpawn();
            if (!Physics2D.OverlapCircle(spawnPos, spawnRadius))
            {
                Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
                Instantiate(enemyToSpawn, spawnPos, randomRotation);
                enemiesRemaining--;
            }
        }
    }

    private Vector2 GetRandomSpawnPosition()
    {
        return new Vector2(Random.Range(transform.position.x - spawnArea.x, transform.position.x + spawnArea.x), Random.Range(transform.position.y - spawnArea.y, transform.position.y + spawnArea.y));
    }

    private GameObject ChooseEnemyToSpawn()
    {
        float yPos = transform.position.y;
        float randomValue = Random.value;

        if (yPos < 500f)
        {
            if (randomValue < turretBallChance)
            {
                return turretBall;
            }
            else if (randomValue < turretBallChance + spikeBallChance)
            {
                return spikeBall;
            }
            else
            {
                return normalBall;
            }
        }
        else if (yPos < 1000f)
        {
            if (randomValue < turretBallChance)
            {
                return turretBall;
            }
            else if (randomValue < turretBallChance + halfBallChance)
            {
                return halfBall;
            }
            else if (randomValue < turretBallChance + halfBallChance + spikeBallChance)
            {
                return spikeBall;
            }
            else
            {
                return normalBall;
            }
        }
        else
        {
            if (randomValue < turretBallChance)
            {
                return turretBall;
            }
            else if (randomValue < turretBallChance + spinningBallChance)
            {
                return spinningBall;
            }
            else if (randomValue < turretBallChance + spinningBallChance + halfBallChance)
            {
                return halfBall;
            }
            else if (randomValue < turretBallChance + spinningBallChance + halfBallChance + normalBallChance)
            {
                return normalBall;
            }
            else
            {
                return spikeBall;
            }
        }
    }

    public void Reset()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        transform.position = new Vector3(0f, 250f, 0f);
    }
}