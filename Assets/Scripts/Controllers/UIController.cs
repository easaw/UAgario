using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {

    public void SetName(string name)
    {
        PlayerPrefs.SetString("NAME",name);
    }

}
