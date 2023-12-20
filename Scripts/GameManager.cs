using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public bool useControll;
    public bool paused;
    public bool armed;
    public int currentCheckPoint;
    public int currentCartrigdes;
    public int currentMunition;
    public int currentHealthPacks;
    public int currentStage = 1;
    public string[] stagesNames;
    private bool loadedStage = true;

    private void Awake()
    {
        if(gameManager == null)
        {
            gameManager = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;
    }

    public float Propotional(float X, float maxX, float minX, float maxY, float minY)
    {
        float normilize = Mathf.Clamp01((X - minX) / (maxX - minX));
        float Y = Mathf.Lerp(minY, maxY, normilize);

        return Y;
    }

    public void LoadCurrentState()
    {
        StartCoroutine(Load());
    }

    public void LoadNextStage()
    {
        if (loadedStage)
        {
            loadedStage = false;

            currentStage++;
            currentCheckPoint = 0;
            currentCartrigdes = PlayerController.currentCartridges;
            currentHealthPacks = PlayerController.currentHealthPacks;
            currentMunition = PlayerController.currentMunition;

            StartCoroutine(Load());
        }
    }

    IEnumerator Load()
    {
        var loading = SceneManager.LoadSceneAsync(stagesNames[currentStage - 1]);

        while (!loading.isDone)
        {
            yield return null;
        }

        loadedStage = true;
    }
}
