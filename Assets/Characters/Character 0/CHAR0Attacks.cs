using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class CHAR0Attacks : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject owner;

    float dmg = 1;

    float invincibilitytimer = 5f;


    [SerializeField]
    private bool ismelee;


    int counter = 0;

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

                float damagetodo = 0;

                damagetodo = 15f * dmg;

                Mathf.Floor(damagetodo);

                collision.gameObject.GetComponent<UniversalEntityProperties>().TakeDamage(owner, damagetodo, 10f, 0f, invincibilitytimer, owner.transform.position, "CHAR0Attack", 2);


                if(ismelee)
                {

                    if (owner.GetComponent<CHAR0>().CaughtInAttacksCounter == 0)
                        owner.GetComponent<CHAR0>().GainUltimateCharge();

                    owner.GetComponent<CHAR0>().CaughtInAttacksCounter += 1;

                }
                else
                {
                    owner.GetComponent<CHAR0>().GainUltimateCharge();
                    Destroy(this.gameObject);
                }




            }


        }


    }



    void Counter0()
    {
        counter = 0;

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
