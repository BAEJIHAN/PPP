using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextScript : MonoBehaviour
{
    Text TextObj;
    float Speed = 1.5f;
    // Start is called before the first frame update
    private void Awake()
    {
        TextObj = GetComponentInChildren<Text>(); 
    }
    void Start()
    {
        Invoke("DisableFunc", 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void FixedUpdate()
    {
        //transform.forward = Camera.main.transform.forward;
        transform.position += Vector3.up*Time.deltaTime*Speed;
    }

    public void Spawn(Vector3 SpawnPos, int Damage, Color color)
    {
        TextObj.text = Damage.ToString();
        TextObj.color = color;
        transform.position = SpawnPos;
    }

    public void SpawnGuard(Vector3 SpawnPos)
    {
        TextObj.text = "Guard";
        TextObj.color = Color.white;
        transform.position = SpawnPos;
    }

    private void OnEnable()
    {
        Invoke("DisableFunc", 2);
    }
    void DisableFunc()
    {
        gameObject.SetActive(false);
        
    }
}
