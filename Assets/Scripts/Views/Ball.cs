using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour, IBall
{
    private Rigidbody _body;
    private GameSettings _settings;
    private LayerMask _collisionMask;
    private SignalBus _signalBus;
    private IField _field;

    private float _speed;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    [Inject]
    protected void Construct(GameSettings settings, LayerMask layerMask, SignalBus signalBus, IField field)
    {
        _settings = settings;
        _collisionMask = layerMask;
        _signalBus = signalBus;
        _field = field;
    }

    public void SetVelocity(Vector3 direction, float speed)
    {
        _body.ResetInertiaTensor();
        _speed = speed;
        _body.velocity = direction.normalized * speed;
    }

    private void FixedUpdate()
    {
        if (!Mathf.Approximately(_body.velocity.sqrMagnitude, _speed * _speed))
            SetVelocity(_body.velocity, _speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != null && IsMaskContainsLayer(_collisionMask, collision.gameObject.layer) && collision.contactCount > 0)
            HandleCollision(collision);
    }

    private void HandleCollision(Collision collision)
    {
        if (collision.collider.TryGetComponent<Racket>(out var racket))
        {
            var fieldForward = _field.GetForwardDirection();
            var direction = racket.type == PlayerType.Player ? fieldForward : -fieldForward;
            var centerPoint = racket.transform.position.x;
            var contactPoint = collision.contacts[0].point.x;
            var offset = contactPoint - centerPoint;
            if (racket.type == PlayerType.Enemy)
                offset = -offset;

            var reflectedAngle = _settings.maxBallReflectionAngle * offset / (racket.size / 2);
            SetVelocity(Quaternion.Euler(0, reflectedAngle, 0) * direction, _speed + _settings.collisionSpeedBoost);
            _signalBus.Fire(new RacketHitSignal(racket.type));
        }
    }

    private bool IsMaskContainsLayer(LayerMask mask, int layer)
    {
        return (mask.value & (1 << layer)) != 0;
    }

    public void SetPosition(Vector3 position) => transform.position = position;

    public Vector3 GetPosition() => transform.position;
}
