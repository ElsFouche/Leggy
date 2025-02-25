using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    private Vector3 position;
    private Vector3 rotation;

    private void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            position = transform.position;
            position.x += (-moveSpeed * Time.deltaTime);
            transform.position = position;
        } else if (Input.GetKey(KeyCode.S))
        {
            position = transform.position;
            position.z += (-moveSpeed * Time.deltaTime);
            transform.position = position;
        } else if (Input.GetKey(KeyCode.D))
        {
            position = transform.position;
            position.x += (moveSpeed * Time.deltaTime);
            transform.position = position;
        } else if (Input.GetKey(KeyCode.W))
        {
            position = transform.position;
            position.z += (moveSpeed * Time.deltaTime);
            transform.position = position;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            position = transform.position;
            position.y += (moveSpeed * Time.deltaTime);
            transform.position = position;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            position = transform.position;
            position.y -= (moveSpeed * Time.deltaTime);
            transform.position = position;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rotation = transform.localEulerAngles;
            rotation.z -= (moveSpeed * 10 * Time.deltaTime);
            transform.localEulerAngles = rotation;
        } else if (Input.GetKey(KeyCode.RightArrow))
        {
            rotation = transform.localEulerAngles;
            rotation.z += (moveSpeed * 10 * Time.deltaTime);
            transform.localEulerAngles = rotation;
        }
    }
}
