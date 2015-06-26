using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PlayerController : NetworkBehaviour
{
    public float startMoveSpeed = 1f;
    [Range(0.001f, 0.1f)]
    public float distanceTreshold = 0.02f;

    [SerializeField]
    private float probeEatSizeIncrease = .1f;
    [SerializeField]
    private float _cameraStartSize = 4f;
    private float _moveSpeed = 1f;
    private float _size = 1f;
    private Camera _camera;
    private bool canMove = true;
    private float _worldClampValue;

    public float PlayerSize
    {
        get { return _size; }
        set
        {
            //Debug.Log("Player size changed");
            _size = value;
            transform.localScale = new Vector3(_size, _size, 1f);
            //_camera.orthographicSize = _size * _cameraStartSize;
        }
    }

    void Start()
    {

    }

    public override void OnStartLocalPlayer()
    {
        _camera = GetComponentInChildren<Camera>();
        if (isLocalPlayer)
        {
            _moveSpeed = startMoveSpeed;
            _camera.orthographicSize = PlayerSize * _cameraStartSize;
        }
        _worldClampValue = FindObjectOfType<LevelController>().worldSize;
    }


    void Update()
    {
        MovePlayer();
        if (Input.GetKeyDown(KeyCode.S)) canMove = !canMove;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Probe") && isLocalPlayer)
        {
            //float distance = Vector3.Distance(transform.position, collision.transform.position);
            //if (distance < PlayerSize - collision.transform.localScale.x)
            //{
            //Debug.Log("Probe collected");
            CmdTellServerProbeEat(collision.gameObject);
            PlayerSize += probeEatSizeIncrease;
            //}
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {

    }

    public void OnTriggerExit2D(Collider2D collision)
    {

    }

    [Command]
    void CmdTellServerProbeEat(GameObject probe)
    {
        Destroy(probe);
    }

    void MovePlayer()
    {
        if (!canMove) return;
        Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector3.Distance(transform.position, new Vector3(mousePos.x, mousePos.y, 0f));
        // Debug.Log(distance);
        // check distance
        if (distance < PlayerSize / 2f)
        {
            // slow down if we are inside our circle
            if (distance > distanceTreshold)
                _moveSpeed = startMoveSpeed * distance / (PlayerSize * 2f); //Mathf.Lerp(_moveSpeed, 0f, Time.deltaTime * lerpRate);
            else
                _moveSpeed = 0f;
        }
        else
        {
            _moveSpeed = startMoveSpeed / PlayerSize;
        }
        Vector3 moveTo = Vector3.MoveTowards(transform.position, new Vector3(mousePos.x, mousePos.y, 0f), _moveSpeed / 10f);
        moveTo.x = Mathf.Clamp(moveTo.x ,- _worldClampValue / 2f, _worldClampValue / 2f);
        moveTo.y = Mathf.Clamp(moveTo.y, -_worldClampValue / 2f, _worldClampValue / 2f);
        transform.position = moveTo;
    }
}
