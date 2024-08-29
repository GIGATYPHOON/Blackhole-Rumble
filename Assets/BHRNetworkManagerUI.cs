using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class BHRNetworkManagerUI : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Button hostbutton;

    [SerializeField] private Button clientbutton;

    void Start()
    {
        
    }

    private void Awake()
    {
        hostbutton.onClick.AddListener(() => { NetworkManager.Singleton.StartHost();});

        clientbutton.onClick.AddListener(() => { NetworkManager.Singleton.StartClient(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
