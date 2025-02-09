using UnityEngine;

public class TrainTileScroller : MonoBehaviour {
    public float speed = 5f;
    public float resetPositionX; // When train goes past this, reset
    public float startPositionX; // Where train should be repositioned

    private void Update() {
        transform.position += Vector3.left * speed * Time.deltaTime;

        // If this train is offscreen, move it back to the start
        if (transform.position.x < resetPositionX) {
            transform.position = new Vector3(startPositionX, transform.position.y, transform.position.z);
        }
    }
}
