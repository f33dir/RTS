using Godot;
using System;

namespace Unit
{
    public class MachineGunTower : Tower
    {
        public override void StatSetup()
        {
            // _HP = 1000;
            _AttackPower = 80;
            _AttackRange = 50f;
            _AttackSpeed = 5f;
            _Cost = 250;
            _CanAttackNow = true;
            _Team = Team.Empty;
            _EnemiesInRange = new Godot.Collections.Array<DynamicUnit>();
            _IsEnemyInRange = false;
            _IsTargetInRange = false;
            _State = State.AttackOnSight;
            var AreaScaleVector = Vector3.One;
            AreaScaleVector *= _AttackRange/10;
            // AreaScaleVector.y = 1f;
            _Area.Scale = AreaScaleVector;
            // _IsAOE = true;
            _IsFreezing = true;
            _Timer.OneShot = false;
            _Timer.WaitTime = _AttackSpeed/10;
        }
    }
}
