using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolController : MonoBehaviour
{
    private Rigidbody2D mushBody;
    private Transform mushLocation;
    public float speed;
    private GameObject mario;
    [HideInInspector]
    public bool isFacingLeft;
    public bool spawnFacingLeft;
    // Start is called before the first frame update
    private Vector2 facingLeft;
    private bool marioFacingRight=true;
    private bool isMoving=false;
    // Start is called before the first frame update
    void Start()
    {
        mushBody = GetComponent<Rigidbody2D>();
        mushBody.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
        mario = GameObject.FindWithTag("Player");
        if (mario != null){
            Debug.Log("Have mario object");
            PlayerControllerEV marioScript = mario.GetComponent<PlayerControllerEV>();
            marioFacingRight = marioScript.GetDirection();
        } else{
            Debug.Log("Mario not found");
        }
        if (!marioFacingRight){
            transform.localScale = facingLeft;
            isFacingLeft = true;
        };
        isMoving = true;
        
    }
    protected virtual void Flip()
    {
        if (isFacingLeft)
        {
            transform.localScale = facingLeft;
        }
        if (!isFacingLeft)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.gameObject.CompareTag("Obstacles"))
        {
            Debug.Log("Collided with Obstacles!");
            Flip();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isMoving){
             Vector2 nextPosition = mushBody.position + speed * (Vector2)transform.localScale * Time.fixedDeltaTime;
            mushBody.MovePosition(nextPosition);
        };
    }
}
