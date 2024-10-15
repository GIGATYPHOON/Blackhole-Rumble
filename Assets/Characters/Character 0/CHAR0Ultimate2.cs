using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHAR0Ultimate2 : MonoBehaviour
{
    [SerializeField]
    private GameObject owner;



    [SerializeField]
    List<GameObject> playersinult;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        foreach(GameObject dumbidiot in playersinult.ToArray())
        {
            if (owner.GetComponent<UniversalEntityProperties>().TeamInt.Value != dumbidiot.GetComponent<UniversalEntityProperties>().TeamInt.Value && dumbidiot.GetComponent<UniversalEntityProperties>().dead.Value == false)
            {
                dumbidiot.gameObject.GetComponent<UniversalEntityProperties>().hitloc = dumbidiot.gameObject.GetComponent<Collider>().ClosestPoint(this.transform.position);


                float dmgmultiplier = 1;

                dmgmultiplier = 1 + (1 - (dumbidiot.GetComponent<UniversalEntityProperties>().HP.Value / dumbidiot.GetComponent<UniversalEntityProperties>().BaseHP.Value));

                dmgmultiplier = Mathf.Pow(dmgmultiplier, 3f);

                dumbidiot.gameObject.GetComponent<UniversalEntityProperties>().TakeDamage(owner, 2f * dmgmultiplier, 0f, 0f, 3f, owner.transform.position, "CHAR0Ultimate", 1);





                Vector3 forceDirection = transform.position - dumbidiot.transform.position;

                // apply force on target towards me
                dumbidiot.GetComponent<Rigidbody>().AddForce(forceDirection.normalized * 2300f * dmgmultiplier * Time.deltaTime, ForceMode.Acceleration);

            }

            if(dumbidiot.GetComponent<UniversalEntityProperties>().dead.Value == true)
            {
                playersinult.Remove(dumbidiot.gameObject);

            }


        }

    }

    private void OnDisable()
    {
        playersinult.Clear();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            if (!playersinult.Contains(collision.gameObject))
            {
                playersinult.Add(collision.gameObject);
            }
        }

    }

    private void OnTriggerStay(Collider collision)
    {

        if (collision.tag == "Player")
        {
   


            if(!playersinult.Contains( collision.gameObject))
            {
                playersinult.Add( collision.gameObject);
            }


        }


    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Player")
        {
            if (playersinult.Contains(collision.gameObject))
            {
                playersinult.Remove(collision.gameObject);
            }
        }
    }

}
