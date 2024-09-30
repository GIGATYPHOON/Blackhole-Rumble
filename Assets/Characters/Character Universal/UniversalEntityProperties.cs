using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class UniversalEntityProperties : NetworkBehaviour
{
    // Start is called before the first frame update


    public NetworkVariable<bool> isFacingRight = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<int> YourTeam = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<int> TeamInt = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField] GameObject healthbar;



    void Start()
    {






        if (!IsOwner)
            return;






    }

    // Update is called once per frame
    void Update()
    {



        print(TeamInt.Value);



        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);

        if(!IsOwner)
        {
            if (TeamInt.Value == NetworkManager.LocalClient.PlayerObject.GetComponent<UniversalEntityProperties>().YourTeam.Value)
            {
                healthbar.GetComponent<SpriteRenderer>().color = Color.blue;
            }
            else
            {

                healthbar.GetComponent<SpriteRenderer>().color = Color.red;
            }

        }

    }


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();


        if(IsOwner)
        {
            if (GameObject.FindGameObjectWithTag("TeamButton").transform.GetChild(0).GetComponent<TMP_Text>().text == "L")
            {
               TeamInt.Value = 0;
                this.transform.position = GameObject.FindGameObjectWithTag("LSpawn").transform.position;

            }
            else
            {
                TeamInt.Value = 1;
                this.transform.position = GameObject.FindGameObjectWithTag("RSpawn").transform.position;
            }

            YourTeam.Value = TeamInt.Value;

            healthbar.GetComponent<SpriteRenderer>().color = Color.green;

            healthbar.transform.GetChild(0).gameObject.SetActive(true);


            healthbar.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;


            GameObject.FindGameObjectWithTag("PreGameCanvas").SetActive(false);

        }
        else
        {


        }
    }
}
