using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetLife : MonoBehaviour
{
    private float pool = 0;
    private float life = 100f;

    public Slider poolSlider;
    public Slider lifeSlider;

    public void depleteLife(float amount)
    {
        life -= amount;
        lifeSlider.value = life / 100f;
        if(life <= 0f)
        {
            lose();
        }
    }

    public void addToPool(float amount)
    {
        pool += amount;
        poolSlider.value = pool / 100f;
        if(pool >= 100)
        {
            win();
        }
    }

    private void win()
    {
        Debug.Log("YOU WIN");
    }

    private void lose()
    {
        Debug.Log("YOU LOSE");
    }

}
