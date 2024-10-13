using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHAR0Attacks : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject owner;

    float dmg = 1;

    float invincibilitytimer = 5f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {

        if(collision.tag == "Player")
        {
            if (owner.GetComponent<UniversalEntityProperties>().TeamInt.Value != collision.GetComponent<UniversalEntityProperties>().TeamInt.Value)
            {
                collision.gameObject.GetComponent<UniversalEntityProperties>().hitloc = collision.gameObject.GetComponent<Collider>().ClosestPoint(this.transform.position);

                collision.gameObject.GetComponent<UniversalEntityProperties>().TakeDamage(owner, 10f * dmg, 10f, 0f, invincibilitytimer, owner.transform.position, "CHAR0Attack", 1);


                owner.GetComponent<CHAR0>().GainUltimateCharge();


            }


        }


    }


    public void SetOwner(GameObject ownerthing)
    {
        owner = ownerthing;

    }


    public void SetDMGMulti(float multiplier)
    {
        dmg = multiplier;
    }


    public void SetInvincibilityTimer(float multiplier)
    {
        invincibilitytimer= multiplier;
    }
}
