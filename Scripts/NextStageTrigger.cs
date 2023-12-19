using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStageTrigger : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.gameManager.LoadNextStage();
    }
}
