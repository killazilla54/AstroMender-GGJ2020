using UnityEngine;

public class SoundClips : MonoBehaviour
{
    public static SoundClips instance;

    public AudioClip EnemyExplosion;
    public AudioClip EnemyExplosion2;
    public AudioClip EnemyExplosion3;
    public AudioClip Laser1;
    public AudioClip Laser2;

    public AudioClip BigPickup;
    public AudioClip MediumPickup;
    public AudioClip SmallPickup;

    public AudioClip AstromenderTitle;
    public AudioClip AstromenderBattleThemeIntro;
    public AudioClip AstromenderBattleThemeLoop;

    public AudioClip Dodge;
    public AudioClip EnemyHit;
    public AudioClip HitConfirm;
    public AudioClip PlayerHit;
    public AudioClip PlayerLaser;
    public AudioClip PlayerDeath;
    public AudioClip PowerDown;
    public AudioClip PowerUp;
    public AudioClip Boost;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
