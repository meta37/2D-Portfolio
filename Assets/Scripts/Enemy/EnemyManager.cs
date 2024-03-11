using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    /*
    public Transform[] points; // 적이 출현할 위치를 저장할 배열
    public GameObject EnemyPrefab; // 적 프리팹
    public float createTime; // 적 생성 주기
    public int maxEnemy = 10; // 최대 적 수
    public bool isGameOver = false; // 게임 오버 여부

    public void Start()
    {
        // "SpawnPoint"를 찾아 모든 Transform 컴포넌트를 points 배열에 저장
        points = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();

        if (points.Length > 0)
        {
            StartCoroutine(this.CreateEnemy()); // 적 생성 코루틴 시작
        }
    }

    // 적을 생성하는 코루틴
    IEnumerator CreateEnemy()
    {
        while (!isGameOver) // 게임 오버가 아닐 때까지 반복
        {
            int enemyCount = (int)GameObject.FindGameObjectsWithTag("Enemy").Length; // 현재 적의 수 계산

            if (enemyCount < maxEnemy) // 최대 적 수보다 적을 경우
            {
                int idx = Random.Range(1, points.Length); // 생성 위치 랜덤 선택
                Instantiate(EnemyPrefab, points[idx].position, points[idx].rotation); // 적 생성
            }

            yield return new WaitForSeconds(createTime); // createTime 만큼 대기 후 다시 반복
        }
    }
    */
    public GameObject[] enemyObjects;
    public Transform[] spawnPoints;
    public GameObject player;

    public float maxSpawnDelay;
    public float curSpawnDelay;

    private void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if(curSpawnDelay > maxSpawnDelay)
        {
            SpawnEnemy();
            maxSpawnDelay = Random.Range(0.5f, 3f);
            curSpawnDelay = 0;
        }
    }

    private void SpawnEnemy()
    {
        // enemyObjects와 spawnPoints의 길이에 따라 랜덤 인덱스 생성
        int ranEnemy = Random.Range(0, enemyObjects.Length);
        int ranPoint = Random.Range(0, spawnPoints.Length);
        GameObject enemy = Instantiate(enemyObjects[ranEnemy], spawnPoints[ranPoint].position, spawnPoints[ranPoint].rotation);

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;
        // 랜덤 생성 위치에 따라 적의 초기 방향과 속도 설정
        if (ranPoint == 5 || ranPoint == 6)
        {
            enemy.transform.Rotate(Vector3.back * 90);
            rigid.velocity = Vector2.left * enemyLogic.speed;
        }
        else if (ranPoint == 7 || ranPoint == 8)
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.velocity = Vector2.right * enemyLogic.speed;
        }
        else
        {
            rigid.velocity = Vector2.down * enemyLogic.speed;
        }
    }
}
