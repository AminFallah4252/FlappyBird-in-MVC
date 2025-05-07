using UnityEngine;

namespace Models
{
    [CreateAssetMenu(fileName = "PipesModel", menuName = "FlappyBird/PipesModel", order = 2)]
    public class PipesModel : ScriptableObject
    {
        [SerializeField] float pipeSpeed;
        [SerializeField] float pipesInterval;
        [SerializeField] float pipesHeightRange;

        public float GetRandomYPosition()
        {
            return Random.Range(-pipesHeightRange, pipesHeightRange);
        }

        public Vector3 GetPipeNextPosition()
        {
            return Vector3.left * (pipeSpeed * Time.deltaTime);
        }

        public float GetPipesInterval() => pipesInterval;
    }
}