using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBrick : MonoBehaviour
{
    private bool broken = false;
    public GameObject prefab;
    private AudioSource bricksAudio;
    // Start is called before the first frame update
    void Start()
    {
        bricksAudio = GetComponent<AudioSource>();
    }

    void  OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.CompareTag("Player") &&  !broken){
            broken  =  true;
            PlayBreakSound();
            // assume we have 5 debris per box
            for (int x =  0; x<5; x++){
                Instantiate(prefab, transform.position, Quaternion.identity);
            }
            gameObject.transform.parent.GetComponent<SpriteRenderer>().enabled  =  false;
            gameObject.transform.parent.GetComponent<BoxCollider2D>().enabled  =  false;
            GetComponent<EdgeCollider2D>().enabled  =  false;
            Interactive();
        }
    }
    void Interactive(){
        CentralManager.centralManagerInstance.increaseScore(ObjectType.gombaEnemy);

    }
    void PlayBreakSound(){
        bricksAudio.PlayOneShot(bricksAudio.clip);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
