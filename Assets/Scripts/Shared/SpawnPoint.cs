namespace Assets.Scripts.Shared
{
    [System.Serializable]
    public class SpawnPoint
    {
        public float xPos;
        public bool isActive;

        public float ZPos { get; set; }

        public SpawnPoint(float xPosition, bool isActiveElement)
        {
            xPos = xPosition;
            isActive = isActiveElement;
        }
    }
}