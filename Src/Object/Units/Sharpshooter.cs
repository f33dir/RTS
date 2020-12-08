using Godot;
using System;

namespace Unit
{
    public class Sharpshooter : DynamicUnit
    {
        public override void StatSetup()
        {
            _CanAttackNow = true;
            _AttackPower = 25;
            _AttackSpeed = 1.25f;
            _AttackRange = 15;
            _HP = 100;
            _Protection = 10;
            _MainResource = 0;
            _Team = Team.Player;
        }
    }
}
