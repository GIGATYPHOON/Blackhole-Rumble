using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class BHRNetworkManagerUI : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Button hostbutton;

    [SerializeField] private Button clientbutton;

    [SerializeField] private Button teambutton;

    [SerializeField] private TMP_InputField codeinput;

    [SerializeField] private GameObject playerPrefab;

    void Start()
    {
        
    }

    private void Awake()
    {
        hostbutton.onClick.AddListener(() => { NetworkManager.Singleton.GetComponent<BHRRelay>().CreateRelay();});

        clientbutton.onClick.AddListener(() => { NetworkManager.Singleton.GetComponent<BHRRelay>().JoinRelay(codeinput.text); });


        teambutton.onClick.AddListener(() => { ChangeTeam(); });
    }

    // Update is called once per frame
    void Update()
    {

    }


    void ChangeTeam()
    {
        print(teambutton.transform.GetChild(0).GetComponent<TMP_Text>().text);

        if(teambutton.transform.GetChild(0).GetComponent<TMP_Text>().text == "L")
        {
            teambutton.transform.GetChild(0).GetComponent<TMP_Text>().text = "R";
        }
        else if (teambutton.transform.GetChild(0).GetComponent<TMP_Text>().text == "R")
        {
            teambutton.transform.GetChild(0).GetComponent<TMP_Text>().text = "L";
        }

    }


    void SpawnGuys()
    {

    }
}
