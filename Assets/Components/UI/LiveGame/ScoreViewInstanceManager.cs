using Components.ObjectPool;
using PrimeTween;
using UnityEngine;

namespace Components.UI.LiveGame
{
    public interface IScoreViewInstanceManager
    {
        void SetPoolObjectManager(IObjectPoolManager objectPoolManager);
        void InstanceScore(int score, Vector3 worldPos);
    }

    public class ScoreViewInstanceManager : MonoBehaviour, IScoreViewInstanceManager
    {
        [SerializeField] private ScoreWorldUI scorePrefab;
        [SerializeField] private float yPosStart = 1.5f;
        [SerializeField] private float yPosEnd = 3;
        [SerializeField] private AnimationCurve animationCurve;
        [SerializeField] private float duration = 0.5f;
        private IObjectPoolManager objectPoolManager;

        public void SetPoolObjectManager(IObjectPoolManager objectPoolManager)
        {
            this.objectPoolManager = objectPoolManager;
            objectPoolManager.CreatePool(scorePrefab, new DefaultPoolLifecycleStrategy());
        }
        public void InstanceScore(int score, Vector3 worldPos)
        {
            objectPoolManager.Spawn(scorePrefab, out PoolObject instance);
            ScoreWorldUI scoreInstance = (ScoreWorldUI)instance;
            scoreInstance.transform.parent = transform;
            scoreInstance.scoreText.text = score.ToString();
            scoreInstance.transform.position = new Vector3(worldPos.x, yPosStart, worldPos.z);
            scoreInstance.scoreText.fontMaterial = new Material(scoreInstance.scoreText.fontMaterial);

            int alphaID = Shader.PropertyToID("_FaceColor");
            scoreInstance.scoreText.fontMaterial.SetColor(alphaID, Color.white);
            Sequence.Create()
                .Group(Tween.PositionY(scoreInstance.transform, yPosEnd, duration, Easing.Curve(animationCurve)))
                .Group(Tween.MaterialAlpha(scoreInstance.scoreText.fontMaterial, alphaID, 0, duration, Easing.Curve(animationCurve)))
                .ChainCallback(() => objectPoolManager.Release(instance));
        }
    }
}
