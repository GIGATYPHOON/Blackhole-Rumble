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


    void Start()
    {

    }
    



    // Update is called once per frame
    void FixedUpdate()
    {
        try
        {
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
        }
        catch
        {

        }


        if(GameMode.Value == 0)
        {
            KingOfTheHillRules();

        }


    }




    void KingOfTheHillRules()
    {
        if(IsHost)
        {
            if (GameObject.FindGameObjectWithTag("TeamAreaPoint").GetComponent<TeamAreaPoint>().TheState.Value == 'L')
            {
                KOTHCapFloat.Value -= 15f * Time.deltaTime;
            }

            else if (GameObject.FindGameObjectWithTag("TeamAreaPoint").GetComponent<TeamAreaPoint>().TheState.Value == 'R')
            {
                KOTHCapFloat.Value += 15f * Time.deltaTime;
            }

            else if (GameObject.FindGameObjectWithTag("TeamAreaPoint").GetComponent<TeamAreaPoint>().TheState.Value == 'N')
            {

                if (KOTHCapFloat.Value > 100.1f)
                {

                    KOTHCapFloat.Value -= 15f * Time.deltaTime;
                }
                else if (KOTHCapFloat.Value < 99.9f)
                {
                    KOTHCapFloat.Value += 15f * Time.deltaTime;
                }


            }
            else
            {

            }




            

            KOTHCapFloat.Value= Mathf.Clamp(KOTHCapFloat.Value, 0f, 200f);

        }


        //bring this and every setcolor to on network spawn or something later

        print(KOTHCapFloat.Value);

        var gradient = new Gradient();

        // Blend color from red at 0% to blue at 100%
        var colors = new GradientColorKey[3];
        colors[0] = new GradientColorKey(LColor, 0.0f);
        colors[1] = new GradientColorKey(Color.white, 0.5f);
        colors[2] = new GradientColorKey(RColor, 1.0f);

        var alphas = new GradientAlphaKey[1];
        alphas[0] = new GradientAlphaKey(1.0f, 1.0f);


        gradient.SetKeys(colors, alphas);   


        GameObject.FindGameObjectWithTag("TeamAreaPoint").GetComponent<TeamAreaPoint>().CapturePointObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color
            = gradient.Evaluate(KOTHCapFloat.Value / 200f);





    }
}
