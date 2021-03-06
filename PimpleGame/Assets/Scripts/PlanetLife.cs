﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlanetLife : MonoBehaviour
{
    private float pool = 0;
    private float life = 100f;
    private bool juicin = false;
    private JuicePouch juicePouch;

    public float juiceFillRate = 5f;
    public float pouchToPoolConversionRate = .1f;
    //public Slider poolSlider;
    //public Slider lifeSlider;
    public GameObject poolCylinder;
    public float minPoolCylinderPos = -0.626f;
    public float maxPoolCylinderPos = -0.294f;
    public ParticleSystem poolParticles;

    AudioSource fillAudio;

    public TextMeshProUGUI endOfGameScreenText;
    public TextMeshProUGUI endOfGameScreenTextBacker;
    public Animator endOfGameScreenAnimator;


    private void Awake()
    {
        fillAudio = GameObject.Find("FillAudio").GetComponent<AudioSource>();
        poolParticles.Stop();
        juicePouch = FindObjectOfType<JuicePouch>();
        handlePoolHeight();

        if (endOfGameScreenText == null) { Debug.LogError("Need to hook up end of game screen"); }
        if (endOfGameScreenTextBacker == null) { Debug.LogError("Need to hook up end of game screen"); }
        if (endOfGameScreenAnimator == null) { Debug.LogError("Need to hook up end of game screen animator"); }
    }

    private void handlePoolHeight()
    {
        poolCylinder.transform.localPosition = new Vector3(poolCylinder.transform.localPosition.x, Mathf.Lerp(minPoolCylinderPos, maxPoolCylinderPos, pool / 100f), poolCylinder.transform.localPosition.z);
    }

    public void depleteLife(float amount)
    {
        life -= amount;
        //lifeSlider.value = life / 100f;
        if(life <= 0f)
        {
            lose();
        }
        handlePoolHeight();
    }

    public void addToPool(float amount)
    {
        pool += amount;
        //poolSlider.value = pool / 100f;
        if(pool >= 100)
        {
            win();
        }
        handlePoolHeight();
    }

    private void win()
    {
        endOfGameScreenAnimator.SetBool("On", true);
        endOfGameScreenText.text = "Y O U W I N";
        endOfGameScreenTextBacker.text = "Y O U W I N";

    }

    private void lose()
    {
        //losing changed to not be affected by planet life

        //endOfGameScreenAnimator.SetBool("On", true);
        //endOfGameScreenText.text = "Y O U L O S E";
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
        poolParticles.Play();
        fillAudio.pitch = 0.6f;
        fillAudio.Play();
        while (juicin && jp.getCurrentJuiceAmount() > 0)
        {
            fillAudio.pitch += 0.006f;
            jp.RemoveJuice(juiceFillRate * Time.deltaTime);
            addToPool(juiceFillRate * pouchToPoolConversionRate * Time.deltaTime);
            yield return null;
        }
        poolParticles.Stop();
        fillAudio.Stop();
        juicin = false;
    }
    
}
