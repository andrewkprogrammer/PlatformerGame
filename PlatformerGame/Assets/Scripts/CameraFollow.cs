using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
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
    [Range(0, 1)]
    float xSensitivity = 1;
    [SerializeField]
    [Range(0, 1)]
    float ySensitivity = 1;

    Vector3 cursorLastPos = Vector3.zero;



    Vector3 playerForward { get { return -Player.transform.forward; } }

    Vector3 targetPos { get { return Player.transform.position + (transform.up * playerHeight) + (playerForward * distance); } }

    Vector3 lookAtPos { get { return Player.transform.position + (Player.transform.up * playerHeight); } }

    // Start is called before the first frame update
    void Start()
    {
        cursorLastPos = Input.mousePosition;
        transform.position = targetPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControlCountdown <= 0)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime);
        }

        if (PlayerControlEnabled && Input.mousePosition != cursorLastPos)
        {
            Vector3 mouseMovement = Input.mousePosition - cursorLastPos;
            float dot = Vector3.Dot(Vector3.down, transform.forward);
            if ((mouseMovement.y < 0 && dot > 0.5f) || (mouseMovement.y > 0 && dot < -0.0f))
                mouseMovement.y = 0;

            transform.RotateAround(lookAtPos, Vector3.up, mouseMovement.x * xSensitivity);
            transform.RotateAround(lookAtPos, -transform.right, mouseMovement.y * ySensitivity);

            playerControlCountdown = playerControlTime;
        }
        transform.LookAt(lookAtPos);
        transform.position = lookAtPos - transform.forward * distance;


        if (playerControlCountdown > 0)
            playerControlCountdown -= Time.deltaTime;
        cursorLastPos = Input.mousePosition;
        
    }
}
