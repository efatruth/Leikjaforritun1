using UnityEngine;

[ExecuteInEditMode]
public class GroundCheck : MonoBehaviour
{
    [Tooltip("Maximum distance from the ground.")]
    public float distanceThreshold = .15f;

    [Tooltip("Whether this transform is grounded now.")]
    public bool isGrounded = true;
    /// <summary>
    /// Called when the ground is touched again.
    /// Kallað þegar jörð er snert aftur.
    /// </summary>
    public event System.Action Grounded;

    const float OriginOffset = .001f;
    Vector3 RaycastOrigin => transform.position + Vector3.up * OriginOffset;
    float RaycastDistance => distanceThreshold + OriginOffset;


    void LateUpdate()
    {
        // Check if we are grounded now.
        // Athugaðu hvort við erum jarðbundin núna.
        bool isGroundedNow = Physics.Raycast(RaycastOrigin, Vector3.down, distanceThreshold * 2);

        // Call event if we were in the air and we are now touching the ground.
        // Hringdu í atburði ef við værum í loftinu og við erum núna að snerta jörðina.
        if (isGroundedNow && !isGrounded)
        {
            Grounded?.Invoke();
        }

        // Update isGrounded.
        // Uppfærsla er Jarðbundið.
        isGrounded = isGroundedNow;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a line in the Editor to show whether we are touching the ground.
        // Teiknaðu línu í ritlinum til að sýna hvort við séum að snerta jörðina.
        Debug.DrawLine(RaycastOrigin, RaycastOrigin + Vector3.down * RaycastDistance, isGrounded ? Color.white : Color.red);
    }
}
