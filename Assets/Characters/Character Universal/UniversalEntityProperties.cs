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

    public NetworkVariable<int> TeamInt = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    

    [SerializeField] GameObject healthbar;

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


    public NetworkVariable<float> HP = new NetworkVariable<float>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> BaseHP = new NetworkVariable<float>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public Vector2 hitloc;

    public NetworkVariable<bool> dead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public float deathtimer = 10f;


    public GameObject deathoverlay;

    public GameObject deathoverlayint;

    public bool isdamageable = true;

    public GameObject sprites;

    [SerializeField]
    private NetworkVariable<float>  hitinvincibilitytimer = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> invuln = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public bool combatcollisionoverride = false;


    public int olddamageprio = 0;
    public int currentdamageprio = 0;

    void Start()
    {






        if (!IsOwner)
            return;






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


        print(TeamInt.Value);



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
                HP.Value = 0;
                dead.Value = true;
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



            if (((hitinvincibilitytimer.Value - Mathf.Floor(hitinvincibilitytimer.Value)) * 2f) < 0.5f)
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



            sprites.SetActive(true);


        }
    }


    void Healthbar()
    {
        if ( healthbar.gameObject)
        {
            healthbar.transform.localScale = new Vector3( (HP.Value / BaseHP.Value) * 3, 0.3333f, 1f);

        }
        else
        {


        }

    }




    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();


        if(IsOwner)
        {
            if (GameObject.FindGameObjectWithTag("TeamButton").transform.GetChild(0).GetComponent<TMP_Text>().text == "L")
            {
               TeamInt.Value = 0;
                this.transform.position = GameObject.FindGameObjectWithTag("LSpawn").transform.position;




            }
            else
            {
                TeamInt.Value = 1;
                this.transform.position = GameObject.FindGameObjectWithTag("RSpawn").transform.position;

            }

            YourTeam.Value = TeamInt.Value;

            healthbar.GetComponent<SpriteRenderer>().color = Color.green;

            youindicator.gameObject.SetActive(true);




            //GameObject.FindGameObjectWithTag("PreGameCanvas").SetActive(false);

        }
        else
        {


        }




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




        if ((invuln.Value == false || damageprio > currentdamageprio) && isdamageable == true && dead.Value == false)
        {
            currentdamageprio = damageprio;




            if(IsOwner)
              GetComponent<UniversalEntityProperties>().HP.Value -= dmg;


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

                }



            }
        }

        else
        {

            GetComponent<Collider>().enabled = true;
        }


    }
}
