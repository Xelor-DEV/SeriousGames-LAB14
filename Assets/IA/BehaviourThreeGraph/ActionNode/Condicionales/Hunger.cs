using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class Hunger : MonoBehaviour
{
    [Header("Hunger UI")]
    public Image HungerBarLocal;

    [Header("Hunger Settings")]
    [Tooltip("Valor actual de hambre")]
    public int hunger;
    [Tooltip("Valor máximo de hambre")]
    public int hungerMax = 100;
    [Tooltip("Cuánto hambre se pierde por segundo")]
    public int hungerDecayPerSecond = 1;
    [Tooltip("Daño de salud por segundo cuando la hambre llega a 0")]
    public int damagePerSecondWhenStarving = 5;

    private Health _health;
    private Coroutine _decayRoutine;

    public bool IsStarving => hunger <= 0;

    void Awake()
    {
        // Referencia al componente Health para aplicarle daño
        _health = GetComponent<Health>();
    }

    void OnEnable()
    {
        
       
        UpdateHungerBar();

        // Arranco la rutina de decaimiento
        _decayRoutine = StartCoroutine(HungerDecay());
    }

    void OnDisable()
    {
        if (_decayRoutine != null)
            StopCoroutine(_decayRoutine);
    }

    private IEnumerator HungerDecay()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            // Disminuye hambre
            hunger = Mathf.Max(0, hunger - hungerDecayPerSecond);
            UpdateHungerBar();

            // Si está hambriento, le hacemos daño
            if (IsStarving)
            {
                _health.Damage(damagePerSecondWhenStarving, null);
            }
        }
    }

    /// <summary>
    /// Llama esto si quieres "comer" algo y restaurar hambre.
    /// </summary>
    /// <param name="amount">Cantidad de hambre a restaurar.</param>
    public void Eat(int amount)
    {
        amount = 10;
        hunger = Mathf.Min(hungerMax, hunger + amount);
        UpdateHungerBar();
    }

    private void UpdateHungerBar()
    {
        if (HungerBarLocal != null && hungerMax > 0)
        {
            HungerBarLocal.fillAmount = (float)hunger / hungerMax;
        }
    }
}
