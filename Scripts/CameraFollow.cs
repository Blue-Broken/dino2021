using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    private GameManager gameM;

    public Transform cameraLook;
    public Transform cameraLookClose;
    public Transform cameraFollowFar;
    public Transform cameraFollowClose;
    private Transform cameraFollow;
    public bool rotateX;
    [SerializeField] private float xMargin;
    [SerializeField] private float yMargin;
    [SerializeField] private float zMargin;
    [SerializeField] private float xSmooth;
    [SerializeField] private float zSmooth;
    [SerializeField] private float yMin, yMax;
    [SerializeField] private float sensibilityY  ;
    private float rotationY;
    [SerializeField] private float sensibility;

    private void Start()
    {
        gameM = GameManager.gameManager;
    }

    bool CheckXMargin()
    {
        return Mathf.Abs(transform.position.x - cameraFollow.position.x) > xMargin;
    }

    bool CheckZMargin()
    {
        return Mathf.Abs(transform.position.z - cameraFollow.position.z) > zMargin;
    }

    void FixedUpdate()
    {
        if (!gameM.paused)
        {
            if (Input.GetButton("Aim") && gameM.armed)
            {
                cameraFollow = cameraFollowClose;

                #region RotateVertical

                if (rotateX)
                {

                    if (Input.GetAxisRaw("Vertical") != 0)
                    {
                        rotationY = Mathf.Lerp(rotationY, -Input.GetAxisRaw("Vertical") * 2, sensibilityY * Time.fixedDeltaTime);
                    }
                    else
                    {
                        rotationY = 0;
                    }

                    Quaternion rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x + rotationY, cameraLookClose.eulerAngles.y, 0), sensibility * 4f * Time.fixedDeltaTime);

                    if(rotation.eulerAngles.x < 0)
                    {
                        transform.rotation = rotation;
                    }
                    else
                    {
                        if(rotation.eulerAngles.x > 180)
                        {
                            if(rotation.eulerAngles.x > (360 - yMax))
                            {
                                transform.rotation = rotation;
                            }
                        }
                        else
                        {
                            if (rotation.eulerAngles.x < -yMin)
                            {
                                transform.rotation = rotation;
                            }
                        }
                    }
                    
                }
                else
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, cameraLook.eulerAngles.y, 0), sensibility * Time.fixedDeltaTime);
                }
                #endregion
            }
            else
            {
                rotationY = 0;
                cameraFollow = cameraFollowFar;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, cameraLook.eulerAngles.y, 0), sensibility * Time.fixedDeltaTime);
            }

            Follow();
        }
    }

    void Follow()
    {
        float targetX = transform.position.x;
        float targetZ = transform.position.z;

        if (CheckXMargin())
            targetX = Mathf.Lerp(transform.position.x, cameraFollow.position.x, xSmooth * Time.fixedDeltaTime);

        if (CheckZMargin())
            targetZ = Mathf.Lerp(transform.position.z, cameraFollow.position.z, zSmooth * Time.fixedDeltaTime);

        transform.position = new Vector3(targetX, cameraFollow.position.y, targetZ);
    }
}
