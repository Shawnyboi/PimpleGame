using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pimple : MonoBehaviour
{
    public float growthRate = 2f;
    public float minimumLanceableSize = 30f;
    public float planetDamageAmount = 34f;
    private float currentSize; //as a percentage
    private bool growing;
    private PlanetLife planetLife;


    private void init()
    {
        currentSize = 0;
        growing = true;
        planetLife = FindObjectOfType<PlanetLife>();

    }
    private void Awake()
    {
        init();
    }

    private void POP()
    {
        if(planetLife != null)
        {
            GameObject.Destroy(this.gameObject);
            planetLife.depleteLife(planetDamageAmount);
        }
        else
        {
            Debug.LogWarning("Pimple popped but no planet to plunder");
        }

        //SPPPPPLLLUUURRRTTT
    }

    private void updateGrowth(float t)
    {
        if (growing)
        {
            if (currentSize >= 100f)
            {
                POP();
            }
            else
            {
                currentSize += growthRate * t;
            }
        }
    }
    private void Update()
    {
        updateGrowth(Time.deltaTime);
    }
    
    private void killPimple()
    {
        Destroy(this.gameObject);
    }

    public void reduceSize(float amt)
    {
        currentSize -= amt;
        if (currentSize < 0f)
        {
            killPimple();
        }
    }

    public void freezeGrowth()
    {
        growing = false;
    }

    public void unfreezeGrowth()
    {
        growing = true;
    }

    public float getCurrentGrowthPercent()
    {
        return currentSize;
    }
}
