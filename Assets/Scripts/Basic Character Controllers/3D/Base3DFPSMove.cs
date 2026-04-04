using UnityEngine;
using UnityEngine.InputSystem;
using ClownLib;

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
            velocity = velocity.XZ().TryAdd2(
                Time.deltaTime * accel * input.normalized, speed).XZ_Y(velocity.y);
        else
            velocity = velocity.XZ().TrySub2(Time.deltaTime * accel).XZ_Y(velocity.y);
    }
}
