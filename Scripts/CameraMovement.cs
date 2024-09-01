using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Transform player;
    const float zBase = -9.95f;

    public float xLeftLimit;
    public float xRightLimit;
    public float yUpperLimit;
    public float yLowerLimit;


    void Update() {
        transform.position = new Vector3(player.position.x + 7f, player.position.y - 1f, -9.95f);

        float x = transform.position.x;
        float y = transform.position.y;
        
        x = Mathf.Min(x, xRightLimit);
        x = Mathf.Max(x, xLeftLimit);

        y = Mathf.Min(y, yUpperLimit);
        y = Mathf.Max(y, yLowerLimit);

        transform.position = new Vector3(x, y, zBase);

    }

}