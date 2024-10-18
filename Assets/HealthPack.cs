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

    public NetworkVariable<bool> consumptionregenbool = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    bool cantake = true;

    public List< GameObject> specialguests;


    public NetworkVariable<NetworkObjectReference> specialguest = new NetworkVariable<NetworkObjectReference>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    void Start()
    {
        consumptionregenbool.OnValueChanged += WHAT_THE_FUCK;


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
       //     consumptionregen.OnValueChanged -= (previousValue, newValue);

        }



    }


    void Replenish()
    {

        if (consumptionregen.Value >= 10f)
        {
            consumptionregen.Value = 10f;
            consumptionregenbool.Value = true;

        }
        else
        {
           consumptionregen.Value += 2f * Time.deltaTime;
        }


    }



    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Player" && consumptionregenbool.Value == true)
        {
            if (!specialguests.Contains(other.gameObject))
            {
                specialguests.Add(other.gameObject);
            }




            if (other.GetComponent<UniversalEntityProperties>().HP.Value < other.GetComponent<UniversalEntityProperties>().BaseHP.Value)
            {



                if (IsOwner)
                {


                    consumptionregen.Value = 0;
                    consumptionregenbool.Value = false;
                }


                cantake = false;


            }


        }


    }

    public void WHAT_THE_FUCK(bool previous, bool current)
    {
        //if(other!= null)
        //{
        //    other.GetComponent<UniversalEntityProperties>().Heal(25f);
        //}    

        if(consumptionregenbool.Value == false)
        {
            GOD_HATES_ROVERS(FindClosestPlayer());


            specialguests.Clear();
        }

    }

    void GOD_HATES_ROVERS(GameObject other)
    {
        if(other!= null)
        {
            other.GetComponent<UniversalEntityProperties>().Heal(25f);

        }

    }




    public GameObject FindClosestPlayer()
    {
        GameObject[] gos;
        gos = specialguests.ToArray();
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
