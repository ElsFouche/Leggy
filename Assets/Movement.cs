using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    private Vector3 position;
    private Vector3 rotation;

    public float minX = -10f, maxX = 10f;
    public float minY = 0f, maxY = 5f;
    public float minZ = -5f, maxZ = 5f;

    private void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        position = transform.position;

        // Movement
        if (Input.GetKey(KeyCode.A))
        {
            position.x = Mathf.Clamp(position.x - (moveSpeed * Time.deltaTime), minX, maxX);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            position.x = Mathf.Clamp(position.x + (moveSpeed * Time.deltaTime), minX, maxX);
        }

        if (Input.GetKey(KeyCode.W))
        {
            position.z = Mathf.Clamp(position.z + (moveSpeed * Time.deltaTime), minZ, maxZ);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            position.z = Mathf.Clamp(position.z - (moveSpeed * Time.deltaTime), minZ, maxZ);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            position.y = Mathf.Clamp(position.y + (moveSpeed * Time.deltaTime), minY, maxY);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            position.y = Mathf.Clamp(position.y - (moveSpeed * Time.deltaTime), minY, maxY);
        }

        transform.position = position;

        // Rotation
        rotation = transform.localEulerAngles;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rotation.z -= moveSpeed * 10 * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rotation.z += moveSpeed * 10 * Time.deltaTime;
        }
        transform.localEulerAngles = rotation;
    }
}
