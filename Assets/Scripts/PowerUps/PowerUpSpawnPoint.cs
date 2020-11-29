namespace Assets.Scripts.PowerUps
{
    public class PowerUpSpawnPoint
    {
        public float XPos { get; set; }
        public float ZPos { get; set; }
        public bool IsActive { get; set; }

        public PowerUpSpawnPoint(float xPos, float zPos,bool isActive)
        {
            XPos = xPos;
            ZPos = zPos;
            IsActive = isActive;
        }
    }
}

