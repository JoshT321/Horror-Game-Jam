using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StartContinueGame : MonoBehaviour
{
    private TMP_Text buttonText;

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
        GameManager.Instance.LoadScene("Doors and Interaction");
    }
}
