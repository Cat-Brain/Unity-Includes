using UnityEngine;
using UnityEngine.InputSystem;

public class Base3DFreeCam : Base3DFPSMove
{
    public override void UpdateVel(ref Vector3 velocity)
    {
        base.UpdateVel(ref velocity);
        velocity.y = CMath.TrySub(velocity.y, accel * Time.deltaTime);
    }
}
