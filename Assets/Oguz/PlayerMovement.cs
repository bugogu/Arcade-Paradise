using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [SerializeField] private DynamicJoystick dynamicJoystick;
    [SerializeField] private ParticleSystem walkParticle;
    [SerializeField] private Animator playerAnimator;

    private Rigidbody _rb;
    private float _runSpeed;
    private float _turnSpeed;
    private float _horizontal = 0;
    private float _vertical = 0;

    #endregion

    private void Awake() => Initial();

    private void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            JoystickMovement();
            walkParticle.Play();
            playerAnimator.SetBool("isTouching", true);
        }
        else
        {
            walkParticle.Pause();
            walkParticle.Clear();
            playerAnimator.SetBool("isTouching", false);
            _rb.velocity = Vector3.zero;
        }
    }

    public void JoystickMovement()
    {
        _horizontal = dynamicJoystick.Horizontal;
        _vertical = dynamicJoystick.Vertical;

        Vector3 newPos = new Vector3(_horizontal * _runSpeed * Time.fixedDeltaTime, 0, _vertical * _runSpeed * Time.fixedDeltaTime);
        _rb.velocity = newPos;

        Vector3 dir = (Vector3.forward * _vertical) + (Vector3.right * _horizontal);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), _turnSpeed * Time.fixedDeltaTime);
    }

    private void Initial()
    {
        _rb = GetComponent<Rigidbody>();
        _runSpeed = DesignManager.Instance.playerSpeed;
        _turnSpeed = DesignManager.Instance.turnSpeed;
    }
}
