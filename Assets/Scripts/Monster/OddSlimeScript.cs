using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class OddSlimeScript : NormalMonRootScript
{

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        Speed = 2;
        RotSpeed = 8;
        AttackDist = 1.0f;
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
        string temps = "Sound/SlimeAttack" + Random.Range(1, 5).ToString();
        AClip = Resources.Load<AudioClip>(temps);
        ASource.PlayOneShot(AClip);
    }
}
