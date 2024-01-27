using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameObject Player;
    GameObject FocusBoss;
    float Speed1 = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        Player=GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {

        if(SampleMgr.Inst.IsOnBossEvent)
        {
            if (!FocusBoss)
                return;
            Vector3 Dir = transform.right - transform.forward * 0.25f;
            transform.transform.position += Dir * Speed1 * Time.deltaTime;
            transform.LookAt(FocusBoss.transform.position + Vector3.up * 2);

        }
        else if (Player)
        {
            Vector3 MovePos = transform.position;
            MovePos.x = Player.transform.position.x;
           
            MovePos.z = Player.transform.position.z - 7;
            transform.position = MovePos;
        }


     
    }

    public void SetFocusBoss(GameObject boss)
    {
        SampleMgr.Inst.IsOnBossEvent = true;
        FocusBoss = boss;
        transform.LookAt(FocusBoss.transform.position + Vector3.up * 3);
        //transform.LookAt(FocusBoss.transform.position+Vector3.up*3);
        Vector3 tempPos = FocusBoss.transform.position + FocusBoss.transform.forward * 3
             + FocusBoss.transform.right * 3 + Vector3.up * 4;


        transform.position = tempPos;
        
        StartCoroutine(BossEventEndCo());

        SampleMgr.Inst.MonsterEventStart();
        SampleMgr.Inst.BossEventUIOn();
    }

    public void EndBossEvent()
    {
        SampleMgr.Inst.IsOnBossEvent = false;
        FocusBoss = null;        
        SampleMgr.Inst.BossEventUIOff();
    }

    IEnumerator BossEventEndCo()
    {
        yield return new WaitForSeconds(5.0f);
        SampleMgr.Inst.IsOnBossEvent = false;
        transform.position = new Vector3(0, 9, 0);
        float angle = 45f;
        Quaternion rot = Quaternion.Euler(angle, 0, 0);
        transform.rotation = rot;
        SampleMgr.Inst.MonsterEventEnd();
        EndBossEvent();
    }
}
