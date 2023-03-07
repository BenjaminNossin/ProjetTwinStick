using UnityEngine;

public class BasicAI : MonoBehaviour
{
    [SerializeField, Range(1, 20)] float unitsPerSeconds = 10; 
    private Vector3 direction;

    private void Start()
    {
        direction = (GameObject.Find("Floor").transform.position - transform.position).normalized;     
    }

    void FixedUpdate()
    {
        transform.Translate(Time.deltaTime * unitsPerSeconds * direction, Space.Self);
    }
}
