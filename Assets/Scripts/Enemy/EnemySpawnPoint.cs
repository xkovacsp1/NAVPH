using UnityEngine.UIElements;

namespace Assets.Scripts.Enemy
{
    public class EnemySpawnPoint
    {
        public float XPos { get; set; }
        public bool IsActive { get; set; }

        public EnemySpawnPoint(float xPos,bool isActive)
        {
            XPos = xPos;
            IsActive = isActive;
        }
    }
}
