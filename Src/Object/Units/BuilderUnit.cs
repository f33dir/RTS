using Godot;
using System;
using System.Collections.Generic;

namespace Unit
{
    class BuilderUnit : Unit
    {
        enum CommandState
        {
            empty,
            build
        }
        public override void _Ready()
        {
            base._Ready();
        }
        public override void _PhysicsProcess(float delta)
        {   
        }
        public override void StatSetup()
        {
            
        }
        public override void TargetCheck()
        {
            if(Target !=null && Target !=this)
            {
                if(Target.Team == Team.Neutral)
                {
                    _State = State.GoingTo;
                }
                else if(Target.Team == this.Team)
                {
                    _State = State.Building;
                }
            }
        }
        public override void MoveTo(Vector3 target){
            var path = _Navigation.Call("find_path",GlobalTransform.origin,target) as Godot.Collections.Dictionary;
            _PathTo = path["points"] as Vector3[];
        }
        public override void UnitEnteredTheArea(Node Unit){}
        public override void UnitExitedTheArea(Node Unit){}
    }
}