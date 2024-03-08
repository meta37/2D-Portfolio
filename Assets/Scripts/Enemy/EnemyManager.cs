using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //���� ������ ��ġ�� ���� �迭
    public Transform[] points;
    //�� �������� �Ҵ��� ����
    public GameObject EnemyPrefab;

    //���� �߻���ų �ֱ�
    public float createTime;
    //���� �ִ� �߻� ����
    public int maxEnemy = 10;
    //���� ���� ���� ����
    public bool isGameOver = false;

    // Use this for initialization
    void Start()
    {
        //Hierarchy View�� Spawn Point�� ã�� ������ �ִ� ��� Transform ������Ʈ�� ã�ƿ�
        points = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();

        if (points.Length > 0)
        {
            //�� ���� �ڷ�ƾ �Լ� ȣ��
            StartCoroutine(this.CreateEnemy());
        }
    }

    IEnumerator CreateEnemy()
    {
        //���� ���� �ñ��� ���� ����
        while (!isGameOver)
        {
            // ���� ������ �� ���� ����
            int enemyCount = (int)GameObject.FindGameObjectsWithTag("Enemy").Length;

            if (enemyCount < maxEnemy)
            {
                // ���� ���� �ֱ� �ð���ŭ ���
                yield return new WaitForSeconds(createTime);

                //�ұ�Ģ���� ��ġ ����
                int idx = Random.Range(1, points.Length);
                // ���� ���� ����
                Instantiate(EnemyPrefab, points[idx].position, points[idx].rotation);
            }
            else
            {
                yield return null;
            }
        }
    }
}
