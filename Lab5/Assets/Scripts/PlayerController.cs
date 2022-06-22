using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    private Rigidbody2D marioBody;
    public float maxSpeed = 10;
    private float moveHorizontal;
    private Vector2 movement;
    private bool onGroundState = true;
    public float upSpeed;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    public Transform enemyLocation;
    private bool countScoreState = false;
    public Text restartText;
    private bool restartState = false;
    private Animator marioAnimator;
    private AudioSource marioAudio;
    // Start is called before the first frame update
    public ParticleSystem dustCloud;
    void  Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator = GetComponent<Animator>();
        marioAudio = GetComponent<AudioSource>();
        GameManager.OnPlayerDeath  +=  PlayerDiesSequence;
    }
    
    // FixedUpdate may be called once per frame. See documentation for details.
    void FixedUpdate()
    {
        // dynamic rigidbody
        moveHorizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(moveHorizontal) > 0){
            movement = new Vector2(moveHorizontal, 0);
            if (marioBody.velocity.magnitude < maxSpeed)
                    marioBody.AddForce(movement * speed);
        }
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d")){
            // stop
            marioBody.velocity = Vector2.zero;
        }
        if (Input.GetKeyDown("space") && onGroundState){
          marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
          onGroundState = false;
           countScoreState = true; //check if Gomba is underneath
        }
    }

//     void OnTriggerEnter2D(Collider2D other)
//   {
//       if (other.gameObject.CompareTag("Enemy"))
//       {
//           Debug.Log("Collided with Gomba!");
//           restartText.text = "Enter \"R\" to restart";
//           restartState = true;
//           Time.timeScale = 0;       
//       }
//   }

    // called when the cube hits the floor
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            dustCloud.Play();
            onGroundState = true; // back on ground
            // countScoreState = false; // reset score state
            // scoreText.text = "Score: " + score.ToString();
        };
        if (col.gameObject.CompareTag("Obstacles")){
            onGroundState = true;
            // countScoreState = false;
            // scoreText.text = "Score: " + score.ToString();
        };
    }

    // Update is called once per frame
    void Update()   
    {
         //Update xSpeed
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
        //Update onGround
        marioAnimator.SetBool("onGround", onGroundState);
        if (Input.GetKeyDown("a") && faceRightState){
          faceRightState = false;
          marioSprite.flipX = true;
          if (Mathf.Abs(marioBody.velocity.x) > 1.0){
            marioAnimator.SetTrigger("onSkid");
          }
        }

        if (Input.GetKeyDown("d") && !faceRightState){
            faceRightState = true;
            marioSprite.flipX = false;
            if (Mathf.Abs(marioBody.velocity.x) > 1.0){
            marioAnimator.SetTrigger("onSkid");
          }
        }

        if (Input.GetKeyDown(KeyCode.R) && restartState)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        // when jumping, and Gomba is near Mario and we haven't registered our score
        if (!onGroundState && countScoreState)
        {
            if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
            {   
                // countScoreState = false;
                // score++;
                // Debug.Log(score);
            }
        }
        if (Input.GetKeyDown("z")){
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.Z,this.gameObject);
        }

        if (Input.GetKeyDown("x")){
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.X,this.gameObject);
        }     
    }

    void  PlayJumpSound(){
        marioAudio.PlayOneShot(marioAudio.clip);
    }

    public bool GetDirection(){
        return faceRightState;
    }
    void  PlayerDiesSequence(){
        // Mario dies
        Debug.Log("Mario dies");
        // do whatever you want here, animate etc
        // ...
        this.transform.localScale = new Vector3(0,0,this.transform.localScale.z);
        restartText.text = "Enter \"R\" to restart";
        restartState = true;
    }
}
