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

    [SerializeField] LayerMask m_LayerMask;

    public List< GameObject> healtargets;


    void Start()
    {
        consumptionregenbool.OnValueChanged += CanHealOnValueChanged;


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


        representation.transform.localScale = Vector3.one * (consumptionregen.Value / 10f);


        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale, Quaternion.identity, m_LayerMask);



        foreach (Collider healtarget in hitColliders)
        {
            if(!healtargets.Contains(healtarget.gameObject) && (healtarget.GetComponent<UniversalEntityProperties>().HP.Value < healtarget.GetComponent<UniversalEntityProperties>().BaseHP.Value))
            {

                healtargets.Add(healtarget.gameObject);
            }

        }


        if (consumptionregenbool.Value == true && healtargets.Count > 0)
        {

            if (IsOwner)
            {


                consumptionregen.Value = 0;
                consumptionregenbool.Value = false;
            }



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



    public void CanHealOnValueChanged(bool previous, bool current)
    {


        if(consumptionregenbool.Value == false)
        {
            foreach(GameObject healtarget in healtargets)
            {

                Heal(healtarget);

            }

        }
        else
        {
            healtargets.Clear();



        }

    }

    void Heal(GameObject other)
    {
        if(other!= null)
        {
            other.GetComponent<UniversalEntityProperties>().Heal(25f);

        }

    }







}
