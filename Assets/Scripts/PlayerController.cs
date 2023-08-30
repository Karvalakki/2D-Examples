using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float jumpPower = 5f;
    public float horizontal;
    public float vertical;
    public Rigidbody2D myRB;
    public Animator myAnim;
    public bool facingRight = true;
    public bool onMovingPlatform = false;
    public bool onJumpThruPlatform = false;
    public bool onLadder = false;
    public bool climbing = false;

    public GameObject activeJumpThruPlatform;
    public GameObject activeLadder;

    // Variables for ground check
    public Transform groundCheck;
    public float groundCheckradius = 0.2f;
    public LayerMask groundLayer;

    //References to other scripts
    [SerializeField] CameraTargetScript cameraTargetScript;
    [SerializeField] CameraController cameraController;

    //Screenshake values
    public float jumpFrequensy = 3f;
    public float jumpTimer = 0.2f;
    public float jumpAmplitude = 0.2f;

    public float hitFrequensy = 0.3f;
    public float hitTimer = 0.5f;
    public float hitAmplitude = 3f;

    //Player death values
    public Transform playerStart;

    public Collider2D myCol;
    public PhysicsMaterial2D slide;
    public PhysicsMaterial2D stop;

    // Coyote time
    public float coyoteTime = 0.3f;
    public float coyoteTimeTimer;
    public Transform jumpMark;

    // Jump buffer
    public float jumpBuffer = 0.2f;
    public float jumpBufferTimer;

    


    
    

    public void Start()
    {
        cameraTargetScript = GameObject.Find("CameraTarget").GetComponent<CameraTargetScript>();
        cameraController = GameObject.Find("2DCamera").GetComponent<CameraController>();
        transform.position = playerStart.position;
        myCol = GetComponent<Collider2D>();
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckradius, groundLayer);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        myRB.gravityScale = 2.51f;

        if(context.performed)
        {
            jumpBufferTimer = jumpTimer;
            //cameraController.ScreenShake(jumpFrequensy, jumpTimer, jumpAmplitude);
        }

        if(context.performed && climbing)
        {
            myRB.AddForce(Vector2.up * jumpPower/2);
            activeLadder.GetComponent<LadderScript>().StartCoroutine("DetachFromLatters");
        }

        

        if(context.canceled && myRB.velocity.y > 0f)
        {
            jumpBufferTimer -= Time.deltaTime;
            myRB.velocity = new Vector2(myRB.velocity.x, myRB.velocity.y * 0.4f);
            coyoteTimeTimer = 0f;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        vertical = context.ReadValue<Vector2>().y;
        
    }

    public void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        facingRight = !facingRight;
        //cameraController.FlipScreenX(facingRight);
    }

    private void FixedUpdate()
    {
        

        if(climbing)
        {
            myRB.velocity = new Vector2(0f, vertical*speed/2);
            myRB.gravityScale = 0f;
        }

        else if(!climbing)
        {
            myRB.velocity = new Vector2(horizontal * speed, myRB.velocity.y);
            myRB.gravityScale = 2.51f;
        }

        

        myAnim.SetFloat("yVelocity", myRB.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {

        if(jumpBufferTimer > 0f && coyoteTimeTimer > 0f)
        {
            myRB.AddForce(Vector2.up * jumpPower);
            jumpBufferTimer = 0f;
        }

        if (onLadder)
        {
            if (vertical > 0.1f)
            {
                climbing = true;
            }
        }

        if (horizontal != 0f && onMovingPlatform)
        {
            myCol.sharedMaterial = slide;
        }

        if(horizontal == 0f && onMovingPlatform)
        {
            myCol.sharedMaterial = stop;
            
        }

        if(horizontal<-0.1f && facingRight)
        {
            Flip();
        }

        if (horizontal > 0.1f && !facingRight)
        {
            Flip();
        }

        if(horizontal != 0f)
        {
            myAnim.SetBool("isWalking", true);
        }

        else if (horizontal == 0f)
        {
            myAnim.SetBool("isWalking", false);
        }

        if(IsGrounded())
        {
            coyoteTimeTimer = coyoteTime;
            myAnim.SetBool("isGrounded", true);

            //Platform snapping
            cameraTargetScript.posY = transform.position.y;
        }

        else
        {
            coyoteTimeTimer -= Time.deltaTime;
            myAnim.SetBool("isGrounded", false);
        }

        if(vertical < -0.1f && onJumpThruPlatform)
        {
            // Option A
            //StartCoroutine("DropThruPlatform");
            //
           //Option B
           activeJumpThruPlatform.GetComponent<JumpThruPlatform>().StartCoroutine("DropThruPlatform");
        }

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            cameraController.ScreenShake(hitFrequensy, hitTimer, hitAmplitude);
            transform.position = playerStart.position;
        }

        if(collision.gameObject.CompareTag("MovingPlatform"))
        {
            //transform.parent = collision.gameObject.transform;
            myCol.sharedMaterial = stop;
            onMovingPlatform = true;
        }

        if(collision.gameObject.CompareTag("JumpThruPlatform"))
        {
            onJumpThruPlatform = true;
            activeJumpThruPlatform = collision.gameObject;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("MovingPlatform"))
        {
           // transform.parent = null;
            myCol.sharedMaterial = slide;
            onMovingPlatform = false;
        }

        if(collision.gameObject.CompareTag("JumpThruPlatform"))
        {
            onJumpThruPlatform = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("CheckPoint"))
        {
            playerStart.position = collision.transform.position;
        }

        if(collision.gameObject.CompareTag("Ladder"))
        {
            onLadder = true;
            activeLadder = collision.gameObject;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Ladder"))
        {
            onLadder = false;
            climbing = false;
        }
    }

    public IEnumerator DropThruPlatform()
    {
        activeJumpThruPlatform.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds (0.3f);
        activeJumpThruPlatform.GetComponent<Collider2D>().enabled = true;
        activeJumpThruPlatform = null;
    }

    public IEnumerator DetachFromLatters()
    {
        activeLadder.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        activeLadder.GetComponent<Collider2D>().enabled = true;
    }

}
