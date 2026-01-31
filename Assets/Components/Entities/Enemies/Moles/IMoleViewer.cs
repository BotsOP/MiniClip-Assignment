using UnityEngine;

namespace Components.Entities.Enemies.Moles
{
    public interface IMoleViewer
    {
        public Animator GetAnimator();
        public GameObject GetGameObject();
    }
}
