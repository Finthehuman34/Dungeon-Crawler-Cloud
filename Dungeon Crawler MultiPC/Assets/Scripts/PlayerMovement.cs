using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // set movement speed of the player

    public Rigidbody2D rb; // creates a variable to access the rigidbody of the player
    public Animator animator; // created the animator variable, so it can attacthed to the player object
    Vector2 movement; // store the x and y movements of the player
    void Update() // used for player input
    {
        movement.x = Input.GetAxisRaw("Horizontal"); // get the input for horizontal movement
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal",movement.x); // gets input for the animator paramaters
        animator.SetFloat("Vertical",movement.y);
        animator.SetFloat("Speed",movement.sqrMagnitude);

    
        
    }

    void FixedUpdate() // used for updating movement faster
    {
        rb.MovePosition(rb.position + movement *moveSpeed * Time.fixedDeltaTime); // changes the position of the player in relation to player input, movement speed and time elapsed since last input
    }
}
