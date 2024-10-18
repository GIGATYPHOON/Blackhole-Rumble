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


    public float fakeconsumptionregen = 10f;

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



        if (fakeconsumptionregen >= 10f)
        {
            fakeconsumptionregen = 10f;

        }
        else
        {
            fakeconsumptionregen += 2f * Time.deltaTime;
        }

        representation.transform.localScale = Vector3.one * (consumptionregen.Value / 10f);



        if (fakeconsumptionregen <= 0.2f)
        {

            if (IsOwner)
            {
                //print("really fuckwad");

                consumptionregen.Value = 0;


            }

        }


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



    //private void OnTriggerEnter(Collider other)
    //{

    //    if (other.tag == "Player" && isreadyforconsumption.Value == true)
    //    {

    //        if (other.GetComponent<UniversalEntityProperties>().HP.Value < other.GetComponent<UniversalEntityProperties>().BaseHP.Value)
    //        {


    //            if (IsHost)
    //            {
    //                print("really fuckwad");
    //                other.GetComponent<UniversalEntityProperties>().Heal(25);

    //                consumptionregen.Value = 0;

    //            }




    //        }


    //    }

    //}




    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Player" && fakeconsumptionregen>= 10f)
        {

            if (other.GetComponent<UniversalEntityProperties>().HP.Value < other.GetComponent<UniversalEntityProperties>().BaseHP.Value)
            {

                
                other.GetComponent<UniversalEntityProperties>().Heal(25f);





                fakeconsumptionregen = 0;

                if (IsOwner)
                {
                    //print("really fuckwad");

                    consumptionregen.Value = 0;


                }
            }


        }

    }









}
