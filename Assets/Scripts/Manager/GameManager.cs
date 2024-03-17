using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public static GameManager Inst { get; private set; }

    public Transform[] spawnPoints;
    public GameObject[] enemyObjects;
    public GameObject player;
    public GameObject gameOverSet;
    public GameObject bombEffect;
    public Text scoreText;
    public Image[] lifeImage;
    public Image[] bombImage;

    public float maxSpawnDelay;
    public float curSpawnDelay;
    public float nextSpawnDelay;

    [HideInInspector] public int score;

    private void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay > maxSpawnDelay)
        {
            SpawnEnemy();
            maxSpawnDelay = Random.Range(0.5f, 3f);
            curSpawnDelay = 0;
        }

        UpdateScore();
    }

    public void UpdateScore()
    {
        if (player != null)
        {
            PlayerController playerLogic = player.GetComponent<PlayerController>();
            if (playerLogic != null)
            {
                scoreText.text = string.Format("{0:n0}", playerLogic.score);
            }
            else
            {
                Debug.LogError("PlayerController component not found on player object.");
            }
        }

    }
    List<Spawn> spawnList = new List<Spawn>();
    int spawnIndex;
    bool spawnEnd;

    private void SpawnEnemy()
    {
        PoolType poolType = PoolType.enemy01;
        if (spawnList[spawnIndex].type == "Enemy01")
            poolType = PoolType.enemy01;
        else if (spawnList[spawnIndex].type == "Enemy02")
            poolType = PoolType.enemy02;
        else if (spawnList[spawnIndex].type == "Enemy03")
            poolType = PoolType.enemy03;
        else if (spawnList[spawnIndex].type == "BOSS")
            poolType = PoolType.enemyBoss;

        int enemyPoint = spawnList[spawnIndex].point;

        var enemyObj = ObjectManager.Inst.MakeObj(poolType);
        enemyObj.transform.position = spawnPoints[enemyPoint].position;
        enemyObj.transform.rotation = spawnPoints[enemyPoint].rotation;

        var rigid = enemyObj.GetComponent<Rigidbody2D>();
        var enemy = enemyObj.GetComponent<Enemy>();

        if (enemyPoint == 5 || enemyPoint == 6)
        {
            enemyObj.transform.Rotate(Vector3.back * 90.0f);
            rigid.velocity = new Vector2(enemy.speed * -1.0f, -1.0f);
        }
        else if (enemyPoint == 7 || enemyPoint == 8)
        {
            enemyObj.transform.Rotate(Vector3.forward * 90.0f);
            rigid.velocity = new Vector2(enemy.speed, -1.0f);
        }
        else
        {
            rigid.velocity = new Vector2(0.0f, enemy.speed * -1.0f);
        }

        // #.리스폰 인덱스 증가
        spawnIndex++;
        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            nextCheckTime = Time.realtimeSinceStartup + 1.0f;
            return;
        }

        // #.다음 리스폰 딜레이 갱신
        nextSpawnDelay = spawnList[spawnIndex].delay;
    }
    float nextCheckTime = 300.0f;
    public void UpdateLifeIcon(int life)
    {
        for (int index = 0; index < 3; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }

        for (int index = 0; index < life; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }
    }

    public void UpdateBombIcon(int bomb)
    {
        for (int index = 0; index < 3; index++)
        {
            bombImage[index].color = new Color(1, 1, 1, 0);
        }

        for (int index = 0; index < bomb; index++)
        {
            bombImage[index].color = new Color(1, 1, 1, 0);
        }
    }

    public void RespawnPlayer()
    {
        Invoke("RespawnPlayerExe", 2f);
    }

    private void RespawnPlayerExe()
    {
        if (player != null)
        {
            player.transform.position = Vector3.down * 0.5f; // Reset player position or set to a spawn point
            player.SetActive(true); // Reactivate the player object
        }
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }

    public void GameRetry()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    uint curBombCnt = 1;
    uint MAX_BOMB = 3;
    bool IsBombTime = false;
    public void AddBomb(uint addCnt)
    {
        if (curBombCnt != MAX_BOMB)
        {
            curBombCnt += addCnt;
            UpdateBombIcon();
        }
        else
        {
            score += 500;
        }
    }

    public void ExecuteBomb()
    {
        if (IsBombTime)
            return;

        if (curBombCnt <= 0)
            return;

        curBombCnt--;
        UpdateBombIcon();

        IsBombTime = true;
        bombEffect.SetActive(true);

        var objList = new List<GameObject>();

        objList.AddRange(ObjectManager.Inst.GetPoolList(PoolType.enemy01));
        objList.AddRange(ObjectManager.Inst.GetPoolList(PoolType.enemy02));
        objList.AddRange(ObjectManager.Inst.GetPoolList(PoolType.enemy03));
        objList.AddRange(ObjectManager.Inst.GetPoolList(PoolType.enemyBoss));
        foreach (var item in objList)
        {
            if (item.activeSelf)
                item.GetComponent<Enemy>().OnHit(1000);
        }
        objList.Clear();

        objList.AddRange(ObjectManager.Inst.GetPoolList(PoolType.EnemyBulletA));
        objList.AddRange(ObjectManager.Inst.GetPoolList(PoolType.EnemyBulletB));
        objList.AddRange(ObjectManager.Inst.GetPoolList(PoolType.bulletEnemyBossA));
        objList.AddRange(ObjectManager.Inst.GetPoolList(PoolType.bulletEnemyBossB));
        foreach (var item in objList)
        {
            if (item.activeSelf)
                item.gameObject.SetActive(false);
        }

        Invoke(nameof(OffBombEffect), 3.0f);
    }

    void OffBombEffect()
    {
        IsBombTime = false;
        bombEffect.SetActive(false);
    }


    void UpdateBombIcon()
    {
        for (int i = 0; i < bombImage.Length; i++)
        {
            bombImage[i].color = Color.gray;
        }

        for (int i = 0; i < curBombCnt; i++)
        {
            bombImage[i].color = Color.white;
        }
    }
}