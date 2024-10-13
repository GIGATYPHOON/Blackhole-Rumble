using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHAR0Ultimate : MonoBehaviour
{
    [SerializeField]
    private GameObject owner;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider collision)
    {

        if (collision.tag == "Player")
        {
            if (owner.GetComponent<UniversalEntityProperties>().TeamInt.Value != collision.GetComponent<UniversalEntityProperties>().TeamInt.Value)
            {



            }


        }


    }


}
