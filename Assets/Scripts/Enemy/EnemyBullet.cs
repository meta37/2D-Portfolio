using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] public float Speed = 3f; // 총알의 속도
    [SerializeField] public int damage; // 총알의 데미지 값
    [SerializeField] public float lifetime = 2f; // 총알이 파괴되기 전까지 존재하는 시간 (초 단위)

    private void Start()
    {
        Destroy(gameObject, lifetime); // 총알 생성 후 지정된 시간(lifetime)이 지나면 자동으로 파괴
    }
}