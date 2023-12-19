using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class TextProgressive : MonoBehaviour
{
    private GameManager gameM;
    private AudioSource audioS;
    private Text textUI;
    public float textDelay;
    private string textToShow;
    private bool showing;
    public bool canSkip;
    private bool canStopText;

    private void Start()
    {
        gameM = GameManager.gameManager;
        audioS = GetComponent<AudioSource>();
        textUI = GetComponent<Text>();
    }

    private void Update()
    {
        if (!showing && Input.GetButtonDown("Action"))
        {
            canStopText = false;
            canSkip = true;
        }

        if (showing && canStopText && Input.GetButtonDown("Action"))
        {
            showing = false;
            StopAllCoroutines();
            audioS.Stop();
            textUI.text = textToShow;
        }
    }

    public void PlayText(string text, AudioClip audio)
    {
        textUI.text = "";
        textToShow = text;
        canSkip = false;
        showing = true;
        StartCoroutine(ShowText(text, audio));
    }

    IEnumerator ShowText(string text, AudioClip audio)
    {
        yield return new WaitUntil(() => !Input.GetButtonDown("Action"));

        canStopText = true;

        if (audio)
        {
            audioS.clip = audio;
            audioS.PlayOneShot(audioS.clip);
        }

        for(int i = 0; i < text.Length; i++)
        {
            textUI.text += text[i];
            yield return new WaitForSecondsRealtime(textDelay);
        }

        canSkip = true;  
        showing = false;
    }
}
