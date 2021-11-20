using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformer : MonoBehaviour
{
    Rigidbody2D rigBod;
    public float maxSpeed;
    public float acceleration;
    float currentSpeed;
    public float wallJumpForce;
    public float jumpForce;

    bool isGrounded = false;
    public Transform isGroundedChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;

    bool isTouchingWall = false;
    public Transform leftWallChecker;
    public Transform rightWallChecker;
    public float checkWallRadius;
    public LayerMask wallLayer;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public float rememberGroundedFor;
    float lastTimeGrounded;

    public int defaultAdditionalJumps = 1;
    int additionalJumps;

    public int defaultHealth;
    public int maxHealth;
    int currentMaxHealth;
    int currentHealth;

    public float wallDragSpeed;
    public float wallDragAcceleration;
    public float wallJumpDivisor;

    public float friction;
    public float topSpeedDeceleration;
    public float dashMultiplier;
    public float dashCooldown;
    float timeSinceLastDash;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector2(-0.1232877f, -1.506849f);
        rigBod = GetComponent<Rigidbody2D>();
        currentHealth = defaultHealth;
        currentMaxHealth = defaultHealth;
        timeSinceLastDash = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        CheckIfGrounded();
        WallJump();
        CheckIfOnWall();
        SpeedCheck();
        //Dash();

        if (Input.GetKeyDown(KeyCode.L)) currentHealth -= 1;
        if (currentHealth <= 0) Start();
    }

    void FixedUpdate()
    {
        SpeedCheck();
    }

    // Move player left and right
    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        if (x != 0)
        {
            float moveBy = x * acceleration;
            rigBod.velocity += new Vector2(moveBy, 0);
        }
        else if (rigBod.velocity.x > 0 && isGrounded)
        {
            rigBod.velocity -= new Vector2(friction, 0);
        }
        else if (rigBod.velocity.x < 0 && isGrounded)
        {
            rigBod.velocity += new Vector2(friction, 0);
        }
    }

    // Makes player jump
    // Controls for a limited number of multiple jumps
    // TODO: Implement controller support
    void Jump()
    {
        if (!isTouchingWall || isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor || additionalJumps > 0))
            {
                rigBod.velocity = new Vector2(rigBod.velocity.x, jumpForce);
                --additionalJumps;
            }
        }

        if (rigBod.velocity.y < 0)
        {
            rigBod.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rigBod.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rigBod.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    // Checks if the player is currently on the ground
    void CheckIfGrounded()
    {
        Collider2D collider = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);
        if (collider != null)
        {
            additionalJumps = defaultAdditionalJumps;
            isGrounded = true;
        }
        else
        {
            if (isGrounded)
            {
                lastTimeGrounded = Time.time;
            }
            isGrounded = false;
        }
    }

    // Implements a wall jump
    // TODO: TUNE
    void WallJump()
    {
        int onWall = CheckIfOnWall();
        if (Input.GetKeyDown(KeyCode.Space) && (onWall == 1))
        {
            rigBod.velocity += new Vector2(wallJumpForce, jumpForce/wallJumpDivisor);
            Debug.Log(rigBod.velocity);
            --additionalJumps;
        }
        if (Input.GetKeyDown(KeyCode.Space) && (onWall == 2))
        {
            rigBod.velocity += new Vector2(-wallJumpForce, jumpForce/wallJumpDivisor);
            Debug.Log(rigBod.velocity);
            --additionalJumps;
        }
    }

    // Checks if player is on the wall and returns which side is on the wall
    int CheckIfOnWall()
    {
        Collider2D leftCollider = Physics2D.OverlapCircle(leftWallChecker.position, checkWallRadius, wallLayer);
        Collider2D rightCollider = Physics2D.OverlapCircle(rightWallChecker.position, checkWallRadius, wallLayer);
        int side;
        if (leftCollider != null && Input.GetKey(KeyCode.A))
        {
            isTouchingWall = true;
            side = 1;
        }
        else if (rightCollider != null && Input.GetKey(KeyCode.D))
        {
            isTouchingWall = true;
            side = 2;
        }
        else
        {
            isTouchingWall = false;
            side = 0;
        }

        if (isTouchingWall && Math.Abs(Input.GetAxisRaw("Horizontal")) > 0)
        {
            if (rigBod.velocity.y >= wallDragSpeed)
                rigBod.velocity = new Vector2(rigBod.velocity.x, rigBod.velocity.y - wallDragAcceleration);
            else rigBod.velocity = new Vector2(rigBod.velocity.x, wallDragSpeed);
            Debug.Log("Slow deceleration");
        }

        return side;
    }

    // Regulates the speed of the player to a set maximum
    // TODO: Implement
    void SpeedCheck()
    {
        if (rigBod.velocity.magnitude > maxSpeed)
        {
            rigBod.velocity = rigBod.velocity.normalized * maxSpeed;
        }
    }

    // Allows a dash move
    // TODO: Implement
    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time - timeSinceLastDash > dashCooldown)
        {
            Debug.Log("Dashed!");
            timeSinceLastDash = Time.time;
            rigBod.velocity = new Vector2(rigBod.velocity.x * dashMultiplier, rigBod.velocity.y + dashMultiplier);
        }
    }

    public int getHealth() { return currentHealth; }
    public int getMaxHealth() { return maxHealth; }
    public int getCurrentMaxHealth() { return currentMaxHealth; }
}
