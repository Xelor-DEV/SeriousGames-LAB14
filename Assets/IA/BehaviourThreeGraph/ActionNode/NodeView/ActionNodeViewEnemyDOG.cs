using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
[TaskCategory("MyAI/View")]
public class ActionNodeViewEnemyDOG : ActionNodeView
{
    public UnitGame Targed;

    public override void OnStart()
    {
        base.OnStart();
    }
   
    public override TaskStatus OnUpdate()
    {
        // 1) Obtenemos el Health del enemigo visto
        var enemy = _IACharacterVehiculo.AIEye.ViewEnemy;
        // 2) Si no hay ninguno, fallo
        if (enemy == null)
            return TaskStatus.Failure;

        // 3) Comparamos su tipo
        if (enemy._UnitGame == Targed)
            return TaskStatus.Success;

        // 4) Si es distinto, fallo
        return TaskStatus.Failure;
    }

}