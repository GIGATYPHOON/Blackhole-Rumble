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

    [SerializeField] GameObject KOTHMiddleBar1;

    [SerializeField] GameObject KOTHMiddleBar2;

    [SerializeField] GameObject KOTHMiddleBar3;

    [SerializeField] GameObject KOTHMiddleBar1Holder;

    [SerializeField] GameObject KOTHMiddleBar2Holder;
    [SerializeField] GameObject KOTHMiddleBar3Holder;

    [SerializeField] GameObject[] KOTHLPoints;
    [SerializeField] GameObject[] KOTHRPoints;


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
        if (GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCapTeamChar.Value == 'N')
        {
            KOTHMiddleBar1Holder.SetActive(true);

            KOTHMiddleBar2Holder.SetActive(false);

            KOTHMiddleBar3Holder.SetActive(false);

            KOTHMiddleBar1.transform.GetChild(0).GetComponent<Image>().color = LColor;


            KOTHMiddleBar1.transform.GetChild(1).GetComponent<Image>().color = RColor;

            if (GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCapFloat.Value > 101f || GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCapFloat.Value < 99f)
            {
                KOTHMiddleBar1.transform.localPosition = new Vector2(Mathf.Lerp(227.8f, -227.8f, GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCapFloat.Value / 200f), 0f);
            }
            else
            {
                KOTHMiddleBar1.transform.localPosition = new Vector2(0f, 0f);

            }

        }
        else if (GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCapTeamChar.Value == 'R' || GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCapTeamChar.Value == 'L')
        {
            KOTHMiddleBar1Holder.SetActive(false);

            KOTHMiddleBar2Holder.SetActive(true);

            KOTHMiddleBar3Holder.SetActive(false);

            KOTHMiddleBar2.transform.GetChild(0).GetComponent<Image>().color = LColor;


            KOTHMiddleBar2.transform.GetChild(1).GetComponent<Image>().color = RColor;



            if (GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCapTeamChar.Value == 'L')
            {
                KOTHMiddleBar2.transform.localPosition = new Vector2(Mathf.Lerp(113.9f, -113.9f, GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCounterCapFloat.Value /100f), 0f);
            }

            if (GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCapTeamChar.Value == 'R')
            {
                KOTHMiddleBar2.transform.localPosition = new Vector2(Mathf.Lerp(-113.9f, 113.9f, GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCounterCapFloat.Value / 100f), 0f);
            }


        }
        else
        {

            KOTHMiddleBar3Holder.SetActive(true);
            KOTHMiddleBar1Holder.SetActive(false);
            KOTHMiddleBar2Holder.SetActive(false);

            float LengthOfThing;

            LengthOfThing = Mathf.Lerp(227.8f, 0, GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHDisabledFloat.Value / 10f);

            KOTHMiddleBar3.GetComponent<RectTransform>().sizeDelta = new Vector2(LengthOfThing, 15f);

        }



        KOTHRSidePercentage.GetComponent<TextMeshProUGUI>().text = Mathf.Ceil(GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHTeamHoldFloatR.Value) + "%";
        KOTHLSidePercentage.GetComponent<TextMeshProUGUI>().text = Mathf.Ceil(GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHTeamHoldFloatL.Value) + "%";



        for(int i = 0; i < GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHTeamLPoints.Value; i++)
        {
            KOTHLPoints[i].SetActive(false);
        }

        for (int i = 0; i < GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHTeamRPoints.Value; i++)
        {
            KOTHRPoints[i].SetActive(false);
        }



        if (GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCapTeamChar.Value == 'L')
        {
            KOTHLBlackToColor.GetComponent<Image>().color = LColor;

            KOTHRBlackToColor.GetComponent<Image>().color = Color.black;

            KOTHLSidePercentage.GetComponent<TextMeshProUGUI>().color = Color.white;

            KOTHRSidePercentage.GetComponent<TextMeshProUGUI>().color = RColor;


        }

        else if (GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().KOTHCapTeamChar.Value == 'R')
        {
            KOTHLBlackToColor.GetComponent<Image>().color = Color.black;

            KOTHRBlackToColor.GetComponent<Image>().color = RColor;

            KOTHRSidePercentage.GetComponent<TextMeshProUGUI>().color = Color.white;

            KOTHLSidePercentage.GetComponent<TextMeshProUGUI>().color = LColor;



        }

        else
        {
            KOTHRSidePercentage.GetComponent<TextMeshProUGUI>().color = RColor;
            KOTHLSidePercentage.GetComponent<TextMeshProUGUI>().color = LColor;

            KOTHLBlackToColor.GetComponent<Image>().color = Color.black;

            KOTHRBlackToColor.GetComponent<Image>().color = Color.black;


        }
    }
}
