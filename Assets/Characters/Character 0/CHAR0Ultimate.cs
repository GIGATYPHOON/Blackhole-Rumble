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

                Vector3 forceDirection = transform.position - collision.transform.position;

                // apply force on target towards me
                collision.GetComponent<Rigidbody>().AddForce(forceDirection.normalized * 3000f * Time.fixedDeltaTime, ForceMode.Force);

            }


        }


    }


}
