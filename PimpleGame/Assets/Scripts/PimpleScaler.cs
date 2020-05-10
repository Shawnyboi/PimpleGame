using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PimpleScaler : MonoBehaviour
{
    public float minScale = .5f;
    public float maxScale = 2f;
    public GameObject pimpleMesh;
    private Pimple pimple;

    private void init()
    {
        pimple = GetComponent<Pimple>();
        if(pimple == null)
        {
            Debug.LogError("PimpleScaler missing pimple reference");
        }
        if (pimpleMesh == null)
        {
            Debug.LogError("PimpleScaler missing mesh reference");
        }
    }
    private void Awake()
    {
        init();
        scaleUpdate();
    }

    private void scaleUpdate()
    {
        float currentGrowthPercent = pimple.getCurrentGrowthPercent();
        float scale = Mathf.Lerp(minScale, maxScale, currentGrowthPercent / 100f);
        pimpleMesh.transform.localScale = new Vector3(scale, scale, scale);
    }
    private void Update()
    {
        scaleUpdate();
    }
}
