using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basic_enemy : MonoBehaviour
{
    public float speed;
    public float knockbackSpeed;
    public platformer player;

    bool isGroundedLeft = false;
    bool isGroundedRight = false;
    public float checkGroundRadius;
    public Transform groundedLeft, groundedRight, wallCheckerLeft, wallCheckerRight;
    public LayerMask groundLayer;

    int movementDirection = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector3 direction = (transform.position - collision.gameObject.transform.position).normalized;

            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-direction * knockbackSpeed);
            player.adjustCurrentHealth(-1);
        }
    }

    void Patrol()
    {
        int grounded = CheckIfGrounded();
        int touchingWall = CheckIfTouchingWall();
        if (movementDirection == 1)
        {
            this.transform.position = new Vector2(this.transform.position.x - speed, this.transform.position.y);
        }
        else if (movementDirection == 2)
        {
            this.transform.position = new Vector2(this.transform.position.x + speed, this.transform.position.y);
        }
        else if (movementDirection == 0)
        {
            movementDirection = 1;
        }

        if (grounded == 1)
        {
            movementDirection = 2;
        }
        else if (grounded == 2)
        {
            movementDirection = 1;
        }
        else if (grounded == 0)
        {
            movementDirection = 0;
        }

        if (touchingWall == 1)
        {
            movementDirection = 2;
        }
        else if (touchingWall == 2)
        {
            movementDirection = 1;
        }
    }

    int CheckIfGrounded()
    {
        Collider2D colliderLeft = Physics2D.OverlapCircle(groundedLeft.position, checkGroundRadius, groundLayer);
        Collider2D colliderRight = Physics2D.OverlapCircle(groundedRight.position, checkGroundRadius, groundLayer);
        if (colliderRight != null && colliderLeft == null)
        {
            return 1;
        }
        else if (colliderLeft != null && colliderRight == null)
        {
            return 2;
        }
        else if (colliderLeft != null && colliderRight != null)
        {
            this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
            return 3;
        }
        else
        {
            return 0;
        }
    }

    int CheckIfTouchingWall()
    {
        Collider2D colliderLeft = Physics2D.OverlapCircle(wallCheckerLeft.position, checkGroundRadius, groundLayer);
        Collider2D colliderRight = Physics2D.OverlapCircle(wallCheckerRight.position, checkGroundRadius, groundLayer);
        if (colliderRight == null && colliderLeft != null)
        {
            return 1;
        }
        else if (colliderLeft == null && colliderRight != null)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }
}
