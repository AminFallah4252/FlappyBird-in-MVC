using UnityEngine;

namespace Views
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PatternLooper : MonoBehaviour
    {
        private float speed = 1f;
        private float width = 6f;
        private SpriteRenderer spriteRenderer;
        private Vector2 startSize;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            startSize = spriteRenderer.size;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            spriteRenderer.size = new Vector2(spriteRenderer.size.x + speed * Time.fixedDeltaTime, spriteRenderer.size.y);
            if (spriteRenderer.size.x > width)
            {
                spriteRenderer.size = startSize;
            }
        }
    }
}