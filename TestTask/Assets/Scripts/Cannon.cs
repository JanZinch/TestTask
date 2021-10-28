using System;
using UnityEngine;

public class Cannon : MonoBehaviour
{

    [SerializeField] private Projectile _projectile = null;
    [SerializeField] private Transform _spawnPoint = null;
    [SerializeField] private Vector3 _force = default;
    [SerializeField] private RectTransform _sight = null;


    private void StartSimulation() {

        if (GameManager.Instance.State == GameManager.SessionState.AIMING) {

            GameManager.Instance.NextStage();
        }

        _projectile.OnCollisionWithBrick -= StartSimulation;
    }



    private void Start()
    {
        SetProjectile();
    }

    public void SetProjectile() {

        _projectile.RigidBody.isKinematic = true;
        _projectile.transform.position = _spawnPoint.position;
        _projectile.OnCollisionWithBrick += StartSimulation;
    }


    


    private void UpdateSightPosition() { 
        
        Vector3 fingerPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        int screenPixelWidth = Camera.main.pixelWidth;
        int screenPixelHeight = Camera.main.pixelHeight;
        float depth = 0.0f;

        _sight.anchoredPosition = new Vector3(fingerPosition.x * screenPixelWidth - screenPixelWidth / 2, 
            fingerPosition.y * screenPixelHeight - screenPixelHeight / 2, depth);
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.State == GameManager.SessionState.AIMING) {

            UpdateSightPosition();
        }

        
    }

    private void OnMouseDrag()
    {
        if (GameManager.Instance.State == GameManager.SessionState.AIMING)
        {
            UpdateSightPosition();
        }

            
    }

    private void OnMouseUp()
    {
        if (GameManager.Instance.State == GameManager.SessionState.AIMING)
        {

            Vector3 spawnPoint = _sight.TransformPoint(_sight.anchoredPosition);
            spawnPoint.z = -5.0f;

            _projectile.transform.position = spawnPoint;
            _projectile.RigidBody.isKinematic = false;
            _projectile.RigidBody.velocity = Vector3.zero;
            _projectile.RigidBody.AddForce(_force);

        }
        else if (GameManager.Instance.State == GameManager.SessionState.PAUSE) {

            GameManager.Instance.NextStage();        
        }

    }

}
