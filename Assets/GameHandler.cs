using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : NetworkBehaviour
{
    // Start is called before the first frame update

    Color LColor;

    Color RColor;

    public NetworkVariable<int> GameMode = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    //0 is king of the hill



    //King of the Hill variables

    public NetworkVariable<float> KOTHCapFloat = new NetworkVariable<float>(100f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<char> KOTHCapTeamChar = new NetworkVariable<char>('N', NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<float> KOTHCounterCapFloat = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<float> KOTHTeamHoldFloatL = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<float> KOTHTeamHoldFloatR = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<float> KOTHDisabledFloat = new NetworkVariable<float>(10f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<int> KOTHTeamLPoints = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<int> KOTHTeamRPoints = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public GameObject KOTHShockwave;

    void Start()
    {


      KOTHCapTeamChar.OnValueChanged += PointDisabledShockwave;
    }
    



    // Update is called once per frame
    void FixedUpdate()
    {

        try
        {
            LColor = NetworkManager.LocalClient.PlayerObject.GetComponent<UniversalEntityProperties>().LColor;
            RColor = NetworkManager.LocalClient.PlayerObject.GetComponent<UniversalEntityProperties>().RColor;



        }
        catch
        {

        }


        if (GameMode.Value == 0)
        {
            KingOfTheHillRules();

        }


    }




    void KingOfTheHillRules()
    {
        if(IsHost)
        {

            //logic

            if(KOTHCapTeamChar.Value == 'N')
            {
                if (GameObject.FindGameObjectWithTag("TeamAreaPoint").GetComponent<TeamAreaPoint>().TheState.Value == 'L')
                {
                    KOTHCapFloat.Value -= 20f * Time.deltaTime;

                    if (KOTHCapFloat.Value > 100.1f)
                    {

                        KOTHCapFloat.Value -= 20f * Time.deltaTime;
                    }

                }

                else if (GameObject.FindGameObjectWithTag("TeamAreaPoint").GetComponent<TeamAreaPoint>().TheState.Value == 'R')
                {
                    KOTHCapFloat.Value += 20f * Time.deltaTime;

                    if (KOTHCapFloat.Value < 99.9f)
                    {
                        KOTHCapFloat.Value += 20f * Time.deltaTime;
                    }
                }

                else if (GameObject.FindGameObjectWithTag("TeamAreaPoint").GetComponent<TeamAreaPoint>().TheState.Value == 'N')
                {

                    if (KOTHCapFloat.Value > 101f)
                    {

                        KOTHCapFloat.Value -= 40f * Time.deltaTime;
                    }
                    else if (KOTHCapFloat.Value < 99f)
                    {
                        KOTHCapFloat.Value += 40f * Time.deltaTime;
                    }


                }
                else
                {

                }



            }

            else if(KOTHCapTeamChar.Value == 'L' || KOTHCapTeamChar.Value == 'R')
            {

                KOTHCapFloat.Value = 100;

                char CounterCaptured = 'N';



                if(KOTHCapTeamChar.Value == 'L' && GameObject.FindGameObjectWithTag("TeamAreaPoint").GetComponent<TeamAreaPoint>().TheState.Value == 'R')
                {

                    CounterCaptured = 'Y';
                }

                if (KOTHCapTeamChar.Value == 'R' && GameObject.FindGameObjectWithTag("TeamAreaPoint").GetComponent<TeamAreaPoint>().TheState.Value == 'L')
                {

                    CounterCaptured = 'Y';
                }

                if(GameObject.FindGameObjectWithTag("TeamAreaPoint").GetComponent<TeamAreaPoint>().TheState.Value == 'C')
                {
                    CounterCaptured = 'M';
                }    


                if (CounterCaptured == 'Y')
                {
                    KOTHCounterCapFloat.Value += 20f * Time.deltaTime;


                }
                else if (CounterCaptured == 'N')
                {
                    KOTHCounterCapFloat.Value -= 20f * Time.deltaTime;

                }
                else
                {

                }

                KOTHCounterCapFloat.Value = Mathf.Clamp(KOTHCounterCapFloat.Value, 0, 100);


                if(KOTHCounterCapFloat.Value >= 99f && KOTHCapTeamChar.Value == 'L')
                {
                    KOTHCapTeamChar.Value = 'R';
                    KOTHCounterCapFloat.Value = 0;

                }
                else if(KOTHCounterCapFloat.Value >= 99f && KOTHCapTeamChar.Value == 'R')
                {
                    KOTHCapTeamChar.Value = 'L';
                    KOTHCounterCapFloat.Value = 0;
                }


                if(KOTHTeamHoldFloatL.Value >= 100f || KOTHTeamHoldFloatR.Value >= 100f)
                {
                    if(KOTHTeamHoldFloatL.Value >= 100f)
                    {

                        if(KOTHTeamLPoints.Value < 2)
                        KOTHTeamLPoints.Value += 1;
                    }
                    else if (KOTHTeamHoldFloatR.Value >= 100f)
                    {
                        if (KOTHTeamRPoints.Value < 2)
                            KOTHTeamRPoints.Value += 1;
                    }


                    KOTHTeamHoldFloatL.Value = 0f;
                    KOTHTeamHoldFloatR.Value = 0f;

                    KOTHCapFloat.Value = 100f;

                    KOTHCounterCapFloat.Value = 0f;


                    KOTHDisabledFloat.Value = 10f;



                    KOTHCapTeamChar.Value = 'D';
                }

            }

            else if(KOTHCapTeamChar.Value == 'D')
            {

                KOTHDisabledFloat.Value -= 1.0f * Time.deltaTime;

                if(KOTHDisabledFloat.Value <= 0f)
                {
                    KOTHCapTeamChar.Value = 'N';
                }


            }






            KOTHCapFloat.Value= Mathf.Clamp(KOTHCapFloat.Value, 0f, 200f);

            if(KOTHCapFloat.Value <= 0)
            {
                KOTHCapTeamChar.Value = 'L';
            }
            else if(KOTHCapFloat.Value >= 200)
            {
                KOTHCapTeamChar.Value = 'R';
            }



            if (KOTHCapTeamChar.Value == 'L')
            {
                KOTHTeamHoldFloatL.Value += 0.9f * Time.deltaTime;

                if(KOTHTeamHoldFloatL.Value >= 100f)
                {
                    KOTHTeamHoldFloatL.Value = 100f;
                }    


            }

            if (KOTHCapTeamChar.Value == 'R')
            {
                KOTHTeamHoldFloatR.Value += 0.9f * Time.deltaTime;


                if (KOTHTeamHoldFloatR.Value >= 100f)
                {
                    KOTHTeamHoldFloatR.Value = 100f;
                }
            }


            

        }








    }



    public void PointDisabledShockwave(char previous, char current)
    {


        if (KOTHCapTeamChar.Value == 'D')
        {


            Instantiate(KOTHShockwave, GameObject.FindGameObjectWithTag("TeamAreaPoint").transform.position, Quaternion.identity);

        }
        else
        {


        }

    }

}
