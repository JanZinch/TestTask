using System;
using UnityEngine;

public class Cannon : MonoBehaviour
{

    [SerializeField] private Rigidbody _projectile = null;
    [SerializeField] private Transform _spawnPoint = null;
    [SerializeField] private Vector3 _force = default;
    [SerializeField] private RectTransform _sight = null;


    public event Action OnShot = null;

    private void Start()
    {
        _projectile.isKinematic = true;
        _projectile.transform.position = _spawnPoint.position;
        //_sight.transform.position = this.transform.position;
    }

    private void OnMouseDown()
    {
        Debug.Log(Input.mousePosition);


        TestFunc();

        //Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //mousePos.z = 5.0f;

        //_sight.anchoredPosition = mousePos;
        //_sight.anchoredPosition = new Vector3(mousePos.x * Camera.main.pixelWidth / 2, mousePos.y * Camera.main.pixelHeight / 2, 0.0f);

        //Vector3 fingerPosition = Camera.main.ScreenToWorldPoint(mousePos);

        //Debug.Log(fingerPosition);

        //_sight.transform.position = new Vector3(fingerPosition.x, fingerPosition.y, _sight.transform.position.z);       
    }


    private void TestFunc() { 
        
        Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //_sight.anchoredPosition = new Vector3(mousePos.x * Camera.main.pixelWidth * 2, mousePos.y * Camera.main.pixelHeight * 2, 0.0f);
        
        
        _sight.anchoredPosition = new Vector3(mousePos.x * Camera.main.pixelWidth - Camera.main.pixelWidth/2, mousePos.y * Camera.main.pixelHeight - Camera.main.pixelHeight /2, 0.0f);
    }


    private void OnMouseDrag()
    {
        Debug.Log(Input.mousePosition);

        TestFunc();

        //Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //mousePos.z = 5.0f;

        //_sight.anchoredPosition = new Vector3(mousePos.x * Camera.main.pixelWidth / 2, mousePos.y * Camera.main.pixelHeight / 2, 0.0f);

        //Vector3 fingerPosition = Camera.main.ScreenToWorldPoint(mousePos);

        //Debug.Log(fingerPosition);

        //_sight.transform.position = new Vector3(fingerPosition.x, fingerPosition.y, _sight.transform.position.z);
    }

    private void OnMouseUp()
    {

        Vector3 point = _sight.TransformPoint(_sight.anchoredPosition);

        _projectile.transform.position = new Vector3(point.x, point.y, -5.0f);

        _projectile.isKinematic = false;
        _projectile.velocity = Vector3.zero;
        _projectile.AddForce(_force);

        OnShot?.Invoke();
    
    }

}
