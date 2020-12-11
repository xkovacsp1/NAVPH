
    namespace Assets.Scripts.Shared
    {
        public class SpawnPoint
        {
            public float XPos { get; set; }
            public float ZPos { get; set; }
            public bool IsActive { get; set; }

        public SpawnPoint(float xPos,bool isActive)
            {
                XPos = xPos;
                IsActive = isActive;
            }
        }
    }


