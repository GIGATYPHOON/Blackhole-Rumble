using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class UniversalEntityProperties : NetworkBehaviour
{
    // Start is called before the first frame update


    public NetworkVariable<bool> isFacingRight = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<int> YourTeam = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<int> TeamInt = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField] GameObject healthbar;

    public bool multihitted = false;
    public GameObject multihitdamagesource;
    public float multihitdmg;
    public float multihitknockback;
    public float multihitknockup;
    public float multihithitinvincibilityfloat;
    public Vector2 multihitdamagepoint;
    public string multihitdamagecause;
    public int multihitdamageprio;


    public float HP;
    public float BaseHP;

    public Vector2 hitloc;

    public bool dead = false;

    public bool isdamageable = true;

    public GameObject sprites;

    [SerializeField]
    private float hitinvincibilitytimer = 0f;
    public bool invuln = false;

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
            if (HP <= 0)
            {
                HP = 0;
                dead = true;
            }

            if (combatcollisionoverride == false)
                INVINCIBLEEEE();

            Healthbar();

            if (multihitted == true)
            {
                TakeDamage(multihitdamagesource, multihitdmg, multihitknockback, multihitknockup, multihithitinvincibilityfloat, multihitdamagepoint, multihitdamagecause, multihitdamageprio);
                if (hitinvincibilitytimer <= 0f)
                {
                    multihitted = false;
                }


            }


        }



    }



    void INVINCIBLEEEE()
    {

        if (hitinvincibilitytimer > 0f)
        {



            hitinvincibilitytimer -= 10f * Time.deltaTime;




            invuln = true;

            if (((hitinvincibilitytimer - Mathf.Floor(hitinvincibilitytimer)) * 2f) < 0.5f)
            {
                sprites.SetActive(false);
            }
            else
            {
                sprites.SetActive(true);
            }
        }
        else if (hitinvincibilitytimer <= 0f)
        {
            hitinvincibilitytimer = 0f;



            currentdamageprio = 0;

            invuln = false;

            sprites.SetActive(true);


        }
    }


    void Healthbar()
    {
        if ( healthbar.gameObject)
        {
            healthbar.transform.localScale = new Vector3( (HP / BaseHP) * 3, 0.3333f, 1f);

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
                this.gameObject.layer = LayerMask.NameToLayer("PlayerTeamL");

            }
            else
            {
                TeamInt.Value = 1;
                this.transform.position = GameObject.FindGameObjectWithTag("RSpawn").transform.position;
                this.gameObject.layer = LayerMask.NameToLayer("PlayerTeamR");
            }

            YourTeam.Value = TeamInt.Value;

            healthbar.GetComponent<SpriteRenderer>().color = Color.green;

            healthbar.transform.GetChild(0).gameObject.SetActive(true);


            healthbar.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;


            GameObject.FindGameObjectWithTag("PreGameCanvas").SetActive(false);

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




        if ((invuln == false || damageprio > currentdamageprio) && isdamageable == true && dead == false)
        {
            currentdamageprio = damageprio;





            GetComponent<UniversalEntityProperties>().HP -= dmg;


            if (HP > 0)
            {

                if (damagepoint.x < this.transform.position.x)
                {
                    GetComponent<Rigidbody2D>().AddForce(Vector2.right * knockback, ForceMode2D.Impulse);
                }
                else
                {
                    GetComponent<Rigidbody2D>().AddForce(Vector2.left * knockback, ForceMode2D.Impulse);
                }


                GetComponent<Rigidbody2D>().AddForce(Vector2.up * knockup, ForceMode2D.Impulse);


            }





            hitinvincibilitytimer = hitinvincibilityfloat;



        }

    }


}
