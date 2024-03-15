using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] public Transform[] spawnPoints;
    [SerializeField] public GameObject[] enemyObjects;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject gameOverSet;
    [SerializeField] public Text scoreText;
    [SerializeField] public Image[] lifeImage;
    public Image[] bombImage;

    public float maxSpawnDelay;
    public float curSpawnDelay;

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

    private void SpawnEnemy()
    {
        if (enemyObjects.Length == 0 || spawnPoints.Length == 0)
        {
            return;
        }

        int ranEnemy = Random.Range(0, enemyObjects.Length);
        int ranPoint = Random.Range(0, spawnPoints.Length);
        GameObject enemy = Instantiate(enemyObjects[ranEnemy], spawnPoints[ranPoint].position, spawnPoints[ranPoint].rotation);

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
    }

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
            player.transform.position = Vector3.zero; // Reset player position or set to a spawn point
            player.SetActive(true); // Reactivate the player object
        }

        else
        {
            Debug.LogError("Player object is not set in GameManager.");
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
}
