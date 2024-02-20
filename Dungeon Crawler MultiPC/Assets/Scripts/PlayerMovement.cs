using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // set movement speed of the player

    public Rigidbody2D rb; // creates a variable to access the rigidbody of the player
    public Animator animator; // created the animator variable, so it can attacthed to the player object
    private Vector2 movement; // store the x and y movements of the player

    private bool isDashing = false; // checks if the player is dashing or not
    public float DashDistance = 1.2f; // the distance of the dash
    public float DashDuration = 0.2f; // duration of the dash
    public float DashCooldown = 1.0f; // this is to delay the use of the dash
    private float DashCooldownTimer = 0f;



    void Update() // used for player input
    {
        if (DashCooldownTimer > 0f)
        {
            DashCooldownTimer -= Time.deltaTime; // updates the dash cooldown timer
        }

        if (!isDashing) // if the player is not currently dashing then the normal movement controls can be used 
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);

            if (Input.GetMouseButtonDown(1) && DashCooldownTimer <= 0f)
            {
                Dash();
            }
        }

    }

    void FixedUpdate() // used for updating movement faster
    {
        if (!isDashing)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime); // changes the position of the player in relation to player input, movement speed and time elapsed since last input
        }
    }

    private void Dash()
    {
        isDashing = true;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // get position of the mouse on screen (similar to the sword mechanic)
        mousePos.z = 0; // lock the z coord as not needed in 2D

        Vector2 dashDirection = (mousePos - transform.position).normalized; // calculate the direction of the dash

        rb.velocity = dashDirection * (DashDistance / DashDuration); // move the player in the direction of the dash

        Invoke("StopDash", DashDuration);
        DashCooldownTimer = DashCooldown; // start the cooldown timer for the dash
    }

    private void StopDash()
    {
        rb.velocity = Vector2.zero; // stops movement of the dash, so the player doesn't continue
        isDashing = false; // set dashing as false
    }
}
