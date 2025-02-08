using UnityEngine;

public class CameraMultipleTeleports : MonoBehaviour
{
    public Transform player;                // Reference to the player
    public Transform[] teleportPositions;   // Array of possible teleport positions
    public float[] teleportBoundaries;      // Array of Y boundaries at which the camera teleports

    private Vector3 initialCameraPosition;

    void Start()
    {
        // Store the initial position of the camera
        initialCameraPosition = transform.position;
    }

    void Update()
    {
        // Get the player's current position
        Vector3 playerPosition = player.position;

        // Loop through the teleport boundaries
        for (int i = 0; i < teleportBoundaries.Length; i++)
        {
            // If the player's Y position exceeds the current teleport boundary, teleport the camera
            if (playerPosition.y > teleportBoundaries[i])
            {
                // Teleport the camera to the corresponding position
                transform.position = teleportPositions[i].position;
            }
        }

        // Optionally, keep the camera at the initial position if no boundaries are exceeded
        if (playerPosition.y <= teleportBoundaries[0])
        {
            transform.position = initialCameraPosition;
        }
    }
}




