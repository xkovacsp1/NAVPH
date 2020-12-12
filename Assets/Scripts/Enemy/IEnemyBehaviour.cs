namespace Assets.Scripts.Enemy
{
    public interface IEnemyBehaviour
    {
        void Move();
        void Attack();
        bool IsActive();
        void TakeDamage(float damage);
    }
}