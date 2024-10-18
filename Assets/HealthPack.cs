using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;

public class HealthPack : NetworkBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject representation;

    bool isreadyforconsumption = true;


    public NetworkVariable<float> consumptionregen = new NetworkVariable<float>(10, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    void Start()
    {
        GetComponent<NetworkObject>().ChangeOwnership(OwnerClientId);
    }

    // Update is called once per frame
    void Update()
    {


        Replenish();


        representation.transform.localScale = Vector3.one * (consumptionregen.Value / 10f);



    }


    void Replenish()
    {

        if(IsOwner)
        {


            if (consumptionregen.Value >= 10f)
            {
                consumptionregen.Value = 10f;
                isreadyforconsumption = true;
            }
            else
            {
                consumptionregen.Value += 2f * Time.deltaTime;
                isreadyforconsumption = false;
            }

        }


    }


    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Player" && isreadyforconsumption)
        {

            if (other.GetComponent<UniversalEntityProperties>().HP.Value < other.GetComponent<UniversalEntityProperties>().BaseHP.Value)
            {
                other.GetComponent<UniversalEntityProperties>().Heal(25);
                if(IsOwner)
                consumptionregen.Value = 0;

            }


        }

    }
}
