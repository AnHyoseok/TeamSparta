using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    public float scrollSpeed = 1f;

    private float backgroundWidth;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;

     
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            backgroundWidth = sr.bounds.size.x;
        }
        else
        {
        
            sr = GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
                backgroundWidth = sr.bounds.size.x;
        }
    }

    void Update()
    {
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        if (transform.position.x <= startPosition.x - backgroundWidth)
        {
            transform.position += Vector3.right * backgroundWidth * 2f;
        }
    }
}
