using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using TMPro;


public class UniversalPlayerScript : NetworkBehaviour
{
    // Start is called before the first frame update

    public NetworkVariable<FixedString32Bytes> Name = new NetworkVariable<FixedString32Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<char> TheTeam = new NetworkVariable<char>('N', NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public Color LColor;

    public Color RColor;

    [SerializeField] List<GameObject> Characters;


    public GameObject PlayerCharacter = null;


    void Start()
    {

        if (GameObject.FindGameObjectWithTag("PreGameCanvas").transform.GetChild(5).transform.GetChild(0).GetComponent<TMP_Text>().text == "Evan and Riza")
        {
            PlayerCharacter = Characters[0];

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (TheTeam.Value == 'L')
        {

            LColor = new Color(0.2f, 0.2f, 1, 1f);
            RColor = new Color(1, 0.2f, 0.2f, 1f);
        }
        else
        {
            RColor = new Color(0.2f, 0.2f, 1, 1f);
            LColor = new Color(1, 0.2f, 0.2f, 1f);
        }




    }

    public override void OnNetworkSpawn()
    {


        if (IsOwner)
        {
            if (GameObject.FindGameObjectWithTag("TeamButton").transform.GetChild(0).GetComponent<TMP_Text>().text == "L")
            {

                TheTeam.Value = 'L';
                this.transform.position = GameObject.FindGameObjectWithTag("LSpawn").transform.position;




            }
            else
            {
                TheTeam.Value = 'R';

                this.transform.position = GameObject.FindGameObjectWithTag("RSpawn").transform.position;

            }


            if (GameObject.FindGameObjectWithTag("PreGameCanvas").transform.GetChild(5).transform.GetChild(0).GetComponent<TMP_Text>().text == "Evan and Riza")
            {
                PlayerCharacter = Characters[0];

            }


            MP_CreatePlayerServerRpc(NetworkManager.LocalClientId);

            //YourTeam.Value = TeamInt.Value;

            //healthbar.GetComponent<SpriteRenderer>().color = Color.green;

            //youindicator.gameObject.SetActive(true);




            //GameObject.FindGameObjectWithTag("PreGameCanvas").SetActive(false);

        }
        else
        {


        }


        base.OnNetworkSpawn();


    }


    [ServerRpc(RequireOwnership = false)] //server owns this object but client can request a spawn
    public void MP_CreatePlayerServerRpc(ulong clientId)
    {
        if (PlayerCharacter != null)
        {
            var instance = Instantiate(PlayerCharacter);

            // instance = (GameObject)Instantiate(instance);
            NetworkObject netObj = instance.GetComponent<NetworkObject>();
            instance.SetActive(true);
            netObj.SpawnWithOwnership(clientId, true);

        }

    }

}
