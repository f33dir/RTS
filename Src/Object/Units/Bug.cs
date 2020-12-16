using Godot;
using System;

namespace Unit
{
    public class Bug : DynamicUnit
    {
        public override void StatSetup()
        {
            HP = 1000;
            _Cost = 25;
            _Team = Team.Enemy;
            _MoveSpeed = 1.5f;
        }
    }
}
