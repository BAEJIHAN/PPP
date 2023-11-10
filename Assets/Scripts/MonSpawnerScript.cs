using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonSpawnerScript : MonoBehaviour
{
    public Transform[] SpawnSpots;
    float SpawnTime = 0;

    int MaxMonCount = 20;
    public List<GameObject> monsterPool = new List<GameObject>();
    public GameObject mon1;
    public GameObject mon2;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < MaxMonCount; i++)
        {
            //몬스터 프리팹을 생성
            int monRannum = Random.Range(0, 2);
            GameObject monster;
            if (monRannum == 0)
            {
                monster = Instantiate(mon1);
            }
            else if (monRannum == 1)
            {
                monster = Instantiate(mon2);
            }
            else
            {
                monster = Instantiate(mon1);
            }
            monster.name = "Monster_" + i.ToString();
            monster.SetActive(false);
            monsterPool.Add(monster);
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
        int SpawnIdx = Random.Range(0, SpawnSpots.Length);

        Vector3 SpawnPosition = SpawnSpots[SpawnIdx].position;
        SpawnPosition.x += Random.Range(-2.0f, 2.0f);
        SpawnPosition.z += Random.Range(-2.0f, 2.0f);

        for (int i = 0; i < monsterPool.Count; i++)
        {

            if (!monsterPool[i].activeSelf)
            {

                monsterPool[i].transform.position = SpawnPosition;

                monsterPool[i].SetActive(true);



                break;
            }


        }
    }
}
