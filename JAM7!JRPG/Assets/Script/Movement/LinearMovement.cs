using UnityEngine;

public class LinearMovement : Movement
{
    public float speed;
    public Vector3 orientation;
    public Vector3 initialPosition;
    public float spawnTime;

    private void OnEnable()
    {
        orientation = orientation.normalized;
        transform.up = orientation;
        transform.position = initialPosition;

        if (spawnTime == 0)
            spawnTime = Time.time;
    }

    private void OnDisable()
    {
        spawnTime = 0;
    }

    private void Update()
    {
		transform.position = initialPosition + (Time.time - spawnTime) * orientation * speed;
	}

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.2f);
        Gizmos.DrawLine(transform.position, transform.position + orientation.normalized);
    }
#endif
}
