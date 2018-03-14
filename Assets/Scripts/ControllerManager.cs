using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour {

    public bool mainHand;

    //Controller References
    protected SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device Device
    {
        get
        {
            return SteamVR_Controller.Input((int)trackedObj.index);
        }
    }

    void Awake()
    {
        //Instantiate lists
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //If trigger is releasee
        if (Device.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger))
        {
            if(mainHand)
            {
                Debug.Log("RIGHT TRIGGER DOWN");
            }
            else
            {
                Debug.Log("LEFT TRIGGER DOWN");
            }
        }

        //If trigger is releasee
        if (Device.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip))
        {
            if (mainHand)
            {
                Debug.Log("RIGHT TRIGGER UP");
            }
            else
            {
                Debug.Log("LEFT TRIGGER UP");
            }
        }
    }
}
