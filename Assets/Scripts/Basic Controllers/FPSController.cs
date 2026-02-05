using com.cyborgAssets.inspectorButtonPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSController : MonoBehaviour
{
    public Rigidbody rb;
    public Collider col;

    public InputActionReference movementAction, jumpAction;

    public float accel, decel, speed;
    public float jumpForce, earlyJumpTime, cayoteJumpTime;
    [HideInInspector] public float earlyJumpTimer = 0, cayoteJumpTimer = 0;
    public float groundedOffset, groundedRadius;
    public LayerMask groundMask;

    public void OnJumpPress(InputAction.CallbackContext context)
    {
        if (context.performed)
            earlyJumpTimer = earlyJumpTime;
    }

    private void Awake()
    {
        jumpAction.action.performed += OnJumpPress;
    }

    private void FixedUpdate()
    {
        bool isGrounded = IsGrounded();

        if (isGrounded)
            cayoteJumpTimer = cayoteJumpTime;
        else
            cayoteJumpTimer = Mathf.Max(0, cayoteJumpTimer - Time.deltaTime);

        if (earlyJumpTimer > 0 && cayoteJumpTimer > 0)
        {
            ActivateJump();
            earlyJumpTimer = 0;
            cayoteJumpTimer = 0;
        }

        earlyJumpTimer = Mathf.Max(0, earlyJumpTimer - Time.deltaTime);

        Vector3 velocity = transform.InverseTransformDirection(rb.linearVelocity);

        Vector2 input = movementAction.action.ReadValue<Vector2>();

        if (input == Vector2.zero)
            velocity = CMath.Vector3XZ_Y(CMath.TrySub2(CMath.Vector3ToXZ(velocity), Time.deltaTime * decel), velocity.y);
        else
            velocity = CMath.Vector3XZ_Y(CMath.TryAdd2(CMath.Vector3ToXZ(velocity), Time.deltaTime * accel * input.normalized, speed), velocity.y);

        rb.linearVelocity = transform.TransformDirection(velocity);
    }

    [ProPlayButton]
    public void ActivateJump()
    {
        Vector3 velocity = transform.InverseTransformDirection(rb.linearVelocity);

        velocity.y = jumpForce + Mathf.Max(0, velocity.y);

        rb.linearVelocity = transform.TransformDirection(velocity);
    }

    public bool IsGrounded()
    {
        int originalLayer = gameObject.layer;
        gameObject.layer = 2;
        bool result = Physics.CheckSphere(transform.position - transform.up * groundedOffset, groundedRadius, groundMask);
        gameObject.layer = originalLayer;
        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position - transform.up * groundedOffset, groundedRadius);
    }
}
