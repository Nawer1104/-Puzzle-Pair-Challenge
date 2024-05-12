using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float speed = 20f;

    private Rigidbody2D rb;

    private bool canMove = false;

    public GameObject vfx;

    private Vector3 startPos;

    public Vector2 direction;

    private int Id;

    public Type type;

    private void Awake()
    {
        startPos = transform.position;

        Id = GetInstanceID();

        rb = GetComponent<Rigidbody2D>();
    }

    private void OnMouseDown()
    {
        canMove = true;
    }

    private void Update()
    {
        if (!canMove) return;

        rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            Turning();
        }
        else if (collision.tag == "Block")
        {
            if (collision.GetComponent<Car>().type == type)
            {
                GameObject vfxExplosin = Instantiate(vfx, transform.position, Quaternion.identity);
                Destroy(vfxExplosin, 0.75f);

                canMove = false;

                GameManager.Instance.levels[GameManager.Instance.GetCurrentIndex()].gameObjects.Remove(gameObject);
                GameManager.Instance.CheckLevelUp();

                gameObject.SetActive(false);
            }
            else
            {
                Turning();
            }
        }
    }

    private void Turning()
    {
        canMove = false;

        transform.position = transform.position - (Vector3)direction * 0.1f;

        startPos = transform.position;

        direction = direction * -1;
    }
}