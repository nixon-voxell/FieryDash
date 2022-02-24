using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private Rigidbody2D rb;
    public Animator camAnim;
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private int direction;

    public GameObject dashEffect;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        dashTime = startDashTime;
    }

    void Update()
    {
        if (direction == 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Instantiate(dashEffect, transform.position, Quaternion.identity);
                direction = 1;
            }
                
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Instantiate(dashEffect, transform.position, Quaternion.identity);
                direction = 2;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Instantiate(dashEffect, transform.position, Quaternion.identity);
                direction = 3;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Instantiate(dashEffect, transform.position, Quaternion.identity);
                direction = 4;
            }
        }
        else
        {
            if (dashTime <= 0)
            {
                direction = 0;
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;
            }
            else
            {
                dashTime -= Time.deltaTime;
                camAnim.SetTrigger("Shake");
                if (direction == 1)
                    rb.velocity = Vector2.left * dashSpeed;
                if (direction == 2)
                    rb.velocity = Vector2.right * dashSpeed;
                if (direction == 3)
                    rb.velocity = Vector2.up * dashSpeed;
                if (direction == 4)
                    rb.velocity = Vector2.down * dashSpeed;
            }

        }

    }
}