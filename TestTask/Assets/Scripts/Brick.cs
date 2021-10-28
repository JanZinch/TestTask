using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody = null;
    [SerializeField] private Collider _collider = null;

    private static LinkedList<Brick> _allBricks = null;

    public static event Action OnAllBricksStationary = null;

    private TransformStateRecorder _stateRecorder = null;


    private void Awake()
    {
        if (_allBricks == null)
        {
            _allBricks = new LinkedList<Brick>();
        }
      
        _allBricks.AddLast(this);
        _stateRecorder = new TransformStateRecorder();
    }

    private bool IsStationary() {

        return _rigidBody.velocity == Vector3.zero;    
    }

    private static bool CheckAllBricksStationary() {

        if (!GameManager.Instance.restIsDisturbed) return false;

        foreach (Brick brick in _allBricks) {

            if (!brick.IsStationary()) {

                return false;
            }        
        }

        return true;    
    }

    private static bool CheckAllBricksReturning()
    {
        foreach (Brick brick in _allBricks)
        {
            if (!brick._stateRecorder.IsEmpty())
            {
                return false;
            }
        }

        return true;
    }

    public static void StartRewinding()
    {
        foreach (Brick brick in _allBricks)
        {
            brick._rigidBody.isKinematic = true;
            brick._collider.enabled = false;

            brick.StartCoroutine(brick.Rewind());
        }
    }

    private static void OnStopRewinding()
    {
        foreach (Brick brick in _allBricks)
        {
            brick._rigidBody.isKinematic = false;
            brick._collider.enabled = true;            
        }
         
        Debug.Log("All are returned!");
    }


    private IEnumerator Rewind() {

        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        while (!_stateRecorder.IsEmpty()) {

            _stateRecorder.PlayNext(transform);
            yield return wait;
        }

        Debug.Log("One is returned!");

        if (CheckAllBricksReturning()) {

            OnStopRewinding();            
        }

        yield return null;
    }


    private void FixedUpdate()
    {
        if (GameManager.Instance.State == GameManager.SessionState.SIMULATING)
        {
            if (CheckAllBricksStationary())
            {
                Debug.Log("All are stationary!");
                GameManager.Instance.NextStage();  // pause
            }

            _stateRecorder.Record(transform);
        }

    }


    private void OnDestroy()
    {
        _allBricks.Remove(this);
    }

}