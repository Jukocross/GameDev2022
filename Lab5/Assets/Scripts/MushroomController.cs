using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
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
    private bool isMoving=true;
    private bool collected=false;
    void Start()
    {
        mushBody = GetComponent<Rigidbody2D>();
        mushBody.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
        mario = GameObject.FindWithTag("Player");
        if (mario != null){
            Debug.Log("Have mario object");
            PlayerController marioScript = mario.GetComponent<PlayerController>();
            marioFacingRight = marioScript.GetDirection();
        } else{
            Debug.Log("Mario not found");
        }
        if (!marioFacingRight){
            transform.localScale = facingLeft;
            isFacingLeft = true;
        };
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

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player")){
            isMoving = false;
            collected = true;
            getAbsorb();
        };
    }
    void getAbsorb(){
        StartCoroutine(absorb());
        Debug.Log("Absorb sequence ends");
    }
    IEnumerator  absorb(){
		Debug.Log("Absorb starts");
		int steps =  5;
		float stepper =  1.0f/(float) steps;
    
        this.transform.localScale = new Vector3(this.transform.localScale.x + 0.5f, this.transform.localScale.y  +  0.5f, this.transform.localScale.z);
        yield  return  null;
        this.transform.localScale = new Vector3(this.transform.localScale.x - 0.5f, this.transform.localScale.y  -  0.5f, this.transform.localScale.z);
        yield  return  null;

		for (int i =  0; i  <  steps; i  ++){
            if (this.transform.localScale.x > 0.2f && this.transform.localScale.y > 0.2){
                this.transform.localScale  =  new  Vector3(this.transform.localScale.x-stepper, this.transform.localScale.y  -  stepper, this.transform.localScale.z);
            };
			// make sure enemy is still above ground
			//this.transform.position  =  new  Vector3(this.transform.position.x, gameConstants.groundSurface  +  GetComponent<SpriteRenderer>().bounds.extents.y, this.transform.position.z);
			yield  return  null;
		}
        this.transform.localScale = new Vector3(0, 0 , this.transform.localScale.z);
		Debug.Log("Absorb ends");
		yield  break;
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

    void  OnBecameInvisible(){
        Debug.Log("Become Invisible");
        Destroy(gameObject);	
    }
}
