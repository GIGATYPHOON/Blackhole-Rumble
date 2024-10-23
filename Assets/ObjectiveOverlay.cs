using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveOverlay : NetworkBehaviour
{
    // Start is called before the first frame update


    Color LColor;

    Color RColor;

    //KOTH STUFF

    [SerializeField] GameObject KOTHLBlackToColor;

    [SerializeField] GameObject KOTHRBlackToColor;

    [SerializeField] GameObject KOTHLSidePercentage;

    [SerializeField] GameObject KOTHRSidePercentage;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void FixedUpdate()
    {

        try
        {
            LColor = NetworkManager.LocalClient.PlayerObject.GetComponent<UniversalEntityProperties>().LColor;
            RColor = NetworkManager.LocalClient.PlayerObject.GetComponent<UniversalEntityProperties>().RColor;



        }
        catch
        {

        }


        KOTHStuff();

    }


    void KOTHStuff()
    {

        if (GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCapTeamChar.Value == 'L')
        {
            KOTHLBlackToColor.GetComponent<Image>().color = LColor;

            KOTHRBlackToColor.GetComponent<Image>().color = Color.black;

            KOTHLSidePercentage.GetComponent<TextMeshProUGUI>().text = Mathf.Ceil(GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHTeamHoldFloatL.Value) + "%";


        }

        else if (GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCapTeamChar.Value == 'R')
        {
            KOTHLBlackToColor.GetComponent<Image>().color = Color.black;

            KOTHRBlackToColor.GetComponent<Image>().color = RColor;


            KOTHLSidePercentage.GetComponent<TextMeshProUGUI>().text = Mathf.Ceil(GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHTeamHoldFloatR.Value) + "%";



        }

        else
        {

            KOTHLBlackToColor.GetComponent<Image>().color = Color.black;

            KOTHRBlackToColor.GetComponent<Image>().color = Color.black;


        }
    }
}
