using UnityEngine;

public class Crouch : MonoBehaviour
{
    public KeyCode key = KeyCode.LeftControl;

    [Header("Slow Movement")]
    [Tooltip("Movement to slow down when crouched.")]
    public FirstPersonMovement movement;
    [Tooltip("Movement speed when crouched.")]
    public float movementSpeed = 2;

    [Header("Low Head")]
    [Tooltip("Head to lower when crouched.")]
    public Transform headToLower;
    [HideInInspector]
    public float? defaultHeadYLocalPosition;
    public float crouchYHeadPosition = 1;
    
    [Tooltip("Collider to lower when crouched.")]
    public CapsuleCollider colliderToLower;
    [HideInInspector]
    public float? defaultColliderHeight;

    public bool IsCrouched { get; private set; }
    public event System.Action CrouchStart, CrouchEnd;


    void Reset()
    {
        // Try to get components.
        // Reyndu að fá íhluti.
        movement = GetComponentInParent<FirstPersonMovement>();
        headToLower = movement.GetComponentInChildren<Camera>().transform;
        colliderToLower = movement.GetComponentInChildren<CapsuleCollider>();
    }

    void LateUpdate()
    {
        if (Input.GetKey(key))
        {
            // Enforce a low head.
            // Þvinga fram lágt höfuð.
            if (headToLower)
            {
                // If we don't have the defaultHeadYLocalPosition, get it now.
                // Ef við höfum ekki defaultHeadYLocalPosition, fáðu það núna.
                if (!defaultHeadYLocalPosition.HasValue)
                {
                    defaultHeadYLocalPosition = headToLower.localPosition.y;
                }

                // Lower the head.
                // Lækkaðu höfuðið.
                headToLower.localPosition = new Vector3(headToLower.localPosition.x, crouchYHeadPosition, headToLower.localPosition.z);
            }

            // Enforce a low colliderToLower.
            // Framfylgja lágan kollider ToLower.
            if (colliderToLower)
            {
                // If we don't have the defaultColliderHeight, get it now.
                // Ef við höfum ekki defaultColliderHeight, fáðu það núna.
                if (!defaultColliderHeight.HasValue)
                {
                    defaultColliderHeight = colliderToLower.height;
                }

                // Get lowering amount.
                // Fáðu lækkandi upphæð.
                if(defaultHeadYLocalPosition.HasValue)
                {
                    loweringAmount = defaultHeadYLocalPosition.Value - crouchYHeadPosition;
                }
                else
                {
                    loweringAmount = defaultColliderHeight.Value * .5f;
                }

                // Lower the colliderToLower.
                // Lækkaðu colliderToLower.
                colliderToLower.height = Mathf.Max(defaultColliderHeight.Value - loweringAmount, 0);
                colliderToLower.center = Vector3.up * colliderToLower.height * .5f;
            }

            // Set IsCrouched state.
            // Stilltu IsCrouched ástand.
            if (!IsCrouched)
            {
                IsCrouched = true;
                SetSpeedOverrideActive(true);
                CrouchStart?.Invoke();
            }
        }
        else
        {
            if (IsCrouched)
            {
                // Rise the head back up.
                // Lyftu höfðinu aftur upp.
                if (headToLower)
                {
                    headToLower.localPosition = new Vector3(headToLower.localPosition.x, defaultHeadYLocalPosition.Value, headToLower.localPosition.z);
                }

                // Reset the colliderToLower's height.
                // Endurstilltu hæð colliderToLower.
                if (colliderToLower)
                {
                    colliderToLower.height = defaultColliderHeight.Value;
                    colliderToLower.center = Vector3.up * colliderToLower.height * .5f;
                }

                // Reset IsCrouched.
                // Endurstilla IsCrouched.
                IsCrouched = false;
                SetSpeedOverrideActive(false);
                CrouchEnd?.Invoke();
            }
        }
    }


    #region Speed override.
    void SetSpeedOverrideActive(bool state)
    {
        // Stop if there is no movement component.
        // Stöðva ef það er enginn hreyfiþáttur.
        if(!movement)
        {
            return;
        }

        // Update SpeedOverride.
        // Uppfærðu SpeedOverride.
        if (state)
        {
            // Try to add the SpeedOverride to the movement component.
            // Reyndu að bæta SpeedOverride við hreyfihlutann.
            if (!movement.speedOverrides.Contains(SpeedOverride))
            {
                movement.speedOverrides.Add(SpeedOverride);
            }
        }
        else
        {
            // Try to remove the SpeedOverride from the movement component.
            // Reyndu að fjarlægja SpeedOverride úr hreyfihlutanum.
            if (movement.speedOverrides.Contains(SpeedOverride))
            {
                movement.speedOverrides.Remove(SpeedOverride);
            }
        }
    }

    float SpeedOverride() => movementSpeed;
    #endregion
}
