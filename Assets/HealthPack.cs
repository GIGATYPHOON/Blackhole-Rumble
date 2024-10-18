using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HealthPack : NetworkBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject representation;

  //  bool isreadyforconsumption = true;

    public NetworkVariable<bool> isreadyforconsumption = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> consumptionregen = new NetworkVariable<float>(10, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if(IsOwner)
          Replenish();




        representation.transform.localScale = Vector3.one * (consumptionregen.Value / 10f);



    }


    void Replenish()
    {

        if (consumptionregen.Value >= 10f)
        {
            consumptionregen.Value = 10f;
            isreadyforconsumption.Value = true;
        }
        else
        {
            consumptionregen.Value += 2f * Time.deltaTime;
            isreadyforconsumption.Value = false;
        }


    }


    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Player" && isreadyforconsumption.Value == true)
        {

            if (other.GetComponent<UniversalEntityProperties>().HP.Value < other.GetComponent<UniversalEntityProperties>().BaseHP.Value)
            {


                if(IsOwner)
                {
                    other.GetComponent<UniversalEntityProperties>().Heal(25);
                    consumptionregen.Value = 0;

                }





            }


        }

    }
}
