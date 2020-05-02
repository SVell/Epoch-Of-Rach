using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerJump : MonoBehaviour
{
    private Rigidbody2D rb;
    public float jumpForce = 9;

    public Animator anim;

    [Range(0,1)]
    public float jumpCutHeight = 1;

    public bool isGrounded;
    public Transform point;
    public float pointRad = 0.3f;
    public LayerMask whatIsGround;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
    
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(point.position,pointRad,whatIsGround);
        
        if (rb.velocity.y < 0 && !isGrounded)
        {
            print("Fly");
            anim.SetInteger("States",2);
        }

        
        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetInteger("States",2);
            
        }
        

        if (Input.GetButtonUp("Jump"))
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y * jumpCutHeight);
                anim.SetInteger("States",2);
            }
            else if (rb.velocity.y < 0)
            {
               anim.SetInteger("States",2);
            }
            
        }
        
    }
}
