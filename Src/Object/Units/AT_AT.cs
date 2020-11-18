using Godot;
using System;

namespace Unit
{
    public class AT_AT : DynamicUnit
    {
        public override void StatSetup()
        {
            _CanAttackNow = true;
            _AttackPower = 50;
            _AttackSpeed = 0.75f;
            _AttackRange = 25;
            _HP = 300;
            _Protection = 30;
            _MainResource = 5;
            _Team = Team.Player;
        }
    }
}
