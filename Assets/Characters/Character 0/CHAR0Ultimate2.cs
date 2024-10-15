using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHAR0Ultimate2 : MonoBehaviour
{
    [SerializeField]
    private GameObject owner;


    public LayerMask m_LayerMask;

    [SerializeField]
    List<GameObject> playersinult;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, transform.localScale.x / 2, m_LayerMask);


        //foreach (GameObject stupid in GameObject.FindGameObjectsWithTag("Player"))
        //{

        //    Physics.IgnoreCollision(owner.gameObject.GetComponent<BoxCollider>(), stupid.gameObject.GetComponent<BoxCollider>(), false);

        //}


        foreach (Collider dumbidiot in hitColliders)
        {
            if (owner.GetComponent<UniversalEntityProperties>().TeamInt.Value != dumbidiot.GetComponent<UniversalEntityProperties>().TeamInt.Value && dumbidiot.GetComponent<UniversalEntityProperties>().dead.Value == false)
            {
                dumbidiot.gameObject.GetComponent<UniversalEntityProperties>().hitloc = dumbidiot.gameObject.GetComponent<Collider>().ClosestPoint(this.transform.position);


                float pullmultiplier = 1;

                pullmultiplier = 1 + (1 - (dumbidiot.GetComponent<UniversalEntityProperties>().HP.Value / dumbidiot.GetComponent<UniversalEntityProperties>().BaseHP.Value));


                pullmultiplier = Mathf.Pow(pullmultiplier, 4f);

                pullmultiplier = Mathf.Floor(pullmultiplier);



                float dmgmultiplier = 1;

                dmgmultiplier = 13f - Vector3.Distance(this.transform.position, dumbidiot.transform.position);

                dmgmultiplier = dmgmultiplier / 2.5f;


                print(dmgmultiplier);


                dmgmultiplier = Mathf.Ceil(dmgmultiplier);


                dumbidiot.gameObject.GetComponent<UniversalEntityProperties>().TakeDamage(owner, 1f * dmgmultiplier, 0f, 0f, 2f, owner.transform.position, "CHAR0Ultimate", 1);
                //if (dumbidiot.GetComponent<UniversalEntityProperties>().HP.Value > dmgmultiplier)
                //{


                //}

                if (Vector3.Distance(this.transform.position, dumbidiot.transform.position) < 3f)
                {

                    dumbidiot.gameObject.GetComponent<UniversalEntityProperties>().TakeDamage(owner, 1f * dmgmultiplier, 0f, 0f, 2f, owner.transform.position, "CHAR0Ultimate", 1);
                }




                //Physics.IgnoreCollision(owner.gameObject.GetComponent<BoxCollider>(), dumbidiot.gameObject.GetComponent<BoxCollider>(),true);





                Vector3 forceDirection = transform.position - dumbidiot.transform.position;

                // apply force on target towards me

                if (Vector3.Distance(this.transform.position, dumbidiot.transform.position) > 3f)
                {
                    dumbidiot.GetComponent<Rigidbody>().AddForce(forceDirection.normalized * 1500f * pullmultiplier * Time.deltaTime, ForceMode.Acceleration);
                }


            }


        }

    }

}
