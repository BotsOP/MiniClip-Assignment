using UnityEngine;

namespace Components.Entities.Enemies.Moles
{
    public class MoleViewer : MonoBehaviour, IMoleViewer
    {
        [SerializeField] private Animator animator;
        public Animator GetAnimator()
        {
            return animator;
        }
        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}
