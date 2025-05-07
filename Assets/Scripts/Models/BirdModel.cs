using UnityEngine;
using UnityEngine.Serialization;

namespace Models
{
    [CreateAssetMenu(fileName = "BirdModel", menuName = "FlappyBird/BirdModel", order = 1)]
    public class BirdModel : ScriptableObject
    {
        [SerializeField] private float birdSpeed;
        [SerializeField] private float birdRotationSpeed;

        public Vector2 GetJumpVelocity()
        {
            return Vector2.up * birdSpeed;
        }

        public float GetRotation(float rbLinearVelocityY)
        {
            return rbLinearVelocityY * birdRotationSpeed;
        }
    }
}