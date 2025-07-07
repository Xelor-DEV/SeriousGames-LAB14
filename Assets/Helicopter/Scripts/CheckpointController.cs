using UnityEngine;
using System.Collections;

public class CheckpointController : MonoBehaviour
{
    public Checkpoint[] CheckpointsList;
    public LookAtTargetController Arrow;

    private Checkpoint CurrentCheckpoint;
    private int CheckpointId;

    // Nueva propiedad para verificar el estado de los checkpoints
    public bool AllCheckpointsPassed { get; private set; }

    public int CurrentCheckpointIndex => CheckpointId;
    public int TotalCheckpoints => CheckpointsList.Length;

    void Start()
    {
        AllCheckpointsPassed = false; // Inicializar como falso
        
        if (CheckpointsList.Length == 0)
        {
            // Si no hay checkpoints, considerar completados por defecto
            AllCheckpointsPassed = true;
            if (Arrow != null) Arrow.gameObject.SetActive(false);
            return;
        }

        for (int index = 0; index < CheckpointsList.Length; index++)
            CheckpointsList[index].gameObject.SetActive(false);

        CheckpointId = 0;
        SetCurrentCheckpoint(CheckpointsList[CheckpointId]);
    }

    private void SetCurrentCheckpoint(Checkpoint checkpoint)
    {
        if (CurrentCheckpoint != null)
        {
            CurrentCheckpoint.gameObject.SetActive(false);
            CurrentCheckpoint.CheckpointActivated -= CheckpointActivated;
        }

        CurrentCheckpoint = checkpoint;
        CurrentCheckpoint.CheckpointActivated += CheckpointActivated;
        if (Arrow != null) Arrow.Target = CurrentCheckpoint.transform;
        CurrentCheckpoint.gameObject.SetActive(true);
    }

    private void CheckpointActivated()
    {
        CheckpointId++;
        if (CheckpointId >= CheckpointsList.Length)
        {
            CurrentCheckpoint.gameObject.SetActive(false);
            CurrentCheckpoint.CheckpointActivated -= CheckpointActivated;
            if (Arrow != null) Arrow.gameObject.SetActive(false);
            AllCheckpointsPassed = true; // Marcar como completado
            return;
        }

        SetCurrentCheckpoint(CheckpointsList[CheckpointId]);
    }

    void Update() { }
}
