using Course.Fundamentals.Core;
using UnityEngine;

namespace Course.Demos
{
    [AddComponentMenu("Course/Demos/Min Max Demo")]
    public sealed class MinMaxDemo : MonoBehaviour
    {
        [Header("Spawning")]
        [Tooltip("Seconds between spawns. Drawn by MinMaxDrawer: one row, two fields.")]
        [SerializeField] private MinMax spawnDelay = new MinMax { min = 1f, max = 3f };

        [Tooltip("Chance (0..1) that a spawn is an elite. [Range] turns the float into a slider.")]
        [SerializeField, Range(0f, 1f)] private float eliteChance = 0.1f;

        [Header("Limits")]
        [Tooltip("[Min] clamps the value but shows a normal field.")]
        [SerializeField, Min(1)] private int maxAlive = 10;

        [Space(12)]
        [SerializeField, TextArea(2, 4)] private string designerNotes = "Attributes change how a field is DRAWN, not what is stored.";

        private void Start()
        {
            Debug.Log($"Next spawn in {spawnDelay.Random():F2} s (range {spawnDelay.min}-{spawnDelay.max}), " +
                      $"elite chance {eliteChance:P0}, max alive {maxAlive}. Notes: {designerNotes}", this);
        }
    }
}