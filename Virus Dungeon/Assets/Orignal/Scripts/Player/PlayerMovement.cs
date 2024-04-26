using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

public class PlayerMovement : PlayerComponent
{
    public static PlayerMovement instance;
    public float speed = 6f;
    public float turnSpeed = 20f;
    public Vector3 movement { get; private set; }
    public Animator anim { get; private set; }
    public CharacterController cc;
    public int floorMask;
    float camRayLength = 100f;
    public Vector3 playerToMouse;

    Quaternion m_Rotation = Quaternion.identity;
    public Transform rotatePart;

    bool _mouseButtonDown;
    bool _isSlowed;
    float _resumeSlowDownTimestamp;
    float _slowValue;
    public bool disableMove;
    public bool simulateMoveForward;

    private void Awake()
    {
        instance = this;
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        _isSlowed = false;
    }

    private void Update()
    {
        if (simulateMoveForward)
        {
            Move(0, 1);
            Animating(0, 1);
            rotatePart.forward = Vector3.forward;
        }

        if (disableMove)
            return;
        if (com.GameTime.timeScale == 0)
            return;

        if (Input.GetMouseButtonDown(0))
            _mouseButtonDown = true;
        if (Input.GetMouseButtonUp(0))
            _mouseButtonDown = false;

        float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        float v = CrossPlatformInputManager.GetAxisRaw("Vertical");
        Move(h, v);
        Animating(h, v);

        TurningMouse();

        if (_isSlowed && com.GameTime.time > _resumeSlowDownTimestamp)
            _isSlowed = false;
    }

    public void SlowSpeed(float duration, float slowValue)
    {
        _isSlowed = true;
        _resumeSlowDownTimestamp = com.GameTime.time + duration;
        _slowValue = slowValue;
    }

    void Move(float h, float v)
    {
        if (com.GameTime.timeScale == 0)
            return;
        movement = new Vector3(h, 0f, v);
        //movement = movement.normalized * speed * Time.deltaTime;
        var realSpeed = speed;
        if (_isSlowed)
            realSpeed -= _slowValue;

        if (cc.enabled)
            cc.SimpleMove(movement.normalized * realSpeed);
    }

    void TurningKeyboard()
    {
        //Vector3 desiredForward = Vector3.RotateTowards(transform.forward, movement, turnSpeed * Time.deltaTime, 0f);
        //m_Rotation = Quaternion.LookRotation(desiredForward);
        var rotateDir = movement;
        rotateDir.y = 0;
        if (rotateDir.magnitude == 0)
            return;
        rotatePart.forward = rotateDir.normalized;
    }

    void TurningMouse()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if (Physics.Raycast(camRay, out floorHit, camRayLength, (1 << floorMask)))
        {
            playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;
            //Debug.Log(playerToMouse.normalized);
            rotatePart.forward = playerToMouse.normalized;
        }
    }

    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }
}