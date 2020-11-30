using Godot;
using System;

namespace Unit
{
    public class AT_AT : DynamicUnit
    {
        public override void StatSetup()
        {
            _CanAttackNow = true;
            _IsEnemyInRange = false;
            _AttackPower = 50;
            _AttackSpeed = 0.75f;
            _AttackRange = 1;
            _HP = 300;
            _Protection = 30;
            _MainResource = 5;
            _Team = Team.Enemy;
            var AreaScaleVector = Vector3.Zero;
            AreaScaleVector.x = (float)_AttackRange / 10;
            AreaScaleVector.z = (float)_AttackRange / 10;
            AreaScaleVector.y = 0.001f;
            _Area.Scale = AreaScaleVector;
        }
    }
}
