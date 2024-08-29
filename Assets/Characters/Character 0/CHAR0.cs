using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class CHAR0 : NetworkBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject ObjectsToFlip;



    string CurrentStance = "Time Stance";

    public NetworkVariable<bool> CurrentStanceBool = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    //bool CurrentStanceBool = false;


    [SerializeField] GameObject StanceText;


    [SerializeField] GameObject HorizonStrikesObject;

    bool HorizonStriking = false;

    [SerializeField] GameObject SingularitySpherePrefab;


    float SingularityCooldown = 0f;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<UniversalEntityProperties>().isFacingRight.Value == true)
        {
            ObjectsToFlip.transform.localScale = Vector3.one;
        }
        else
        {
            ObjectsToFlip.transform.localScale = new Vector3(-1, 1, 1);
        }

        StancesMultiplayer();



        if (!IsOwner)
            return;
        Facing();

        PrimarySecondary();

        Stances();

    }



    void Facing()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            GetComponent<UniversalEntityProperties>().isFacingRight.Value = true;
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            GetComponent<UniversalEntityProperties>().isFacingRight.Value = false;
        }





    }




    void Stances()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            CurrentStanceBool.Value = !CurrentStanceBool.Value;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            CurrentStanceBool.Value = !CurrentStanceBool.Value;
        }

    }


    void StancesMultiplayer()
    {
        if (CurrentStanceBool.Value == true)
        {
            CurrentStance = "Space Stance";
        }
        else
        {
            CurrentStance = "Time Stance";
        }




        if (CurrentStance == "Time Stance")
        {
            GetComponent<Animator>().SetFloat("AnimMultiplier", 2.2f);
        }
        else
        {
            GetComponent<Animator>().SetFloat("AnimMultiplier", 1f);
        }


        if (CurrentStanceBool.Value == true)
        {
            HorizonStrikesObject.transform.localScale = new Vector3(5f, 1.5f, 1f);
        }
        else
        {
            HorizonStrikesObject.transform.localScale = new Vector3(2f, 1f, 1f);
        }




        StanceText.GetComponent<TextMeshProUGUI>().text = CurrentStance;

    }


    void PrimarySecondary()
    {
        if(Input.GetButton("Fire1"))
        {


            GetComponent<Animator>().Play("HorizonStrikes");
        }


        else if (Input.GetButton("Fire2") && SingularityCooldown <= 0)
        {


            GetComponent<Animator>().Play("SingularitySphere");


        }

        if(SingularityCooldown >0)
        {
            SingularityCooldown -= 6f * Time.deltaTime;
        }

    }


    void SingularitySphere()
    {
        GameObject TheSphere = Instantiate(SingularitySpherePrefab, HorizonStrikesObject.transform.position, Quaternion.identity);

        if(GetComponent<UniversalEntityProperties>().isFacingRight.Value == true)
        {
            TheSphere.GetComponent<Rigidbody>().AddForce(Vector3.right * 50f, ForceMode.VelocityChange);
            //TheSphere.GetComponent<ConstantForce>().force = Vector3.right * 50f;
        }
        else
        {
            TheSphere.GetComponent<Rigidbody>().AddForce(Vector3.left * 50f, ForceMode.VelocityChange);
            //TheSphere.GetComponent<ConstantForce>().force = Vector3.left * 50f;
        }





        if (CurrentStance == "Space Stance")
        {
            SingularityCooldown = 4f;

            TheSphere.transform.localScale = Vector3.one * 2;

            TheSphere.GetComponent<CHAR0SingularitySphereScript>().WhenDoIDestroyMyself = 1f;

        }
        else
        {
            SingularityCooldown = 2f;
        }

    }



    void EmptySpace()
    {

    }

}