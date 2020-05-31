using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pimple : MonoBehaviour
{
    public float growthRate = 2f;
    public float minimumLanceableSize = 30f;
    public bool ReadyToLance => currentSize >= minimumLanceableSize;

    public float planetDamageAmount = 34f;
    private float currentSize; //as a percentage
    private bool growing;
    private PlanetLife planetLife;
    [SerializeField] MeshRenderer pimpleRenderer = null;
    [HideInInspector] public PimpleSpawner spawner;
    public UnityEventBool onPumpingStarted = null;
    public UnityEventBool onPumpingEnded = null;
    public UnityEvent onLancedAway = null;
    public UnityEvent onPopped = null;

    bool popped = false;
    public bool Popped => popped;
    public GameObject explosion;


    private void init()
    {
        currentSize = 0;
        growing = true;
        planetLife = FindObjectOfType<PlanetLife>();
        if (!ReadyToLance)
        {
          pimpleRenderer.material.DisableKeyword("_EMISSION");
        }
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
        popped = true;
        onPopped.Invoke();
        Instantiate(explosion, transform.position, transform.rotation);
        GameObject.Destroy(this.gameObject);
        spawner.Pimples.Remove(this);

        //SPPPPPLLLUUURRRTTT
  }

    private void updateGrowth(float t)
    {
        if (growing)
        {
            bool wasLanceable = ReadyToLance;

            if (currentSize >= 100f)
            {
                POP();
            }
            else
            {
                currentSize += growthRate * t;
                if (!wasLanceable && ReadyToLance)
                {
                    pimpleRenderer.material.EnableKeyword("_EMISSION");
                }
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
        Debug.Log("Reducing Size by " + amt);
        currentSize -= amt;
        if (currentSize < 0f)
        {
            onLancedAway.Invoke();
            killPimple();
        }
    }

    public void freezeGrowth()
    {
        growing = false;
        onPumpingStarted.Invoke(true);
    }

    public void unfreezeGrowth()
    {
        growing = true;
        onPumpingEnded.Invoke(false);
    }

    public float getCurrentGrowthPercent()
    {
        return currentSize;
    }
}
