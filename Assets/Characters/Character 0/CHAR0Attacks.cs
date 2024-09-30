using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHAR0Attacks : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject owner;

    bool teamcheck;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {

        try
        {
            if (owner.GetComponent<UniversalEntityProperties>().TeamInt != collision.GetComponent<UniversalEntityProperties>().TeamInt)
            {
                collision.gameObject.GetComponent<UniversalEntityProperties>().hitloc = collision.gameObject.GetComponent<Collider>().ClosestPoint(this.transform.position);


                collision.gameObject.GetComponent<UniversalEntityProperties>().TakeDamage(owner, 20f, 10f, 0f, 5f, owner.transform.position, "CHAR0Attack", 1);

            }
        }
        catch
        {

        }





    }
}
