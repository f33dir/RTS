using Godot;
using System;
using CameraBase;
using Unit;

namespace Player
{
    //Collision mask of game objects like building, units etc.
    public enum CollisionMask
    {
        Nothing,
        Environment,
        Units,
    }
    public enum MouseModifier // пока пусть будет енум на всякий, 
    {                         //шифт лучше вынести в отдельный триггер для ситуаций по типу shift+"модификатор"+ПКМ(ЛКМ)
        None = 0,
        Attack,               // "A" modifier
    }
    public class Player : Spatial
    {
        private Godot.Collections.Array<Unit.Unit> _SelectedUnits;
        private Unit.Unit _CurrentUnit;
        private MouseModifier _Modifier;
        private bool _ShiftModifier;
        private uint _Resource;
        private Team _Team = Team.Player;
        private CameraBase.CameraBase _Camera;
        //Setup
        public override void _Ready()
        {
            _Camera = GetParent().GetNode<CameraBase.CameraBase>("CameraBase");
            _SelectedUnits = new Godot.Collections.Array<Unit.Unit>();
        }
        //Player interactions
        public override void _Process(float delta)
        {
            if(Input.IsActionJustReleased("alt_command"))
            {
                if(_Modifier == MouseModifier.Attack)
                {
                    if(_SelectedUnits.Count != 0)
                    {
                        foreach (var Unit in _SelectedUnits)
                            Unit.State = State.AttackOnSight;
                        MoveSelectedUnits(GetViewport().GetMousePosition());
                    }
                }
                else if(_CurrentUnit != null)
                    _CurrentUnit = null;
                else
                {
                    if(_Camera.SelectUnits(GetViewport().GetMousePosition(),_SelectedUnits).Count != 0)
                        _SelectedUnits = _Camera.SelectUnits(GetViewport().GetMousePosition(),_SelectedUnits);
                    GD.Print("Selected units:");
                    foreach (var unit in _SelectedUnits)
                    {
                        GD.Print("-> " + unit.Name);
                    }
                }
            }
            if(Input.IsActionJustPressed("action_command"))
            {
                if(_Modifier == MouseModifier.Attack)
                    _Modifier = MouseModifier.None;
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
            if(Input.IsActionJustPressed("Tab"))
            {
                if(_CurrentUnit != null && _SelectedUnits != null &&_SelectedUnits.Count != 0)
                {
                    _CurrentUnit = _SelectedUnits[(_SelectedUnits.IndexOf(_CurrentUnit)+1)%_SelectedUnits.Count];
                }
            }
            if(Input.IsActionJustPressed("AttackModifier"))
                _Modifier = MouseModifier.Attack;
            // if(Input.IsActionJustPressed("exit"))
        }
        //Self-explanatory
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
        public CameraBase.CameraBase GetCamera()
        {
            return _Camera;
        }
    }
}
