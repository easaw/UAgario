using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ProbeNetwokId : NetworkBehaviour {

    public void DestroyProbe()
    {
        //Debug.Log("Destroy probe");
        NetworkServer.Destroy(this.gameObject);
    }
}
