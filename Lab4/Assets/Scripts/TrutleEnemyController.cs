using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrutleEnemyController : MonoBehaviour
{
    public  GameConstants gameConstants;
    private int moveRight = -1;
    private float originalX;
    private Vector2 velocity;
    private Rigidbody2D enemyBody;
    private Animator trutleAnimator;
    private bool isMoving=true;
    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        trutleAnimator = GetComponent<Animator>();
        // get the starting position
        originalX = transform.position.x;
        ComputeVelocity();
        // subscribe to player event
        GameManager.OnPlayerDeath  +=  EnemyRejoice;
    }
    void ComputeVelocity(){
        velocity = new Vector2((moveRight)*gameConstants.maxOffset / gameConstants.enemyPatroltime, 0);
    }
    void MoveGomba(){
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }
    void  OnTriggerEnter2D(Collider2D other){
		// check if it collides with Mario
		if (other.gameObject.tag  ==  "Player"){
			// check if collides on top
			float yoffset = (other.transform.position.y  -  this.transform.position.y);
			if (yoffset  >  0.75f){
				KillSelf();
			}
			else{
				// hurt player, implement later
                CentralManager.centralManagerInstance.damagePlayer();
			}
		}
	}
    void  KillSelf(){
		// enemy dies
		CentralManager.centralManagerInstance.increaseScore(ObjectType.gombaEnemy);
		StartCoroutine(flatten());
		Debug.Log("Kill sequence ends");
	}
    IEnumerator  flatten(){
		Debug.Log("Flatten starts");
		int steps =  5;
		float stepper =  1.0f/(float) steps;

		for (int i =  0; i  <  steps; i  ++){
			this.transform.localScale  =  new  Vector3(this.transform.localScale.x, this.transform.localScale.y  -  stepper, this.transform.localScale.z);

			// make sure enemy is still above ground
			this.transform.position  =  new  Vector3(this.transform.position.x, gameConstants.groundSurface  +  GetComponent<SpriteRenderer>().bounds.extents.y, this.transform.position.z);
			yield  return  null;
		}
		Debug.Log("Flatten ends");
		this.gameObject.SetActive(false);
		Debug.Log("Enemy returned to pool");
		yield  break;
	}

    // animation when player is dead
    void  EnemyRejoice(){
        Debug.Log("Enemy killed Mario");
        // do whatever you want here, animate etc
        // ...
        trutleAnimator.SetTrigger("marioDied");
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving){
            if (Mathf.Abs(enemyBody.position.x - originalX) < gameConstants.maxOffset)
            {// move gomba
                MoveGomba();
            }
            else{
                // change direction
                moveRight *= -1;
                ComputeVelocity();
                MoveGomba();
            }
        }
    }
}
