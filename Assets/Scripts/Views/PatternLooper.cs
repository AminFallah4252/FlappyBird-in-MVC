using UnityEngine;

namespace Views
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PatternLooper : MonoBehaviour
    {
        [Range(-1f, 1f)] [SerializeField] private float speed = .35f;
        private float width;
        private SpriteRenderer spriteRenderer;
        private Vector2 startSize;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            startSize = spriteRenderer.size;
            width = startSize.x * 10;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            spriteRenderer.size =
                new Vector2(spriteRenderer.size.x + speed * Time.fixedDeltaTime, spriteRenderer.size.y);
            if (Mathf.Approximately(spriteRenderer.size.x, width))
            {
                spriteRenderer.size = startSize;
            }
        }
    }
}