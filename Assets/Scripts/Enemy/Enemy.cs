using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int hp; // 적의 체력
    public int enemyScore;
    public float speed; // 적의 속도

    public float maxShotDelay;
    public float curShotDelay;

    public GameObject EnemybulletA;
    public GameObject EnemybulletB;
    public GameObject player;

    private void Update()
    {
        Fire();
        Reload();
    }

    private void Awake()
    {
        // 초기화 코드는 여기에 작성 (현재는 비어 있음)
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
        curShotDelay = 0;
    }

    private void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    // 적이 피해를 받았을 때 호출
    private void OnTakeHit(int damage)
    {
        hp -= damage; // 받은 피해만큼 체력을 감소

        if (hp <= 0) // 체력이 0 이하면 적을 파괴
        {
            PlayerController playerLogic = player.GetComponent<PlayerController>();
            playerLogic.score += enemyScore;
            Destroy(gameObject);
        }
    }

    // 다른 Collider와 충돌했을 때 자동으로 호출
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyBullet") // 적의 총알에 맞았을 경우
            Destroy(gameObject); // 적을 바로 파괴
        else if (collision.gameObject.tag == "PlayerBullet") // 플레이어의 총알에 맞았을 경우
        {
            PlayerBullet bullet = collision.gameObject.GetComponent<PlayerBullet>(); // 충돌한 총알의 스크립트를 가져옴
            OnTakeHit(bullet.damage); // 피해 처리 함수 호출
            // 적 파괴는 OnTakeHit 내에서 체력 검사 후 처리
        }
    }
}
