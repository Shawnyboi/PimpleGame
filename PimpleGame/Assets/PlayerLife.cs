using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    public int hitPoints;
    public TextMeshProUGUI endOfGameScreenText;
    public TextMeshProUGUI endOfGameScreenTextBacker;
    public Animator endOfGameScreenAnimator;
    public List<Image> hitSprites;

    public Sprite aliveHeart;
    public Sprite deadHeart;

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
        updateSprites();
        checkIfGameOver();
    }

    private void checkIfGameOver()
    {
        if(hitsTaken >= hitPoints)
        {
            lose();
        }
    }

    private void updateSprites()
    {
        if (hitsTaken <= 3)
        {
            for (int i = hitsTaken - 1; i >= 0; i--)
            {
                hitSprites[i].sprite = deadHeart;
            }
        }
    }


    private void lose()
    {
        endOfGameScreenAnimator.SetBool("On", true);
        endOfGameScreenText.text = "Y O U L O S E";
        endOfGameScreenTextBacker.text = "Y O U L O S E";
    }
}
