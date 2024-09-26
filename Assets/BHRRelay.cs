using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class BHRRelay : MonoBehaviour
{
    [SerializeField] private TMP_InputField codeinput;

    private async void Start()
    {
        await UnityServices.InitializeAsync();



        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();


    }




    public async void CreateRelay()
    {
        try
        {
           Allocation allocation=  await RelayService.Instance.CreateAllocationAsync(9, "asia-southeast1");

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4, 
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes, 
                allocation.Key, 
                allocation.ConnectionData);


            codeinput.text = joinCode;
            NetworkManager.Singleton.StartHost();



        }

        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }




    }


    public async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Joining relay with " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                joinAllocation.RelayServer.IpV4, 
                (ushort)joinAllocation.RelayServer.Port, 
                joinAllocation.AllocationIdBytes, 
                joinAllocation.Key, 
                joinAllocation.ConnectionData, 
                joinAllocation.HostConnectionData);


            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);

        }

    }


}