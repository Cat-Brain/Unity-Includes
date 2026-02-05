using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPCamera : MonoBehaviour
{
    [Tooltip("This will be rotated around its local Y axis")]
    public Transform playerBody;

    public PlayerInput playerInput;
    public InputActionReference lookAction;
    public List<string> sensitivityStrings;
    public List<float> sensitivityValues;
    public float minLookAngle, maxLookAngle;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Vector2 input = GetSensitivity() * lookAction.action.ReadValue<Vector2>() * Time.timeScale;

        playerBody.Rotate(0, input.x, 0);

        float currentVertical = CMath.Rot0To360IntoN180To180(transform.localEulerAngles.x);
        currentVertical -= input.y; // <- Subtract because a negative rotation looks up
        currentVertical = Mathf.Clamp(currentVertical, minLookAngle, maxLookAngle);
        transform.localRotation = Quaternion.Euler(currentVertical, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    public float GetSensitivity()
    {
        return sensitivityValues[sensitivityStrings.IndexOf(playerInput.currentControlScheme)];
    }
}
