using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonSpawnerScript : MonoBehaviour
{
    public Transform[] SpawnSpots;
    float SpawnTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpawnTime += Time.deltaTime;
        if(SpawnTime>5)
        {
            SpawnTime = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        int SpawnIdx=Random.Range(0, SpawnSpots.Length);

        Vector3 SpawnPosition = SpawnSpots[SpawnIdx].position;
        SpawnPosition.x += Random.Range(-2.0f, 2.0f);
        SpawnPosition.z += Random.Range(-2.0f, 2.0f);

        GameObject MonSource = Resources.Load("Prefab/Slime") as GameObject;
        GameObject MonObj = Instantiate(MonSource);
        MonObj.transform.position = SpawnPosition;

    }
}
