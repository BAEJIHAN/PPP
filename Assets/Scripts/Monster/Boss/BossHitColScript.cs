using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitColScript : MonoBehaviour
{
    BossRootScript Pivot;
    public GameObject PObj;
    public bool OnHitReady = true;
    
    private void Awake()
    {
        Pivot = PObj.GetComponent<BossRootScript>();
        if (!Pivot)
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
            Pivot.Hit();
            Vector3 collisionPoint = other.ClosestPointOnBounds(transform.position);
            EffectSpawnerScript.Inst.SpawnAttackEffect(collisionPoint);
            EffectSpawnerScript.Inst.SpawnDamageText(collisionPoint, GValue.PlayerDamage, Color.red);
            Pivot.TakeDamage(GValue.PlayerDamage);
           


        }
        
      

    }
}
