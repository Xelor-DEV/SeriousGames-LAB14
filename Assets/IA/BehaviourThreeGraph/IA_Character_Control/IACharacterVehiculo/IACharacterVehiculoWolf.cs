using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class SpeedFuzzyLogicProfile
{
    [Tooltip("Función difusa para 'distancia cercana' a la gallina.")]
    public FuzzyFunction cercaGallinaFunction = new FuzzyFunction(); // Renombrado para claridad
    [Tooltip("Función difusa para 'distancia media' a la gallina.")]
    public FuzzyFunction mediaGallinaFunction = new FuzzyFunction(); // Renombrado para claridad
    [Tooltip("Función difusa para 'distancia lejana' a la gallina.")]
    public FuzzyFunction lejosGallinaFunction = new FuzzyFunction();   // Renombrado para claridad
}
public class IACharacterVehiculoWolf : IACharacterVehiculo
{
    [Header("Lógica Difusa de Velocidad del Lobo")]
    [Tooltip("Perfil con las funciones difusas (cerca, medio, lejos) para ajustar la velocidad al seguir a la gallina.")]
    public SpeedFuzzyLogicProfile fuzzySpeedProfile = new SpeedFuzzyLogicProfile();

    [Tooltip("Velocidad mínima que el lobo puede alcanzar al usar lógica difusa.")]
    public float minSpeedFuzzy = 3.0f;

    [Tooltip("Velocidad máxima que el lobo puede alcanzar al usar lógica difusa.")]
    public float maxSpeedFuzzy = 12.0f;

    [Tooltip("Velocidad por defecto del lobo cuando no está aplicando lógica difusa (ej. patrullando).")]
    public float defaultPatrolSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        this.LoadComponent();
    }
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (agent != null)
        {
            // Al inicio, el lobo podría tener su velocidad de patrulla por defecto
            agent.speed = defaultPatrolSpeed;
        }
    }
    
    public override void MoveToPosition(Vector3 pos)
    {
        base.MoveToPosition(pos);
    }

    public override void MoveToEnemy()
    {
        if (AIEye == null || AIEye.ViewEnemy == null || AIEye.ViewEnemy.IsDead)
        {
            if (agent != null)
            {
                // Si no hay enemigo o está muerto, podría detenerse o volver a patrullar.
                // Por ahora, simplemente establecemos una velocidad por defecto y no nos movemos.
                // El Behavior Tree debería manejar el cambio a otro estado (ej. patrullar).
                agent.speed = defaultPatrolSpeed;
                // agent.isStopped = true; // Opcional: detener al lobo si no hay objetivo
            }
            return;
        }

        // Si hay un enemigo (gallina) y está vivo:
        if (agent != null)
        {
            // 1. Calcular la distancia a la gallina
            float distanceToGallina = Vector3.Distance(transform.position, AIEye.ViewEnemy.transform.position);

            // 2. Evaluar las funciones de lógica difusa para la distancia
            float fCerca = fuzzySpeedProfile.cercaGallinaFunction.Evaluate(distanceToGallina);
            float fMedia = fuzzySpeedProfile.mediaGallinaFunction.Evaluate(distanceToGallina);
            float fLejos = fuzzySpeedProfile.lejosGallinaFunction.Evaluate(distanceToGallina);

            // 3. Calcular la velocidad deseada usando la fórmula de promedio ponderado
            // (Asegúrate que los Singletons de tus FuzzyFunction estén configurados con los valores de velocidad deseados)
            float numerator = (fCerca * fuzzySpeedProfile.cercaGallinaFunction.Singleton) +
                              (fMedia * fuzzySpeedProfile.mediaGallinaFunction.Singleton) +
                              (fLejos * fuzzySpeedProfile.lejosGallinaFunction.Singleton);

            float denominator = fCerca + fMedia + fLejos;

            float calculatedSpeed;
            if (Mathf.Approximately(denominator, 0))
            {
                // Si ninguna regla se activa, usar la velocidad de patrulla por defecto o la velocidad actual
                calculatedSpeed = defaultPatrolSpeed; // O agent.speed para mantener la actual
                // Debug.LogWarning("Lobo - Lógica Difusa: Denominador es cero. Usando velocidad por defecto.");
            }
            else
            {
                calculatedSpeed = numerator / denominator;
            }

            // 4. Aplicar la velocidad calculada al NavMeshAgent del lobo
            agent.speed = Mathf.Clamp(calculatedSpeed, minSpeedFuzzy, maxSpeedFuzzy);
            // Debug.Log($"Lobo Speed (Fuzzy): {agent.speed} (Dist: {distanceToGallina})");

            // 5. Establecer el destino
            agent.SetDestination(AIEye.ViewEnemy.transform.position);
            // agent.isStopped = false; // Asegurarse que el agente se mueva
        }

        // La lógica de LookEnemy puede seguir aquí o ser llamada por el Behavior Tree también.
        // base.LookEnemy(); // Si quieres que también mire al enemigo desde este método.
    }
    public override void MoveToAllied()
    {
        base.MoveToAllied( );
    }
    public override void MoveToEvadEnemy()
    {
        base.MoveToEvadEnemy( );
    }

}
