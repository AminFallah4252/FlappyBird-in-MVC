using UnityEngine;
using Zenject;

namespace Views
{
    public class PipesView : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<PipesView>
        {
        }
    }
}