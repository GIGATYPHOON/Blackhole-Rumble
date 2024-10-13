using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CapturePoint : NetworkBehaviour
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


    void Start()
    {

        m_Started = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        try
        {
            if (NetworkManager.LocalClient.PlayerObject.GetComponent<UniversalEntityProperties>().YourTeam.Value == 0)
            {

                LColor = new Color(0, 0, 1, 0.2f);
                RColor = new Color(1, 0, 0, 0.2f);
            }
            else
            {
                RColor = new Color(0, 0, 1, 0.2f);
                LColor = new Color(1, 0, 0, 0.2f);
            }
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


        }
        else if (RTeamCount.Value > 0 && LTeamCount.Value == 0)
        {

            this.GetComponent<SpriteRenderer>().color = RColor;
        }
        else if (LTeamCount.Value == 0 && RTeamCount.Value ==0)
        {


            this.GetComponent<SpriteRenderer>().color =  new Color(0.5f, 0.5f, 0.5f, 0.2f);

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
