using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed; // 플레이어 이동 속도
    public bool isTouchLeft, isTouchRight, isTouchBottom; // 플레이어가 각각의 경계에 닿았는지 여부
    public bool isHit;

    public PooledObject bulletA; // 총알 오브젝트 풀 참조
    public PooledObject bulletB; // 대체 총알 프리팹

    public float maxShotDelay; // 총알 발사 사이의 최대 대기 시간
    public float curShotDelay; // 현재 총알 발사까지 남은 대기 시간
    public int life;
    public int score;

    public GameManager manager;

    void Start()
    {
        Manager.Pool.CreatePool(bulletA, 100, 20); // 게임 시작 시 총알 오브젝트 풀 생성
        Manager.Pool.CreatePool(bulletB, 100, 20);
    }

    void Update()
    {
        Move(); // 플레이어 이동 처리
        Fire(); // 총알 발사 처리
        Reload(); // 총알 재장전(대기 시간 업데이트) 처리
    }

    void Move()
    {
        // 키보드 입력을 통해 이동 방향 결정
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 플레이어가 경계에 닿았다면 해당 방향으로는 이동하지 않음
        if ((isTouchLeft && h == -1) || (isTouchRight && h == 1)) h = 0;

        Vector3 curPos = transform.position; // 현재 위치
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime; // 다음 위치 계산

        transform.position = curPos + nextPos; // 플레이어 위치 업데이트
    }

    void Fire()
    {
        // 발사 버튼을 누르지 않았거나, 재장전이 완료되지 않았다면 발사하지 않음
        if (!Input.GetButton("Fire1") || curShotDelay < maxShotDelay) return;

        Manager.Pool.GetPool(bulletA, transform.position, Quaternion.Euler(0, 0, 0)); // 총알 발사
        curShotDelay = 0; // 총알 발사 후 대기 시간을 0으로 리셋
        Manager.Pool.GetPool(bulletB, transform.position, Quaternion.Euler(0, 0, 0));
        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime; // 대기 시간 업데이트
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어가 게임 경계에 닿았을 때의 처리
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Left": isTouchLeft = true;
                    break;
                case "Right": isTouchRight = true;
                    break;
                case "Bottom": isTouchBottom = true;
                    break;
            }
        }
        else if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {
            if (isHit)
                return;
            isHit = true;


            manager.RespawnPlayer();
            gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // 플레이어가 게임 경계에서 벗어났을 때의 처리
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Left": isTouchLeft = false;
                    break;
                case "Right": isTouchRight = false;
                    break;
                case "Bottom": isTouchBottom = false;
                    break;
            }
        }
    }
}