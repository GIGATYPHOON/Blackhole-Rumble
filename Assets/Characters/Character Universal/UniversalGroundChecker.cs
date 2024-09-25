using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalGroundChecker : MonoBehaviour
{
    // Start is called before the first frame update

    public bool onGround;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 9)
        {
            onGround = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 9)
        {
            onGround = false;
        }
    }
}
