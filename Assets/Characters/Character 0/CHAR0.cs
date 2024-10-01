using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class CHAR0 : NetworkBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject ObjectsToFlip;

    [SerializeField] GameObject SelfCamera;



    string CurrentStance = "Time Stance";

    public NetworkVariable<bool> CurrentStanceBool = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    //bool CurrentStanceBool = false;


    [SerializeField] GameObject StanceText;


    [SerializeField] GameObject HorizonStrikesObject;


    public NetworkVariable<bool> HorizonStriking = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    //bool HorizonStriking = false;

    [SerializeField] GameObject SingularitySpherePrefab;


    float SingularityCooldown = 0f;


    [SerializeField] GameObject DeadIndicator;


    void Start()
    {
        if (!IsOwner)
            return;
        SelfCamera.SetActive(true);


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

        if (HorizonStriking.Value == true)
        {
            GetComponent<Animator>().Play("HorizonStrikes");
        }

        Dead();


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
        if (Input.GetButtonDown("Special1"))
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
            HorizonStrikesObject.transform.GetChild(0).GetComponent<CHAR0Attacks>().SetDMGMulti(1.4f);
        }
        else
        {
            HorizonStrikesObject.transform.localScale = new Vector3(2f, 1f, 1f);
            HorizonStrikesObject.transform.GetChild(0).GetComponent<CHAR0Attacks>().SetDMGMulti(1f);
        }




        StanceText.GetComponent<TextMeshProUGUI>().text = CurrentStance;

    }


    void PrimarySecondary()
    {
        if(Input.GetButton("Fire1"))
        {

            HorizonStriking.Value = true;

            
        }
        else
        {

            HorizonStriking.Value = false;
        }




        if (Input.GetButton("Fire2") && SingularityCooldown <= 0 && HorizonStriking.Value == false)
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

        TheSphere.GetComponent<CHAR0Attacks>().SetOwner(this.gameObject);




        if (CurrentStance == "Space Stance")
        {
            SingularityCooldown = 4f;

            TheSphere.transform.localScale = Vector3.one * 2;

            TheSphere.GetComponent<CHAR0SingularitySphereScript>().WhenDoIDestroyMyself = 1f;

            TheSphere.GetComponent<CHAR0Attacks>().SetDMGMulti(1.2f);

        }
        else
        {
            SingularityCooldown = 2f;

            TheSphere.GetComponent<CHAR0Attacks>().SetDMGMulti(0.8f);
        }

    }



    void Dead()
    {
        if(GetComponent<UniversalEntityProperties>().dead)
        {
            this.enabled = false;
            DeadIndicator.SetActive(true);
        }
    }


}
