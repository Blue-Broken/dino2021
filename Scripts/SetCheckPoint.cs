using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCheckPoint : MonoBehaviour
{
    private void OnEnable() 
    {
        GameManager.gameManager.currentMunition = PlayerController.currentMunition;   
        GameManager.gameManager.currentHealthPacks = PlayerController.currentHealthPacks;   
        GameManager.gameManager.currentCartrigdes = PlayerController.currentCartridges;   
        
        GameManager.gameManager.currentCheckPoint++;   
    }
}
