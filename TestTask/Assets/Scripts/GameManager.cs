using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Cannon _cannon = null;
    [SerializeField] private TextMeshProUGUI _continueUIText = null;

    public bool CollisionHappened { get; set; } = false;

    public static GameManager Instance { get; private set; } = null;

    public SessionState State { get; private set; } = SessionState.AIMING;

    
    private void Awake()
    {
        if (Instance != null) Debug.LogError("Game Manager is not alone");

        Instance = this;
    }

    private void Start()
    {
        _continueUIText.gameObject.SetActive(false);
    }


    public void NextStage() {

        switch (State) {

            case SessionState.AIMING:

                State = SessionState.SIMULATING;

                Debug.Log("New State: " + State);

                break;

            case SessionState.SIMULATING:

                State = SessionState.PAUSE;

                Debug.Log("New State: " + State);
                CollisionHappened = false;
                _continueUIText.gameObject.SetActive(true);

                break;

            case SessionState.PAUSE:

                State = SessionState.REWINDING;

                Debug.Log("New State: " + State);

                
                _continueUIText.gameObject.SetActive(false);
                Brick.StartRewinding();
                _cannon.StartRewinding();
                
                break;

            case SessionState.REWINDING:

                State = SessionState.AIMING;

                Debug.Log("New State: " + State);

                _cannon.SetProjectile();
                
                break;
        
        }
    
    }


    private void OnDestroy()
    {
        Instance = null;
    }

    public enum SessionState : byte { 
    
        AIMING, SIMULATING, PAUSE, REWINDING
    
    }

}
