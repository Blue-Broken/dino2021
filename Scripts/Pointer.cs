using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    private GameManager gameM;
    public static Vector3 mousePos;
    public static Vector3 virtualMousePos;

    [Header("Variables of Control")]
    public float speedX;
    public float speedY;
    private float axisX;
    private float axisY;
    void Start()
    {
        gameM = GameManager.gameManager;
        Cursor.visible = false;
        if (gameM.useControll)
        {
            axisX = Screen.width / 2;
            axisY = (Screen.height / 2) - (Screen.height / 15);
        }
    }

    private void Update()
    {
        if (gameM.paused)
        {
            if (!gameM.useControll)
            {
                virtualMousePos = new Vector3((Input.mousePosition.x < Screen.width) ? (Input.mousePosition.x > 0) ? Input.mousePosition.x : 0 : Screen.width, (Input.mousePosition.y < Screen.height) ? (Input.mousePosition.y > 0) ? Input.mousePosition.y : 0 : Screen.height, 0.4f);
                mousePos = virtualMousePos;
                transform.position = Camera.main.ScreenToWorldPoint(mousePos);
            }
            else
            {
                axisX += Input.GetAxisRaw("Horizontal2") * speedX * Time.deltaTime;
                axisY += Input.GetAxisRaw("Vertical2") * speedY * Time.deltaTime;

                axisX = axisX < Screen.width ? axisX < 0 ? 0 : axisX : Screen.width;
                axisY = axisY < Screen.height ? axisY < 0 ? 0 : axisY : Screen.height;

                virtualMousePos = new Vector3(axisX, axisY, 0.4f);
                
                mousePos = virtualMousePos;

                transform.position = Camera.main.ScreenToWorldPoint(mousePos);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!gameM.useControll)
        {
            virtualMousePos = new Vector3((Input.mousePosition.x < Screen.width) ? (Input.mousePosition.x > 0) ? Input.mousePosition.x : 0 : Screen.width, (Input.mousePosition.y < Screen.height) ? (Input.mousePosition.y > 0) ? Input.mousePosition.y : 0 : Screen.height, 0.4f);

            if (Input.GetButton("Aim") && gameM.armed)
            {
                mousePos = new Vector3(Screen.width / 2, (Screen.height / 2) - (Screen.height / 15), 0.4f);
            }
            else
            {
                mousePos = virtualMousePos;
            }

            transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }
        else
        {
            axisX += Input.GetAxisRaw("Horizontal2") * speedX * Time.fixedDeltaTime;
            axisY += Input.GetAxisRaw("Vertical2") * speedY * Time.fixedDeltaTime;

            axisX = axisX < Screen.width ? axisX < 0 ? 0 : axisX : Screen.width;
            axisY = axisY < Screen.height ? axisY < 0 ? 0 : axisY : Screen.height;

            virtualMousePos = new Vector3(axisX, axisY, 0.4f);

            if (Input.GetButton("Aim") && gameM.armed)
            {
                axisX = Screen.width / 2;
                axisY = (Screen.height / 2) - (Screen.height / 15);
                mousePos = new Vector3(Screen.width / 2, (Screen.height / 2) - (Screen.height / 15), 0.4f);
            }
            else
            {
                mousePos = virtualMousePos;
            }

            transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }
    }
}
