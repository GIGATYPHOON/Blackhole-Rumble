using System.Collections;
using System.Collections.Generic;
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

    bool ContestedFakeSineWaveBool = false;

    float ContestedFakeSineWave = 0f;




    public NetworkVariable<char> TheState = new NetworkVariable<char>('A', NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    //state 0 is neutral, state 1 is left side cap, state 2 is right side cap, state 3 is contested;

    //other thingd


    [SerializeField] public GameObject CapturePointObject;

    [SerializeField] public GameObject CapRMeter;

    [SerializeField] public GameObject CapLMeter;

    [SerializeField] public GameObject CapIndicator1;


    [SerializeField] public GameObject CapIndicator2;

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

            LColor = new Color(LColor.r, LColor.g, LColor.b, 0.2f);

            RColor = new Color(RColor.r, RColor.g, RColor.b, 0.2f);
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
            this.GetComponent<SpriteRenderer>().color = LColor;

            if (IsHost)
            {
                TheState.Value =  'L';

            }

        }
        else if (RTeamCount.Value > 0 && LTeamCount.Value == 0)
        {

            this.GetComponent<SpriteRenderer>().color = RColor;


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

            this.GetComponent<SpriteRenderer>().color = Color.Lerp(RColor,LColor, ContestedFakeSineWave/10f);

        }



    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    //private void OnTriggerEnter(Collider other)
    //{

    //    if (other.gameObject.GetComponent<UniversalEntityProperties>())
    //    {

    //        if(other.gameObject.GetComponent<UniversalEntityProperties>().TeamInt.Value == 0)
    //        {
    //            if (!LPlayersInside.Contains(other.gameObject))
    //                LPlayersInside.Add(other.gameObject);

    //        }
    //        else if (other.gameObject.GetComponent<UniversalEntityProperties>().TeamInt.Value == 1)
    //        {
    //            if (!RPlayersInside.Contains(other.gameObject))
    //                RPlayersInside.Add(other.gameObject);

    //        }


    //    }

    //}

    //private void OnTriggerStay(Collider other)
    //{


    //    if (other.gameObject.GetComponent<UniversalEntityProperties>())
    //    {

    //        if (other.gameObject.GetComponent<UniversalEntityProperties>().TeamInt.Value == 0)
    //        {
    //            if (!LPlayersInside.Contains(other.gameObject))
    //                LPlayersInside.Add(other.gameObject);

    //        }
    //        else if (other.gameObject.GetComponent<UniversalEntityProperties>().TeamInt.Value == 1)
    //        {
    //            if (!RPlayersInside.Contains(other.gameObject))
    //                RPlayersInside.Add(other.gameObject);

    //        }


    //    }

    //}


    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.GetComponent<UniversalEntityProperties>())
    //    {

    //        if (other.gameObject.GetComponent<UniversalEntityProperties>().TeamInt.Value == 0)
    //        {
    //            if (LPlayersInside.Contains(other.gameObject))
    //                LPlayersInside.Remove(other.gameObject);

    //        }
    //        else if (other.gameObject.GetComponent<UniversalEntityProperties>().TeamInt.Value == 1)
    //        {
    //            if (RPlayersInside.Contains(other.gameObject))
    //                RPlayersInside.Remove(other.gameObject);

    //        }


    //    }
    //}





}
