using Godot;
using System;

namespace Unit
{
    public class Bruhmmy : DynamicUnit
    {
        public override void StatSetup()
        {
            _Team = Team.Enemy;
            _HP = 100;
        }
    }
}
