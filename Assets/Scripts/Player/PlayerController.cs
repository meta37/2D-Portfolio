using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public bool isTouchLeft;
    public bool isTouchRight;
    public bool isTouchBottom;

    public PooledObject bulletA;
    public GameObject bulletB;

    public float maxShotDelay;
    public float curShotDelay;

    private void Start()
    {
        Manager.Pool.CreatePool(bulletA, 10, 20);
    }
    private void Update()
    {
        Move();
       

        Fire();

        Reload();
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if ((isTouchLeft && h == -1) || (isTouchRight && h == 1))
            h = 0;
        float v = Input.GetAxisRaw("Vertical");
        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;

    }

    private void Fire()
    {
        if (!Input.GetButton("Fire1"))

        if (curShotDelay < maxShotDelay)
            return;
        Manager.Pool.GetPool(bulletA, transform.position,  Quaternion.Euler(0, 0, 0));
        // GameObject bullet = Instantiate(bulletA, transform.position, transform.rotation);
        // Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        // rigid.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);

        curShotDelay = 0;
    }

    private void Reload()
    {
        curShotDelay += Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Left":
                    isTouchLeft = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
            }
        }
    }
}