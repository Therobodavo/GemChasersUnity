using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontalMove;
    private float verticalMove;

    private Vector3 velocity;

    private float horizontalAim;
    private float verticalAim;

    [SerializeField]
    private float speed;
    private float currentLookDir;

    // Start is called before the first frame update
    void Start()
    {
        horizontalMove = 0f;
        verticalMove = 0f;

        horizontalAim = 0f;
        verticalAim = 0f;

        currentLookDir = 0f;

        LookDirection(horizontalAim, verticalAim);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();

        // if player is moving
        if (horizontalMove != 0 || verticalMove != 0)
        {
            MovePlayer();
        }

        // else use the direction the player is moving
        if (horizontalMove != 0 || verticalMove != 0)
        {
            LookDirection(horizontalMove, verticalMove);
        }
    }

    void CheckInput()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        //horizontalAim = Input.GetAxis("HorizontalAim");
        //verticalAim = Input.GetAxis("VerticalAim");
    }

    void MovePlayer()
    {
        float hypot = Mathf.Sqrt(Mathf.Pow(horizontalMove, 2) + Mathf.Pow(verticalMove, 2));
        if(hypot > 0)
        {
            horizontalMove /= hypot;
            verticalMove /= hypot;
        }

        velocity = new Vector3(horizontalMove, 0, verticalMove);

        float xPos = transform.position.x + (velocity.x * speed * Time.deltaTime);
        float zPos = transform.position.z + (velocity.z * speed * Time.deltaTime);

        transform.position = new Vector3(xPos, transform.position.y, zPos);
    }

    void LookDirection(float h, float v)
    {
        float angleOfRotation = Mathf.Atan2(h, v) * Mathf.Rad2Deg;

        currentLookDir = Mathf.Lerp(currentLookDir, angleOfRotation, 0.25f);
        transform.rotation = Quaternion.Euler(0, currentLookDir, 0);
    }

    public Vector3 GetMoveDirection()
    {
        return velocity;
    }
}
