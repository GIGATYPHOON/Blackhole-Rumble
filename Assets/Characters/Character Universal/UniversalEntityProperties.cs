using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Networking.Transport;
using UnityEngine;

public class UniversalEntityProperties : NetworkBehaviour
{
    // Start is called before the first frame update


    public NetworkVariable<bool> isFacingRight = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<int> YourTeam = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    public Color LColor;

    public Color RColor;


    //0 is left, 1 is right

    public NetworkVariable<int> TeamInt = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    [SerializeField] GameObject SelfCamera;


    [SerializeField] GameObject healthbar;

    [SerializeField] GameObject healthmeasureR;


    [SerializeField] GameObject healthmeasureL;


    [SerializeField] GameObject shieldbar;

    [SerializeField] GameObject youindicator;


    public bool multihitted = false;
    public GameObject multihitdamagesource;
    public float multihitdmg;
    public float multihitknockback;
    public float multihitknockup;
    public float multihithitinvincibilityfloat;
    public Vector2 multihitdamagepoint;
    public string multihitdamagecause;
    public int multihitdamageprio;


    public NetworkVariable<float> HP = new NetworkVariable<float>(100f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> BaseHP = new NetworkVariable<float>(100f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    public NetworkVariable<float> Shield = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    public Vector2 hitloc;

    public NetworkVariable<bool> dead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public float deathtimer = 10f;


    public GameObject deathoverlay;

    public GameObject deathoverlayint;

    public GameObject objectiveoverlay;

    public bool isdamageable = true;

    public GameObject sprites;

    [SerializeField]
    private NetworkVariable<float>  hitinvincibilitytimer = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> invuln = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public bool combatcollisionoverride = false;


    public int olddamageprio = 0;
    public int currentdamageprio = 0;

    public string lastspecificcause = "";


    public NetworkVariable<bool> ghosted = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<float> healcooldowntimer = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    void Start()
    {



        if (!IsOwner)
            return;
        SelfCamera.SetActive(true);

        objectiveoverlay.SetActive(true);



    }

    // Update is called once per frame
    void Update()
    {
       

        if (TeamInt.Value == 0)
        {
            this.gameObject.layer = 3;
        }
        else if (TeamInt.Value == 1)
        {
            this.gameObject.layer = 7;
        }

        if (NetworkManager.LocalClient.PlayerObject.GetComponent<UniversalEntityProperties>().YourTeam.Value == 0)
        {

            LColor = new Color(0, 0, 1, 1f);
            RColor = new Color(1, 0, 0, 1f);
        }
        else
        {
            RColor = new Color(0, 0, 1, 1f);
            LColor = new Color(1, 0, 0, 1f);
        }




        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);

        if(!IsOwner)
        {
            if (TeamInt.Value == NetworkManager.LocalClient.PlayerObject.GetComponent<UniversalEntityProperties>().YourTeam.Value)
            {
                healthbar.GetComponent<SpriteRenderer>().color = Color.blue;
            }
            else
            {

                healthbar.GetComponent<SpriteRenderer>().color = Color.red;
            }


        }


 
        if (isdamageable == true)
        {
            if (HP.Value <= 0)
            {
                if(IsOwner)
                {
                    HP.Value = 0;
                    dead.Value = true;
                }

            }

            if(HP.Value > BaseHP.Value)
            {
                if (IsOwner)
                {
                    HP.Value = BaseHP.Value;
                }

            }

            //if (combatcollisionoverride == false)
            //{ 
            //}

            INVINCIBLEEEE();



            Healthbar();

            if (multihitted == true)
            {
                TakeDamage(multihitdamagesource, multihitdmg, multihitknockback, multihitknockup, multihithitinvincibilityfloat, multihitdamagepoint, multihitdamagecause, multihitdamageprio);
                if (hitinvincibilitytimer.Value <= 0f)
                {
                    multihitted = false;
                }


            }


        }


        DeadAndRespawn();


    }



    void INVINCIBLEEEE()
    {

        if (hitinvincibilitytimer.Value > 0f)
        {


            if(IsOwner)
            {
                hitinvincibilitytimer.Value -= 10f * Time.deltaTime;
                invuln.Value = true;
            }



            if (((hitinvincibilitytimer.Value - Mathf.Floor(hitinvincibilitytimer.Value)) * 2f) < 0.8f)
            {
                sprites.SetActive(false);
            }
            else
            {
                sprites.SetActive(true);
            }




        }
        else if (hitinvincibilitytimer.Value <= 0f)
        {
            if (IsOwner)
            {
                hitinvincibilitytimer.Value = 0f;
                invuln.Value = false;
            }




            currentdamageprio = 0;

            lastspecificcause = "";


            sprites.SetActive(true);


        }
    }




    void Healthbar()
    {
        if ( healthbar.gameObject)
        {

            if((HP.Value + Shield.Value) <= BaseHP.Value )
            {

                healthbar.transform.localScale = new Vector3((HP.Value / BaseHP.Value) * 3, 0.3333f, 1f);

                shieldbar.transform.localScale = new Vector3(((HP.Value + Shield.Value) / BaseHP.Value) * 3, 0.3333f, 1f);

                healthmeasureL.GetComponent<SpriteRenderer>().size = new Vector2(BaseHP.Value / 100f, 0.2f);

                healthmeasureL.transform.localScale = new Vector3(-1.5f / (BaseHP.Value / 100f), 1.6665f, 1);

                healthmeasureR.GetComponent<SpriteRenderer>().size = new Vector2(BaseHP.Value / 100f, 0.2f);

                healthmeasureR.transform.localScale = new Vector3(1.5f / (BaseHP.Value / 100f), 1.6665f, 1);

            }
            else
            {
                healthbar.transform.localScale = new Vector3((HP.Value  / (Shield.Value + HP.Value)) * 3, 0.3333f, 1f);

                shieldbar.transform.localScale = new Vector3((Shield.Value / (Shield.Value + 0.001f)) * 3, 0.3333f, 1f); ;


                healthmeasureL.GetComponent<SpriteRenderer>().size = new Vector2((Shield.Value + HP.Value) / 100f, 0.2f);

                healthmeasureL.transform.localScale = new Vector3(-1.5f / ((Shield.Value + HP.Value) / 100f), 1.6665f, 1);

                healthmeasureR.GetComponent<SpriteRenderer>().size = new Vector2((Shield.Value + HP.Value) / 100f, 0.2f);

                healthmeasureR.transform.localScale = new Vector3(1.5f / ((Shield.Value + HP.Value) / 100f), 1.6665f, 1);



                //healthmeasure.GetComponent<SpriteRenderer>().size = new Vector2((Shield.Value / 2) * 3, 0.1665f);
            }

            //assuming hp is 100 and shield is 100 the inteded values here are healthbar 0.5 and shield 1

            //assuming hp is 100 and shield is 50 the inteded values here are healthbar 0.666 and shield 1

            //assuming hp is 75 and shield is 100 the inteded values here are healthbar 


        }
        else
        {


        }

    }









    public override void OnNetworkSpawn()
    {


        if(IsOwner)
        {
            if (GameObject.FindGameObjectWithTag("TeamButton").transform.GetChild(0).GetComponent<TMP_Text>().text == "L")
            {
               TeamInt.Value = 0;
                this.transform.position = GameObject.FindGameObjectWithTag("LSpawn").transform.position;

                isFacingRight.Value = true;


            }
            else
            {
                TeamInt.Value = 1;
                this.transform.position = GameObject.FindGameObjectWithTag("RSpawn").transform.position;

                isFacingRight.Value = false;
            }

            YourTeam.Value = TeamInt.Value;

            healthbar.GetComponent<SpriteRenderer>().color = Color.green;

            youindicator.gameObject.SetActive(true);




            //GameObject.FindGameObjectWithTag("PreGameCanvas").SetActive(false);

        }
        else
        {


        }


        base.OnNetworkSpawn();


    }


    public void MultiPartTakeDamage(GameObject damagesource, float dmg, float knockback, float knockup, float hitinvincibilityfloat, Vector2 damagepoint, string specificcause, int damageprio)
    {
        multihitted = true;

        multihitdamagesource = damagesource;
        multihitdmg = dmg;
        multihitknockback = knockback;
        multihitknockup = knockup;
        multihithitinvincibilityfloat = hitinvincibilityfloat;
        multihitdamagepoint = damagepoint;
        multihitdamagecause = specificcause;
        multihitdamageprio = damageprio;
    }


    public void TakeDamage(GameObject damagesource, float dmg, float knockback, float knockup, float hitinvincibilityfloat, Vector2 damagepoint, string specificcause, int damageprio)
    {
        //if(damagesource == null)
        //{



        //    damagesource = this.gameObject;
        //}




        if ((invuln.Value == false || specificcause != lastspecificcause) && isdamageable == true && dead.Value == false)
        {
            //currentdamageprio = damageprio;

            lastspecificcause = specificcause;

            float leftovershielddamage = 0;



            if(IsOwner)
            {
                OnDamageTakenEffects(dmg);

                GetComponent<UniversalEntityProperties>().Shield.Value -= dmg;

                if(Shield.Value < 0)
                {
                    leftovershielddamage = -Shield.Value;
                    Shield.Value = 0;

                }



                GetComponent<UniversalEntityProperties>().HP.Value -= leftovershielddamage;

            }



            if (HP.Value > 0)
            {

                if (damagepoint.x < this.transform.position.x)
                {
                    GetComponent<Rigidbody>().AddForce(Vector2.right * knockback, ForceMode.Impulse);
                }
                else
                {
                    GetComponent<Rigidbody>().AddForce(Vector2.left * knockback, ForceMode.Impulse);
                }


                GetComponent<Rigidbody>().AddForce(Vector2.up * knockup, ForceMode.Impulse);


            }




            if(IsOwner)
              hitinvincibilitytimer.Value = hitinvincibilityfloat;



        }

    }



    void DeadAndRespawn()
    {
        if(dead.Value == true)
        {
            if (IsOwner)
            {
                GetComponent<UniversalCharacterMovement>().enabled = false;

                deathoverlay.SetActive(true);

                deathoverlayint.GetComponent<TextMeshProUGUI>().text = Mathf.FloorToInt(deathtimer) + "";

                deathtimer -= 1f * Time.deltaTime;

                GetComponent<Rigidbody>().velocity = Vector3.zero; 
            }


            GetComponent<Collider>().enabled = false;



            if(deathtimer <=0 )
            {

                if (IsOwner)
                {


                    deathoverlay.SetActive(false);

                    HP.Value = BaseHP.Value;

                    if (TeamInt.Value == 0)
                    {

                        this.transform.position = GameObject.FindGameObjectWithTag("LSpawn").transform.position;




                    }
                    else if (TeamInt.Value == 1)
                    {
                        this.transform.position = GameObject.FindGameObjectWithTag("RSpawn").transform.position;

                    }


                    deathtimer = 10f;

                    GetComponent<UniversalCharacterMovement>().enabled = true;




                    dead.Value = false;

                    OnRespawnEffects();

                }



            }
        }

        else
        {

            GetComponent<Collider>().enabled = true;
        }


    }



    public void Heal(float healamount)
    {
        if(IsOwner)
        {
            GetComponent<UniversalEntityProperties>().HP.Value += healamount;
        }


    }


    public void Shielding(float shieldamount)
    {
        if (IsOwner)
        {
            GetComponent<UniversalEntityProperties>().Shield.Value += shieldamount;
        }


    }




    void OnRespawnEffects()
    {
        if (GetComponent<CHAR0>())
        {
            GetComponent<CHAR0>().shielddecreaseincrement = 20f;
            Shielding(20f);
        }


    }


    void OnDamageTakenEffects(float DamageAboutToBeTaken)
    {

        if(GetComponent<CHAR0>())
        {

            CHAR0ShieldIncrementAdjust(DamageAboutToBeTaken);
        }
        
    }



    void CHAR0ShieldIncrementAdjust(float thingy)
    {
        GetComponent<CHAR0>().shielddecreaseincrement -= thingy;

        if (GetComponent<CHAR0>().shielddecreaseincrement <= 0)
        {
            GetComponent<CHAR0>().shielddecreaseincrement = 0;
        }




    }


}
