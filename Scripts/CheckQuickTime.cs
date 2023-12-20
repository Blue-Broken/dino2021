using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckQuickTime : MonoBehaviour
{
    public int pressedTimesNeeded;
    public GameObject defeatCutscene;
    public GameObject passedCutscene;
    public GameObject currentCutscene;
    public SpriteRenderer blackCover;
    private bool load;
    private bool defeated;
    private bool called = true;

    private void OnEnable()
    {
        if(called)
        {            
            called = false;
            Debug.Log("Checked");
            Debug.Log(QuickTimeControll.pressedTimes);
            if (QuickTimeControll.pressedTimes < pressedTimesNeeded)
            {
                
                if (!defeated)
                {
                    Debug.Log("SetDefeted");
                    defeatCutscene.SetActive(true);
                    currentCutscene.SetActive(false);
                    defeated = true;
                    //gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("Load Defeat");
                   
                }

                StartCoroutine(LoadPassed());
            }
        }
    }

    private void Start()
    {
        
    }

    IEnumerator LoadPassed()
    {
        Vector4 colorAlpha = Vector4.zero;

        for (int i = 0; i < 100; i++)
        {
            colorAlpha += new Vector4(0, 0, 0, 0.01f);
            blackCover.color = colorAlpha;
            yield return new WaitForSeconds(0.01f);
        }

        blackCover.color = Color.black;

        dynamic loading = null;

        if (!defeated)
        {
            Debug.Log("Load next");
            GameManager.gameManager.LoadNextStage();
        }
        else
        {
            Debug.Log("Load restart");
            loading = SceneManager.LoadSceneAsync("RestartScene");
        }

        while (!loading.isDone)
        {
            yield return null;
        }
    }
}
