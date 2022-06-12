using UnityEditor;
using UnityEngine;

namespace Gameplay.Management
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Levels/New Level Data", order = 1)]
    public class Level : ScriptableObject
    {
        [SerializeField] public SceneAsset scene;
        [SerializeField] private int fishRequired;
    }
}