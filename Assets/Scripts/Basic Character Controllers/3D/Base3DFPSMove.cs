using UnityEngine;
using UnityEngine.InputSystem;

public class Base3DFPSMove : MonoBehaviour
{
    public Rigidbody rb;
    public InputActionReference moveAction;

    public float accel, speed;

    void FixedUpdate()
    {
        Vector3 velocity = transform.InverseTransformDirection(rb.linearVelocity);
        UpdateVel(ref velocity);
        rb.linearVelocity = transform.TransformDirection(velocity);
    }

    public virtual void UpdateVel(ref Vector3 velocity)
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        if (input.sqrMagnitude > 0.05f)
            velocity = CMath.Vector3XZ_Y(CMath.TryAdd2(CMath.Vector3ToXZ(velocity),
                Time.deltaTime * accel * input.normalized, speed), velocity.y);
        else
            velocity = CMath.Vector3XZ_Y(CMath.TrySub2(CMath.Vector3ToXZ(velocity),
                Time.deltaTime * accel), velocity.y);
    }
}
