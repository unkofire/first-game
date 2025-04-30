using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // private Rigidbody RB;
    public float speed;
    public float height;
    private bool isGrounded = false;
    public Transform camTarget;
    public float maxAngle = 60;
    private Vector3 playerStartPosition;
    private Vector3 velocity; // keeps track of front and backwards movement
    private float yVelocity; // keeps track of up and down movement
    private CharacterController player;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundMask;
    public float gravityMult = 1f;
    private bool onCoyoteTime = false;
    public float maxCoyoteTime = 0.5f;
    private bool jumpInput = false;
    public float jumpBufferTime = 0.5f;
    private Coroutine jumpCoroutine;
    private bool groundedLastFrame = false; 
    // Start is called before the first frame update
    void Start()
    {
        playerStartPosition = this.transform.position;
        // RB = GetComponent<Rigidbody>();
        player = GetComponent<CharacterController>();
        Cursor.visible = false; // cursor not visible 
        Cursor.lockState = CursorLockMode.Locked; // keeps cursors in the middle of the screen

    }

    // Update is called once per frame
    void Update()
    {
        #region Camera Controls
        camTarget.position = this.transform.position; // cam target position follows player positions

        if (Input.GetMouseButton(1)) // left click = 0, right click = 1, scroll = 2
        {
            float mouseMovementX = Input.GetAxis("Mouse X"); // right and left. -1 is left, 1 is right 
            float mouseMovementY = -Input.GetAxis("Mouse Y"); // up and down. -1 is down, 1 is up 
            camTarget.rotation *= Quaternion.AngleAxis(mouseMovementX, Vector3.up); // Vector3.up is the Y axis  
            if ((mouseMovementY > 0) && ((camTarget.eulerAngles.x + mouseMovementY) > maxAngle) && (camTarget.eulerAngles.x <= 180)) // Are we looking up and are we above our max? 
            {
                camTarget.rotation = Quaternion.Euler(maxAngle, camTarget.eulerAngles.y, 0);
            }
            else if ((mouseMovementY < 0) && ((camTarget.eulerAngles.x + mouseMovementY) < 360 - maxAngle) && (camTarget.eulerAngles.x > 180)) // Are we looking down and are we at our max? 
            {
                camTarget.rotation = Quaternion.Euler(360 - maxAngle, camTarget.eulerAngles.y, 0);
            }
            else
            {
                camTarget.rotation *= Quaternion.AngleAxis(mouseMovementY, Vector3.right); // Vector3.right is the X axis  
            }
            camTarget.rotation = Quaternion.Euler(camTarget.eulerAngles.x, camTarget.eulerAngles.y, 0);
        }
        #endregion

        #region Inputs
        int x = 0; // 1 is right, left is -1
        int z = 0; // 1 forward, -1 backward
        if (Input.GetKey(KeyCode.W))
        {
            // RB.AddForce(camTarget.forward * speed); THIS IS PURE PHYSICS FOR IF YOU WANT TO MAKE A PHYSICS BASED GAME
            z += 1;

        }

        if (Input.GetKey(KeyCode.A))
        {
            //  RB.AddForce(camTarget.right * -speed);
            x -= 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            //RB.AddForce(camTarget.forward * -speed);
            z -= 1;
        }

        if (Input.GetKey(KeyCode.D))    // True if key is held 
        {
            // RB.AddForce(camTarget.right * speed); // updates with camera rotation 
            x += 1;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpCoroutine != null) // if jumpCoroutine is playing already
            {
                StopCoroutine(jumpCoroutine);
            }
            jumpCoroutine = StartCoroutine(jumpBuffer()); // If a player is approaching the ground and pressed space, the input is saved, and so the character jumps right away when they hit the ground. 

        }

        velocity = new Vector3(x, 0, z);

        if (velocity.magnitude > 0f) // if the player is trying to move
        {
            transform.rotation = Quaternion.Euler(0, camTarget.rotation.eulerAngles.y, 0);

        }

        velocity = Vector3.ClampMagnitude(velocity, 1); // prevents player from going super fast if moving diagonally 

        velocity *= speed;  // player goes faster than 1

        velocity = transform.TransformDirection(velocity); // Tells unity that this is the player's x and z.  

        player.Move(velocity * Time.deltaTime); // applys movement to player (while not depending on framerate) 
        #endregion


        #region up and down movement
        groundedLastFrame = isGrounded; 

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
        bool canJump = isGrounded || onCoyoteTime; 
        if (isGrounded && yVelocity < 0) // if you are on the ground and the velocity is increasing 
        {
            yVelocity = 0; // reset velocity so it doesn't increase when you are not in the air 
        }

        if (groundedLastFrame && !isGrounded && !onCoyoteTime) // when you walk off of a ledge 
        {
            StartCoroutine(coyoteTime());
        }


        if (jumpInput && canJump) 
        {
            //  RB.AddForce(new Vector3(0, height, 0), ForceMode.Impulse);
            yVelocity += Mathf.Sqrt(height * -3 * Physics.gravity.y * gravityMult); // take current gravity force and multiply it by -3 to counter gravity so you actually jump 
            isGrounded = false; // prevents coyoteTime from activating during a jump 
        }

        yVelocity += Physics.gravity.y * Time.deltaTime * gravityMult;

        player.Move(new Vector3(0, yVelocity, 0) * Time.deltaTime);
        Debug.Log(isGrounded);
        #endregion
        




        #region Player Reset
        if (this.transform.position.y <= -10)
        {
            this.transform.position = playerStartPosition; 
        }
        #endregion


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }

    private IEnumerator coyoteTime()
    {
        onCoyoteTime = true; 
        yield return new WaitForSeconds(maxCoyoteTime); // wait until x amount of time has passed 
        onCoyoteTime = false; 
    }

    private IEnumerator jumpBuffer()
    {
        jumpInput = true;
        yield return new WaitForSeconds(jumpBufferTime);
        jumpInput = false; 
    }
}
