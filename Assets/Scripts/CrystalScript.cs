using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalScript : MonoBehaviour
{
    Vector3 rotAngle=new Vector3(0, 0, 10.0f);
    float rotSpeed=10.0f;
    float Speed = 0.1f;
    GameObject Player;
    bool IsCollected = false;
    private void Awake()
    {
        Player = GameObject.Find("Player");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotAngle * rotSpeed * Time.deltaTime);

        FindPlayer();

       
    }

    private void OnDisable()
    {
        Speed = 0.1f;
        StopCoroutine(CollectCo());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
           
        }
    }
    void FindPlayer()
    {
        if ((Player.transform.position - transform.position).magnitude < 5)
        {
            StartCoroutine(CollectCo());
        }
    }
    IEnumerator CollectCo()
    {
        yield return null;
        Vector3 temp = Player.transform.position;
        temp.y += 0.5f;
        Vector3 Dir = temp - transform.position;
        Dir.Normalize();

        transform.position += Dir * Speed * Time.deltaTime;
        Speed *= 1.01f;
    }

   

}
