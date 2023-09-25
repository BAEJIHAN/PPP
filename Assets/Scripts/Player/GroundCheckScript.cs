using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckScript : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Boxcast Property")]
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask groundLayer;

    [Header("Debug")]
    [SerializeField] private bool drawGizmo;

    private void OnDrawGizmos()
    {
        if (!drawGizmo) return;

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position+Vector3.up, Vector3.down);
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position+ Vector3.up, Vector3.down, maxDistance, groundLayer);
    }
}
