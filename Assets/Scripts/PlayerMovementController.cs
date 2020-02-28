using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    float playerSpeed = 0.5f;
    Magic equipedMagic;

    // Start is called before the first frame update
    void Start()
    {
        equipedMagic = gameObject.AddComponent<Fire>();
        transform.forward = -Camera.main.transform.forward;
        transform.up = Camera.main.transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += playerSpeed * new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += playerSpeed * new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += playerSpeed * new Vector3(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += playerSpeed * new Vector3(0, 0, -1);
        }

        if (Input.GetMouseButton(0))
        {
            equipedMagic.Cast();
        }
    }
}
