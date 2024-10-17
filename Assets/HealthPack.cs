using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HealthPack : NetworkBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject representation;

    bool isreadyforconsumption = true;


    float consumptionregen = 10f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        consumptionregen += 0.12f * Time.deltaTime;



        if (consumptionregen >= 10f)
        {
            consumptionregen = 10f;
            isreadyforconsumption = true;
        }
        else
        {
            consumptionregen += 6f * Time.deltaTime;
            isreadyforconsumption = false;
        }

        representation.transform.localScale = Vector3.one * (consumptionregen / 10f);



    }


    void Replenish()
    {




    }


    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Player" && isreadyforconsumption)
        {

            if (other.GetComponent<UniversalEntityProperties>().HP.Value < other.GetComponent<UniversalEntityProperties>().BaseHP.Value)
            {
                other.GetComponent<UniversalEntityProperties>().Heal(25);
                consumptionregen = 0;

            }


        }

    }
}
