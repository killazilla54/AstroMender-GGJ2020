namespace GGJ2020.Projectiles
{
    using UnityEngine;

    /// <summary> Handler for basic bullets. </summary>
    public class BasicBullet : Projectile
    {
        /// <summary> How fast this bullet travels. </summary>
        [SerializeField]
        [Tooltip("How fast this bullet travels.")]
        private float speed;

        /// <summary> The velocity of the player that fired this bullet </summary>
        private Vector3 playerVelocity;

        /// <summary> The rigidbody for this bullet. </summary>
        [SerializeField]
        [Tooltip("The rigidbody for this bullet.")]
        private Rigidbody rgbdy;

        public GameObject hitFX;

        protected override void LocalInitialize()
        {
            //transform.GetChild(0).localScale = Vector3.one * 2f;
        }

        protected override void LocalReInitialize()
        {
        }

        protected override void LocalUpdate()
        {

        }

        protected override void LocalFixedUpdate()
        {
            rgbdy.MovePosition(transform.position + (transform.forward * speed + playerVelocity) * Time.fixedDeltaTime);
            //transform.GetChild(0).localScale = Vector3.Lerp(transform.GetChild(0).localScale, Vector3.one, Time.deltaTime * 5f);
        }

        public void SetPlayerVelocity(Vector3 playerVelocity)
        {
            this.playerVelocity = playerVelocity;
        }

        protected override void LocalDeallocate()
        {
            this.rgbdy.velocity = Vector3.zero;
            if(GameManager.instance.playerController.startupAnimation.activeSelf) return;
            if(hitFX != null)
            {
                GameObject temp = Instantiate(hitFX, transform.position, transform.rotation);
                Rigidbody tempRbody = temp.GetComponent<Rigidbody>();
                if (tempRbody != null)
                {
                    tempRbody.velocity = GameManager.instance.playerController.GetVelocity()/1.2f;
                }
                Destroy(temp, 0.5f);
            }
        }

        protected override void LocalDelete()
        {
        }
    }
}
