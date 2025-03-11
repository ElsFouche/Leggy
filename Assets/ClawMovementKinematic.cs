    using UnityEngine;
    using UnityEngine.InputSystem;

    public class ClawMovementKinematic : MonoBehaviour
    {
        public GameObject hitobjectFlag;
        public BoxCollider Wrist_Kinematic_Collider;
        public float moveSpeed = 1.0f;
        public bool canClose = true;

        private Vector3 position;
        private Collider clawCollider;
        private ClawParent clawParent;
        private float castDistance;
        private RaycastHit hitResult;
        private bool castHit = false;
        private float successfulGrabRange = 0.5f;
        private float distanceToCenter;
        public GameObject hitObject;
        public bool playerMovement = false;
        private bool negativeDirection = false;
        private GameObject clawL;
        private GameObject clawR;

        // Input System
        private InputAction moveClawAction;
        private InputAction openClawAction;
        private InputAction closeClawAction;

        private float moveInput;
        private bool openClawInput;
        private bool closeClawInput;
        private float stopMin, stopMax;

        private void Awake()
        {
            var controls = new ClawControls();
            openClawAction = controls.Player.OpenClaw;
            closeClawAction = controls.Player.CloseClaw;

            openClawAction.performed += ctx => openClawInput = true;
            openClawAction.canceled += ctx => openClawInput = false;

            closeClawAction.performed += ctx => closeClawInput = true;
            closeClawAction.canceled += ctx => closeClawInput = false;
        }

        private void OnEnable()
        {
            openClawAction.Enable();
            closeClawAction.Enable();
        }

        private void OnDisable()
        {
            openClawAction.Disable();
            closeClawAction.Disable();
        }

        void Start()
        {
            clawParent = transform.parent.GetComponent<ClawParent>();
            clawCollider = GetComponent<Collider>();
            castDistance = clawParent.castDistance;
            successfulGrabRange = clawParent.maxGrabRange;

            clawL = GameObject.Find("Claw_L");
            clawR = GameObject.Find("Claw_R");

            // Set correct stop limits and movement direction
            if (transform.localPosition.x < 0) // Left Claw
            {
                negativeDirection = true;
                stopMin = -2f;
                stopMax = -0.05f;
            }
            else // Right Claw
            {
                negativeDirection = false;
                stopMin = 0.05f;
                stopMax = 2f;
            }
        }

        void Update()
        {
            position = transform.localPosition;

            // Move claw with Left/Right Stick or Q/E keys
            if (moveInput != 0)
            {
                position.x = Mathf.Clamp(position.x + (moveInput * moveSpeed * Time.deltaTime), stopMin, stopMax);
                transform.localPosition = position;
                playerMovement = true;
            }
            else
            {
                playerMovement = false;
            }

        
            if (closeClawInput && canClose) 
            {
                if (negativeDirection) 
                    position.x = Mathf.Clamp(position.x + (moveSpeed * Time.deltaTime), stopMin, stopMax);
                else 
                    position.x = Mathf.Clamp(position.x - (moveSpeed * Time.deltaTime), stopMin, stopMax);

                playerMovement = true;
                transform.localPosition = position;
            }
            else if (openClawInput)
            {
                if (negativeDirection)
                    position.x = Mathf.Clamp(position.x - (moveSpeed * Time.deltaTime), stopMin, stopMax);
                else
                    position.x = Mathf.Clamp(position.x + (moveSpeed * Time.deltaTime), stopMin, stopMax);
                playerMovement = true;
                transform.localPosition = position;
                //clawParent.clawIsGrabbing();
            
            }

            // BoxCast for detecting objects
            castHit = Physics.BoxCast(clawCollider.bounds.center,
                                      transform.localScale * 0.5f,
                                      transform.right,
                                      out hitResult,
                                      transform.localRotation,
                                      castDistance);

            if (castHit && hitResult.transform.CompareTag("Grabbable"))
            {
                hitObject = hitResult.transform.gameObject;
                distanceToCenter = Vector3.Distance(hitObject.GetComponent<Rigidbody>().centerOfMass, clawCollider.bounds.center);

                if (!gameObject.CompareTag(hitObject.tag))
                {
                    canClose = false;
                }

                if (!(hitObject.CompareTag("ClawL") || hitObject.CompareTag("ClawR")))
                {
                    hitobjectFlag.transform.position = hitObject.transform.position;

                    if (openClawInput && distanceToCenter < successfulGrabRange)
                    {
                        clawParent.clawIsGrabbing(hitObject.gameObject);
                    }
                }

                
                if (distanceToCenter > successfulGrabRange)
                {
                    Debug.Log("Too far from target.");
                    clawParent.clawIsGrabbing();
                }
            }
            else
            {
                canClose = true;
                hitObject = null;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // Check if the object is grabbable
            if (other.CompareTag("Grabbable"))
            {
                hitObject = other.gameObject;
                hitobjectFlag.transform.position = hitObject.transform.position;

                if (openClawInput && Vector3.Distance(hitObject.transform.position, transform.position) < successfulGrabRange)
                {
                    clawParent.clawIsGrabbing(hitObject); 
                }
            }
        }

        private void OnTriggerStary(Collider other)
    {
        // Check if the object is grabbable
        if (other.CompareTag("Grabbable"))
        {
            hitObject = other.gameObject;
            hitobjectFlag.transform.position = hitObject.transform.position;

            if (openClawInput && Vector3.Distance(hitObject.transform.position, transform.position) < successfulGrabRange)
            {
                clawParent.clawIsGrabbing(hitObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Grabbable"))
            {
            
                if (hitObject == other.gameObject)
                {
                    hitObject = null;
                    canClose = true;
                }
            }
        }
    }
