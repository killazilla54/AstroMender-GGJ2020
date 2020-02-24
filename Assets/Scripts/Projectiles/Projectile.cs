namespace GGJ2020.Projectiles
{
    using ObjectPooling;
    using UnityEngine;

    public abstract class Projectile : MonoBehaviour, IPoolable
    {
        /// <summary> How long this bullet can exist untouched in the scene. </summary>
        [SerializeField]
        [Tooltip("How long this bullet can exist untouched in the scene.")]
        private float lifeTime = 1;

        /// <summary> The type of bullet this is.  (Object pooling management) </summary>
        [SerializeField]
        [Tooltip("The type of bullet this is.  (Object pooling management)")]
        private ProjectilePool.ProjectileTypes type = ProjectilePool.ProjectileTypes.Player;

        /// <summary> The type of bullet this is.  (Object pooling management) </summary>
        public ProjectilePool.ProjectileTypes Type { get { return this.type; } }

        /// <summary> The index for this bullet in the pool. (Object pooling management) </summary>
        private int referenceIndex = 0;

        /// <summary> Tracks the remaing time for the bullet to exist in the scene. </summary>
        private float currentLifeTime = 0;

        private void Update()
        {
            LocalUpdate();
            if ((this.currentLifeTime -= Time.deltaTime) <= 0)
            {
                ReturnProjectile();
            }
        }

        private void FixedUpdate()
        {
            LocalFixedUpdate();
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            //if (Managers.SceneState.Instance.CurrentState != Managers.SceneState.State.Playing)
            //    return;

            this.currentLifeTime = 0;
        }

        public IPoolable SpawnCopy(int referenceIndex)
        {
            Projectile projectile = Instantiate<Projectile>(this);
            projectile.referenceIndex = referenceIndex;
            return projectile;
        }

        public GameObject GetGameObject()
        {
            return this.gameObject;
        }

        public int GetReferenceIndex()
        {
            return this.referenceIndex;
        }

        public void Initialize()
        {
            LocalInitialize();
        }

        public void ReInitialize()
        {
            LocalReInitialize();
            this.currentLifeTime = this.lifeTime;
            this.gameObject.SetActive(true);
        }

        public void Deallocate()
        {
            LocalDeallocate();
            this.gameObject.SetActive(false);
        }

        public void Delete()
        {
            LocalDelete();
            Destroy(this.gameObject);
        }

        protected void ReturnProjectile()
        {
            ProjectilePool.Instance.ReturnProjectile(this.type, this.gameObject);
        }

        /// <summary> Local Update for subclasses. </summary>
        protected abstract void LocalUpdate();
        /// <summary> Local Update for subclasses. </summary>
        protected abstract void LocalFixedUpdate();
        /// <summary> Local Initialize for subclasses. </summary>
        protected abstract void LocalInitialize();
        /// <summary> Local ReInitialize for subclasses. </summary>
        protected abstract void LocalReInitialize();
        /// <summary> Local Deallocate for subclasses. </summary>
        protected abstract void LocalDeallocate();
        /// <summary> Local Delete for subclasses. </summary>
        protected abstract void LocalDelete();
    }
}
