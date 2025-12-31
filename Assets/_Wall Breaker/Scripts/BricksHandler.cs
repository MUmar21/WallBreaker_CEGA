using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BricksHandler : MonoBehaviour
{
    [Header("Impact Settings")]

    [SerializeField] private float impactForce = 400f;

    [SerializeField] private float fallForce = 300f;

    [SerializeField] private float torqueAmount = 50f;

    [SerializeField] private float impactRadius = 1.5f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float destroyDelay = 3f;

    [HideInInspector] public Rigidbody rb;

    public bool isDetached = false;

    public UnityEvent unityEvent;

    // Return void (nothing)
    public static Action<BricksHandler> OnDetached;
    public Action<BricksHandler> brickDetachAction;
    public static Action AddScoreEvent;


    private void OnEnable()
    {
        brickDetachAction += OnBrickDetached;
    }

    private void OnDisable()
    {
        brickDetachAction -= OnBrickDetached;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnClicked(Vector3 impactPoint)
    {
        if (isDetached) return; // Already falling

        DetachBrick(impactPoint);

        ApplyImpactToNearby(impactPoint);
    }

    void DetachBrick(Vector3 impactPoint)
    {
        rb.isKinematic = false;

        Vector3 direction = (transform.position - impactPoint).normalized;

        rb.AddForce(direction * impactForce, ForceMode.Impulse);

        rb.AddForce(Vector3.down * fallForce, ForceMode.Impulse);

        rb.AddTorque(UnityEngine.Random.onUnitSphere * torqueAmount, ForceMode.Impulse);

        brickDetachAction?.Invoke(this);
    }

    void ApplyImpactToNearby(Vector3 impactPoint)
    {
        Collider[] nearby = Physics.OverlapSphere(
        transform.position,
        impactRadius,
        layerMask
        );

        foreach (Collider col in nearby)
        {
            BricksHandler brick = col.GetComponent<BricksHandler>();

            if (brick != null && brick != this)
            {
                // Small push, not full detach
                Rigidbody otherRb = brick.GetComponent<Rigidbody>();
                otherRb.isKinematic = false;
                otherRb.AddExplosionForce(impactForce, impactPoint, impactRadius, 2.0f, ForceMode.Impulse);
                otherRb.AddTorque(UnityEngine.Random.onUnitSphere * torqueAmount, ForceMode.Impulse);
                brick.brickDetachAction?.Invoke(brick);
                //OnDetached?.Invoke(brick);
            }
        }
    }

    private void OnBrickDetached(BricksHandler brick)
    {
        if (brick.isDetached) return;

        brick.isDetached = true;
        StartCoroutine(brick.DestroyBrick());
        AddScoreEvent?.Invoke();
        OnDetached?.Invoke(brick);
        brick.unityEvent.Invoke();
    }

    public IEnumerator DestroyBrick()
    {
        yield return new WaitForSeconds(destroyDelay);
        gameObject.SetActive(false);
    }
}
