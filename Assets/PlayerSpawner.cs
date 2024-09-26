using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : NetworkBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private GameObject Player;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneLoaded;
    }


    private void SceneLoaded(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List <ulong> clientsTimedOut)
    {
        if(IsHost)
        {

            foreach (ulong id in clientsCompleted)
            {
                GameObject player = Instantiate(Player);
                player.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, true);

            }

        }
    }
}
