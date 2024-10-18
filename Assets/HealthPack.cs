using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;

using UnityEngine;

public class HealthPack : NetworkBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject representation;

  //  bool isreadyforconsumption = true;

    public NetworkVariable<float> fakeconsumptionregen = new NetworkVariable<float>(10, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    float consumptionregen = 10f;

    bool cantake = true;


    void Start()
    {
        if(IsOwner)
            GetComponent<NetworkObject>().ChangeOwnership(OwnerClientId);
    }

    // Update is called once per frame
    void Update()
    {

        if (consumptionregen >= 10f)
        {
            consumptionregen = 10f;

        }
        else
        {
            consumptionregen += 2f * Time.deltaTime;
        }

        if (IsOwner)
          Replenish();


        representation.transform.localScale = Vector3.one * (consumptionregen / 10f);


        //closestguy = FindClosestPlayer();

        //if (closestguy.GetComponent<NetworkObject>().OwnerClientId != OwnerClientId)
        //{
        //    GetComponent<NetworkObject>().ChangeOwnership(closestguy.GetComponent<NetworkObject>().OwnerClientId);
        //}




    }


    void Replenish()
    {

        if (fakeconsumptionregen.Value >= 10f)
        {
            fakeconsumptionregen.Value = 10f;

        }
        else
        {
            fakeconsumptionregen.Value += 2f * Time.deltaTime;
        }


    }



    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Player" && consumptionregen>= 10f)
        {

            if (other.GetComponent<UniversalEntityProperties>().HP.Value < other.GetComponent<UniversalEntityProperties>().BaseHP.Value)
            {
                if (IsOwner)
                {


                    fakeconsumptionregen.Value = 0;
                }


                other.GetComponent<UniversalEntityProperties>().Heal(25f);
                consumptionregen = 0f;






            }


        }


    }




    public GameObject FindClosestPlayer()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }






}
