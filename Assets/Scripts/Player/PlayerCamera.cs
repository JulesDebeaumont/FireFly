using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Player Player;
    public Camera MainCamera;
    private bool IsDefaultOverride = false;
    private readonly float CameraSpeed = 0.125f;
    private Vector3 Offset = new Vector3(0, 1, -6);


    // Start is called before the first frame update
    void Start()
    {
        this.MainCamera.transform.position = this.Player.transform.position + this.Offset;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (this.IsDefaultOverride == false)
        {
            this.MainCamera.transform.position = GetDefaultCameraPosition();
        }
    }

    private Vector3 GetDefaultCameraPosition()
    {
        return Vector3.Lerp(this.MainCamera.transform.position, this.Player.transform.position + this.Offset, this.CameraSpeed);
    }

    public void FadeToBlack()
    {
        // https://discussions.unity.com/t/free-basic-camera-fade-in-script/686081
    }

    public void FadeFromBlack()
    {

    }

    public void AllBlack()
    {

    }

}
