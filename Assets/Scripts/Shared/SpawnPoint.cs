
    namespace Assets.Scripts.Shared
    {
        [System.Serializable]
        public class SpawnPoint
        {
            public float xPos;
            public float ZPos { get; set; }
            public bool isActive;

        public SpawnPoint(float xPosition, bool isActiveElement)
        {
            xPos = xPosition;
            isActive = isActiveElement;
        }
    }



    }


