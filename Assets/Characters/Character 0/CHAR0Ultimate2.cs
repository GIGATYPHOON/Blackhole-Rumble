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




        foreach (Collider dumbidiot in hitColliders)
        {
            if (owner.GetComponent<UniversalEntityProperties>().TeamInt.Value != dumbidiot.GetComponent<UniversalEntityProperties>().TeamInt.Value && dumbidiot.GetComponent<UniversalEntityProperties>().dead.Value == false)
            {
                dumbidiot.gameObject.GetComponent<UniversalEntityProperties>().hitloc = dumbidiot.gameObject.GetComponent<Collider>().ClosestPoint(this.transform.position);


                float dmgmultiplier = 1;

                dmgmultiplier = 1 + (1 - (dumbidiot.GetComponent<UniversalEntityProperties>().HP.Value / dumbidiot.GetComponent<UniversalEntityProperties>().BaseHP.Value));


                dmgmultiplier = Mathf.Pow(dmgmultiplier, 3f);

                dmgmultiplier = Mathf.Floor(dmgmultiplier);


                print(dmgmultiplier);


                dumbidiot.gameObject.GetComponent<UniversalEntityProperties>().TakeDamage(owner, 3f * dmgmultiplier, 0f, 0f, 3f, owner.transform.position, "CHAR0Ultimate", 1);





                Vector3 forceDirection = transform.position - dumbidiot.transform.position;

                // apply force on target towards me
                dumbidiot.GetComponent<Rigidbody>().AddForce(forceDirection.normalized * 1300f * dmgmultiplier * Time.deltaTime, ForceMode.Acceleration);

            }


        }

    }

}
