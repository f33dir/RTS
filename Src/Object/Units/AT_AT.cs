using Godot;
using System;

namespace Unit
{
    public class AT_AT : DynamicUnit
    {
        public override void StatSetup()
        {
            HP = 1000;
            _Team = Team.Enemy;
            _MoveSpeed = 1.5f;
        }
    }
}
