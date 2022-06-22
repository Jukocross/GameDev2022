using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName =  "GameConstants", menuName =  "ScriptableObjects/GameConstants", order =  1)]
public  class GameConstants : ScriptableObject
{
    // Mario basic starting values
    public int playerStartingMaxSpeed = 5;
    public int playerMaxJumpSpeed = 30;
    public int playerDefaultForce = 150;
    // for Scoring system
    int currentScore;
    int currentPlayerHealth;
    public  float groundDistance =  -4.5f;

    // for Reset values
    Vector3 gombaSpawnPointStart = new Vector3(2.5f, -0.45f, 0); // hardcoded location
    Vector3 greenSpawnPointStart = new Vector3(2.5f, -0.45f, 0);
    // .. other reset values 

    // for EnemyController.cs
    public float maxOffset = 5.0f;
    public float enemyPatroltime = 2.0f;
    public float groundSurface = -3.4f;

    // for Consume.cs
    public  int consumeTimeStep =  10;
    public  int consumeLargestScale =  4;
    
    // for Break.cs
    public  int breakTimeStep =  30;
    public  int breakDebrisTorque =  10;
    public  int breakDebrisForce =  10;
    
    // for SpawnDebris.cs
    public  int spawnNumberOfDebris =  10;
    
    // for Rotator.cs
    public  int rotatorRotateSpeed =  6;
    
    // for testing
    public  int testValue;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
