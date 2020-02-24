using System.Collections;
using System.Collections.Generic;
using GGJ2020.Projectiles;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rBody;

    private Vector2 smoothAim;

    public Vector2 aimDirection;

    public Transform aimReticle, playerModel, thruster;

    public float pitchSpeed = 2f, yawSpeed = 2f;
    public float aimAcceleration = 1f;

    public float maxPitchAngle = 60f, mouseSensitivity = 2f, joystickSensitivity = 0.5f;

    public float aimMoveAssistFactor = 0.1f, reticleMoveFactor = 0.5f;

    public float maxAimY, maxAimX;

    public AnimationCurve speedCurve, dodgeRollCurve;
    public float speedMultiplier;
    public float boostSpeed, decelerateSpeed;

    private float currentSpeedNormalized;

    public float camFollowDistance, aimDistance;

    public float dodgeRollDuration;

    private float dodgeRollTimer;

    public bool keyboardMode = false, mouseMode = false;

    public ParticleSystem tier1, tier2;

    private Rewired.Player player;

    public float delayStart = 25f;

    public GameObject startupAnimation;

    public HurtCanvas hurtCanvas;

    IEnumerator Start()
    {
        rBody = GetComponent<Rigidbody>();
        if (mouseMode) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        player = Rewired.ReInput.players.GetPlayer(0);

        dodgeRollTimer = dodgeRollDuration;

        InputManager.instance.AssignFunctionToDodgeRollPressDelegate(HandleDodgeRoll);

        GetComponent<CollisionWrapper>().AssignFunctionToTriggerEnterDelegate(HandleEnemyBulletHit);

        StartCoroutine(PlayStartupMusic());

        transform.GetChild(0).gameObject.SetActive(false);

        yield return new WaitForSeconds(delayStart);

        transform.GetChild(0).gameObject.SetActive(true);

        yield return new WaitForSeconds(10f);
        
        startupAnimation.gameObject.SetActive(false);
    }

    private IEnumerator PlayStartupMusic()
    {
        SoundPool.instance.PlaySound(SoundClips.instance.AstromenderBattleThemeIntro, transform.position, false);
        yield return new WaitForSeconds(121f);
        SoundPool.instance.PlaySound(SoundClips.instance.AstromenderBattleThemeLoop, transform.position, false, true);
    }

    private void HandleEnemyBulletHit (Collider other)
    {
        if(dodgeRollTimer >= dodgeRollDuration)
        {
            ProjectilePool.Instance.ReturnProjectile(ProjectilePool.ProjectileTypes.Enemy, other.transform.root.gameObject);
            EnergySystem.instance.SpendEnergy(CombatBalancing.enemyBulletDamage);
            SoundPool.instance.PlaySound(SoundClips.instance.PlayerHit, transform.position, false);
            hurtCanvas.TakeDamage();
        }
    }

    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
        {
            startupAnimation.gameObject.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(true);
        }
        if(!transform.GetChild(0).gameObject.activeSelf) return;

        float currentSpeed = speedCurve.Evaluate(currentSpeedNormalized) * speedMultiplier;
        Vector3 targetPosition = transform.position + transform.forward * Time.fixedDeltaTime * currentSpeed;
        
        if(keyboardMode) {
            aimDirection = Vector2.zero;
            if (Input.GetKey(KeyCode.W))
            {
                aimDirection.y += pitchSpeed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                aimDirection.y -= pitchSpeed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                aimDirection.x += yawSpeed;
            }
            if (Input.GetKey(KeyCode.A))
            {
                aimDirection.x -= yawSpeed;
            }
        }
        else
        {
            aimDirection = Vector2.Lerp(aimDirection, Vector2.zero, Time.deltaTime*2f);
            if (mouseMode) {
                Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X") * yawSpeed, -Input.GetAxis("Mouse Y") * pitchSpeed);
                aimDirection += mouseDelta * mouseSensitivity;
            }
            else
            {
                Vector2 gamepadInput = new Vector2(player.GetAxis("MoveHorizontal") * yawSpeed, player.GetAxis("MoveVertical") * pitchSpeed);
                if (transform.position.y > CombatBalancing.yRange)
                {
                    gamepadInput.y = 10 + 5f * Mathf.Abs(transform.position.y - CombatBalancing.yRange);
                }
                else if (transform.position.y < -CombatBalancing.yRange)
                {
                    gamepadInput.y = -10 - 5f * Mathf.Abs(CombatBalancing.yRange + transform.position.y);
                }
                aimDirection += gamepadInput * joystickSensitivity;
            }
        }

        EnergySystem.instance.MoveShip(aimDirection.magnitude);

        smoothAim = Vector2.Lerp(smoothAim, aimDirection, Time.deltaTime * aimAcceleration);

        targetPosition += transform.TransformDirection(smoothAim) * Time.fixedDeltaTime * aimMoveAssistFactor;

        ClampAimDirection();

        UpdateAimReticle();

        HandleBoost();

        Vector3 targetRotation = Vector3.Scale(transform.eulerAngles, new Vector3(1,1,0)) + new Vector3(smoothAim.y, smoothAim.x,0f) * Time.fixedDeltaTime;

        if (Mathf.DeltaAngle(targetRotation.x, 0) > maxPitchAngle)
        {
            targetRotation.x = -maxPitchAngle;
        }
        else if (Mathf.DeltaAngle(targetRotation.x, 0) < -maxPitchAngle)
        {
            targetRotation.x = maxPitchAngle;
        }

        // if (transform.position.y > CombatBalancing.yRange)
        // {
        //     targetPosition.y = CombatBalancing.yRange;
        // }
        // else if(transform.position.y < -CombatBalancing.yRange)
        // {
        //     targetPosition.y = -CombatBalancing.yRange;
        // }

        // if (transform.position.y > CombatBalancing.yRange)
        // {
        //     Debug.Log(targetRotation.x + " " + (transform.position.y - CombatBalancing.yRange));
        //     targetRotation.x = Mathf.Min(targetRotation.x, (transform.position.y - CombatBalancing.yRange));
        // }
        // else if(transform.position.y < -CombatBalancing.yRange && targetRotation.x > 0f)
        // {
        //     targetRotation.x = 0f;
        // }

        if (dodgeRollTimer < dodgeRollDuration)
        {
            dodgeRollTimer += Time.fixedDeltaTime;
            float dodgeRollTimeValue = dodgeRollCurve.Evaluate(dodgeRollTimer/dodgeRollDuration);
            Quaternion dodgeRollRot = Quaternion.Euler(Vector3.forward*Mathf.LerpUnclamped(0f,360f, dodgeRollTimeValue));
            playerModel.localRotation = Quaternion.Lerp(playerModel.localRotation, dodgeRollRot, Time.fixedDeltaTime * (5f + 100f*dodgeRollTimer/dodgeRollDuration));
        }
        else
        {
            playerModel.localRotation = Quaternion.Lerp(playerModel.localRotation, Quaternion.Euler(Vector3.forward * smoothAim.x * -0.5f), Time.fixedDeltaTime * 5f);
        }

        rBody.MoveRotation(Quaternion.Euler(targetRotation));
        rBody.MovePosition(targetPosition);

        // thruster.rotation = Quaternion.Lerp(thruster.rotation, transform.rotation, Time.deltaTime * 2f);
    }

    private void HandleDodgeRoll()
    {
        if(dodgeRollTimer < dodgeRollDuration) return;
        if(EnergySystem.instance.DodgeRoll()) {
            SoundPool.instance.PlaySound(SoundClips.instance.Dodge, transform.position, false);
            dodgeRollTimer = 0f;
        }
    }

    private void HandleBoost()
    {
        if ((Input.GetKey(KeyCode.Space) || player.GetButton("Boost")) && EnergySystem.instance.Boost()) {
            if(Input.GetKeyDown(KeyCode.Space) || player.GetButtonDown("Boost"))
            {
                SoundPool.instance.PlaySound(SoundClips.instance.Boost, transform.position, false, false, 0.25f);
            }
            currentSpeedNormalized += boostSpeed;
            if (currentSpeedNormalized >= 1f)
            {
                currentSpeedNormalized = 1f;
            }
        }
        else
        {
            currentSpeedNormalized -= decelerateSpeed;
            if (currentSpeedNormalized <= 0f)
            {
                currentSpeedNormalized = 0f;
            }
        }
    }

    private void UpdateAimReticle()
    {
        aimReticle.position = transform.position + transform.TransformDirection(Vector3.Scale(new Vector2(1f,-1f),smoothAim)) * reticleMoveFactor + transform.forward * aimDistance;
        aimReticle.LookAt(transform);
    }

    private void ClampAimDirection()
    {
        if(aimDirection.x > maxAimX)
        {
            aimDirection.x = maxAimX;
        }
        else if(aimDirection.x < -maxAimX)
        {
            aimDirection.x = -maxAimX;
        }
        
        if (aimDirection.y > maxAimY)
        {
            aimDirection.y = maxAimY;
        }
        else if (aimDirection.y < -maxAimY)
        {
            aimDirection.y = -maxAimY;
        }

    }

    public Vector3 AimPosition
    {
        get
        {
            return aimReticle.position;
        }
    }

    public float AimHeight
    {
        get
        {
            return aimDirection.y / maxAimY;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Energy"))
        {
            EnergySystem.instance.RefillEnergy(other.GetComponent<EnergyPickup>().energyAmount);
            other.enabled = false;
            if(other.GetComponent<EnergyPickup>().tier == 1)
                tier1.Emit(1);
            else if(other.GetComponent<EnergyPickup>().tier == 2)
                tier2.Play(true);
             
            int pickupTier = other.GetComponent<EnergyPickup>().tier;
            switch(pickupTier)
            {
                case 1:
                    SoundPool.instance.PlaySound(SoundClips.instance.SmallPickup, transform.position, true, false, 0.25f);
                break;
                case 2:
                    SoundPool.instance.PlaySound(SoundClips.instance.MediumPickup, transform.position, false);
                break;
                case 3:
                    SoundPool.instance.PlaySound(SoundClips.instance.BigPickup, transform.position, false);
                break;
            }
        }
    }

    public Vector3 GetVelocity()
    {
        if(rBody == null) return Vector3.zero;
        return rBody.velocity;
    }
}
