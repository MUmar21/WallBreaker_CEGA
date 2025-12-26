using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class BrickPhysics : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 50f;
    [SerializeField] private float forwardForce = 50f;
    [SerializeField] private float jumpForce = 80f;

    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference fireAction;
    [SerializeField] private LayerMask LayerMask;
    RaycastHit[] m_Results = new RaycastHit[5];


    private Rigidbody rb;
    private float horizontalInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.mass = 5f;
        rb.linearDamping = 0.1f;
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    private void OnEnable()
    {
        jumpAction.action.started += Jump;
        fireAction.action.started += Fire;
        jumpAction.action.canceled += OnJumpCancelled;
    }

    private void OnJumpCancelled(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            Debug.Log("Jump input cancelled");
        }
    }

    private void OnDisable()
    {
        jumpAction.action.started -= Jump;
        fireAction.action.started -= Fire;
        jumpAction.action.canceled -= OnJumpCancelled;
    }

    private void Update()
    {
        //horizontalInput = moveAction.action.ReadValue<Vector2>().x * horizontalSpeed;
        int hits = Physics.RaycastNonAlloc(transform.position, transform.TransformDirection(Vector3.forward), m_Results, 200f);

        if (hits > 0)
        {
            for (int i = 0; i < hits; i++)
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.green);
            }
        }

        RaycastHit[] hitInfos = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward));

        for (int i = 0; i < hitInfos.Length; i++)
        {
            //EnemyClass enemy = hitInfos[i].collider.GetComponent<EnemyClass>();

            //if (enemy != null)
            //{
            //    enemy.TakeDamage();
            //}
            Debug.Log("Brick Hit Multiple : " + hitInfos[i].collider.name);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitInfos[i].distance, Color.green);
        }
    }

    void FixedUpdate()
    {
        //rb.AddForce(horizontalInput, 0f, forwardForce * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            Coins coin = collision.gameObject.GetComponent<Coins>();
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Brick hit an obstacle!");
            Debug.Log("Game Over!");
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (rb.linearVelocity.y < 2f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("Brick Jump!");
        }

        if (context.started)
        {
            Debug.Log("Jump input started");
        }
    }

    private void Fire(InputAction.CallbackContext context)
    {
        Debug.Log("Fire action triggered");
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, 200f, LayerMask))
        {
            Debug.Log("Brick Hit : " + hitInfo.collider.name);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.green);

            Vector3 hitPoint = hitInfo.point;

            Debug.Log("Hit Point: " + hitPoint);
            //EnemyClass enemy = hitInfo.collider.GetComponent<EnemyClass>();

            //if (enemy != null)
            //{
            //    enemy.TakeDamage();
            //}
        }
    }

}