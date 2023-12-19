using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickTimeControll : MonoBehaviour
{
    public static int pressedTimes;
    public GameObject mouse;
    public GameObject control;
    private void Update()
    {
        if (Input.GetButtonDown("Action"))
        {
            pressedTimes++;
        }

        if(GameManager.gameManager.useControll)
        {
             control.SetActive(true);
        }
        else
        {
            mouse.SetActive(true);
        }
    }
}
