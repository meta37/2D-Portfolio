using System.Collections;
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

    public GameManager manager;
    public GameObject SpecialBomb;
    Animator anim;

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
                case "Left": isTouchLeft = true;
                    break;
                case "Right": isTouchRight = true;
                    break;
                case "Bottom": isTouchBottom = true;
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

            if(life == 0)
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
            Item item = collision.gameObject.GetComponent<Item>();
            switch (item.type)
            {
                case "Power":
                    if (power == maxpower)
                        score += 500;
                    else
                        power++;
                    break;
                case "Bomb":
                    SpecialBomb.SetActive(true);
                    Invoke("OffSpecialBomb", 3f);
                    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    for(int index =0; index < enemies.Length; index++)
                    {
                        Enemy enemyLogic = enemies[index].GetComponent<Enemy>();
                        enemyLogic.OnHit(100);
                    }
                    
                    GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
                    for (int index = 0; index < bullets.Length; index++)
                    {
                        Destroy(bullets[index]);
                    }
                    break;
            }
            Destroy(collision.gameObject);
        }
    }
    private void OffSpecialBomb()
    {
        SpecialBomb.SetActive(false);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
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
        else if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
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
}