using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LevelController : NetworkBehaviour
{
    public float worldSize = 100f;
    public float spawnRate = 2f;

    [SerializeField]
    GameObject _spawnableProbe;
    [SerializeField]
    int _spawnableProbeStartSize = 500;

    private NetworkManager _networkManager;

    public override void OnStartServer()
    {
        //Debug.Log("Spawn server");
        _networkManager = FindObjectOfType<NetworkManager>();
        SpawnStartProbes();
    }

    private void SpawnStartProbes()
    {
        for (int i = 0; i < _spawnableProbeStartSize; i++)
        {
            Spawn();
        }
        // TODO change this for a better spawn handling by checking number of probes and their distribution
        StartCoroutine("SpawnNewProbes");
    }

    private void Spawn()
    {
        Vector3 position = new Vector3(Random.Range(-worldSize / 2f, worldSize / 2f), Random.Range(-worldSize / 2f, worldSize / 2f), 0f);
        GameObject probe = GameObject.Instantiate(_spawnableProbe, position, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(probe);//,NetworkHash128.Parse("Probe"+i));
    }

    IEnumerator SpawnNewProbes()
    {
        while (true)
        {
           if(_networkManager.numPlayers > 0)
                Spawn();
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
