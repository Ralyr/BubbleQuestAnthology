using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Camera should follow the player.
    GameObject playerObj;
    Vector3 pos;

    private void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        pos = new Vector3(playerObj.transform.position.x, playerObj.transform.position.y, -10f);
    }

    void UpdatePos()
    {
        pos.x = playerObj.transform.position.x;
        pos.y = playerObj.transform.position.y;
    }

    private void Update()
    {
        UpdatePos();
        transform.position = pos;
    }

}
