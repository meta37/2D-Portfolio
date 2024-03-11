using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float Speed = 5f; // 총알의 속도
    public int damage; // 총알의 데미지 값
    public float lifetime = 2f; // 총알이 파괴되기 전까지 존재하는 시간 (초 단위)

    private void Start()
    {
        Destroy(gameObject, lifetime); // 총알 생성 후 지정된 시간(lifetime)이 지나면 자동으로 파괴
    }

    void Update()
    {
        transform.Translate(Vector2.up * (Speed * Time.deltaTime), Space.Self); // 총알을 매 프레임마다 위로 이동시킴
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy") // 총알이 적과 충돌한 경우
        {
            Destroy(gameObject); // 총알 파괴
        }
    }
}