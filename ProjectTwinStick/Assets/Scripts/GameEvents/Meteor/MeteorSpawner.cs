using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 5f);
        Gizmos.DrawLine(transform.position, Vector3.zero);
    }
}
