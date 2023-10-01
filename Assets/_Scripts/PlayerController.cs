using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocity;
    private Rigidbody2D rb;
    private float attackInterval, attackStartTime;
    private bool isOnArena = true;

    void Start()
    {
        transform.position = Vector2.zero;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isOnArena) {
            rb.velocity = new Vector2(
                Mathf.Lerp(0, Input.GetAxisRaw("Horizontal") * velocity, 0.8f), 
                Mathf.Lerp(0, Input.GetAxisRaw("Vertical") * velocity, 0.8f)
            );

            if (Input.GetKeyDown(KeyCode.Space) && Time.time - attackStartTime >= attackInterval) {
                attackStartTime = Time.time;
                print("Attack");
            }
        }
        else {
            rb.velocity = new Vector2(
                Mathf.Lerp(rb.velocity.x, 0, 0.5f), 
                Mathf.Lerp(rb.velocity.x, 0, 0.5f)
            );

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Arena")) {
            isOnArena = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Arena")) {
            isOnArena = false;
            print("Player fell off the arena");
        }
    }
}
