using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnerScript : MonoBehaviour
{
    public static ItemSpawnerScript Inst;
    public GameObject Item1;
    int MaxItemCount = 50;
    public List<GameObject> ItemPool = new List<GameObject>();
    // Start is called before the first frame update
    private void Awake()
    {
        Inst = this;
    }
    void Start()
    {
        for (int i = 0; i < MaxItemCount; i++)
        {            
            GameObject Item;
           
            Item = Instantiate(Item1);

            Item.name = "Item" + i.ToString();
            Item.SetActive(false);
            ItemPool.Add(Item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnItem(Vector3 SpawnPos)
    {
        for(int i=0; i<ItemPool.Count; i++)
        {
            if (!ItemPool[i].activeSelf)
            {
                ItemPool[i].SetActive(true);
                SpawnPos.y = 0.5f;
                ItemPool[i].transform.position = SpawnPos;
                break;

            }
        }
    }
}
