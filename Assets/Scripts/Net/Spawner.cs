using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkRunner networkRunner;
    [SerializeField] private NetworkPrefabRef playerPrefab;

    [SerializeField] Vector3 spawnPosition;

    private Dictionary<PlayerRef, NetworkObject> playerList = new Dictionary<PlayerRef, NetworkObject>();
    private GameInput gameInput;

    private void Start()
    {
        gameInput = GameObject.Find("GameInput").GetComponent<GameInput>();

        StartGame(GameMode.AutoHostOrClient); // 自动检测有没有host 没有的话就当host 否则加入
    }
    async void StartGame(GameMode mode)
    {
        networkRunner.ProvideInput = true;

        await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "OverCook",
            Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    public void OnConnectedToServer(NetworkRunner runner){}

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason){}

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token){}

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data){}

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason){}

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken){}

    public void OnInput(NetworkRunner runner, NetworkInput input){
        var data = new PlayerInput();
        
        data.moveDir =  gameInput.GetComponent<GameInput>().getMoveVectorNormalized();

        data.buttons.Set(PlayerInputButtons.Intersect, Input.GetKey(KeyCode.E));
        data.buttons.Set(PlayerInputButtons.Cut, Input.GetKey(KeyCode.F));

        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input){ }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){}

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){}

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);

        playerList.Add(player, networkPlayerObject);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player){
        if (playerList.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            playerList.Remove(player);
        }
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress){ }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data){}

    public void OnSceneLoadDone(NetworkRunner runner){}

    public void OnSceneLoadStart(NetworkRunner runner){}

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList){}

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason){}

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message){}
}
