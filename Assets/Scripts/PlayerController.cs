using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float moveSpeed = 2f;
    public CharacterController cc;
    private float yAxisLocation = 0.8f;

    //Player states
    public bool isBlind = false;
    public bool isDeaf = false;
    public bool isMute = true;

    //Player hands
    public GameObject defaultHands;
    public GameObject runningHands;
    public GameObject blindHands;

    void Update()
    {
        Move();
    }

    private void Move()
    {
        //defaul state, always walking forward in the direction the player faces
        float horizontal = 0f;
        float vertical = 1f;

        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        cc.Move(move * moveSpeed * Time.deltaTime);

        //movement while player is blind (can't move)
        if (Input.GetKeyDown(KeyCode.B))
        {
            isMute = false;
            isDeaf = false;
            isBlind = true;

            defaultHands.SetActive(false);
            runningHands.SetActive(false);
            blindHands.SetActive(true);

            moveSpeed = 0f;
            cc.Move(move * moveSpeed * Time.deltaTime);
        }

        //movement while player is deaf (running)
        if (Input.GetKeyDown(KeyCode.N))
        {
            isMute = false;
            isBlind = false;
            isDeaf = true;

            defaultHands.SetActive(false);
            blindHands.SetActive(false);
            runningHands.SetActive(true);

            moveSpeed = 4f;
            cc.Move(move * moveSpeed * Time.deltaTime);
        }

        //movement while player is mute (default, walk)
        if (Input.GetKeyDown(KeyCode.M))
        {
            isDeaf = false;
            isBlind = false;
            isMute = true;

            runningHands.SetActive(false);
            blindHands.SetActive(false);
            defaultHands.SetActive(true);

            moveSpeed = 2f;
            cc.Move(move * moveSpeed * Time.deltaTime);
        }

        //Check if floating
        if (transform.position.y > yAxisLocation)
        {
            move.y = (yAxisLocation - transform.position.y - 0.05f) * 0.9f;
            cc.Move(move * Time.deltaTime);
        }
    }
}
