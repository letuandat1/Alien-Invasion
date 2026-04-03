using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private Rigidbody2D rb;
    private Animator animator;
    private CharacterStats stats;
    private GunManager gunManager;
    private Vector2 moveInput;
    private float healthRecoverTimer = 0f;
    private float manaRecoverTimer = 0f;

    private float recoverInterval = 5f;
    private void Awake()
    {
        gunManager = GetComponent<GunManager>();
        inputActions = new PlayerInputActions(); // tạo instance của InputAction
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        stats = GetComponent<CharacterStats>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.FireGun.performed += gunManager.OnFireGunPressed;
        inputActions.Player.Special.performed += gunManager.OnSpecialAttack;
        inputActions.Player.FireGun.canceled += gunManager.OnFireGunReleased;
        inputActions.Player.SwitchGun.performed += gunManager.OnSwitchGun;
        inputActions.Player.ReloadGun.performed += gunManager.OnReloadGun;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Special.performed -= gunManager.OnSpecialAttack;
        inputActions.Player.FireGun.performed -= gunManager.OnFireGunPressed;
        inputActions.Player.FireGun.canceled -= gunManager.OnFireGunReleased;
        inputActions.Player.SwitchGun.performed -= gunManager.OnSwitchGun;
        inputActions.Player.ReloadGun.performed -= gunManager.OnReloadGun;

        inputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (stats != null && stats.IsStunned())
    {
        rb.linearVelocity = Vector2.zero;
        animator.SetFloat("Speed", 0);
        return;
    }

    float currentSpeed = stats.moveSpeed;

    // Giảm tốc độ nếu đang bắn hoặc reload
    if (gunManager.IsFiring || gunManager.isReloading)
    {
        currentSpeed *= 0.5f;
    }

    rb.linearVelocity = moveInput * currentSpeed;

    animator.SetFloat("Speed", moveInput.magnitude * currentSpeed);

    if (moveInput.x > 0.01f)
        transform.localScale = new Vector3(1, 1, 1);
    else if (moveInput.x < -0.01f)
        transform.localScale = new Vector3(-1, 1, 1);
    }
    void Start()
    {

    }
    void Update()
    {
        healthRecoverTimer += Time.deltaTime;
        manaRecoverTimer += Time.deltaTime;

        if (healthRecoverTimer >= recoverInterval)
        {
            stats.RecoverHealth();
            healthRecoverTimer = 0f;
        }

        if (manaRecoverTimer >= recoverInterval)
        {
            stats.RecoverMana();
            manaRecoverTimer = 0f;
        }
    }
    public void PlayReloadAnimation()
    {
        if (animator != null)
            animator.SetTrigger("Reload");
    }
public void ResetInput()
{
    moveInput = Vector2.zero; // Reset biến input về không
}
}
