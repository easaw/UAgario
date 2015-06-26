using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PlayerNetworkController : NetworkBehaviour
{
    [SyncVar(hook = "SyncPositionValues")]
    private Vector3 syncPos;
    [SyncVar(hook = "SyncColorValues")]
    private Color syncColor = Color.white;
    [SyncVar(hook = "SyncNameValue")]
    private string syncName;
    [SyncVar(hook = "SyncSizeValue")]
    private float syncSize;

    [SerializeField]
    Transform _playerTransform;
    [SerializeField]
    private TextMesh _playerName;
    [SerializeField]
    private GameObject _openOnLocal;
    [SerializeField]
    private float _lerpRate = 16;
    private Vector3 lastPos, lastSize;
    private float _threshold = 0.5f;

    PlayerController _pController;

    void Start()
    {
        Setup();
    }

    public override void OnStartLocalPlayer()
    {
        // activate components
        _pController = GetComponent<PlayerController>();
        _pController.enabled = true;
        _openOnLocal.SetActive(true);
    }

    void Setup()
    {
        if (!isLocalPlayer)
        {
            // set sync values to clients
            GetComponent<SpriteRenderer>().color = syncColor;
            _playerName.text = syncName;
            transform.position = syncPos;
        }
        else
        {
            // set our ininital values
            SetRandoms();
            SetName();
        }
    }

    protected void SetRandoms()
    {
        // set pos 
        float worldSize = FindObjectOfType<LevelController>().worldSize;
        var pos = Random.insideUnitCircle * worldSize;
        transform.position = new Vector3(pos.x, pos.y, 0f);
        // set color 
        SetColor();
    }

    void Update()
    {
        LerpPosition();
    }

    void FixedUpdate()
    {
        TransmitPosition();
        SendSizeValue();
    }

    void LerpPosition()
    {
        if (!isLocalPlayer)
        {
            Lerp();
            //Debug.Log(Time.deltaTime.ToString());
        }
    }

    [Command]
    void CmdProvidePositionToServer(Vector3 pos)
    {
        syncPos = pos;
    }
    [Command]
    void CmdProvideColorToServer(Color color)
    {
        syncColor = color;
    }
    [Command]
    void CmdProvideNameToServer(string name)
    {
        syncName = name;
    }
    [Command]
    void CmdProvideSizeToServer(float size)
    {
        syncSize = size;
    }

    [ClientCallback]
    void TransmitPosition()
    {
        if (isLocalPlayer && Vector3.Distance(_playerTransform.position, lastPos) > _threshold)
        {
            CmdProvidePositionToServer(_playerTransform.position);
            lastPos = _playerTransform.position;
        }
    }
    [ClientCallback]
    private void SetColor()
    {
        Color newcolor = new Color(Random.Range(0f, .7f), Random.Range(0f, .7f), Random.Range(0f, .7f));
        GetComponent<SpriteRenderer>().color = newcolor;
        //Debug.Log("color set to " + newcolor + "  " + transform.name);
        syncColor = newcolor;
        CmdProvideColorToServer(newcolor);
    }
    [ClientCallback]
    private void SetName()
    {
        string name = PlayerPrefs.GetString("NAME", "NoName");//FindObjectOfType<MyNetworkManager>().PlayerName;// Network.player.ToString();//transform.name;
        _playerName.text = name;
        CmdProvideNameToServer(name);
    }
    [ClientCallback]
    private void SendSizeValue()
    {
        if (isLocalPlayer && lastSize != _playerTransform.localScale)
        {
            //Debug.Log(pController.PlayerSize);
            CmdProvideSizeToServer(_pController.PlayerSize);
            lastSize = _playerTransform.localScale;
        }
    }

    [Client]
    void SyncPositionValues(Vector3 latestPos)
    {
        syncPos = latestPos;
        //if (!isLocalPlayer) transform.position = syncPos;
    }
    [Client]
    void SyncColorValues(Color color)
    {
        syncColor = color;
        if (!isLocalPlayer)
        {
            //Debug.Log("Color synced " + color + " on "+transform.name);
            GetComponent<SpriteRenderer>().color = syncColor;
        }
    }
    [Client]
    void SyncNameValue(string name)
    {
        syncName = name;
        if (!isLocalPlayer) _playerName.text = name;
    }
    [Client]
    private void SyncSizeValue(float size)
    {
        syncSize = size;
        if (!isLocalPlayer) transform.localScale = new Vector3(syncSize, syncSize, 1f);
    }
    void Lerp()
    {
        _playerTransform.position = Vector3.Lerp(_playerTransform.position, syncPos, Time.deltaTime * _lerpRate);
    }

}
