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
    bool IsFalling = true;
    bool IsFindPlayer = false;


    ///////////////////

    
    
    float HSpeed = 0;
    float VSpeed = 0;
    float MaxVSpeed = 10;
    float Acc=-9.8f;
    Vector3 HDir = Vector3.zero;


    bool IsUp = false;
    AudioSource ASource;
  
    private void Awake()
    {
        ASource=GetComponent<AudioSource>();
        Player = GameObject.Find("Player");
    }
    // Start is called before the first frame update
    void Start()
    {
        HSpeed = Random.Range(1.0f, 2.0f);
        HDir.x = Random.Range(-1.0f, 1.0f);
        HDir.z = Random.Range(-1.0f, 1.0f);
        HDir.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
       
        transform.Rotate(rotAngle * rotSpeed * Time.deltaTime);        
        
        if(IsFindPlayer)
        {
            Vector3 temp = Player.transform.position;
            temp.y += 0.5f;
            Vector3 Dir = temp - transform.position;
            Dir.Normalize();

            transform.position += Dir * Speed * Time.deltaTime;
            Speed *= 1.01f;
            return;
        }

        if(IsFalling)
        {
            transform.position += HDir * HSpeed * Time.deltaTime;
            
            VSpeed += Time.deltaTime * Acc;
            
            transform.position += Vector3.up * VSpeed * Time.deltaTime;
          
            
            if(transform.position.y<=0.5f)
            {
                if(!IsUp)
                {
                    MaxVSpeed *= 0.7f;
                    ASource.Play();
                    ASource.volume *= 0.7f;
                }
                    

                IsUp = true;
                
                VSpeed = MaxVSpeed;
               
                if (MaxVSpeed < 1)
                    IsFalling = false;
            }

            if(IsUp && VSpeed<=0)
            {
                IsUp = false;
            }         

        }
        else
        {
            FindPlayer();
        }
       
    }
    private void OnEnable()
    {
        HSpeed = Random.Range(1.0f, 2.0f);
        HDir.x = Random.Range(-1.0f, 1.0f);
        HDir.z = Random.Range(-1.0f, 1.0f);
        HDir.Normalize();
        IsFindPlayer = false;
        ASource.volume = 1;
    }
    private void OnDisable()
    {
        Speed = 0.1f;
        ASource.volume = 1;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            
            IsFalling = true;
            gameObject.SetActive(false);
            SampleMgr.Inst.SetExpBar(10);
           
        }
    }

    
    void FindPlayer()
    {
        if (!Player)
            return;

        if (IsFindPlayer)
            return;

        if ((Player.transform.position - transform.position).magnitude < 5)
        {
            IsFindPlayer = true;
        }
    }
   

   

   

}
