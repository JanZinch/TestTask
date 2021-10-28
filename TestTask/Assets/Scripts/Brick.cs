using System;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{

    private static List<Brick> _bricks = null;

    [SerializeField] private Rigidbody _rigidBody = null;

    public static event Action OnAllBricksStationary = null;


    public bool SimulationStarted { get; set; } = false;

    private bool _recordingComplete = false;

    private void Awake()
    {
        _bricks.Add(this);
    }


    private bool isStationary() {

        return _rigidBody.velocity == Vector3.zero;    
    }

    private static bool CheckAllBricksStationary() {

        foreach (Brick brick in _bricks) {

            if (!brick.isStationary()) {

                return false;
            }        
        }

        return true;    
    }


    private void FixedUpdate()
    {
        if (SimulationStarted && !_recordingComplete) {

            if (isStationary())
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