using UnityEngine;

namespace Interfaces
{
    public interface IBirdController
    {
        GameObject GetGameObject();
        void SetParent(Transform parent);
        void OnPlayerFlap();
    }
}