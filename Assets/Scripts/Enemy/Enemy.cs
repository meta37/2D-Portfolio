using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int hp; // ���� ü��
    public int enemyScore;
    public int maxPower;
    public int maxBomb;
    public float speed; // ���� �ӵ�
    public Sprite[] sprites;

    public float maxShotDelay;
    public float curShotDelay;

    public GameObject EnemybulletA;
    public GameObject EnemybulletB;
    public GameObject itemPower;
    public GameObject itemBomb;

    public PlayerController player;

    SpriteRenderer spriteRenderer;

    private void Update()
    {
        Fire();
        Reload();
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        curShotDelay = maxShotDelay;
    }
    private void Fire()
    {
        if (curShotDelay < maxShotDelay)
            return;

        if (enemyName == "Enemy01")
        {
            GameObject bullet = Instantiate(EnemybulletA, transform.position,transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 dirVec = player.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }
        else if (enemyName == "Enemy02")
        {
            GameObject bulletA = Instantiate(EnemybulletA, transform.position + Vector3.right * 0.3f,transform.rotation);
            Rigidbody2D rigidA = bulletA.GetComponent <Rigidbody2D>();
            Vector3 dirVecA = player.transform.position - (transform.position + Vector3.right * 0.3f);
            rigidA.AddForce(dirVecA.normalized * 4, ForceMode2D.Impulse);

            GameObject bulletB = Instantiate(EnemybulletA, transform.position + Vector3.left * 0.3f , transform.rotation);
            Rigidbody2D rigidB = bulletB.GetComponent<Rigidbody2D>();
            Vector3 dirVecB = player.transform.position - (transform.position + Vector3.left * 0.3f);
            rigidB.AddForce(dirVecB.normalized * 4, ForceMode2D.Impulse);
        }
        else if(enemyName == "Enemy03")
        {
            GameObject bulletA = Instantiate(EnemybulletA, transform.position + Vector3.right * 0.3f, transform.rotation);
            Rigidbody2D rigidA = bulletA.GetComponent<Rigidbody2D>();
            Vector3 dirVecA = player.transform.position - (transform.position + Vector3.right * 0.3f);
            rigidA.AddForce(dirVecA.normalized * 4, ForceMode2D.Impulse);
        }
        curShotDelay = 0;
    }

    private void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    // ���� ���ظ� �޾��� �� ȣ��
    public void OnHit(int damage)
    {
        hp -= damage; // ���� ���ظ�ŭ ü���� ����
        spriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);

        if (hp <= 0) // ü���� 0 ���ϸ� ���� �ı�
        {
            PlayerController playerLogic = player.GetComponent<PlayerController>();
            playerLogic.score += enemyScore;

            int ran = Random.Range(0, 10);
            if(ran < 5)
            {
                Debug.Log("Not Item");
            }
            else if(ran < 7)
            {
                Debug.Log("Not Item");
            }
            Destroy(gameObject);
        }
    }

    private void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }
    // �ٸ� Collider�� �浹���� �� �ڵ����� ȣ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� �÷��̾��� �Ѿ˿� �¾��� ��츸 ó��
        if (collision.gameObject.tag == "PlayerBullet")
        {
            PlayerBullet bullet = collision.gameObject.GetComponent<PlayerBullet>(); // �浹�� �Ѿ��� ��ũ��Ʈ�� ������
            OnHit(bullet.damage); // ���� ó�� �Լ� ȣ��
                                  // �� �ı��� OnHit ������ ü�� �˻� �� ó��
        }
    }
}
