using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
[TaskCategory("MyAI/View")]
public class ActionNodeNotViewWolf : ActionNodeView
{
    public UnitGame Targed;


    public override void OnStart()
    {
        base.OnStart();
        Targed = UnitGame.Wolf;
    }
   

    public override TaskStatus OnUpdate()
    {
        // 1) Obtenemos el Health del enemigo visto
        var enemy = _IACharacterVehiculo.AIEye.ViewEnemy;
        // 2) Si no hay ninguno, fallo
        if (enemy == null)
            return TaskStatus.Success;

        // 3) Comparamos su tipo
        if (enemy._UnitGame == Targed)
            return TaskStatus.Failure;

        // 4) Si es distinto, fallo
        return TaskStatus.Success;
    }
}