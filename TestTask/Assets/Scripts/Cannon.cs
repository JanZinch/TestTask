using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private Projectile _projectile = null;
    [SerializeField] private Vector3 _force = default;
    [SerializeField] private RectTransform _sight = null;
    [SerializeField] private float _distaceFromScreen = 0.0f;

    public void StartRewinding() {

        _projectile.StartRewinding();    
    }

    private void OnProjectileCollision()
    {
        GameManager.Instance.restIsDisturbed = true;
        _projectile.OnCollisionWithBrick -= OnProjectileCollision;
    }

    private void Start()
    {
        SetProjectile();
    }

    public void SetProjectile() {

        _projectile.RigidBody.isKinematic = true;
        _projectile.transform.position = default;
        _projectile.OnCollisionWithBrick += OnProjectileCollision;

        _projectile.gameObject.SetActive(false);
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

            _sight.gameObject.SetActive(true);
            UpdateSightPosition();
        }        
    }


    private void OnMouseDrag()
    {
        if (GameManager.Instance.State == GameManager.SessionState.AIMING)
        {
            _sight.gameObject.SetActive(true);
            UpdateSightPosition();
        }    
    }


    private void OnMouseUp()
    {
        if (GameManager.Instance.State == GameManager.SessionState.AIMING)
        {
            _sight.gameObject.SetActive(false);
            Vector3 spawnPoint = _sight.TransformPoint(_sight.anchoredPosition);
            spawnPoint.z = transform.position.z + _distaceFromScreen;

            if (spawnPoint.y <= GameManager.Instance.FloorLevel) return;
            
            _projectile.gameObject.SetActive(true);
            _projectile.transform.position = spawnPoint;
            _projectile.RigidBody.isKinematic = false;
            _projectile.RigidBody.velocity = Vector3.zero;
            _projectile.RigidBody.AddForce(_force);

            GameManager.Instance.NextStage();  // simulating

        }
        else if (GameManager.Instance.State == GameManager.SessionState.PAUSE) {

            GameManager.Instance.NextStage();  // rewinding      
        }

    }

}
