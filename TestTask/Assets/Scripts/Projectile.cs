using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody = null;   
    
    public Rigidbody RigidBody { get { return _rigidBody; } private set { _rigidBody = value; } }

    public Action OnCollisionWithBrick = null;

    private TransformStateRecorder _stateRecorder = null;

    private void Awake()
    {
        _stateRecorder = new TransformStateRecorder();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.State == GameManager.SessionState.SIMULATING)
        {
            _stateRecorder.Record(transform);
        }
        else if (GameManager.Instance.State == GameManager.SessionState.REWINDING) {

            if (!_stateRecorder.IsEmpty())
            {
                _stateRecorder.PlayNext(transform);
            }
            else {

                GameManager.Instance.NextStage();  // aiming
            }
        }        
    }

    public void StartRewinding()
    {
        _rigidBody.isKinematic = true;
        _rigidBody.velocity = Vector3.zero;        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Brick>(out Brick other) 
            || collision.gameObject.CompareTag(GameManager.WallTag))
        {
            OnCollisionWithBrick?.Invoke();
        }

    }



}