using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpThruPlatform : MonoBehaviour
{
    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            player.onJumpThruPlatform = true;
            player.activeJumpThruPlatform = gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        player.onJumpThruPlatform = false;
    }

    public IEnumerator DropThruPlatform()
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(0.3f);
        GetComponent<Collider2D>().enabled = true;
        //player.activeJumpThruPlatform = null;
    }

}
