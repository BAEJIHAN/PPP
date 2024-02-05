using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class OrkScript : NormalMonRootScript
{
    public GameObject AxePos;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        Speed = 1.5f;
        RotSpeed = 8;
        AttackDist = 5.5f;
    }


    new void Update()
    {
        base.Update();

       
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void AttackSoundFunc()
    {
        string temps = "Sound/Axe";
        AClip = Resources.Load<AudioClip>(temps);
        ASource.PlayOneShot(AClip);
    }
}
