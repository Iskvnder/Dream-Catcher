using UnityEngine;

public class DreamGenerator : MonoBehaviour
{
    public GameObject dreamPrefab;   // The prefab of the ball to generate
    public float spawnInterval = 1f;    // The interval between ball spawns
    public float minSpawnX = -30f;   // The minimum X position for ball spawning
    public float maxSpawnX = 30f;    // The maximum X position for ball spawning
    public float dreamSpeed = 5f;    // The speed at which the ball moves along the Z axis

    private float timeSinceLastSpawn = 0f;    // The time since the last ball spawn

    void Update()
    {
        // Update the time since the last ball spawn
        timeSinceLastSpawn += Time.deltaTime;
        
        // If the spawn interval has passed, spawn a ball
        if (timeSinceLastSpawn >= spawnInterval)
        {
            // Reset the time since the last ball spawn
            timeSinceLastSpawn = 0f;

            // Generate a random X position for the ball
            float spawnX = Random.Range(minSpawnX, maxSpawnX);

            // Spawn the ball at the random X position and at the same Y and Z positions as the generator
            GameObject newDream = Instantiate(dreamPrefab, new Vector3(spawnX, transform.position.y, transform.position.z), Quaternion.identity);

            // Move the ball to the target position
            Vector3 targetPosition = new Vector3(0f, 2f, 0f); // replace with your desired target position
            Rigidbody dreamRigidbody = newDream.GetComponent<Rigidbody>();
            Vector3 direction = (targetPosition - newDream.transform.position).normalized;
            dreamRigidbody.velocity = direction * dreamSpeed;
        }
    }


}