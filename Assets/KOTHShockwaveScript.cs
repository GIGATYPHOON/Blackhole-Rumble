using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KOTHShockwaveScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {






            Vector3 forceDirection = transform.position - other.transform.position;

            // apply force on target towards me

            if(other.GetComponent<UniversalEntityProperties>().dead.Value== false)
            {

                other.GetComponent<Rigidbody>().AddForce(forceDirection.normalized * -112000f * Time.deltaTime, ForceMode.Force);
            }

        }



    }
}
