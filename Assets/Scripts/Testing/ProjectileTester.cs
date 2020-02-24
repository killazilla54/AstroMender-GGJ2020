namespace GGJ2020.Testing
{
    using UnityEngine;
    using Projectiles;

    public class ProjectileTester : MonoBehaviour
    {
        public ProjectilePool pool;

        private void Start()
        {
            pool.Init();
        }

        private void Update()
        {
            Projectile projectile = null;
            if (Input.GetKeyUp(KeyCode.Alpha1))
                projectile = pool.GetProjectile(ProjectilePool.ProjectileTypes.Player)?.GetComponent<Projectile>();
            else if (Input.GetKeyUp(KeyCode.Alpha2))
                projectile = pool.GetProjectile(ProjectilePool.ProjectileTypes.Enemy)?.GetComponent<Projectile>();

            if (projectile != null)
            {
                projectile.transform.position = this.transform.position;
                projectile.transform.localRotation = this.transform.localRotation;
            }
        }
    }
}
