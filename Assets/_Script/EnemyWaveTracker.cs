using UnityEngine;

namespace MaiNull
{
    public class EnemyWaveTracker : MonoBehaviour
    {
        // put this on your enemy prefabs. You could just copy the on destroy onto a pre-existing script if you want.
        void OnDestroy()
        {
            FindFirstObjectByType<WaveEnemySpawner>()?.spawnedEnemies.Remove(gameObject);
        }
    }
}

