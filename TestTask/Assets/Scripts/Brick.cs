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

    public static void StartRewinding()
    {



        //GameManager.Instance.NextStage(); //= GameManager.SessionState.REWINDING;

        foreach (Brick brick in _allBricks)
        {
            brick._rigidBody.isKinematic = true;
            brick._collider.enabled = false;

            brick.StartCoroutine(brick.Rewind());
        }

    }

    private static void OnStopRewinding()
    {

        //GameManager.Instance.NextStage(); //= GameManager.SessionState.AIMING;

        foreach (Brick brick in _allBricks)
        {
            brick._rigidBody.isKinematic = false;
            brick._collider.enabled = true;

            brick._stateRecorder.Record(brick._firstFramePosition, brick._firstFrameRotation);
            brick.transform.position = brick._firstFramePosition;
            brick.transform.eulerAngles = brick._firstFrameRotation;
            
            //brick._stateRecorder.Record(brick.transform);
        }

        GameManager.Instance.NextStage(); // start aiming


        Debug.Log("All is returned!");

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

    private void Start()
    {
        _firstFramePosition = transform.position;
        _firstFrameRotation = transform.eulerAngles;

        _stateRecorder.Record(transform);
    }


    private void FixedUpdate()
    {
        if (GameManager.Instance.State == GameManager.SessionState.SIMULATING)
        {
            if (CheckAllBricksStationary())
            {

                OnAllBricksStationary?.Invoke();
                OnAllBricksStationary = null;

                Debug.Log("All stationary!");


                GameManager.Instance.NextStage();  // pause
                //StartRewinding();

            }


            _stateRecorder.Record(transform);



        }

    }


    private void OnDestroy()
    {
        _allBricks.Remove(this);
    }


    

}