using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    public GameObject Fireball;
    public GameObject Explosion;
    public GameObject FireballRange;
    public GameObject AttackObj;
    GameObject FireballRangeObj;
    float Speed = 5;
    Vector3 TargetPos;
    Vector3 Dir;
    bool IsGrounded = false;

    AudioSource ASource;
    AudioClip AClip;

    private void Awake()
    {
        ASource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Dir=TargetPos-gameObject.transform.position;
        Dir.Normalize();

        FireballRangeObj = Instantiate(FireballRange);
        Vector3 RangePos = TargetPos;
        RangePos.y = 0.01f;
        FireballRangeObj.transform.position = RangePos;

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void FixedUpdate()
    {

        MoveUpdate();
    }

    void MoveUpdate()
    {
        if (IsGrounded)
            return;

       
        transform.position += Dir * Speed * Time.deltaTime;
        

        if (transform.position.y <= 0)
        {
            AClip = Resources.Load<AudioClip>("Sound/DragonExplosion");
            ASource.PlayOneShot(AClip);
            AttackObj.SetActive(false);
            Destroy(FireballRangeObj);
            IsGrounded = true;
            Fireball.SetActive(false);
            Explosion.SetActive(true);
            StartCoroutine(DeathCo());
        }
    }
    public void SetTargetPos(Vector3 pos)
    {
        TargetPos = pos;
    }

    IEnumerator DeathCo()
    {
        yield return new WaitForSeconds(4.0f);
        Destroy(gameObject);
    }
}
