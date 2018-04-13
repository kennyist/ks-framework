using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core;
using KS_Core.IO;
using KS_Core.Input;

public class KS_CharacterController : KS_Behaviour {

    public enum PlayerState
    {
        idle,
        walking,
        running,
        jumping,
        falling,
        swimming,
        underwater,
    }

    public Transform camera;

    public float cameraYSpeed = 2f;
    public float cameraXSpeed = 4f;
    public float smooth = 2f;
    public float movementSmooth = 2f;
    public float moveSpeed = 20f;
    public float swimSpeed = 20f;
    public float runSpeed = 3f;
    public float jumpForce = 5f;
    public float crouchHeight = 0.5f;
    private float startHeight;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private Rigidbody rb;
    private Collider collider;
    public PlayerState state = PlayerState.idle;

    public bool test = false;

    private bool running = false;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        startHeight = GetComponent<CapsuleCollider>().height;

        KS_SaveLoad.OnSave += OnSave;
        KS_SaveLoad.OnLoad += OnLoad;
	}

    private void OnDestroy()
    {
        KS_SaveLoad.OnSave -= OnSave;
        KS_SaveLoad.OnLoad -= OnLoad;
    }

    void OnSave(ref Dictionary<string, object> saveGame)
    {
        Debug.Log("ROTATION Save: " + transform.rotation);
        saveGame.Add("KS_CC_CAM", camera.GetComponent<Camera>());
        Dictionary<string, string> toSave = new Dictionary<string, string>();
        toSave.Add("pitch", pitch.ToString());
        toSave.Add("yaw", yaw.ToString());
        saveGame.Add("KS_CC", toSave);
    }

    void OnLoad(KS_SaveGame save)
    {
        Debug.Log("ROTATION load: " + transform.rotation);
        Camera cam = camera.GetComponent<Camera>();
        Camera oldcam = (Camera)save.SaveData["KS_CC_CAM"];

        cam.depth = oldcam.depth;
        cam.clearFlags = oldcam.clearFlags;
        cam.backgroundColor = oldcam.backgroundColor;
        cam.cullingMask = oldcam.cullingMask;
        cam.transform.rotation = oldcam.transform.rotation;

        Destroy(oldcam);

        Dictionary<string, string> fromSave = (Dictionary<string, string>) save.SaveData["KS_CC"];

        pitch = float.Parse(fromSave["pitch"]);
        yaw = float.Parse(fromSave["yaw"]);
    }

    private bool IsCrouching = false;

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Manager.State != KS_Manager.GameState.Playing) return;


        MoveCamera(KS_Input.GetAxis("View Horizontal"), KS_Input.GetAxis("View Vertical"));
        MovePlayer(KS_Input.GetAxis("Move Horizontal"), KS_Input.GetAxis("Move Vertical"));

        currentVelocity = Vector3.Slerp(currentVelocity, targetVelocity, Time.deltaTime * movementSmooth);

        rb.AddRelativeForce(targetVelocity, ForceMode.Force);

        if (IsGrounded() || IsSwimming) rb.drag = 2.5f;
        else rb.drag = 1;

        if (KS_Input.GetInputDown("jump"))
        {
            if ((IsGrounded() || IsSwimming))
            {
                Jump();
            }
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
        return Physics.Raycast(transform.position, -Vector3.up, collider.bounds.extents.y + 0.1f);
    }

    public bool IsSwimming = false;

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

        transform.eulerAngles = new Vector3(0, yaw, 0);
        camera.GetComponent<Transform>().localEulerAngles = new Vector3(pitch, 0, 0.0f);
    }

    Vector3 currentVelocity = new Vector3();
    Vector3 targetVelocity = new Vector3();

    void MovePlayer(float X, float Y)
    {
        Vector3 direction = new Vector3();

        direction -= Vector3.right * X;
        direction += Vector3.forward * Y;

        if(state == PlayerState.swimming || state == PlayerState.underwater)
        {
            targetVelocity = direction * swimSpeed;
        }
        else if (state == PlayerState.jumping || state == PlayerState.running || state == PlayerState.idle || state == PlayerState.walking)
        {
            if (running) targetVelocity = direction * runSpeed;
            else
            {
                targetVelocity = direction * moveSpeed;
            }
        }
    }
}
