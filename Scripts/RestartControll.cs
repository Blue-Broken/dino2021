using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartControll : MonoBehaviour
{
    private GameManager gameM;
    private Animator anim;
    public Transform choiceSprite;
    public Transform[] choicePositions;
    private int choice;
    private float lastAxis;
    private bool showingOptions;
    private bool isLoading;

    private void Start()
    {
        gameM = GameManager.gameManager;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != lastAxis)
        {
            lastAxis = Input.GetAxisRaw("Horizontal");

            if (lastAxis > 0)
            {
                choice = 1;
            }

            if (lastAxis < 0)
            {
                choice = 0;
            }

            choiceSprite.position = choicePositions[choice].position + new Vector3(0,10,0);
        }

        if ((Input.GetButtonDown("Submit") || Input.GetButtonDown("Action")) && showingOptions)
        {
            if(!isLoading)
            {
                isLoading = true;
                StartCoroutine(LoadChoice());
            }
        }

        if ((Input.GetButtonDown("Submit") || Input.GetButtonDown("Action") || Input.GetMouseButtonDown(0)) && !showingOptions)
        {
            ShowingOptions();
        }
    }

    IEnumerator LoadChoice()
    {
        anim.SetTrigger("Load");

        if(choice == 1)
        {
            gameM.currentCheckPoint = 0;
            gameM.currentCartrigdes = 0;
            gameM.currentHealthPacks = 0;
            gameM.currentMunition = 0;
            gameM.currentStage = 0;
            var loading = SceneManager.LoadSceneAsync("TitleScreen");

            while (!loading.isDone)
            {
                yield return null;
            }
            
        }
        else
        {
           gameM.LoadCurrentState();         
        }
    }

    public void ShowingOptions()
    {
        anim.SetTrigger("Show");
        showingOptions = true;
    }
}
