namespace GGJ2020.Projectiles
{
    using UnityEngine;
    using ObjectPooling;

    public class ProjectilePool : ObjectPool
    {
        /// <summary> The types of bullets to pool. </summary>
        [SerializeField]
        [Tooltip("The types of projectiles to pool.")]
        private Projectile[] projectilePools = null;

        /// <summary> How many of each type to pool. </summary>
        [SerializeField]
        [Tooltip("How many of each type to pool.")]
        private int[] projectilePoolSizes = null;

        /// <summary>
        /// The different types of bullets:
        /// Player: Player bullets
        /// Enemy: Enemy bullets
        /// Explosion: AoE Explosion
        /// </summary>
        public enum ProjectileTypes { Player, Enemy, Explosion }

        /// <summary> Singleton instance for this object pool. </summary>
        public static ProjectilePool Instance { get; private set; }

        protected override void PreInit()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Duplicate Projectile Pool detected: removing " + this.gameObject.name);
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
        }

        protected override IPoolable[] GetTemplets()
        {
            return this.projectilePools;
        }

        protected override int[] GetPoolSizes()
        {
            return this.projectilePoolSizes;
        }

        /// <summary> Retrieve a bullet from a pool. </summary>
        /// <param name="type"> The type of bullet to get. </param>
        /// <returns> The gameObject of the bullet or null if there are none available. </returns>
        public GameObject GetProjectile(ProjectileTypes type)
        {
            IPoolable entity = AllocateEntity(this.projectilePools[(int)type]);
            if (entity == null)
                return null;

            if (type == ProjectileTypes.Enemy)
            {
                SoundPool.instance.PlaySound(SoundClips.instance.Laser2, entity.GetGameObject().transform.position, false, false, 0.25f);
            }
            else if(type == ProjectileTypes.Player)
            {
                SoundPool.instance.PlaySound(SoundClips.instance.PlayerLaser, transform.position, false, false, 0.1f);
            }
            return entity.GetGameObject();
        }

        /// <summary> Returns a bullet to the pool. </summary>
        /// <param name="type"> The type of bullet being returned. </param>
        /// <param name="bullet"> The bullet being returned. </param>
        public void ReturnProjectile(ProjectileTypes type, GameObject bullet)
        {
            IPoolable entity = bullet.GetComponent<IPoolable>();
            DeallocateEntity(this.projectilePools[(int)type], entity);
        }
    }
}
