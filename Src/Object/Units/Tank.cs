using Godot;
using System;

namespace Unit
{
    public class Tank : DynamicUnit
    {
        public override void StatSetup()
        {
            _HP = 500;
            _Protection = 50;
            _Team = Team.Player;
        }
    }
}
