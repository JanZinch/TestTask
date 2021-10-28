using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance = null;

    public Mode GameMode = Mode.AIMING;

    [SerializeField] private Cannon _cannon = null;

    private void Awake()
    {
        if (Instance != null) Debug.LogError("Game Manager is not alone");

        Instance = this;
    }


    public void StartSimulation() {

        //Brick.SimulationStarted = true;

        GameMode = Mode.SIMULATING;
    
    }

    private void OnEnable()
    {
        //_cannon.OnHit += () => Brick.SimulationStarted = true;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public enum Mode : byte { 
    
        AIMING, SIMULATING, REWINDING
    
    }

}
