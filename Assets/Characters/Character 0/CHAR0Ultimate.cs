using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHAR0Ultimate : MonoBehaviour
{
    [SerializeField]
    private GameObject owner;


    public float multiplier = 1;

    public float size = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay(Collider collision)
    {

        if (collision.tag == "Player")
        {
            if (owner.GetComponent<UniversalEntityProperties>().TeamInt.Value != collision.GetComponent<UniversalEntityProperties>().TeamInt.Value)
            {
                collision.gameObject.GetComponent<UniversalEntityProperties>().hitloc = collision.gameObject.GetComponent<Collider>().ClosestPoint(this.transform.position);


                float dmgmultiplier = 1;

                dmgmultiplier = 1 + (1-(collision.GetComponent<UniversalEntityProperties>().HP.Value / collision.GetComponent<UniversalEntityProperties>().BaseHP.Value));

                dmgmultiplier = Mathf.Pow(dmgmultiplier, 2f);

                collision.gameObject.GetComponent<UniversalEntityProperties>().TakeDamage(owner, 2f * dmgmultiplier, 0f, 0f, 3f, owner.transform.position, "CHAR0Ultimate", 1);





                    Vector3 forceDirection = transform.position - collision.transform.position;

                // apply force on target towards me
                collision.GetComponent<Rigidbody>().AddForce(forceDirection.normalized * 3000f * dmgmultiplier* Time.fixedDeltaTime, ForceMode.Force);

            }


        }


    }


}
