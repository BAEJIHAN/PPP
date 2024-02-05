using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MushroomScript : NormalMonRootScript
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
        string temps = "Sound/MushRoomAttack" + Random.Range(1, 3).ToString();
        AClip = Resources.Load<AudioClip>(temps);
        ASource.PlayOneShot(AClip);
    }
}
