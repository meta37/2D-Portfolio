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
}
