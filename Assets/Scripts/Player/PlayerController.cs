using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float speed;
    public bool isTouchLeft, isTouchRight, isTouchBottom;
    public bool isHit;
    public bool isBombTime;

    public PooledObject bulletA;
    public PooledObject bulletB;

    [SerializeField] private float maxShotDelay;
    public float curShotDelay;
    public int power;
    public int maxpower;
    public int life;
    public int score;
    public int bomb;
    public int MAX_POWER = 3;


    public GameManager manager;
    public GameObject SpecialBomb;
    [SerializeField]Animator anim;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        Manager.Pool.CreatePool(bulletA, 100, 20);
        Manager.Pool.CreatePool(bulletB, 100, 20);
    }

    void Update()
    {
        Move();
        Fire();
        Bomb();
        Reload();
    }

    private void Bomb()
    {
        if (!Input.GetButton("Fire2"))
            return;
        if (isBombTime)
            return;

        if (bomb == 0)
            return;

        bomb--;
        SpecialBomb.SetActive(true);
        Invoke("OffSpecialBomb", 3f);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int index = 0; index < enemies.Length; index++)
        {
            Enemy enemyLogic = enemies[index].GetComponent<Enemy>();
            enemyLogic.OnHit(100);
        }

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        for (int index = 0; index < bullets.Length; index++)
        {
            Destroy(bullets[index]);
        }
    }
    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if ((isTouchLeft && h == -1) || (isTouchRight && h == 1)) h = 0;

        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;
        transform.position += nextPos;
    }

    void Fire()
    {
        if (!Input.GetButton("Fire1") || curShotDelay < maxShotDelay) return;

        Manager.Pool.GetPool(bulletA, transform.position, Quaternion.identity);
        Manager.Pool.GetPool(bulletB, transform.position, Quaternion.identity);
        curShotDelay = 0;
    }

    private void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Left":
                    isTouchLeft = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
            }
        }
        else if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {
            if (isHit)
                return;
            isHit = true;
            life--;
            manager.UpdateLifeIcon(life);

            if (life == 0)
            {
                manager.GameOver();
            }
            else
            {
                manager.RespawnPlayer();
            }
            gameObject.SetActive(false);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Item")
        { 
 
            var item = collision.gameObject.GetComponent<Item>();
            switch (item.itemType)
            {
                case Item.ItemType.Power:
                    if (power != MAX_POWER)
                    {
                        power++;
                        AddFollower();
                    }
                    else
                        GameManager.Inst.score += 500;
                    break;
                case Item.ItemType.Bomb:
                    GameManager.Inst.AddBomb(1);
                    break;
            }
            item.gameObject.SetActive(false);
        }
    }
    private void OffSpecialBomb()
    {
        SpecialBomb.SetActive(false);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {
            manager.RespawnPlayer();
            gameObject.SetActive(false);
            Destroy(collision.gameObject);
        }
    }

    public void Respawn()
    {
        isHit = false; // Reset isHit on respawn
        this.gameObject.SetActive(true);
        transform.position = Vector3.zero; // or any other starting position
    }

    List<Follower> followers = new List<Follower>();
    void AddFollower()
    {
        if (power >= 4)
        {
            var follower = ObjectManager.Inst.MakeObj(PoolType.follower).GetComponent<Follower>();
            follower.transform.position = this.gameObject.transform.position;
            followers.Add(follower);

            if (followers.Count == 1)
            {
                follower.parent = this.gameObject.transform;
            }
            else
            {
                follower.parent = followers[followers.Count - 2].transform;
            }
        }
    }

    public void FollowerActivate(bool state)
    {
        foreach (var follower in followers)
        {
            //follower.gameObject.SetActive(state);

            if (state)
            {
                follower.transform.position = this.gameObject.transform.position;
                follower.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                follower.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.25f);
            }
        }
    }
}