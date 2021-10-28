using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{

    private static List<Brick> _allBricks = null;

    [SerializeField] private Rigidbody _rigidBody = null;

    public static event Action OnAllBricksStationary = null;

    private TransformStateRecorder _stateRecorder = null;


    //public static bool SimulationStarted { get; set; } = false;

    private bool _recordingComplete = false;
    private bool _rewindingComplete = false;

    private void Awake()
    {
        if (_allBricks == null)
        {
            _allBricks = new List<Brick>();
        }
      
        _allBricks.Add(this);

        _stateRecorder = new TransformStateRecorder();
    }


    private bool IsStationary() {

        return _rigidBody.velocity == Vector3.zero;    
    }

    private static bool CheckAllBricksStationary() {

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

    private static void StartRewinding()
    {

        GameManager.Instance.GameMode = GameManager.Mode.REWINDING;

        foreach (Brick brick in _allBricks)
        {
            brick._rigidBody.isKinematic = true;

            brick.StartCoroutine(brick.Rewind());
        }

    }

    private IEnumerator Rewind() {

        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        while (!_stateRecorder.IsEmpty()) {

            transform.position = _stateRecorder.Play();
            yield return wait;
        }

        _recordingComplete = true;
        Debug.Log("One is returned!");


        if (CheckAllBricksReturning()) {

            GameManager.Instance.GameMode = GameManager.Mode.AIMING;
            Debug.Log("All is returned!");
        }

        yield return null;
    }


    

    private void FixedUpdate()
    {
        if (GameManager.Instance.GameMode == GameManager.Mode.SIMULATING && !_recordingComplete)
        {

            if (IsStationary())
            {
                _recordingComplete = true;

                Debug.Log("One is stationary!");

                if (CheckAllBricksStationary())
                {

                    OnAllBricksStationary?.Invoke();
                    OnAllBricksStationary = null;

                    Debug.Log("All stationary!");

                    StartRewinding();

                    //GameManager.Instance.GameMode = GameManager.Mode.REWINDING;
                }

            }
            else
            {

                _stateRecorder.Record(transform.position);

                // recording

            }

        }
        //else if (GameManager.Instance.GameMode == GameManager.Mode.REWINDING) {

        //    transform.position = _stateRecorder.Play();
        
        //}
    }


    private void OnDestroy()
    {
        _allBricks.Remove(this);
    }


    

}