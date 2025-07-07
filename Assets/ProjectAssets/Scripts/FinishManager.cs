using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class FinishManager : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnWin;

    [SerializeField] private CheckpointController checkpointController;

    private void Start()
    {
        Time.timeScale = 1f;
    }

    private void OnEnable()
    {
        OnWin.AddListener(Pause);
    }

    private void OnDisable()
    {
        OnWin.RemoveListener(Pause);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            bool allCheckpointsPassed = checkpointController == null || checkpointController.AllCheckpointsPassed;
            
            if (allCheckpointsPassed)
            {
                OnWin?.Invoke();
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}