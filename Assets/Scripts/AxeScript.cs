using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class AxeScript : MonoBehaviour
{
    Vector3 rotAngle = new Vector3(-10.0f, 0, 0);
    float rotSpeed = 20.0f;
    float Speed = 3.0f;
    GameObject Player;

    Vector3 Dir=Vector3.zero;
    float Dist = 8;
    private void Awake()
    {
        Player = GameObject.Find("Player");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        transform.Rotate(rotAngle * rotSpeed * Time.deltaTime);


       

        transform.position += Dir * Speed * Time.deltaTime;

        Dist -= Speed * Time.deltaTime;

        if (Dist <0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);           

        }
    }

    public void SetDir(Vector3 Pos)
    {
        Pos.y = 0.5f;
       
        Dir = Pos - transform.position;
        Dir.Normalize();
    }
}
