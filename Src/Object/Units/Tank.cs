using Godot;
using System;

namespace Unit
{
    public class Tank : DynamicUnit
    {
        public override void StatSetup()
        {
            _CanAttackNow = true;
            _AttackPower = 100;
            _AttackSpeed = 1.0f;
            _AttackRange = 25;
            _HP = 500;
            _Protection = 50;
            _MainResource = 5;
            _Team = Team.Player;
        }
    }
}
