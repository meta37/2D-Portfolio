using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float speed;
    public bool isTouchLeft, isTouchRight, isTouchBottom;
    public bool isHit;

    public PooledObject bulletA;
    public PooledObject bulletB;

    [SerializeField] private float maxShotDelay;
    public float curShotDelay;
    public int life;
    public int score;

    public GameManager manager;

    void Start()
    {
        Manager.Pool.CreatePool(bulletA, 100, 20);
        Manager.Pool.CreatePool(bulletB, 100, 20);
    }

    void Update()
    {
        Move();
        Fire();
        Reload();
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
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Left": isTouchLeft = false; break;
                case "Right": isTouchRight = false; break;
                case "Bottom": isTouchBottom = false; break;
            }
        }
    }

    public void Respawn()
    {
        isHit = false; // Reset isHit on respawn
        this.gameObject.SetActive(true);
        transform.position = Vector3.zero; // or any other starting position
    }
}