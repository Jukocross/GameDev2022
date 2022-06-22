using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerControllerEV : MonoBehaviour
{
    private float force;
    public IntVariable marioUpSpeed;
    public IntVariable marioMaxSpeed;
    public GameConstants gameConstants;
    private bool isDead, isADKeyUp = true, isSpacebarUp = true, faceRightState;
    private bool onGroundState = true, countScoreState;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private Animator marioAnimator;
    private AudioSource marioAudio;
    public UnityEvent<KeyCode> onPlayerCastPowerup;
    // Start is called before the first frame update
    void Start()
    {
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator = GetComponent<Animator>();
        marioAudio = GetComponent<AudioSource>();
        marioUpSpeed.SetValue(gameConstants.playerMaxJumpSpeed);
        marioMaxSpeed.SetValue(gameConstants.playerStartingMaxSpeed);
        force = gameConstants.playerDefaultForce;
    }
     void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            //dustCloud.Play();
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

    void FixedUpdate()
    {
        if (!isDead)
        {
            //check if a or d is pressed currently
            if (!isADKeyUp)
            {
                float direction = faceRightState ? 1.0f : -1.0f;
                Vector2 movement = new Vector2(force * direction, 0);
                if (marioBody.velocity.magnitude < marioMaxSpeed.Value)
                    marioBody.AddForce(movement);
            }

            if (!isSpacebarUp && onGroundState)
            {
                marioBody.AddForce(Vector2.up * marioUpSpeed.Value, ForceMode2D.Impulse);
                onGroundState = false;
                // part 2
                marioAnimator.SetBool("onGround", onGroundState);
                countScoreState = true; //check if goomba is underneath
            }
        }
        if (Input.GetKeyUp("a") ||  Input.GetKeyUp("d")){
            isADKeyUp = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
        marioAnimator.SetBool("onGround", onGroundState);
        if (Input.GetKeyDown("a")){
            isADKeyUp = false;
            marioSprite.flipX = true;
            if (faceRightState){
                faceRightState = false;
            }
            if (Mathf.Abs(marioBody.velocity.x) > 1.0){
                marioAnimator.SetTrigger("onSkid");
            }
        }
        if (Input.GetKeyDown("d")){
            isADKeyUp = false;
            marioSprite.flipX = false;
            if (!faceRightState){
                faceRightState = true;
            }
            if (Mathf.Abs(marioBody.velocity.x) > 1.0){
            marioAnimator.SetTrigger("onSkid");
          }
        }
        if (Input.GetKeyDown("space")){
            isSpacebarUp = false;
        } else if (Input.GetKeyUp("space")){
            isSpacebarUp = true;
        } if(Input.GetKeyDown("z")){
            onPlayerCastPowerup.Invoke(KeyCode.Z);
        } if(Input.GetKeyDown("x")){
            onPlayerCastPowerup.Invoke(KeyCode.X);
        }
    }
    public void PlayerDiesSequence()
    {
        isDead = true;
        marioAnimator.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        marioBody.AddForce(Vector3.up * 30, ForceMode2D.Impulse);
        marioBody.gravityScale = 30;
        StartCoroutine(dead());
    }
    
    IEnumerator dead()
    {
        yield return new WaitForSeconds(1.0f);
        marioBody.bodyType = RigidbodyType2D.Static;
    }
    void  PlayJumpSound(){
        marioAudio.PlayOneShot(marioAudio.clip);
    }
    public bool GetDirection(){
        return faceRightState;
    }
}
