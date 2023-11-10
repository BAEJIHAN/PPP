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
        Vector3 Dir = Target.transform.position - transform.position;
        Dir.Normalize();

        transform.position += Dir * Time.deltaTime * Speed;

        if (Target.GetComponent<NormalMonRootScript>().IsDead)
            gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        StopCoroutine(SetOff());
        IsCol = false;
    }
    public void SetTarget(GameObject target)
    {
        if (target == null)
            return;
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
    IEnumerator SetOff()
    {
        yield return new WaitForSeconds(3);

        gameObject.SetActive(false);

    }

   

}
