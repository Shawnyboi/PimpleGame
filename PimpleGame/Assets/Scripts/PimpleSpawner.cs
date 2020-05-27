using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PimpleSpawner : MonoBehaviour
{
    public float minSpawnRate = .5f;
    public float maxSpawnRate = 3f;
    public AnimationCurve spawnRateCurve;
    public float maxRateTime = 300f;

    public float planetRadius = 10f;
    public int maxPimples = 10;
    public GameObject planet;
    public GameObject pimplePrefab;

    private float spawnRate;
    private float timePassed;

    private List<Pimple> pimples;
    public List<Pimple> Pimples => pimples;

    private void init()
    {
        pimples = new List<Pimple>();
        spawnRate = minSpawnRate;
    }

    private void Awake()
    {
        init();
    }

    private bool rollForSpawn(float t)
    {
        float roll = Random.Range(0f, 1f);
        if(spawnRate * t > roll)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private Vector3 getOrigin()
    {
        if (planet != null)
        {
            return planet.transform.position;
        }
        else
        {
            return this.transform.position;
        }
    }

    private Vector3 getSpawnPosition()
    {
        Vector3 randomDirection = Vector3.Normalize(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
        return planetRadius * randomDirection + getOrigin();
    }

    private void spawnPimple()
    {
        Vector3 pos = getSpawnPosition();

        if (!Physics.Raycast(getOrigin(), pos - getOrigin(), 1 << LayerMask.NameToLayer("DontSpawnOn")))
        {
            GameObject inst = Instantiate(pimplePrefab, pos, Quaternion.identity);
            Quaternion rot = Quaternion.LookRotation(pos - getOrigin());
            inst.transform.rotation = rot;
            pimples.Add(inst.GetComponent<Pimple>());
            inst.GetComponent<Pimple>().spawner = this;
        }
    }

    private void handleSpawnRateGrowth()
    {
        float curveVal = spawnRateCurve.Evaluate(Mathf.Min(timePassed, maxRateTime) / maxRateTime);
        spawnRate = Mathf.Lerp(minSpawnRate, maxSpawnRate, curveVal);
    }

    private void Update()
    {
        timePassed += Time.deltaTime;
        if(pimples.Count < maxPimples)
        {
            if (rollForSpawn(Time.deltaTime))
            {
                spawnPimple();
            }
        }
    }
}
