using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{       
    public float cameraPadding = 6.0f;

    public void SetCamera(int mapWidth, int mapHeight) {
        // Set the camera position to the center of the map
        transform.position = new Vector3(mapWidth / 2.0f, mapHeight / 2.0f, -10.0f );

        // Set the camera size based on the map size and the padding value
        float cameraSize = Mathf.Max(mapWidth, mapHeight) / 2.0f + cameraPadding;
        GetComponent<Camera>().orthographicSize = cameraSize;
    }
}
