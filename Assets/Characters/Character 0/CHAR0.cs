using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class CHAR0 : NetworkBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject ObjectsToFlip;



    string CurrentStance = "Time";

    public NetworkVariable<bool> CurrentStanceBool = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    //bool CurrentStanceBool = false;

    public NetworkVariable<float> StanceCooldown = new NetworkVariable<float>(10f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField] GameObject StanceBar;

    [SerializeField] GameObject StanceText;


    [SerializeField] GameObject HorizonStrikesObject;


    public NetworkVariable<bool> HorizonStriking = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    //bool HorizonStriking = false;

    [SerializeField] GameObject SingularitySpherePrefab;


    float SingularityCooldown = 0f;


    [SerializeField] GameObject DeadIndicator;

    public NetworkVariable<float> UltimateCharge = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField] GameObject UltimateChargeIndicator;

    [SerializeField] GameObject Eventus;


    NetworkVariable<bool> EventusMode = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    public int CaughtInAttacksCounter = 0;


    public float shielddecreasefloat = 0f;

    public int shielddecreaseincrement = 0;


    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        Dead();
        Ultimate();

        if (GetComponent<UniversalEntityProperties>().dead.Value == false)
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



            StanceBar.transform.localScale = new Vector3(StanceCooldown.Value / 10f, 1, 1);


            if (!IsOwner)
                return;
            Facing();

            PrimarySecondary();


            Stances();


        }




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
        if(StanceCooldown.Value <10f)
        {
            StanceCooldown.Value += 2f * Time.deltaTime;
        }




        if (Input.GetButtonDown("Special1") && StanceCooldown.Value >= 10f)
        {
            GetComponent<UniversalEntityProperties>().Shielding(45f);
            CurrentStanceBool.Value = !CurrentStanceBool.Value;
            StanceCooldown.Value = 0;
            shielddecreaseincrement += 45;
            shielddecreasefloat += 1f;
        }

        if(shielddecreaseincrement > 0f && GetComponent<UniversalEntityProperties>().Shield.Value > 0f)
        {

            shielddecreasefloat -= 14f * Time.deltaTime;

            if(shielddecreasefloat <=0f)
            {
                GetComponent<UniversalEntityProperties>().Shielding(-1f);


                shielddecreaseincrement -= 1;
                shielddecreasefloat = 1f;
            }


        }
        else
        {
            shielddecreaseincrement = 0;
            shielddecreasefloat = 0;


        }




    }


    void StancesMultiplayer()
    {
        if (CurrentStanceBool.Value == true)
        {
            CurrentStance = "Space";
        }
        else
        {
            CurrentStance = "Time";
        }



        if (CurrentStance == "Time")
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
            HorizonStrikesObject.transform.GetChild(0).GetComponent<CHAR0Attacks>().SetInvincibilityTimer(3f);
            HorizonStrikesObject.transform.GetChild(0).GetComponent<CHAR0Attacks>().SetDMGMulti(1f);

        }
        else
        {
            HorizonStrikesObject.transform.localScale = new Vector3(2f, 1f, 1f);
            HorizonStrikesObject.transform.GetChild(0).GetComponent<CHAR0Attacks>().SetInvincibilityTimer(1.2f);

            HorizonStrikesObject.transform.GetChild(0).GetComponent<CHAR0Attacks>().SetDMGMulti(0.6f);
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




        if (CurrentStance == "Space")
        {
            SingularityCooldown = 4f;

            TheSphere.transform.localScale = Vector3.one * 2;



            TheSphere.GetComponent<CHAR0Attacks>().SetInvincibilityTimer(2.2f);

            TheSphere.GetComponent<CHAR0Attacks>().SetDMGMulti(0.9f);



        }
        else
        {
            SingularityCooldown = 2f;

            if (GetComponent<UniversalEntityProperties>().isFacingRight.Value == true)
            {
                TheSphere.GetComponent<Rigidbody>().AddForce(Vector3.right * 14f, ForceMode.VelocityChange);
                //TheSphere.GetComponent<ConstantForce>().force = Vector3.right * 50f;
            }
            else
            {
                TheSphere.GetComponent<Rigidbody>().AddForce(Vector3.left * 14f, ForceMode.VelocityChange);
                //TheSphere.GetComponent<ConstantForce>().force = Vector3.left * 50f;
            }

            TheSphere.GetComponent<CHAR0Attacks>().SetInvincibilityTimer(0.4f);

            TheSphere.GetComponent<CHAR0Attacks>().SetDMGMulti(0.45f);

        }

    }



    void Dead()
    {
        if(GetComponent<UniversalEntityProperties>().dead.Value == true)
        {
            DeadIndicator.SetActive(true);
        }
        else
        {
            DeadIndicator.SetActive(false);
        }
    }



    void Ultimate()
    {

        if(IsOwner)
        {




            if (EventusMode.Value == true)
            {
                UltimateCharge.Value -= 10f * Time.deltaTime;


            }
            else
            {

                if(GetComponent<UniversalEntityProperties>().dead.Value == false)
                {
                    if (CurrentStanceBool.Value == true)
                    {
                        UltimateCharge.Value += 1f * Time.deltaTime;
                    }
                    else
                    {
                        UltimateCharge.Value += 3f * Time.deltaTime;
                    }

                }

            }


            if (UltimateCharge.Value <= 0 || GetComponent<UniversalEntityProperties>().HP.Value <= 0)
            {
                UltimateCharge.Value = 0f;
                EventusMode.Value = false;
            }


            if (UltimateCharge.Value >= 100 || Input.GetKeyDown(KeyCode.U))
            {
                UltimateCharge.Value = 100f;
            }

            if(Input.GetKeyDown(KeyCode.E))
            {
                GetComponent<UniversalEntityProperties>().HP.Value -= 5f;;

            }


            if (Input.GetKeyDown(KeyCode.R))
            {
                GetComponent<UniversalEntityProperties>().Shield.Value += 5f;

            }




            if (Input.GetButton("Special2") && UltimateCharge.Value >= 100)
            {

                EventusMode.Value = true;

            }

        }

        if(EventusMode.Value == true)
        {
            Eventus.SetActive(true);

        }
        else
        {

            Eventus.SetActive(false);
        }


        UltimateChargeIndicator.GetComponent<TextMeshProUGUI>().text = Mathf.FloorToInt(UltimateCharge.Value) + "%";


    }


    public void GainUltimateCharge()
    {
        if(IsOwner && EventusMode.Value == false)
            UltimateCharge.Value += 8f;


    }



    void CaughtInAttacksCounterReset()
    {
        CaughtInAttacksCounter = 0;
    }


    void EmptySpace()
    {

    }
}
