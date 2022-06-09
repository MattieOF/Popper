using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class SpawnerObject
{
    public GameObject gameObject;
    public float weight;
}

public class ObjectSpawner : MonoBehaviour
{
    public List<SpawnerObject> objects;
    public Vector3             bounds;

    [FormerlySerializedAs("_totalWeight")] public float totalWeight;

    private void Start()
    {
        CalculateTotalWeight();
    }

    public void CalculateTotalWeight()
    {
        totalWeight = 0;
        foreach (SpawnerObject so in objects)
        {
            totalWeight += so.weight;
        }
    }

    public GameObject GetObjectFromNumber(float value)
    {
        if (value > totalWeight) CalculateTotalWeight();
        if (value > totalWeight) return null;

        GameObject currentObject;
        float      currentWeight = 0;
        foreach (SpawnerObject so in objects)
        {
            currentObject = so.gameObject;
            currentWeight += so.weight;
            if (value <= currentWeight)
                return currentObject;
        }

        return null;
    }

    public Vector3 GetPointInBounds()
    {
        Vector3 min = transform.position - bounds / 2;
        Vector3 max = transform.position + bounds / 2;
        return new Vector3
        {
            x = Random.Range(min.x, max.x),
            y = Random.Range(min.y, max.y),
            z = Random.Range(min.z, max.z)
        };
    }

    public void Spawn()
    {
        float number = Random.Range(0, totalWeight);
        Instantiate(GetObjectFromNumber(number), GetPointInBounds(), Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, bounds);
    }
}
