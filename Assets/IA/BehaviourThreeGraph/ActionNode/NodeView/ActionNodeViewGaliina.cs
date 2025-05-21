using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
[TaskCategory("MyAI/View")]
public class ActionNodeViewGallina : ActionNodeView
{
    public UnitGame Targed;

    public override void OnStart()
    {
        base.OnStart();
        Targed = UnitGame.Gallina;
    }
   
    public override TaskStatus OnUpdate()
    {
        var enemy = _IACharacterVehiculo.AIEye.ViewEnemy;
        if (enemy == null)
            return TaskStatus.Failure;

        if (enemy._UnitGame == Targed)
            return TaskStatus.Success;

        return TaskStatus.Failure;
    }

}