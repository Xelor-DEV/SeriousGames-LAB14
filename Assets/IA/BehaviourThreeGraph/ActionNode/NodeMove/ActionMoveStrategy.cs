using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
[TaskCategory("MyAI/Move")]
public class ActionMoveStrategy : ActionNodeVehicle
{
    public override void OnStart()
    {
        base.OnStart();
    }
    public override TaskStatus OnUpdate()
    {
        if(_IACharacterVehiculo.health.IsDead)
            return TaskStatus.Failure;

        SwitchUnit();

        return TaskStatus.Success;

    }
    void SwitchUnit()
    {


        switch (_UnitGame)
        {
            case UnitGame.Wolf:
                
                break;
            case UnitGame.Gallina:
                if (_IACharacterVehiculo is IACharacterVehiculoGallina)
                {
                    ((IACharacterVehiculoGallina)_IACharacterVehiculo).MoveToStrategy();
                    ((IACharacterVehiculoGallina)_IACharacterVehiculo).LookEnemy();
                }
                break;
            case UnitGame.None:
                break;
            default:
                break;
        }



    }

}