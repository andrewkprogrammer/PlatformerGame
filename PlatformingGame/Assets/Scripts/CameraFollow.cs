using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Blackboard blackboard;
    [SerializeField]
    GameObject Player;

    [SerializeField]
    float distance = 5;
    [SerializeField]
    float playerHeight = 2;

    [Header("Player Control")]
    [SerializeField]
    bool PlayerControlEnabled = true;
    [SerializeField]
    float playerControlTime = 1;
    float playerControlCountdown = 0;
    [SerializeField]
    [Range(0, 120)]
    public float xSensitivity = 90;
    [SerializeField]
    [Range(0, 120)]
    public float ySensitivity = 90;

    [Tooltip("How high the camera can go, 1 being directly above the player, -1 being beneath")]
    [SerializeField]
    [Range(-1, 1)]
    float cameraMaxHeight = 0.5f;
    [Tooltip("How high the camera can go, 1 being directly above the player, -1 being beneath")]
    [SerializeField]
    [Range(-1, 1)]
    float cameraMinHeight = -0.1f;


    Vector3 playerForward { get { return -Player.transform.forward; } }

    Vector3 targetPos { get { return Player.transform.position + (transform.up * playerHeight) + (playerForward * distance); } }

    Vector3 lookAtPos { get { return Player.transform.position + (Player.transform.up * playerHeight); } }

    bool cursorInWindow { get {
            return Input.mousePosition.x < Screen.width && Input.mousePosition.x > 0
            && Input.mousePosition.y < Screen.height && Input.mousePosition.y > 0;
        } }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = targetPos;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (!blackboard)
            blackboard = GameObject.Find("GameManager").GetComponent<Blackboard>();
    }

    // Update is called once per frame
    void Update()
    {
        if (blackboard.IsPaused)
            return;

        // if the player has controlled the camera recently, the script will wait a bit before reverting to automatic movement
        if (playerControlCountdown <= 0)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime);
        }
        else
            playerControlCountdown -= Time.deltaTime;

        Vector3 mouseDelta = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
        if (PlayerControlEnabled && mouseDelta.magnitude > 0)
        {
            // This function will need to check how far the camera movement will go in case it goes outside of the intended bounds.
            // I.E. If camera will go past bounds, decrease mouseDelta so that it only go up to the bounds instead of past them.

            // Checks if camera is already too high or too low
            float dot = Vector3.Dot(Vector3.down, transform.forward);
            if ((mouseDelta.y < 0 && dot > cameraMaxHeight) || (mouseDelta.y > 0 && dot < cameraMinHeight))
                mouseDelta.y = 0;
            //

            // Rotate the camera around the player based on the mouse movement
            transform.RotateAround(lookAtPos, Vector3.up, mouseDelta.x * xSensitivity);
            transform.RotateAround(lookAtPos, -transform.right, mouseDelta.y * ySensitivity);
            //
            
            playerControlCountdown = playerControlTime;
        }
        transform.LookAt(lookAtPos);
        transform.position = lookAtPos - transform.forward * distance;

        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void setSensX(float value)
    {
        xSensitivity = value;
    }

    public void setSensY(float value)
    {
        ySensitivity = value;
    }
}
