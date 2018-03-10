using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBehavior : MonoBehaviour
{

    [SerializeField] public Rigidbody2D Target;
    [SerializeField] protected float MaxSpeed = 10;

    private ScreenShake _screenShake;
    private SpriteRenderer _renderer;

    // public UnityEvent OnDeath;
    [SerializeField] private ParticleSystem _deathParticleSystem;
    [SerializeField] private float _shakeAmount;
    [SerializeField] private float _shakeDuration;

    [SerializeField] private Bounds _inPlayArea;

    protected Transform Transform;

    protected Rigidbody2D Rigidbody;



    // Use this for initialization
	protected virtual void Start ()
	{
	    Transform = transform;
	    _renderer = GetComponent<SpriteRenderer>();
	    _screenShake = Camera.main.GetComponent<ScreenShake>();
        Rigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	protected virtual void FixedUpdate ()
	{
        // Retarget to nearest player
        Target = EnemyGenerator.Instance.NearestPlayer(Transform.position);

	    if (!_inPlayArea.Contains(Transform.position))
	    {
	        Destroy(gameObject);
	    }
	    Transform.right = Rigidbody.velocity;

        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireCube(_inPlayArea.center, _inPlayArea.size);
    }

    public void Destroy()
    {
        ParticleSystem o = Instantiate(_deathParticleSystem);
        ParticleSystem.MainModule module = o.main;
        Color pColor = _renderer.color;
        pColor.a = module.startColor.color.a;
        module.startColor = pColor;
        o.transform.position = Transform.position;
        if (_screenShake)
            _screenShake.Shake(_shakeAmount, _shakeDuration);
    }
}
