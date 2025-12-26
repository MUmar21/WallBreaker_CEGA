using UnityEngine;

public class SensorMaster : MonoBehaviour
{
    public enum SensorType
    {
        Raycast,
        SphereCast,
        BoxCast,
        OverlapSphere
    }

    [Header("Toggle Sensors")]
    public SensorType sensorType;

    [Header("Settings")]
    public float distance = 5f;
    public float radius = 0.5f;
    public Vector3 boxSize = Vector3.one;

    void OnDrawGizmos()
    {
        // 1. Raycast (Red)
        if (sensorType == SensorType.Raycast)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * distance);
        }

        // 2. SphereCast (Green)
        if (sensorType == SensorType.SphereCast)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, radius); // Start
            Gizmos.DrawWireSphere(transform.position + (transform.forward * distance), radius); // End
            Gizmos.DrawLine(transform.position, transform.position + (transform.forward * distance)); // Path
        }

        // 3. BoxCast (Blue)
        if (sensorType == SensorType.BoxCast)
        {
            Gizmos.color = Color.cyan;
            // Align the Gizmo with the object's rotation
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
            Gizmos.DrawWireCube(Vector3.zero, boxSize); // Start
            Gizmos.DrawWireCube(Vector3.forward * distance, boxSize); // End
            Gizmos.matrix = Matrix4x4.identity; // Reset matrix
        }

        // 4. OverlapSphere (Yellow)
        if (sensorType== SensorType.OverlapSphere)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}