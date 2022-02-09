using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensitivity = 3f;

    [SerializeField] GameObject mainCamera;
    [SerializeField] Transform neckBoneTransform;
    [SerializeField] Transform playerTransform;

    float UpAngle = 280f;
    float DownAngle = 80f;


    private void Awake()
    {
        //Check for local player

        GameObject.Instantiate(mainCamera, transform);
    }

    // Update is called once per frame
    void Update()
    {

        neckBoneTransform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.eulerAngles.x, neckBoneTransform.localRotation.eulerAngles.y, neckBoneTransform.localRotation.eulerAngles.z));
        //Check for local player

        if (Input.GetAxisRaw("Mouse Y") != 0f)
        {
            float tilt = transform.localRotation.eulerAngles.x + -(Input.GetAxisRaw("Mouse Y") * sensitivity);
            if (tilt > DownAngle && tilt < UpAngle) tilt = tilt < 115 ? DownAngle : UpAngle;
            transform.localRotation = Quaternion.Euler(new Vector3(tilt, 0));
            neckBoneTransform.GetChild(0).position = transform.position;
        }
        if (Input.GetAxisRaw("Mouse X") != 0f)
        {
            float yaw = playerTransform.localRotation.eulerAngles.y + (Input.GetAxisRaw("Mouse X") * sensitivity);
            playerTransform.localRotation = Quaternion.Euler(new Vector3(0, yaw));
        }
    }
}
