using Godot;
using System;
using CameraBase;
using Unit;

public class Player : Spatial
{
    private Godot.Collections.Array<Unit.Unit> _SelectedUnits;
    private uint _Resource;
    private CameraBase.CameraBase _Camera;

    public override void _Ready()
    {
        _Camera = GetParent().GetNode<CameraBase.CameraBase>("CameraBase");
        _SelectedUnits = new Godot.Collections.Array<Unit.Unit>();
    }
    public override void _Process(float delta)
    {
        if(Input.IsActionJustReleased("alt_command"))
            _SelectedUnits = _Camera.SelectUnits(GetViewport().GetMousePosition(),_SelectedUnits);
        if(Input.IsActionJustPressed("action_command"))
        {
            var unit = _Camera.GetUnitUnderMouse(GetViewport().GetMousePosition());
            if(_SelectedUnits.Count == 0 && unit != null)
            {
                unit.Select();
                _SelectedUnits.Add(unit);
            }
            else MoveSelectedUnits(GetViewport().GetMousePosition());
        }
        if(Input.IsActionJustPressed("exit"))
            GetTree().Quit();
    }
    public void MoveSelectedUnits(Vector2 MousePos)
    {
        var result = _Camera.RaycastFromMousePosition(MousePos,1);
        if(result != null && result.Count != 0)
            foreach (var unit in _SelectedUnits)
            {
                unit.MoveTo((Vector3)result["position"]);
            }
    }
}
