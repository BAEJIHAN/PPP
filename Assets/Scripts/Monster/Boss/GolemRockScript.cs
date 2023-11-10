using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GolemRockScript : MonoBehaviour
{
    bool IsFlying = false;
    bool IsRolling = false;
    Vector3 TargetPos;
    float Speed = 8;
    float RollSpeed=5;
    float RotSpeed = 150;
    float RollRotSpeed = 150;
    float Dist;
    Vector3 Dir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        FlyUpdate();
        RollUpdate();
    }

    void FlyUpdate()
    {
        if (!IsFlying)
            return;

        transform.Rotate(0, RotSpeed*Time.deltaTime, 0);

        Dir = TargetPos - transform.position;
        Dir.Normalize();
        transform.position += Dir * Time.deltaTime * Speed;

        Dist -= Time.deltaTime * Speed;
        if (Dist < 0.01f)
        {
            IsRolling = true;
            IsFlying = false;
            Dir.y = 0;
        }
            
    }

    void RollUpdate()
    {
        if (!IsRolling)
            return;
        
        transform.Rotate(-RollRotSpeed * Time.deltaTime, 0, 0);
        

        transform.position += Dir * Time.deltaTime * RollSpeed;

        RollRotSpeed *= 0.99f;
        RollSpeed *= 0.99f;

        if (RollSpeed < 1.5f)
            Destroy(gameObject);
    }

    public void StartFly(Vector3 tPos)
    {
        IsFlying = true;
        TargetPos = tPos;
        Dist = (TargetPos - transform.position).magnitude;
    }
}
