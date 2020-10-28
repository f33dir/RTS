using Godot;
using System;

namespace Unit
{
    public class Sharpshooter : DynamicUnit
    {
        // public override void StatSetup()
        // {
        //     _AttackPower = 20;
        //     _HP = 100;
        //     _MainResource = 0;
        //     _AttackRange = 10f;
        // }
        public override void StatSetup()
        {
            _AttackPower = 100;
            _AttackRange = 10;
            _HP = 100;
            _Protection = 10;
            _MainResource = 0;
        }
    }
}
