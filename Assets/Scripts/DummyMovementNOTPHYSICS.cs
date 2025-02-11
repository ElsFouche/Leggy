using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMovementNOTPHYSICS : MonoBehaviour
{
    [SerializeField] private ClawGrabChild LeggyLeftClaw;
    [SerializeField] private ClawGrabChild LeggyRightClaw;

    public float speed = 2f;
    private bool opened = true;
    private bool closing = false;
    private bool opening = false;

    public GameObject ClawLeft;
    private Vector3 ClawLeftOrigin;

    public GameObject ClawRight;
    private Vector3 ClawRightOrigin;

    public float maxClawDistance = 0.1f;

    void Start()
    {
        ClawLeftOrigin = ClawLeft.transform.localPosition;
        ClawRightOrigin = ClawRight.transform.localPosition;
    }

    void Update()
    {
        HandleClawMovement();
        EnforceSymmetricClawMovement();
        ClampClawPosition();
    }

    private void HandleClawMovement()
    {
        if (Input.GetMouseButtonDown(1) && (!opened || opening))
        {
            OpenClaw();
        }

        if (Input.GetMouseButtonDown(0) || closing)
        {
            CloseClaw();
        }
    }

    private void ClampClawPosition()
    {
        ClawLeft.transform.localPosition = new Vector3(Mathf.Clamp(ClawLeft.transform.localPosition.x, ClawLeftOrigin.x - maxClawDistance, ClawLeftOrigin.x), 0f, 0f);
        ClawRight.transform.localPosition = new Vector3(Mathf.Clamp(ClawRight.transform.localPosition.x, ClawRightOrigin.x, ClawRightOrigin.x + maxClawDistance), 0f, 0f);
    }

    private void ResetClawPosition()
    {
        opened = true;
        opening = false;
        closing = false;

        ClawRight.transform.localPosition = ClawRightOrigin;
        ClawLeft.transform.localPosition = ClawLeftOrigin;
    }

    private void OpenClaw()
    {
        opening = true;
        closing = false;
        opened = false;
        MoveClaws(-speed * Time.deltaTime);
    }

    private void CloseClaw()
    {
        opened = false;
        opening = false;
        closing = true;
        MoveClaws(speed * Time.deltaTime);
    }

    private void MoveClaws(float movement)
    {
        ClawLeft.transform.localPosition += new Vector3(movement, 0f, 0f);
        ClawRight.transform.localPosition -= new Vector3(movement, 0f, 0f);
    }

    void EnforceSymmetricClawMovement()
    {
        float leftOffset = ClawLeft.transform.localPosition.x - ClawLeftOrigin.x;
        ClawRight.transform.localPosition = new Vector3(ClawRightOrigin.x - leftOffset, 0f, 0f);
    }
}
