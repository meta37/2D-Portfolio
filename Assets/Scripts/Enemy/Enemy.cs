using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int hp; // 적의 체력
    public int enemyScore;
    public int maxPower;
    public int maxBomb;
    public float speed; // 적의 속도
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

    // 적이 피해를 받았을 때 호출
    public void OnHit(int damage)
    {
        hp -= damage; // 받은 피해만큼 체력을 감소
        spriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);

        if (hp <= 0) // 체력이 0 이하면 적을 파괴
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
    // 다른 Collider와 충돌했을 때 자동으로 호출
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 적이 플레이어의 총알에 맞았을 경우만 처리
        if (collision.gameObject.tag == "PlayerBullet")
        {
            PlayerBullet bullet = collision.gameObject.GetComponent<PlayerBullet>(); // 충돌한 총알의 스크립트를 가져옴
            OnHit(bullet.damage); // 피해 처리 함수 호출
                                  // 적 파괴는 OnHit 내에서 체력 검사 후 처리
        }
    }
}
