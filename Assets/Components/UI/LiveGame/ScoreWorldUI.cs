using Components.ObjectPool;
using TMPro;
using UnityEngine;

namespace Components.UI.LiveGame
{
    public class ScoreWorldUI : PoolObject
    {
        [SerializeField] public TMP_Text scoreText;
    }
}
