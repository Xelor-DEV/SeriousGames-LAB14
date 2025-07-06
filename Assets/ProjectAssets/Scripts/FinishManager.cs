using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class FinishManager : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnWin;

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
            OnWin?.Invoke();
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