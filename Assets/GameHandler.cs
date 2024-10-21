using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

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

    [SerializeField] GameObject KOTHCapR;


    [SerializeField] GameObject KOTHCapB;


    void Start()
    {

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

            if(KOTHCapTeamChar.Value == 'N')
            {
                if (GameObject.FindGameObjectWithTag("TeamAreaPoint").GetComponent<TeamAreaPoint>().TheState.Value == 'L')
                {
                    KOTHCapFloat.Value -= 20f * Time.deltaTime;
                }

                else if (GameObject.FindGameObjectWithTag("TeamAreaPoint").GetComponent<TeamAreaPoint>().TheState.Value == 'R')
                {
                    KOTHCapFloat.Value += 20f * Time.deltaTime;
                }

                else if (GameObject.FindGameObjectWithTag("TeamAreaPoint").GetComponent<TeamAreaPoint>().TheState.Value == 'N')
                {

                    if (KOTHCapFloat.Value > 100.1f)
                    {

                        KOTHCapFloat.Value -= 20f * Time.deltaTime;
                    }
                    else if (KOTHCapFloat.Value < 99.9f)
                    {
                        KOTHCapFloat.Value += 20f * Time.deltaTime;
                    }


                }
                else
                {

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

        }


        //bring this and every setcolor to on network spawn or something later

        print(KOTHCapFloat.Value);



        //var gradient = new Gradient();

        //var colors = new GradientColorKey[3];
        //colors[0] = new GradientColorKey(LColor, 0.0f);
        //colors[1] = new GradientColorKey(Color.gray * 1.6f, 0.5f);
        //colors[2] = new GradientColorKey(RColor, 1.0f);

        //var alphas = new GradientAlphaKey[1];
        //alphas[0] = new GradientAlphaKey(1.0f, 1.0f);


        //gradient.SetKeys(colors, alphas);   


        //GameObject.FindGameObjectWithTag("TeamAreaPoint").GetComponent<TeamAreaPoint>().CapturePointObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color
        //    = gradient.Evaluate(KOTHCapFloat.Value / 200f);


        if (KOTHCapFloat.Value < 100)
        {
            KOTHCapTeamChar.Value = 'L';
        }
        else if (KOTHCapFloat.Value >= 200)
        {
            KOTHCapTeamChar.Value = 'R';
        }




    }
}