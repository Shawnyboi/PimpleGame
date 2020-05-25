using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetLife : MonoBehaviour
{
    private float pool = 0;
    private float life = 100f;
    private bool juicin = false;
    private JuicePouch juicePouch;

    public float juiceFillRate = 5f;
    public Slider poolSlider;
    public Slider lifeSlider;

    private void Awake()
    {
        juicePouch = FindObjectOfType<JuicePouch>();
    }

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

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("Trigger enter with name " + other.gameObject.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Trigger enter with player");
            if(juicePouch != null && !juicin)
            {
                Debug.Log("Juicing");
                StartCoroutine(JUICEIT(juicePouch));
            }

            if(juicePouch == null)
            {
                Debug.LogError("Didn't catch the juice pouchs");
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        juicin = false;
    }

    private IEnumerator JUICEIT(JuicePouch jp)
    {
        juicin = true;
        while(juicin && jp.getCurrentJuiceAmount() > 0)
        {
            jp.RemoveJuice(juiceFillRate * Time.deltaTime);
            addToPool(juiceFillRate * Time.deltaTime);
            yield return null;
        }
        juicin = false;
    }
    
}
