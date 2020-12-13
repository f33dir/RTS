using Godot;
using System;

namespace Unit
{
    public class Tank : DynamicUnit
    {
        public override void StatSetup()
        {
            _HP = 500;
            _Cost = 100;
            _MainResource = 5;
            _Team = Team.Enemy;
        }
    }
}
