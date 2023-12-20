using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    private GameManager gameM;
    public Text logoText;
    public Text buttonText;
    public GameObject controlsPage;
    private void Start()
    {
        gameM = GameManager.gameManager;
    }
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            LoadNextScene();
        }

        if (Input.GetButtonDown("Select"))
        {
            InvertUseControl();
        }
    }

    public void InvertUseControl()
    {
        if (gameM.useControll)
        {
            gameM.useControll = false;
            logoText.text = "PRESSIONE ENTER";
            buttonText.text = "Controle";
        }
        else
        {
            gameM.useControll = true;
            logoText.text = "PRESSIONE START";
            buttonText.text = "Mouse";
        }
    }

    void LoadNextScene()
    {
        GameManager.gameManager.LoadNextStage();
    }

    public void opencontrol()
    {
        controlsPage.SetActive(true);
    }

    public void closecontrol()
    {
        controlsPage.SetActive(false);
    }
}
