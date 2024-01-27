using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonSpawnerScript : MonoBehaviour
{
    public Transform[] SpawnSpots;
    float SpawnTime = 0;

    int MaxMonCount = 30;
    List<List<GameObject>> monsterPool = new List<List<GameObject>>();

   
    public GameObject[] MonPrefab;
   
    // Start is called before the first frame update
    void Start()
    {
        GameObject monster;
        int MonNum = 0;
        for (int i = 0; i < 4; i++)
        {
            monsterPool.Add(new List<GameObject>());
            
        }

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < MaxMonCount; j++)
            {
                monster = Instantiate(MonPrefab[i]);


                monster.name = "Monster_" + MonNum.ToString();
                MonNum++;
                monster.SetActive(false);
                monsterPool[i].Add(monster);
            }
                
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        SpawnTime += Time.deltaTime;
        if (SpawnTime > 5)
        {
            SpawnTime = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        if (GValue.MaxNMonNum <= GValue.NMonNum)
            return;

        if (SampleMgr.Inst.IsBoss)
            return;

        int SpawnIdx = Random.Range(0, SpawnSpots.Length);

        Vector3 SpawnPosition = SpawnSpots[SpawnIdx].position;
        SpawnPosition.x += Random.Range(-2.0f, 2.0f);
        SpawnPosition.z += Random.Range(-2.0f, 2.0f);

        int MonIndex = 0;

        if(GValue.Level>=2)
        {
            MonIndex= Random.Range(0, 4);
        }
        else
        {
            MonIndex = Random.Range(0, 2);
        }

        for (int i = 0; i < monsterPool[MonIndex].Count; i++)
        {
            if (!monsterPool[MonIndex][i].activeSelf)
            {

                monsterPool[MonIndex][i].transform.position = SpawnPosition;

                monsterPool[MonIndex][i].SetActive(true);

                EffectSpawnerScript.Inst.SpawnSpawnEffect(SpawnPosition);
                GValue.NMonNum++;
                break;
            }

        }

    }
}
