using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_SavingLoading;

public class KS_CharacterController : KS_Behaviour {

    public enum PlayerState
    {
        idle,
        walking,
        running,
        jumping,
    }

    public camera camera;

    public float cameraYSpeed = 2f;
    public float cameraXSpeed = 4f;
    public float smooth = 2f;
    public float moveSpeed = 20f;
    public float runSpeed = 3f;
    public float jumpForce = 5f;
    public float crouchHeight = 0.5f;
    private float startHeight;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private Rigidbody rb;
    private Collider collider;
    private PlayerState state = PlayerState.idle;

    private bool running = false;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        startHeight = GetComponent<CapsuleCollider>().height;

        KS_SaveLoad.OnSave += OnSave;
        KS_SaveLoad.OnLoad += OnLoad;
	}

    void OnSave(ref Dictionary<string, object> saveGame)
    {
        saveGame.Add("KS_CC_CAM", camera.GetComponent<Camera>());
    }

    void OnLoad(KS_SaveGame save)
    {
        Debug.Log("Character load");
        Camera cam = save.SaveData["KS_CC_CAM"] as Camera;

        Camera oldCam = camera.gameObject.GetComponent<Camera>();
        oldCam = cam;
    }

    private bool IsCrouching = false;

    // Update is called once per frame
    void Update()
    {
        if (Manager.State != KS_Manager.GameState.Playing) return;

        MoveCamera(KS_Input.GetAxis("View Horizontal"), KS_Input.GetAxis("View Vertical"));
        MovePlayer(KS_Input.GetAxis("Move Horizontal"), KS_Input.GetAxis("Move Vertical"));

        if (KS_Input.GetInputDown("jump") && IsGrounded())
        {
            Jump();
        }

        if (KS_Input.GetInputDown("crouch"))
        {
            Crouch();
        }

        if (KS_Input.GetInputDown("run"))
        {
            print("Running");
            running = true;
        }

        if (KS_Input.GetInputUp("run"))
        {
            print("walking");
            running = false;
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(camera.position, -Vector3.up, collider.bounds.extents.y + 0.1f);
    }

    void Crouch()
    {
        Debug.Log("Crouch");
        if (IsCrouching)
        {
            IsCrouching = false;
            GetComponent<CapsuleCollider>().height = crouchHeight;
        }
        else
        {
            IsCrouching = true;
            GetComponent<CapsuleCollider>().height = startHeight;
        }
    }

    void Jump()
    {
        Debug.Log("Jump");
        state = PlayerState.jumping;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void MoveCamera(float X, float Y)
    {
        if (X < -1f || X > 1f) yaw += X;
        else yaw += X * cameraXSpeed;

        if (Y < -1f || Y > 1f) pitch += Y;
        else pitch += Y * cameraYSpeed;

        if (pitch > 80f) pitch = 80f;
        if (pitch < -80f) pitch = -80f;

        camera.eulerAngles = new Vector3(0, yaw, 0);
        camera.camera.localEulerAngles = new Vector3(pitch, 0, 0.0f);
    }

    void MovePlayer(float X, float Y)
    {
        Vector3 direction = new Vector3();

        direction += Vector3.left * X;
        direction += Vector3.forward * Y;

        if (running)
        {
            rb.AddRelativeForce(direction * runSpeed);
        }
        else
        {
            rb.AddRelativeForce(direction * moveSpeed);
        }
    }
}
