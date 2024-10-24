using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class TeamAreaPoint : NetworkBehaviour
{
    // Start is called before the first frame update
    bool m_Started;

    public LayerMask m_LayerMask;

    public NetworkVariable<int> LTeamCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<int> RTeamCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

   // public List<GameObject> LPlayersInside;

  //  public List<GameObject> RPlayersInside;


    Color LColor;

    Color RColor;

    Color LColorAlpha;

    Color RColorAlpha;

    bool ContestedFakeSineWaveBool = false;

    float ContestedFakeSineWave = 0f;




    public NetworkVariable<char> TheState = new NetworkVariable<char>('A', NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    //other thingd


    [SerializeField] public GameObject CapturePointObject;

    [SerializeField] public GameObject CapRMeter;

    [SerializeField] public GameObject CapLMeter;

    [SerializeField] public GameObject[] CapIndicators;

    [SerializeField] public GameObject CapDim;


    //koth



    void Start()
    {

        m_Started = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        try
        {
            LColor = NetworkManager.LocalClient.PlayerObject.GetComponent<UniversalEntityProperties>().LColor;
            RColor = NetworkManager.LocalClient.PlayerObject.GetComponent<UniversalEntityProperties>().RColor;

            LColorAlpha = new Color(LColor.r, LColor.g, LColor.b, 0.2f);

            RColorAlpha = new Color(RColor.r, RColor.g, RColor.b, 0.2f);

            CapLMeter.transform.GetChild(0).GetComponent<SpriteRenderer>().color = LColor;
            CapRMeter.transform.GetChild(0).GetComponent<SpriteRenderer>().color = RColor;
        }
        catch
        {

        }

        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, m_LayerMask);
        int i = 0;
        int LPlayersInside = 0;
        int RPlayersInside = 0;



        foreach(Collider thingamajig in hitColliders)
        {
            if(thingamajig.gameObject.GetComponent<UniversalEntityProperties>().dead.Value == false)
            {

                if(thingamajig.gameObject.GetComponent< UniversalEntityProperties>().TeamInt.Value == 0)
                {
                    LPlayersInside++;
                }
                if (thingamajig.gameObject.GetComponent<UniversalEntityProperties>().TeamInt.Value == 1)
                {
                    RPlayersInside++;
                }


            }


        }



        if (IsHost)
        {
            LTeamCount.Value = LPlayersInside;

            RTeamCount.Value = RPlayersInside;

        }




        if (LTeamCount.Value > 0 && RTeamCount.Value == 0)
        {
            this.GetComponent<SpriteRenderer>().color = LColorAlpha;

            if (IsHost)
            {
                TheState.Value =  'L';

            }

        }
        else if (RTeamCount.Value > 0 && LTeamCount.Value == 0)
        {

            this.GetComponent<SpriteRenderer>().color = RColorAlpha;


            if (IsHost)
            {
                TheState.Value = 'R';

            }

        }
        else if (LTeamCount.Value == 0 && RTeamCount.Value ==0)
        {


            this.GetComponent<SpriteRenderer>().color =  new Color(0.5f, 0.5f, 0.5f, 0.4f);


            if (IsHost)
            {
                TheState.Value = 'N';

            }

        }
        else
        {


            if(ContestedFakeSineWaveBool == false)
            {

                ContestedFakeSineWave += 11f * Time.deltaTime;

                if(ContestedFakeSineWave > 10f)
                {
                    ContestedFakeSineWaveBool = true;
                }
            }
            else
            {
                ContestedFakeSineWave -= 11f * Time.deltaTime;

                if (ContestedFakeSineWave < 0f)
                {
                    ContestedFakeSineWaveBool = false;
                }
            }

            if (IsHost)
            {
                TheState.Value = 'C';

            }

            this.GetComponent<SpriteRenderer>().color = Color.Lerp(RColorAlpha,LColorAlpha, ContestedFakeSineWave/10f);

        }


        KOTHFunction();


    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, transform.localScale);
    }






    void KOTHFunction()
    {



        //if neutral

        if (GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCapTeamChar.Value == 'N')

        {

            //CAP METER GO RETURN

            if (GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCapFloat.Value > 101f)
            {
                CapRMeter.transform.localScale = new Vector3(1, Mathf.Lerp(1, 0, (200f - GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCapFloat.Value) / 100f));
            }
            else if (GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCapFloat.Value < 99f)
            {
                CapLMeter.transform.localScale = new Vector3(1, Mathf.Lerp(0, 1, (100f - GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCapFloat.Value) / 100f));
            }
            else
            {
                CapRMeter.transform.localScale = new Vector3(1, 0);
                CapLMeter.transform.localScale = new Vector3(1, 0);

            }




        }
        else
        {

            //CAP METER GO UP

            if (GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCapTeamChar.Value == 'L')
            {
                CapRMeter.transform.localScale = new Vector3(1, Mathf.Lerp(0, 1, GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCounterCapFloat.Value / 100f));
                CapLMeter.transform.localScale = new Vector3(1, 1);
            }

            if (GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCapTeamChar.Value == 'R')
            {
                CapLMeter.transform.localScale = new Vector3(1, Mathf.Lerp(0, 1, GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCounterCapFloat.Value / 100f));
                CapRMeter.transform.localScale = new Vector3(1, 1);
            }

            if (GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCapTeamChar.Value == 'D')
            {
                CapLMeter.transform.localScale = new Vector3(1, 0);
                CapRMeter.transform.localScale = new Vector3(1, 0);

                CapDim.transform.localScale = new Vector3(1, Mathf.Lerp(0, 1, GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHDisabledFloat.Value / 10f));
            }

        }



        //THESE ARE THINGS FOR IF THE THING IS CAPPED

        if (GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCapTeamChar.Value == 'L')
        {
            CapRMeter.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1;

            CapLMeter.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 0;


            //forgive me for this is concrete sinning




        }

        else if (GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCapTeamChar.Value == 'R')
        {
            CapRMeter.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 0;

            CapLMeter.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1;






        }

        else
        {

            CapRMeter.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 0;

            CapLMeter.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder =0;



        }






    }

}
