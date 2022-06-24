using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StartContinueGame : MonoBehaviour
{
    private TMP_Text buttonText;
    public GameObject screenHider;

    private void Awake()
    {
        buttonText = transform.Find("Text").GetComponent<TMP_Text>();
    }

    private void Start()
    {
        if (SaveData.current.profile.checkpointIndex > 0)
        {
            buttonText.text = "Continue";
        }
    }


    public void StartGame_Button()
    {

        StartGame();
    }

    public void StartGame()
    {
        StartCoroutine(StartGame_Co());
    }

    public IEnumerator StartGame_Co()
    {
        Image screenHiderImage = transform.parent.Find("ScreenHider").GetComponent<Image>();
        float elapsedTime = 0f;
        float timeToWait = 1.5f;
        
        while (elapsedTime <= timeToWait)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0f, 1f, elapsedTime / timeToWait);
            screenHiderImage.color = new Color(0f, 0f, 0f, newAlpha);
            yield return null;
        }
        GameManager.Instance.LoadScene("Doors and Interaction");
    }
}
