using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class BHRRelay : MonoBehaviour
{
    private async void Start()
    {
        await UnityServices.InitializeAsync();



        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }




    private async void CreateRelay()
    {
        try
        {
           Allocation allocation=  await RelayService.Instance.CreateAllocationAsync(5);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        }

        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }




    }

}
