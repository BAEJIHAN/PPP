using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpawnerScript : MonoBehaviour
{   
   

    int MaxDrakeAttackCount = 5;
    public List<GameObject> DrakeAttackPool = new List<GameObject>();
    public GameObject DrakeAttack;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < MaxDrakeAttackCount; i++)
        {
            //몬스터 프리팹을 생성            
            GameObject DAttack;

            DAttack = Instantiate(DrakeAttack);


            DAttack.name = "DrakeAttack" + i.ToString();
            DAttack.SetActive(false);
            DrakeAttackPool.Add(DAttack);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrakeAttackSpawn(Vector3 spawnPos, GameObject target)
    {
       
        for (int i = 0; i < DrakeAttackPool.Count; i++)
        {

            if (!DrakeAttackPool[i].activeSelf)
            {

                DrakeAttackPool[i].transform.position = spawnPos;

                DrakeAttackPool[i].SetActive(true);

                DrakeAttackPool[i].GetComponent<DrakeAttackScript>().SetTarget(target);

                break;
            }


        }
    }
}
