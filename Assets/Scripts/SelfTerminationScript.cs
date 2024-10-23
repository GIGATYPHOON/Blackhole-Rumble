using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfTerminationScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] bool killselfnotthroughanimator = false;

    [SerializeField] float killself = 0f;


    void Start()
    {
        if(killselfnotthroughanimator)
        {
            killself -= 5f * Time.deltaTime;

            if(killself<0)
            {
                Destroy(this.gameObject);
            }


        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void Terminate()
    {
        Destroy(this.gameObject);
    }
}
