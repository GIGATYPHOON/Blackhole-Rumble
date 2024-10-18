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

    bool cantake = true;

    GameObject specialguest;


    void Start()
    {
        consumptionregen.OnValueChanged += (previousValue, newValue) => WHAT_THE_FUCK(specialguest);


        if (IsOwner)
            GetComponent<NetworkObject>().ChangeOwnership(OwnerClientId);
    }

    // Update is called once per frame
    void Update()
    {


        if (IsOwner)
        {
            Replenish();

        }

        if(consumptionregen.Value > 9f && consumptionregen.Value < 10f)
        {
            cantake = true;
        }


        representation.transform.localScale = Vector3.one * (consumptionregen.Value / 10f);


        //closestguy = FindClosestPlayer();

        //if (closestguy.GetComponent<NetworkObject>().OwnerClientId != OwnerClientId)
        //{
        //    GetComponent<NetworkObject>().ChangeOwnership(closestguy.GetComponent<NetworkObject>().OwnerClientId);
        //}


        if(consumptionregen.Value < 10f)
        {
            consumptionregen.OnValueChanged -= (previousValue, newValue) => WHAT_THE_FUCK(specialguest);

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



    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Player" && cantake == true)
        {

            if (other.GetComponent<UniversalEntityProperties>().HP.Value < other.GetComponent<UniversalEntityProperties>().BaseHP.Value)
            {


                if (IsOwner)
                {


                    consumptionregen.Value = 0;
                }

                specialguest = other.gameObject;

                cantake = false;


            }


        }


    }

    public void WHAT_THE_FUCK(GameObject other)
    {
        if(other!= null)
        {
            other.GetComponent<UniversalEntityProperties>().Heal(25f);
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
