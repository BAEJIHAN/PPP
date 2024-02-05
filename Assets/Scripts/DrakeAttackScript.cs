using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrakeAttackScript : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject Target;
    float Speed = 5;
    
    public bool IsCol = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Target == null)
            return;


       

        Vector3 Dir = Target.transform.position - transform.position;
        Dir.Normalize();

        transform.position += Dir * Time.deltaTime * Speed;

        if(!Target.GetComponent<MonRootScript>())
        {

        }
        else if (Target.GetComponent<MonRootScript>().IsDead)
        {
            gameObject.SetActive(false);
        }
            
    }

    private void OnDisable()
    {
        StopCoroutine(SetOff());
        IsCol = false;
    }

    private void OnEnable()
    {
        StopCoroutine(SetOff());
        IsCol = false;
        
    }
    public void SetTarget(GameObject target)
    {
        if (target == null)
        {
            StartDeath();
            return;
        }
            
        Target = target;
    }

    public void StartSetOff()
    {
        StartCoroutine(SetOff());
    }

    public void CancelAttack()
    {
        gameObject.SetActive(false);
    }

    public void StartDeath()
    {
        StartCoroutine(DeathCol());
    }
    IEnumerator SetOff()
    {
        yield return new WaitForSeconds(3);

        gameObject.SetActive(false);

    }

    IEnumerator DeathCol()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

   

}
