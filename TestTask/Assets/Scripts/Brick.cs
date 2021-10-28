using System;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{

    private static List<Brick> _bricks = null;

    [SerializeField] private Rigidbody _rigidBody = null;

    public static event Action OnAllBricksStationary = null;


    public static bool SimulationStarted { get; set; } = false;

    private bool _recordingComplete = false;

    private void Awake()
    {
        if (_bricks == null)
        {
            _bricks = new List<Brick>();
        }
      
        _bricks.Add(this);
    }


    private bool IsStationary() {

        return _rigidBody.velocity == Vector3.zero;    
    }

    private static bool CheckAllBricksStationary() {

        foreach (Brick brick in _bricks) {

            if (!brick.IsStationary()) {

                return false;
            }        
        }

        return true;    
    }


    private void FixedUpdate()
    {
        if (SimulationStarted && !_recordingComplete) {

            if (IsStationary())
            {                
                _recordingComplete = true;

                Debug.Log("One is stationary!");

                if (CheckAllBricksStationary()) {

                    OnAllBricksStationary?.Invoke();
                    OnAllBricksStationary = null;

                    Debug.Log("All stationary!");
                }

            }
            else { 
            
                // recording
            
            }
        
        }
    }


    private void OnDestroy()
    {
        _bricks.Remove(this);
    }

}