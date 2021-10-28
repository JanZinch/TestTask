using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] private Rigidbody _rigidBody = null;   

    public Rigidbody RigidBody { get { return _rigidBody; } private set { _rigidBody = value; } }

    public Action OnCollisionWithBrick = null;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Brick>(out Brick other)) {

            OnCollisionWithBrick?.Invoke();        
        }
    }



}