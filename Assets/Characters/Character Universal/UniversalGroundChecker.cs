using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalGroundChecker : MonoBehaviour
{
    // Start is called before the first frame update

    public bool onGround;

    public string whatisfloor;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

       // print(whatisfloor);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 8)
        {
            onGround = true;

            whatisfloor = collision.gameObject.tag;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 8)
        {
            onGround = false;

            whatisfloor = "";
        }
    }
}
