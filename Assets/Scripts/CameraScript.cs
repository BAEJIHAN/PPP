using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Player=GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Player)
        {
            Vector3 MovePos = transform.position;
            MovePos.x = Player.transform.position.x;
            MovePos.z = Player.transform.position.z-7;
            transform.position = MovePos;
        }
    }
}
