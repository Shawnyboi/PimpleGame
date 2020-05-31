﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerLife : MonoBehaviour
{
    public int hitPoints;
    public TextMeshProUGUI endOfGameScreenText;
    public TextMeshProUGUI endOfGameScreenTextBacker;
    public Animator endOfGameScreenAnimator;


    private int hitsTaken;

    private void Awake()
    {
        hitsTaken = 0;
        if (endOfGameScreenText == null) { Debug.LogError("Need to hook up end of game screen"); }
        if (endOfGameScreenTextBacker == null) { Debug.LogError("Need to hook up end of game screen"); }
        if (endOfGameScreenAnimator == null) { Debug.LogError("Need to hook up end of game screen animator"); }
    }
    public void hit()
    {
        hitsTaken += 1;
        checkIfGameOver();
    }

    private void checkIfGameOver()
    {
        if(hitsTaken >= hitPoints)
        {
            lose();
        }
    }

    private void lose()
    {
        endOfGameScreenAnimator.SetBool("On", true);
        endOfGameScreenText.text = "Y O U L O S E";
        endOfGameScreenTextBacker.text = "Y O U L O S E";
    }
}