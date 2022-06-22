using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum PowerupIndex
{
    ORANGEMUSHROOM = 0,
    REDMUSHROOM = 1
}
public class PowerupManagerEV : MonoBehaviour
{
  // reference of all player stats affected
  public IntVariable marioJumpSpeed;
  public IntVariable marioMaxSpeed;
  public PowerupInventory powerupInventory;
  public List<GameObject> powerupIcons;

  void Start()
  {
      if (!powerupInventory.gameStarted)
      {
          powerupInventory.gameStarted = true;
          powerupInventory.Setup(powerupIcons.Count);
          resetPowerup();
      }
      else
      {
            resetPowerup();
          // re-render the contents of the powerup from the previous time
          for (int i = 0; i < powerupInventory.Items.Count; i++)
          {
            Debug.Log("Current Content in the powerup");
            Debug.Log(i);
              Powerup p = powerupInventory.Get(i);
              if (p != null)
              {
                  AddPowerupUI(i, p.powerupTexture);
              }
          }
      }
  }


    
  public void resetPowerup()
  {
    for (int i = 0; i < powerupIcons.Count; i++)
    {
        powerupIcons[i].SetActive(false);
    }
  }
    
  void AddPowerupUI(int index, Texture t)
  {
      powerupIcons[index].GetComponent<RawImage>().texture = t;
      powerupIcons[index].SetActive(true);
  }

  public void AddPowerup(Powerup p)
  {
      powerupInventory.Add(p, (int)p.index);
      AddPowerupUI((int)p.index, p.powerupTexture);
  }

  public void PowerUpEffect(Powerup p, int index){
    Debug.Log("Adding powerup effect");
    Debug.Log(p.absoluteJumpBooster);
    Debug.Log(p.aboluteSpeedBooster);
    marioJumpSpeed.ApplyChange(p.absoluteJumpBooster);
    marioMaxSpeed.ApplyChange(p.aboluteSpeedBooster);
    RemovePowerupUI(index);
    powerupInventory.Remove((int)p.index);
    StartCoroutine(removePowerUpEffect(p));
  }

  IEnumerator removePowerUpEffect(Powerup p){
    yield return new WaitForSeconds(5.0f);
    marioJumpSpeed.ApplyChange(-p.absoluteJumpBooster);
    marioMaxSpeed.ApplyChange(-p.aboluteSpeedBooster);
  }

  void RemovePowerupUI(int index){
    powerupIcons[index].SetActive(false);
    Debug.Log("Removed Icon from the powerup");
  }

  public void AttemptConsumePowerup(KeyCode k){
    if (k == KeyCode.Z){
        Powerup p = powerupInventory.Get(0);
        if (p != null){
            PowerUpEffect(p, 0);
        } else{
            Debug.Log("Powerup not available");
        }
    }
    else if (k == KeyCode.X){
        Powerup p = powerupInventory.Get(1);
        if (p != null){
            PowerUpEffect(p, 1);
        }else{
            Debug.Log("Powerup not available");
        }
    }else{
        Debug.Log("Wrong attempt to consume Powerup");
    }
  }
  public void ResetValues(){
    resetPowerup();
    powerupInventory.gameStarted = false;
    powerupInventory.Clear();
  }

  public void OnApplicationQuit()
  {
      ResetValues();
  }
}
