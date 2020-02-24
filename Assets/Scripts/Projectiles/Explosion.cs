namespace GGJ2020.Projectiles
{
    using UnityEngine;
    using Util;

    public class Explosion : Projectile
    {
        public float finalSize = 5;
        public float speedOfGrowth = 1;
        public SoundPlayer explosionSFX;

        private float size;

        protected override void LocalInitialize()
        {
        }

        protected override void LocalReInitialize()
        {
            transform.localScale = Vector3.zero;
            size = 0;
            if (explosionSFX != null)
                explosionSFX.PlaySong(0);
        }

        protected override void LocalUpdate()
        {
            if (size < finalSize)
            {
                size += Time.deltaTime * speedOfGrowth;
                transform.localScale = Vector3.one * size;
            }
        }

        protected override void LocalFixedUpdate()
        {
        }

        protected override void LocalDeallocate()
        {
        }

        protected override void LocalDelete()
        {
        }

        protected override void OnCollisionEnter(Collision collision)
        {
        }
    }
}
