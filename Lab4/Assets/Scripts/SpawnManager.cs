using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    void Awake(){
        for (int j =  0; j  <  2; j++)
	        spawnFromPooler(ObjectType.gombaEnemy);
    }
    // Start is called before the first frame update
    void Start()
    {
        GameManager.SpawnNewMob += spawnFromPooler;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void  spawnFromPooler(ObjectType i){
        // static method access
        GameObject item =  ObjectPooler.SharedInstance.GetPooledObject(i);
        if (item  !=  null){
            Debug.Log("Debugging item transform position Y");
            Debug.Log(item.transform.position.y);
            //set position, and other necessary states
            item.transform.position  =  new  Vector3(Random.Range(0f, 4.5f), item.transform.position.y, 0);
            item.SetActive(true);
        }
        else{
            Debug.Log("not enough items in the pool.");
        }
    }
}
