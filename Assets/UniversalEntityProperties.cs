using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UniversalEntityProperties : NetworkBehaviour
{
    // Start is called before the first frame update


    public NetworkVariable<bool> isFacingRight = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
