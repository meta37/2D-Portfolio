using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    /*
    public Transform[] points; // ���� ������ ��ġ�� ������ �迭
    public GameObject EnemyPrefab; // �� ������
    public float createTime; // �� ���� �ֱ�
    public int maxEnemy = 10; // �ִ� �� ��
    public bool isGameOver = false; // ���� ���� ����

    public void Start()
    {
        // "SpawnPoint"�� ã�� ��� Transform ������Ʈ�� points �迭�� ����
        points = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();

        if (points.Length > 0)
        {
            StartCoroutine(this.CreateEnemy()); // �� ���� �ڷ�ƾ ����
        }
    }

    // ���� �����ϴ� �ڷ�ƾ
    IEnumerator CreateEnemy()
    {
        while (!isGameOver) // ���� ������ �ƴ� ������ �ݺ�
        {
            int enemyCount = (int)GameObject.FindGameObjectsWithTag("Enemy").Length; // ���� ���� �� ���

            if (enemyCount < maxEnemy) // �ִ� �� ������ ���� ���
            {
                int idx = Random.Range(1, points.Length); // ���� ��ġ ���� ����
                Instantiate(EnemyPrefab, points[idx].position, points[idx].rotation); // �� ����
            }

            yield return new WaitForSeconds(createTime); // createTime ��ŭ ��� �� �ٽ� �ݺ�
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
        // enemyObjects�� spawnPoints�� ���̿� ���� ���� �ε��� ����
        int ranEnemy = Random.Range(0, enemyObjects.Length);
        int ranPoint = Random.Range(0, spawnPoints.Length);
        GameObject enemy = Instantiate(enemyObjects[ranEnemy], spawnPoints[ranPoint].position, spawnPoints[ranPoint].rotation);

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;
        // ���� ���� ��ġ�� ���� ���� �ʱ� ����� �ӵ� ����
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
