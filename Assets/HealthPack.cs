using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;

using UnityEngine;

public class HealthPack : NetworkBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject representation;

  //  bool isreadyforconsumption = true;

    public NetworkVariable<float> consumptionregen = new NetworkVariable<float>(10, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    public float shortcooldown = 10f;

    void Start()
    {
        if(IsHost)
            GetComponent<NetworkObject>().ChangeOwnership(OwnerClientId);
    }

    // Update is called once per frame
    void Update()
    {

        if(IsHost)
          Replenish();



        if (shortcooldown >= 10f)
        {
            shortcooldown = 10f;

        }
        else
        {
            shortcooldown += 10f * Time.deltaTime;
        }

        representation.transform.localScale = Vector3.one * (consumptionregen.Value / 10f);




    }


    void Replenish()
    {

        if (consumptionregen.Value >= 10f)
        {
            consumptionregen.Value = 10f;

        }
        else
        {
            consumptionregen.Value += 2f * Time.deltaTime;
        }


    }



    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Player" && consumptionregen.Value >= 10f)
        {

            if (other.GetComponent<UniversalEntityProperties>().HP.Value < other.GetComponent<UniversalEntityProperties>().BaseHP.Value)
            {

                if(shortcooldown >=10f)
                {
                    other.GetComponent<UniversalEntityProperties>().Heal(25f);

                    shortcooldown = 0;
                }





                if (IsOwner)
                {

                    consumptionregen.Value = 0;
                }

            }


        }

    }









}
