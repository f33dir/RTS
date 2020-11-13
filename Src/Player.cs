using Godot;
using System;
using CameraBase;
using Unit;

namespace Player
{
    public enum CollisionMask
    {
        Nothing,
        Environment,
        Units,
    }
    public class Player : Spatial
    {
        private Godot.Collections.Array<Unit.Unit> _SelectedUnits;
        private uint _Resource;
        private Team _Team = Team.Player;
        private CameraBase.CameraBase _Camera;
        

        public override void _Ready()
        {
            _Camera = GetParent().GetNode<CameraBase.CameraBase>("CameraBase");
            _SelectedUnits = new Godot.Collections.Array<Unit.Unit>();
        }
        public override void _Process(float delta)
        {
            if(Input.IsActionJustReleased("alt_command"))
            {
                if(_Camera.SelectUnits(GetViewport().GetMousePosition(),_SelectedUnits).Count != 0)
                    _SelectedUnits = _Camera.SelectUnits(GetViewport().GetMousePosition(),_SelectedUnits);
                GD.Print("Selected units:");
                foreach (var unit in _SelectedUnits)
                {
                    GD.Print("-> " + unit.Name);
                }
            }
            if(Input.IsActionJustPressed("action_command"))
            {
                var unit = _Camera.GetUnitUnderMouse(GetViewport().GetMousePosition());
                if(unit != null && unit.Team == Team.Player)
                    if(_SelectedUnits.Count == 0 && unit != null)
                    {
                        unit.Select();
                        _SelectedUnits.Add(unit);
                    }
                if(unit != null && (unit.Team == Team.Enemy || unit.Team == Team.Player))
                {
                    foreach (var SelectedUnit in _SelectedUnits)
                    {
                        SelectedUnit.Target = unit;
                        SelectedUnit.TargetCheck();
                    }
                }
                else
                {
                    foreach (var Unit in _SelectedUnits)
                    {
                        Unit.Target = Unit;
                    }
                    MoveSelectedUnits(GetViewport().GetMousePosition());
                }
            }
            if(Input.IsActionJustPressed("exit"))
                GetTree().Quit();
        }
        public void MoveSelectedUnits(Vector2 MousePos)
        {
            var result = _Camera.RaycastFromMousePosition(MousePos,(uint)CollisionMask.Environment);
            if(result != null && result.Count != 0)
                foreach (var unit in _SelectedUnits)
                {
                    unit.State = State.GoingTo;
                    unit.MoveTo((Vector3)result["position"]);
                    GD.Print(unit.State);
                }
        }
    }
}