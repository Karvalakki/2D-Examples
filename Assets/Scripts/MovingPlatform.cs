using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Rigidbody2D myRB;
    public float speedX = 0.5f;
    public float speedY = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myRB.velocity = new Vector2(speedX, speedY);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Bumper"))
        {
            speedX = -speedX;
            speedY = -speedY;
        }

       
    }
}
