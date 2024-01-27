using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StoneMonScript : NormalMonRootScript
{
    float AttackTime=0;
    float MaxAttackTime = 3;
    float Attack1PreSpeed = 2;
    float Attack1Speed = 4;
    bool IsAttacking = false;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        Speed = 3;
        RotSpeed = 8;
        AttackDist = 2.0f;
    }


    new void Update()
    {
        base.Update();
   

    }

    new void FixedUpdate()
    {
        base.FixedUpdate();

        AttackUpdate();
    }

    void AttackUpdate()
    {
        if (MONSTATE.ATTACK1 != State)
            return;

        AttackTime += Time.deltaTime;
        if(AttackTime > MaxAttackTime)
        {
            Attack1EndFunc();
        }


       
        if (AttackTime<0.5f)
        {
            if(!IsAttacking)
            {
                Attack1StartFunc();
            }

            Vector3 Dir = Player.transform.position - transform.position;
            Dir.y = 0;
            Dir.Normalize();
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Dir), RotSpeed * Time.deltaTime);

            transform.position -= Time.deltaTime * transform.forward * Attack1PreSpeed;
        }
        else
        {
            transform.position += Time.deltaTime * transform.forward * Attack1Speed;
        }


    }
}
