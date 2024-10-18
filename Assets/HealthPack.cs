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

    public NetworkVariable<GameObject> closestguy = new NetworkVariable<GameObject>(null, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    void Start()
    {
        if(IsOwner)
            GetComponent<NetworkObject>().ChangeOwnership(OwnerClientId);
    }

    // Update is called once per frame
    void Update()
    {

        if(IsOwner)

          Replenish();



        representation.transform.localScale = Vector3.one * (consumptionregen.Value / 10f);


        if (IsOwner)
        {
            closestguy.Value = FindClosestPlayer();

            if(Vector3.Distance(closestguy.Value.transform.position, this.transform.position) < 4f && closestguy.Value.GetComponent<NetworkObject>().OwnerClientId != OwnerClientId)
            {
                GetComponent<NetworkObject>().ChangeOwnership(closestguy.Value.GetComponent<NetworkObject>().OwnerClientId);
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



    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Player" && consumptionregen.Value >= 10f)
        {

            if (other.GetComponent<UniversalEntityProperties>().HP.Value < other.GetComponent<UniversalEntityProperties>().BaseHP.Value)
            {








            }


        }
        else if(other.tag == "Player" && consumptionregen.Value == 0f)
        {
         //   other.GetComponent<UniversalEntityProperties>().Heal(25f);

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
