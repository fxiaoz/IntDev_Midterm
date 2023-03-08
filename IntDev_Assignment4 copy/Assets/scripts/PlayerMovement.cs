using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float horizontalMove;
    public float speed = 5f;

    Rigidbody2D myBody;
    Animator myAnim;
    GameManager gameManager;

    bool grounded = false;
    public bool onWall = false;
    bool onRightWall = false;
    bool onLeftWall = false;

    public int wallSide;

    bool jump = false;

    public float castDist = 1f;

    public float jumpLimit = 5f;
    public float gravityScale = 1f; //rising
    public float gravityFall = 5f; //falling

    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        gameManager = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && grounded || Input.GetButtonDown("Jump") && onWall)
        {
            jump = true;
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
    }

    void FixedUpdate()
    {
        float moveSpeed = horizontalMove * speed;

        if (jump)
        {
            myBody.AddForce(Vector2.up * jumpLimit, ForceMode2D.Impulse);
            jump = false;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            gameManager.goal1Reached = true;
            Destroy(collision.gameObject);
        }
    }
}
