using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected Rigidbody2D _rigidbody;

    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private Transform weaponPivot;

    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection { get => movementDirection; }

    protected Vector2 lookDirection = Vector2.zero;

    public Vector2 LookDirection { get => lookDirection; }

    private Vector2 knockback = Vector2.zero;
    private float knockbackDuration = 0.0f;

    protected AnimationHandler animationHandler;
    protected StatHandler statHandler;

    [SerializeField] public WeaponHandler WeaponPrefab;
    protected WeaponHandler weaponHnadler;

    protected bool isAttacking;
    private float timeSinceLastAttack = float.MaxValue;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animationHandler = GetComponent<AnimationHandler>();
        statHandler = GetComponent<StatHandler>();

        if (WeaponPrefab != null)
            weaponHnadler = Instantiate(WeaponPrefab, weaponPivot);
        else
            weaponHnadler = GetComponentInChildren<WeaponHandler>();

    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleAction();
        Rotate(lookDirection);
        HandleAttackDelay();
    }

    protected virtual void FixedUpdate()
    {
        Movement(movementDirection);
        if (knockbackDuration > 0.0f)
        {
            knockbackDuration -= Time.deltaTime;
        }
    }

    protected virtual void HandleAction()
    {

    }

    private void Movement(Vector2 dirextion)
    {
        dirextion = dirextion * statHandler.Speed;
        if (knockbackDuration > 0.0f)
        {
            dirextion *= 0.2f;
            dirextion += knockback;
        }

        _rigidbody.velocity = dirextion;
        animationHandler.Move(dirextion);
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f;

        characterRenderer.flipX = isLeft;

        if (weaponPivot != null)
        {
            weaponPivot.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }

        weaponHnadler?.Rotate(isLeft);
    }

    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        knockback = -(other.position - transform.position).normalized * power;
    }

    private void HandleAttackDelay()
    {
        if (weaponHnadler == null)
            return;

        if (timeSinceLastAttack <= weaponHnadler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        if (isAttacking && timeSinceLastAttack > weaponHnadler.Delay)
        {
            timeSinceLastAttack = 0;
            Attack();
        }

    }

    protected virtual void Attack()
    {
        if (lookDirection != Vector2.zero)
        {
            weaponHnadler?.Attack();
        }
    }

}

