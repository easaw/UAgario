using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager {

    public GameObject canvasName;

    private string _name;
    private NetworkConnection conn;
    short playerId;

    public string PlayerName
    {
        get { return _name; }
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        //base.OnServerAddPlayer(conn, playerControllerId);
        this.conn = conn;
        this.playerId = playerControllerId;
        canvasName.SetActive(true);
    }

    public void SetName(string name)
    {
        this._name = name;
    }

    public void CreatePlayer()
    {
        canvasName.SetActive(false);
        var player = (GameObject)GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerId);
    }
}
