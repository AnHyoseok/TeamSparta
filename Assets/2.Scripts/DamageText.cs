using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float lifeTime = 1f;

    private TextMeshPro text;
    private Vector3 floatDirection = Vector3.up;
    private float fadeDuration = 0.5f;
    private float timer = 0f;

    void Start()
    {
        text = GetComponent<TextMeshPro>();
        Destroy(gameObject, lifeTime); 
    }

    void Update()
    {
        timer += Time.deltaTime;

    
        transform.position += floatDirection * floatSpeed * Time.deltaTime;

        
        if (text != null)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        }
    }

    public void SetText(string value)
    {
        if (text == null) text = GetComponent<TextMeshPro>();
        text.text = value;
    }
}
