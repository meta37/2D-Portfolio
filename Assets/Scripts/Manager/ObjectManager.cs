using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    enemy01,
    enemy02,
    enemy03,
    enemyBoss,

    follower,

    itemCoin,
    itemPower,
    itemBomb,

    BulletA,
    BulletB,
    EnemyBulletA,
    EnemyBulletB,
    bulletFollowerA,
    bulletEnemyBossA,
    bulletEnemyBossB,

    explosion,


    MAX
};

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Inst { get; private set; }

    [SerializeField] GameObject base_enemy01;
    [SerializeField] GameObject base_enemy02;
    [SerializeField] GameObject base_enemy03;
    [SerializeField] GameObject base_enemyBoss;
    [SerializeField] GameObject base_follower;

    [SerializeField] GameObject base_itemCoin;
    [SerializeField] GameObject base_itemPower;
    [SerializeField] GameObject base_itemBomb;

    [SerializeField] GameObject base_BulletA;
    [SerializeField] GameObject base_BulletB;
    [SerializeField] GameObject base_EnemyBulletA;
    [SerializeField] GameObject base_EnemyBulletB;
    [SerializeField] GameObject base_bulletFollowerA;
    [SerializeField] GameObject base_bulletEnemyBossA;
    [SerializeField] GameObject base_bulletEnemyBossB;

    [SerializeField] GameObject base_explosion;

    List<GameObject> pool_enemy01 = new List<GameObject>();
    List<GameObject> pool_enemy02 = new List<GameObject>();
    List<GameObject> pool_enemy03 = new List<GameObject>();
    List<GameObject> pool_enemyBoss = new List<GameObject>();
    List<GameObject> pool_follower = new List<GameObject>();

    List<GameObject> pool_itemCoin = new List<GameObject>();
    List<GameObject> pool_itemPower = new List<GameObject>();
    List<GameObject> pool_itemBomb = new List<GameObject>();

    List<GameObject> pool_BulletA = new List<GameObject>();
    List<GameObject> pool_BulletB = new List<GameObject>();
    List<GameObject> pool_EnemyBulletA = new List<GameObject>();
    List<GameObject> pool_EnemyBulletB = new List<GameObject>();
    List<GameObject> pool_bulletFollowerA = new List<GameObject>();
    List<GameObject> pool_bulletEnemyBossA = new List<GameObject>();
    List<GameObject> pool_bulletEnemyBossB = new List<GameObject>();

    List<GameObject> pool_explosion = new List<GameObject>();


    void Awake()
    {
        Inst = this;
        Generate();
    }

    void Generate()
    {
        pool_enemyBoss.Add(Instantiate(base_enemyBoss, this.gameObject.transform));
        ObjectAllHide(pool_enemyBoss);

        for (int i = 0; i < 5; i++)
        {
            pool_enemy01.Add(Instantiate(base_enemy01, this.gameObject.transform));
            pool_enemy02.Add(Instantiate(base_enemy02, this.gameObject.transform));
            pool_enemy03.Add(Instantiate(base_enemy03, this.gameObject.transform));
            pool_follower.Add(Instantiate(base_follower, this.gameObject.transform));

            pool_itemCoin.Add(Instantiate(base_itemCoin, this.gameObject.transform));
            pool_itemPower.Add(Instantiate(base_itemPower, this.gameObject.transform));
            pool_itemBomb.Add(Instantiate(base_itemBomb, this.gameObject.transform));

            pool_explosion.Add(Instantiate(base_explosion, this.gameObject.transform));

        }

        ObjectAllHide(pool_enemy01);
        ObjectAllHide(pool_enemy02);
        ObjectAllHide(pool_enemy03);
        ObjectAllHide(pool_follower);

        ObjectAllHide(pool_itemCoin);
        ObjectAllHide(pool_itemPower);
        ObjectAllHide(pool_itemBomb);

        ObjectAllHide(pool_explosion);

        for (int i = 0; i < 10; i++)
        {
            pool_BulletA.Add(Instantiate(base_BulletA, this.gameObject.transform));
            pool_BulletB.Add(Instantiate(base_BulletB, this.gameObject.transform));

            pool_EnemyBulletA.Add(Instantiate(base_EnemyBulletA, this.gameObject.transform));
            pool_EnemyBulletB.Add(Instantiate(base_EnemyBulletB, this.gameObject.transform));
            pool_bulletFollowerA.Add(Instantiate(base_bulletFollowerA, this.gameObject.transform));

            pool_bulletEnemyBossA.Add(Instantiate(base_bulletEnemyBossA, this.gameObject.transform));
            pool_bulletEnemyBossB.Add(Instantiate(base_bulletEnemyBossB, this.gameObject.transform));
        }

        ObjectAllHide(pool_BulletA);
        ObjectAllHide(pool_BulletB);

        ObjectAllHide(pool_EnemyBulletA);
        ObjectAllHide(pool_EnemyBulletB);
        ObjectAllHide(pool_bulletFollowerA);
        ObjectAllHide(pool_bulletEnemyBossA);
        ObjectAllHide(pool_bulletEnemyBossB);
    }
    public GameObject MakeObj(PoolType poolType)
    {
        var objList = GetPoolList(poolType);

        foreach (var item in objList)
        {
            if (item.activeSelf == false)
            {
                item.SetActive(true);
                return item;
            }
        }

        var newObject = Instantiate(GetPoolBasePrefab(poolType), this.gameObject.transform);
        objList.Add(newObject);
        return newObject;
    }

    void ObjectAllHide(List<GameObject> objList)
    {
        foreach (var item in objList)
        {
            item.SetActive(false);
        }
    }

    public List<GameObject> GetPoolList(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.enemy01: return pool_enemy01;
            case PoolType.enemy02: return pool_enemy02;
            case PoolType.enemy03: return pool_enemy03;
            case PoolType.enemyBoss: return pool_enemyBoss;
            case PoolType.follower: return pool_follower;

            case PoolType.itemCoin: return pool_itemCoin;
            case PoolType.itemPower: return pool_itemPower;
            case PoolType.itemBomb: return pool_itemBomb;

            case PoolType.BulletA: return pool_BulletA;
            case PoolType.BulletB: return pool_BulletB;
            case PoolType.EnemyBulletA: return pool_EnemyBulletA;
            case PoolType.EnemyBulletB: return pool_EnemyBulletB;
            case PoolType.bulletFollowerA: return pool_bulletFollowerA;
            case PoolType.bulletEnemyBossA: return pool_bulletEnemyBossA;
            case PoolType.bulletEnemyBossB: return pool_bulletEnemyBossB;

            case PoolType.explosion: return pool_explosion;
        }

        return null;
    }

    GameObject GetPoolBasePrefab(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.enemy01: return base_enemy01;
            case PoolType.enemy02: return base_enemy02;
            case PoolType.enemy03: return base_enemy03;
            case PoolType.enemyBoss: return base_enemyBoss;
            case PoolType.follower: return base_follower;

            case PoolType.itemCoin: return base_itemCoin;
            case PoolType.itemPower: return base_itemPower;
            case PoolType.itemBomb: return base_itemBomb;

            case PoolType.BulletA: return base_BulletA;
            case PoolType.BulletB: return base_BulletB;
            case PoolType.EnemyBulletA: return base_EnemyBulletA;
            case PoolType.EnemyBulletB: return base_EnemyBulletB;
            case PoolType.bulletFollowerA: return base_bulletFollowerA;
            case PoolType.bulletEnemyBossA: return base_bulletEnemyBossA;
            case PoolType.bulletEnemyBossB: return base_bulletEnemyBossB;

            case PoolType.explosion: return base_explosion;
        }

        return null;
    }
}
