using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemHitColScript : MonoBehaviour
{
    [SerializeField] GolemScript Golem;
    public GameObject PGolem;
    public bool OnHitReady = true;
    
    private void Awake()
    {
        Golem=PGolem.GetComponent<GolemScript>();
        if (!Golem)
            Debug.Log("golnull");
    }
    // Start is called before the first frame update
    void Start()
    {
        OnHitReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == "PlayerAttack" && OnHitReady)
        {
            OnHitReady = false;
            Golem.GolemHit();
            Golem.TakeDamage(3);
           


        }
    }
}
