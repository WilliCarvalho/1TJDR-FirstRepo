using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float velocity;
    [SerializeField] private float jumpForce;

    private Rigidbody2D rigidbody;
    private Transform playerTransform;
    private SpriteRenderer spriteRenderer;

    private bool canJump = true;
    private int doubleJump = 0;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal") * velocity * Time.deltaTime;
        playerTransform.Translate(new Vector3(moveX, 0));

        if (Input.GetButtonDown("Jump") && canJump /*isOnFloor == true*/)
        {
            doubleJump++;
            if (doubleJump >= 2)
            {
                canJump = false;
            }
            rigidbody.AddForce(Vector2.up * jumpForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        doubleJump = 0;
        canJump = true;

        if (collision.gameObject.tag == "Enemy")
        {
            StartCoroutine(DestroyPlayer());
        }
    }

    private IEnumerator DestroyPlayer()
    {
        Color color = spriteRenderer.material.color;
        for (float alpha = spriteRenderer.color.a; alpha < 0; alpha -= 0.1f)
        {
            color.a = alpha;
            yield return new WaitForSeconds(0.5f);
            spriteRenderer.material.color = color;
        }

        yield return new WaitForSeconds(1f);
        playerTransform.position = new Vector3(0, 0, 0);
    }
}
