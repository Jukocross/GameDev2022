using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class GameManager : MonoBehaviour
{
	public  Text score;
	private  int playerScore =  0;
	public  delegate  void gameEvent();
    public  static  event  gameEvent OnPlayerDeath;
    public  delegate  void gameEvent1(ObjectType i);
    public  static  event  gameEvent1 SpawnNewMob;
	public  void  increaseScore(ObjectType e){
		playerScore  +=  1;
		score.text  =  "SCORE: "  +  playerScore.ToString();
        SpawnNewMob(e);
	}
    public  void  damagePlayer(){
        OnPlayerDeath();
    }
}