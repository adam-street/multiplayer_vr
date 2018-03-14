using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Net;
using SocketIO;

// using WebSocket4Net;

public class Client : MonoBehaviour
{
    private SocketIOComponent socket;

    public GameObject controllerLeft;
    public GameObject controllerRight;
    public GameObject cameraHead;

    public GameObject connectedPlayerPrefab;

    public ConnectedPlayerManager cp;
    private bool playerReady = false;
    public int playerId;

    private Dictionary<int, GameObject> playerList;

    // Update is called once per frame
    void Update()
    {
        if (playerReady)
        {
            //cp.UpdatePosition(cameraHead.transform.position, cameraHead.transform.rotation, controllerRight.transform.position, controllerRight.transform.rotation, controllerLeft.transform.position, controllerLeft.transform.rotation);
        } 
    }

    public void Start()
    {
        playerList = new Dictionary<int, GameObject>();
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();

        // Setup Calls From Server Here
        socket.On("open", Open);
        socket.On("error", Error);
        socket.On("close", Close);

        socket.On("getplayer", GetPlayer);
        socket.On("getnewplayer", GetNewPlayer);
        socket.On("updatepos", UpdatePlayerPos);
    }

    public void Open(SocketIOEvent e)
    {
        if (!playerReady)
        {
            socket.Emit("PostNewPlayer");
        }
    }

    public void Error(SocketIOEvent e)
    {
        playerReady = false;
    }

    public void Close(SocketIOEvent e)
    {
        playerReady = false;
    }

    public void GetPlayer(SocketIOEvent e)
    {
        NewPlayerMessage data = JsonUtility.FromJson<NewPlayerMessage>(e.data.ToString());
        playerId = data.uid;
        playerReady = true;

        StartCoroutine("PositionUpdateLoop");
    }

    public void GetNewPlayer(SocketIOEvent e)
    {
        GameObject player = Instantiate(connectedPlayerPrefab);
        ConnectedPlayerManager pm = player.GetComponent<ConnectedPlayerManager>();
        NewPlayerMessage data = JsonUtility.FromJson<NewPlayerMessage>(e.data.ToString());

        pm.uid = data.uid;
        playerList.Add(pm.uid, player);
    }

    public void SendPlayerPosition()
    {

        UpdatePlayerPositionMessage upp = new UpdatePlayerPositionMessage()
        {
            uid = playerId,
            headPos = cameraHead.transform.position,
            headRot = cameraHead.transform.rotation,
            handRightPos = controllerRight.transform.position,
            handRightRot = controllerRight.transform.rotation,
            handLeftPos = controllerLeft.transform.position,
            handLeftRot = controllerLeft.transform.rotation
        };

        string st = JsonUtility.ToJson(upp);
        JSONObject data = new JSONObject(st);

        socket.Emit("PostPlayerPosition", data);
    }

    public void UpdatePlayerPos(SocketIOEvent e)
    {
        UpdatePlayerPositionMessage data = JsonUtility.FromJson<UpdatePlayerPositionMessage>(e.data.ToString());
        
        GameObject player = playerList[data.uid];
        player.GetComponent<ConnectedPlayerManager>().UpdatePosition(data.headPos, data.headRot, data.handRightPos, data.handRightRot, data.handLeftPos, data.handLeftRot);
        
    }

    private IEnumerator PositionUpdateLoop()
    {
        // wait 1 seconds and continue
        yield return new WaitForSeconds(0.01f);
        SendPlayerPosition();
        StartCoroutine("PositionUpdateLoop");

    }
}

public class UpdatePlayerPositionMessage {

    public int uid;

    public Vector3 headPos;
    public Quaternion headRot;

    public Vector3 handRightPos;
    public Quaternion handRightRot;

    public Vector3 handLeftPos;
    public Quaternion handLeftRot;
}

public class NewPlayerMessage
{
    public int uid;
}
