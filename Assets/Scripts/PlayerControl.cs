using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// NOTE: PUBLIC variables can have their values edited on unity, PRIVATE cannot. 
[RequireComponent(typeof(AnimationController))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerControl : MonoBehaviour
{
    // private Rigidbody RB;
    public float speed; // speed of player
    public float height; // height of jump 
    private bool isGrounded = false; // is the player on the ground currently? 
    public Transform camTarget; // where the camera faces/follows (player) 
    public float maxAngle = 60; // the max that a person can look (up and down). 
    private Vector3 playerStartPosition; // where the player first psawns 
    private Vector3 velocity; // keeps track of front and backwards movement
    private float yVelocity; // keeps track of up and down movement
    private CharacterController player; 
    public Transform groundCheck; // checks if the player's feet is on the ground 
    public float groundCheckRadius = 0.2f; // the radius of ground check (size) 
    public LayerMask groundMask; // what layers count as the ground 
    public float gravityMult = 1f; // how much gravity affects jump and fall 
    private bool onCoyoteTime = false; // if player just walked off a ledge
    public float maxCoyoteTime = 0.5f; // max time that the player can air jump after walking off ledge
    private bool jumpInput = false; // if jump was pressed
    public float jumpBufferTime = 0.5f; // amount of time between player jump and the ground, valid for another jump 
    private Coroutine jumpCoroutine; // sets jump imput on a timer (how long the jump input is valid for before you have to press space again) If player lands within 0.5 seconds, then they automatically jump.
    private Coroutine coyoteCoroutine; // sets coyote time 
    private bool groundedLastFrame = false; // if the player was on the ground in the previous frame 
    public int maxAirJumpCount = 2; // how many times you can jump in the air
    private int jumpCount = 0; // actual jump count
    public AudioClip jumpSFX;
    public AudioClip footstepSFX;
    private AudioSource audioSource;
    private AnimationController _animControl;
    private int x = 0;
    private int z = 0; 


    // Start is called before the first frame update
    void Start()
    {
        playerStartPosition = this.transform.position;
        // RB = GetComponent<Rigidbody>();
        player = GetComponent<CharacterController>();
        Cursor.visible = false; // cursor not visible 
        Cursor.lockState = CursorLockMode.Locked; // keeps cursors in the middle of the screen
        audioSource = GetComponent<AudioSource>();
        jumpCount = maxAirJumpCount;
        _animControl = GetComponent<AnimationController>();
       
    }

    // Update is called once per frame
    void Update()
    {
        _CameraControls();

        _PlayerInputMapping();
        

        _animControl.AnimationUpdate(x, z); 
       

        #region input physics

        // Jump detection
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if (jumpCoroutine != null) // if jumpCoroutine buffer time is already playing 
            {
                StopCoroutine(jumpCoroutine); // reset jump buffer to prevent the player from jumping again within 0.5 seconds
            }
            jumpCoroutine = StartCoroutine(_JumpBuffer()); // If a player is approaching the ground and pressed space, the input is saved, and so the character jumps right away when they hit the ground. 

        }

        //forwards and backwards physics
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


        #region up and down movement (jumping & falling) 

        groundedLastFrame = isGrounded;  // groundedLastFrame checks isGrounded for the previous frame before isGrounded updates for the current frame. 

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask); // checks if the player is on the ground by seeing if the position and radius of the blue sphere...
                                                                                      // ...is on an object in the ground layer (groundMask)
        if (isGrounded) // if grounded
        {
            jumpCount = maxAirJumpCount; // reset jump count 
        }
        _animControl.UpdateAnimationGrounded(isGrounded);
        bool canJump = isGrounded || onCoyoteTime || jumpCount > 0; 
        if (isGrounded && yVelocity < 0) // if you are on the ground and the velocity is increasing 
        {
            yVelocity = 0; // reset velocity so it doesn't increase when you are not in the air 
        }

        if (groundedLastFrame && !isGrounded && !onCoyoteTime) // when you walk off of a ledge 
        {
            coyoteCoroutine = StartCoroutine(_CoyoteTime());
        }


        if (canJump && jumpInput) // If you can jump, and you are pressing space
        {

            if (jumpCoroutine != null) // if buffer is active 
            {
                StopCoroutine(jumpCoroutine); // clean up the buffer so the jump finishes completely without auto jumping if a player lands too early. 
                jumpInput = false;
            }

            if (coyoteCoroutine != null) // if coyote timer is already active
            {
                StopCoroutine(coyoteCoroutine); // Stop coroutine so coyotetime is no longer active (prevents player from jumping into infinity) 
                onCoyoteTime = false;
                yVelocity = 0; // stops us from falling so we can do the full jump
            }
            audioSource.clip = jumpSFX;
            audioSource.Play();
            if (!isGrounded) // if the player is already in the air
            {
                jumpCount--;
                yVelocity = 0; //ignore gravity to do a full jump
            }
            yVelocity += Mathf.Sqrt(height * -3 * Physics.gravity.y * gravityMult); // take current gravity force and multiply it by -3 to counter gravity so you ACTUALLY JUMP and fall
            isGrounded = false; // prevents _CoyoteTime from activating during a jump 
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

    private void OnDrawGizmos() // allows us to see the ground check in unity as a blue sphere. 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }



    private IEnumerator _CoyoteTime() // starts coyote time NOTE: IEnumerator is saved in a coroutine so you can stop the coroutine
    {
        onCoyoteTime = true; 
        yield return new WaitForSeconds(maxCoyoteTime); // wait until x amount of time has passed 
        onCoyoteTime = false; 
    }



    private IEnumerator _JumpBuffer() // starts time for jump input. 
    {
        jumpInput = true;
        yield return new WaitForSeconds(jumpBufferTime);
        jumpInput = false; 
    }



    public void PlayFootstep()
    {
        audioSource.clip = footstepSFX;
        audioSource.Play(); 
    }


    // The keybinds for player Controls.
    private void _PlayerInputMapping()
    {
        
        x = 0; // 1 is right, left is -1
        z = 0; // 1 forward, -1 backward
        if (Input.GetKey(KeyCode.W))
        {

            z += 1;


        }

        if (Input.GetKey(KeyCode.A))
        {

            x -= 1;
        }

        if (Input.GetKey(KeyCode.S))
        {

            z -= 1;
        }

        if (Input.GetKey(KeyCode.D))    // True if key is held 
        {

            x += 1;
        }

    }

    // Camera follows the player when they move. 
    private void _CameraControls()
    {
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
    }
}
