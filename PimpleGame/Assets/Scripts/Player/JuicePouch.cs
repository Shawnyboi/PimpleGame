using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuicePouch : MonoBehaviour
{
    float juice = 0;
    [SerializeField] float maxJuice = 100;
    [SerializeField] float minScale = 1;
    [SerializeField] float maxScale = 3;
    [SerializeField] MeshRenderer juiceRenderer = null;
    [SerializeField] int juiceMaterial = 0;

    private void Start()
    {
        displayJuice();
    }

    public float getCurrentJuiceAmount()
    {
        return juice;
    }

    public void AddJuice(float value)
    {

        if (value <= Helper.Epsilon)
        {
            return;
        }
        Debug.Log("Adding juice to pouch " + value);
        value = Mathf.Min(value, maxJuice - juice);
        juice += value;
        displayJuice();
    }

    public void RemoveJuice(float value)
    {
        //Debug.Log("Removing juice, current juice amount " + juice);
        if (value <= Helper.Epsilon)
        {
            return;
        }

        value = Mathf.Min(value, juice);
        juice -= value;
        displayJuice();
    }

    void displayJuice()
    {
        juiceRenderer.materials[juiceMaterial].SetFloat("_FillAmount", juice / maxJuice);
    }
}
