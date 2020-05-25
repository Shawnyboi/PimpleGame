using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuicePouch : MonoBehaviour
{
    float juice = 0;
    [SerializeField] float maxJuice = 100;
    [SerializeField] float minScale = 1;
    [SerializeField] float maxScale = 3;

    public float getCurrentJuiceAmount()
    {
        return juice;
    }

    public void AddJuice(float value)
    {
        //Debug.Log("Adding juice, current juice amount " + juice);
        if (value <= Helper.Epsilon)
        {
            return;
        }

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
        float scale = minScale + ((maxScale - minScale) * (juice / maxJuice));
        // TODO figure out a way to scale while anchored to back.
        transform.localScale = new Vector3(scale, transform.localScale.y, transform.localScale.z);
    }
}
