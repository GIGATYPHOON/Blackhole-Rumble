using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class UniversalEntityProperties : NetworkBehaviour
{
    // Start is called before the first frame update


    public NetworkVariable<bool> isFacingRight = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<string> TeamString = new NetworkVariable<string>("none", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField] GameObject healthbar;

    void Start()
    {

        TeamString.Value = GameObject.FindGameObjectWithTag("TeamButton").transform.GetChild(0).GetComponent<TMP_Text>().text;

        print(TeamString.Value);




        if (TeamString == )



        if (IsOwner)
        {
            healthbar.GetComponent<SpriteRenderer>().color = Color.green;

            healthbar.transform.GetChild(0).gameObject.SetActive(true);


            healthbar.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;


        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
