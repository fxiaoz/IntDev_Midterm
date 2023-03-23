using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float horizontalMove;
    public float speed = 5f;
    public float climbSpeed = 0.5f;

    public Rigidbody2D myBody;
    public Animator myAnim;
    public SpriteRenderer myRenderer;
    public GameManager gameManager;

    bool grounded = false;
    public bool onWall = false;
    bool onRightWall = false;
    bool onLeftWall = false;

    public int wallSide;

    bool jump = false;
    bool climb = false;

    public float castDist = 1f;

    public float jumpLimit = 11f;
    public float gravityScale = 2f; //rising
    public float gravityFall = 5f; //falling

    public bool intimidate = false;

    public AudioSource mySource;
    public AudioClip scareSound;
    public AudioClip pickupSound;


    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            myRenderer.flipX = false;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            myRenderer.flipX = true;
        }

        horizontalMove = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && grounded /*|| Input.GetButtonDown("Jump") && onWall*/)
        {
            jump = true;
        }

        if (Input.GetKey(KeyCode.Space) && onWall == true)
        {
            climb = true;
        }

        else
        {
            climb = false;
        }

        if (horizontalMove > 0.2f || horizontalMove < -0.2f)
        {
            myAnim.SetBool("walking", true);
        }

        else
        {
            myAnim.SetBool("walking", false);
        }

        if (onWall)
        {
            myAnim.SetBool("grabbing", true);
        }

        else
        {
            myAnim.SetBool("grabbing", false);
        }

        wallSide = onRightWall ? 1 : -1;
        wallSide = onLeftWall ? 1 : 1;

        if (myBody.transform.position.x > 0)
        {
            wallSide = 1;
        }

        if (myBody.transform.position.x < 0)
        {
            wallSide = -1;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            intimidate = true;
            myAnim.SetBool("intimidate", true);
        }

        else
        {
            intimidate = false;
            myAnim.SetBool("intimidate", false);
        }
    }

    void FixedUpdate()
    {
        float moveSpeed = horizontalMove * speed;

        if (jump)
        {
            myBody.AddForce(Vector2.up * jumpLimit, ForceMode2D.Impulse);
            jump = false;
        }

        if (climb)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + climbSpeed, transform.position.z);
        }

        if (myBody.velocity.y > 0)
        {
            myBody.gravityScale = gravityScale;
        }

        else if (myBody.velocity.y < 0)
        {
            myBody.gravityScale = gravityFall;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castDist);
        Debug.DrawRay(transform.position, Vector2.down, Color.red);

        myBody.velocity = new Vector3(moveSpeed, myBody.velocity.y, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }

        if (collision.gameObject.tag == "Wall")
        {
            onWall = true;
        }

        if(collision.gameObject.tag == "Butterfly" && intimidate == true)
        {
            mySource.PlayOneShot(scareSound);
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
        }

        if (collision.gameObject.tag == "Wall")
        {
            onWall = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Leaf1")
        {
            gameManager.score1 += 1;
            mySource.PlayOneShot(pickupSound);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Leaf2")
        {
            gameManager.score2 += 1;
            mySource.PlayOneShot(pickupSound);
            Destroy(collision.gameObject);
        }
    }
}
