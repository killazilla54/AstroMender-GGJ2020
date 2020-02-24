using UnityEngine;
using Rewired;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    private Player player; // The Rewired Player
    int playerId = 0;

    public delegate void MoveHorizontalDelegate(float magnitude);
    MoveHorizontalDelegate moveHorizontalDelegate;

    public delegate void MoveVerticalDelegate(float magnitude);
    MoveVerticalDelegate moveVerticalDelegate;

    public delegate void ShootPressDelegate();
    ShootPressDelegate shootPressDelegate;
    public delegate void ShootHeldDelegate();
    ShootHeldDelegate shootHeldDelegate;
    public delegate void ShootReleaseDelegate();
    ShootReleaseDelegate shootReleaseDelegate;

    public delegate void BoostPressDelegate();
    BoostPressDelegate boostPressDelegate;
    public delegate void BoostHeldDelegate();
    BoostHeldDelegate boostHeldDelegate;
    public delegate void BoostReleaseDelegate();
    BoostReleaseDelegate boostReleaseDelegate;

    public delegate void DodgeRollPress();
    DodgeRollPress dodgeRollPressDelegate;

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

    void Start()
    {
        player = ReInput.players.GetPlayer(playerId);
    }
    
    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if (moveHorizontalDelegate != null)
        {
            moveHorizontalDelegate(player.GetAxis("MoveHorizontal"));
        }
        if (moveVerticalDelegate != null)
        {
            moveVerticalDelegate(player.GetAxis("MoveVertical"));
        }

        if (shootPressDelegate != null)
        {
            if (player.GetButtonDown("Shoot"))
            {
                shootPressDelegate();
            }
        }
        if (shootHeldDelegate != null)
        {
            if (player.GetButton("Shoot"))
            {
                shootHeldDelegate();
            }
        }
        if (shootReleaseDelegate != null)
        {
            if (player.GetButtonUp("Shoot"))
            {
                shootReleaseDelegate();
            }
        }

        if (boostPressDelegate != null)
        {
            if (player.GetButtonDown("Boost"))
            {
                boostPressDelegate();
            }
        }
        if (boostHeldDelegate != null)
        {
            if (player.GetButton("Boost"))
            {
                boostHeldDelegate();
            }
        }
        if (boostReleaseDelegate != null)
        {
            if (player.GetButtonUp("Boost"))
            {
                boostReleaseDelegate();
            }
        }

        if (dodgeRollPressDelegate != null)
        {
            if (player.GetButtonDown("DodgeRoll"))
            {
                dodgeRollPressDelegate();
            }
        }
    }

    public void AssignFunctionToMoveHorizontalDelegate(MoveHorizontalDelegate func)
    {
        moveHorizontalDelegate += func;
    }

    public void AssignFunctionToMoveVerticalDelegate(MoveVerticalDelegate func)
    {
        moveVerticalDelegate += func;
    }

    public void AssignFunctionToShootPressDelegate(ShootPressDelegate func)
    {
        shootPressDelegate += func;
    }

    public void AssignFunctionToShootHeldDelegate(ShootHeldDelegate func)
    {
        shootHeldDelegate += func;
    }

    public void AssignFunctionToShootReleaseDelegate(ShootReleaseDelegate func)
    {
        shootReleaseDelegate += func;
    }

    public void AssignFunctionToBoostPressDelegate(BoostPressDelegate func)
    {
        boostPressDelegate += func;
    }

    public void AssignFunctionToBoostHeldDelegate(BoostHeldDelegate func)
    {
        boostHeldDelegate += func;
    }

    public void AssignFunctionToBoostReleaseDelegate(BoostReleaseDelegate func)
    {
        boostReleaseDelegate += func;
    }

    public void AssignFunctionToDodgeRollPressDelegate(DodgeRollPress func)
    {
        dodgeRollPressDelegate += func;
    }
}
