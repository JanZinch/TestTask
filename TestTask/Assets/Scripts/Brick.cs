using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{

    private static List<Brick> _allBricks = null;

    [SerializeField] private Rigidbody _rigidBody = null;
    [SerializeField] private Collider _collider = null;

    public static event Action OnAllBricksStationary = null;

    private TransformStateRecorder _stateRecorder = null;

    private Vector3 _firstFramePosition = default;
    private Vector3 _firstFrameRotation = default;

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

    public static void RecordFirstFrame() {

        //foreach (Brick brick in _allBricks) {

        //    _stateRecorder.Record(transform);

        //}

            

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
            brick._collider.enabled = false;

            brick.StartCoroutine(brick.Rewind());
        }

    }

    private static void OnStopRewinding()
    {

        GameManager.Instance.GameMode = GameManager.Mode.AIMING;

        foreach (Brick brick in _allBricks)
        {
            brick._rigidBody.isKinematic = false;
            brick._collider.enabled = true;

            brick._stateRecorder.Record(brick._firstFramePosition, brick._firstFrameRotation);
            brick.transform.position = brick._firstFramePosition;
            brick.transform.eulerAngles = brick._firstFrameRotation;
            
            //brick._stateRecorder.Record(brick.transform);
        }

        GameManager.Instance.StartAiming();

        foreach (Brick brick in _allBricks) {

            //brick._stateRecorder.Record(brick._firstFramePosition, brick._firstFrameRotation);

        }

        Debug.Log("All is returned!");

    }


    private IEnumerator Rewind() {

        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        while (!_stateRecorder.IsEmpty()) {

            _stateRecorder.PlayNext(transform);
            yield return wait;
        }

        _recordingComplete = true;
        Debug.Log("One is returned!");


        if (CheckAllBricksReturning()) {

            OnStopRewinding();            
        }

        yield return null;
    }

    private void Start()
    {
        _firstFramePosition = transform.position;
        _firstFrameRotation = transform.eulerAngles;

        _stateRecorder.Record(transform);
    }


    private void FixedUpdate()
    {
        if (GameManager.Instance.GameMode == GameManager.Mode.SIMULATING /* && !_recordingComplete*/)
        {

                if (CheckAllBricksStationary())
                {

                    OnAllBricksStationary?.Invoke();
                    OnAllBricksStationary = null;

                    Debug.Log("All stationary!");

                    StartRewinding();

                }

  
                _stateRecorder.Record(transform);


        }

    }


    private void OnDestroy()
    {
        _allBricks.Remove(this);
    }


    

}