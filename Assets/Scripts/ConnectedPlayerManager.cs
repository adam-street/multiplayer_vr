using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedPlayerManager : MonoBehaviour {

    public GameObject head;
    public GameObject handLeft;
    public GameObject handRight;

    public int uid;

    public void UpdatePosition(Vector3 headPos, Quaternion headRot, Vector3 handRightPos, Quaternion handRightRot, Vector3 handLeftPos, Quaternion handLeftRot)
    {
        // Head
        head.transform.position = headPos;
        head.transform.rotation = headRot;

        // Right Hand
        handRight.transform.position = handRightPos;
        handRight.transform.rotation = handRightRot;

        // Left Hand
        handLeft.transform.position = handLeftPos;
        handLeft.transform.rotation = handLeftRot;
    }
}
