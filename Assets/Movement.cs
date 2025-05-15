using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Components")]
    public CharacterController controller;
    public Transform playerModel;
    public AudioSource jumpSound; // ��������� ������ �� AudioSource ��� ����� ������

    [Header("Movement Settings")]
    public float maxSpeed = 8f;
    [Tooltip("����� ������� �� 0 �� maxSpeed (�������)")]
    public float accelerationTime = 0.15f;
    [Tooltip("����� ��������� �� maxSpeed �� 0 (�������)")]
    public float decelerationTime = 0.1f;
    public float rotationSpeed = 25f;
    private float currentSpeed;
    private Vector3 currentVelocity;

    [Header("Jump Settings")]
    public float jumpHeight = 2f;
    public float gravityMultiplier = 3f;
    private float verticalVelocity;
    private bool wasGrounded; // ��� ������������ ����������� ��������� grounded

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;

    public bool IsGrounded => isGrounded;
    public float moveSpeed = 5f;

    void Update()
    {
        HandleGroundCheck();
        HandleMovement();
        HandleJump();
        ApplyGravity();
    }

    void HandleGroundCheck()
    {
        bool wasGrounded = isGrounded; // ��������� ���������� ���������
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // ���� ������ ��� ������������ ����� �������
        if (!wasGrounded && isGrounded)
        {
            // ����� �������� ���� ����������� �����, ���� �����
        }
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;

        float targetSpeed = inputDirection.magnitude > 0.1f ? maxSpeed : 0f;
        float acceleration = maxSpeed / accelerationTime;
        float deceleration = maxSpeed / decelerationTime;

        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            targetSpeed,
            (targetSpeed > currentSpeed ? acceleration : deceleration) * Time.deltaTime
        );

        if (inputDirection.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            playerModel.rotation = Quaternion.Slerp(
                playerModel.rotation,
                Quaternion.Euler(0, targetAngle, 0),
                rotationSpeed * Time.deltaTime
            );
        }

        if (currentSpeed > 0.1f)
        {
            Vector3 moveDirection = playerModel.forward * currentSpeed;
            controller.Move(moveDirection * Time.deltaTime);
        }
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);

            // ����������� ���� ������
            if (jumpSound != null)
            {
                jumpSound.Play();
            }
        }
    }

    void ApplyGravity()
    {
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -3f;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }
}