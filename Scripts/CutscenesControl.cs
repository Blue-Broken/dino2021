using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutscenesControl : MonoBehaviour
{

    public GameObject initalCutscene;
    public GameObject[] cutscenes;
    public Collider[] cutscenesTriggers;
    private static Collider currentTrigger;
    
    private void Awake()
    {
        if(GameManager.gameManager.currentCheckPoint > 0)
        {
            Destroy(initalCutscene);
        }
    }

    private void Update()
    {
        if (currentTrigger != null)
        {
            int selectedCutscene = 0;

            for(int i = 0; i < cutscenesTriggers.Length; i++)
            {
                if(cutscenesTriggers[i] == currentTrigger)
                {
                    selectedCutscene = i;
                }
            }
            
            cutscenes[selectedCutscene].SetActive(true);

            currentTrigger = null;
        }
    }

    public static void PlayCutsceneParameters(Collider cutSceneTrigger)
    {
        currentTrigger = cutSceneTrigger;
    }
}
