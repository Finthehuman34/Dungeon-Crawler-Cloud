using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // set movement speed of the player

    public Rigidbody2D rb; // creates a variable to access the rigidbody of the player

    Vector2 movement; // store the x and y movements of the player
    void Update() // used for player input
    {
        movement.x = Input.GetAxisRaw("Horizontal"); // get the input for horizontal movement
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate() // used for updating movement faster
    {
        rb.MovePosition(rb.position + movement *moveSpeed * Time.fixedDeltaTime); // changes the position of the player in relation to player input, movement speed and time elapsed since last input
    }
}
