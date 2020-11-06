using Godot;
using System;

namespace Unit
{
    public class Sharpshooter : DynamicUnit
    {
        public override void StatSetup()
        {
            _CanAttackNow = true;
            _AttackPower = 50;
            _AttackSpeed = 1.25f;
            _AttackRange = 10;
            _HP = 100;
            _Protection = 10;
            _MainResource = 0;
            _Team = Team.Player;
        }
        // public override void UnitEnteredTheArea(Node unit)
        // {
        //     var UnitInArea = unit as Unit;
        //     if(UnitInArea._Team == Team.Enemy)
        //     {
        //         GD.Print("Unit entered: " + unit.Name);
        //         LookAt(UnitInArea.GlobalTransform.origin, Vector3.Up);
        //     }
        // }
    }
}
