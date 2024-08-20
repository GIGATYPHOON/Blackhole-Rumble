using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHAR0SingularitySphereScript : MonoBehaviour
{
    // Start is called before the first frame update

    public float WhenDoIDestroyMyself = 0.333f;


    void Start()
    {
        Destroy(this.gameObject, WhenDoIDestroyMyself);
    }

    // Update is called once per frame
    void Update()
    {
        
    }





}
