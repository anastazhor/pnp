using UnityEngine;
public class MovingPlatform : MonoBehaviour
{
    public float speed = 3.0f;
    public float distance = 5.0f;
    private Vector3 _startPosition;
    void Start()
    {
        _startPosition = transform.position;
    }
    void Update()
    {
        float delta = Mathf.PingPong(Time.time * speed, distance * 2) - distance;
        transform.position = _startPosition + new Vector3(0, 0, delta);
    }
}
