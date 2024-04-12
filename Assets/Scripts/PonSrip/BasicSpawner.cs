using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkRunner networkRunner = null;
    [SerializeField] private NetworkPrefabRef playerPrefab;

    private Dictionary<PlayerRef, NetworkObject> playerList = new Dictionary<PlayerRef, NetworkObject>();
    private void Start()
    {
        StartGame(GameMode.AutoHostOrClient); // 自动检测有没有host 没有的话就当host 否则加入
    }
    async void StartGame(GameMode mode)
    {
        networkRunner.ProvideInput = true;

        await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "Fusion Test",
            Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){}

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){}

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)// 第一个参数是场景中的network runner 第二个参数是进入的玩家
    {
        Vector3 spawnPosition = Vector3.up;
        NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);

        playerList.Add(player, networkPlayerObject);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (playerList.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            playerList.Remove(player);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        if (Input.GetKey(KeyCode.W))
            data.movementInput += Vector3.forward;
        if (Input.GetKey(KeyCode.D))
            data.movementInput += Vector3.right;
        if (Input.GetKey(KeyCode.A))
            data.movementInput += Vector3.left;
        if (Input.GetKey(KeyCode.S))
            data.movementInput += Vector3.back;

        data.buttons.Set(InputButtons.JUMP, Input.GetKey(KeyCode.Space));
        data.buttons.Set(InputButtons.FIRE, Input.GetKey(KeyCode.Mouse0));

        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        //throw new NotImplementedException();
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        //throw new NotImplementedException();
    }

    public void OnConnectedToServer(NetworkRunner runner){}

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason){}

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        //throw new NotImplementedException();
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        //throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        //throw new NotImplementedException();
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList){}

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data){}

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken){}

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data){}

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress){}

    public void OnSceneLoadDone(NetworkRunner runner){}

    public void OnSceneLoadStart(NetworkRunner runner){}
}
