using Godot;
using System;
using Unit;

namespace CameraBase
{
    public class CameraBase : Spatial
    {
        private const float MOVE_MARGIN = 30;
        private const float MOVE_SPEED = 30;
        private const float RAY_LENGTH = 10000;

        private Godot.Camera Cam;
        private Vector2 StartSelPos;
        private SelectionBox SelectionBox;
        private Godot.Collections.Array<Unit.testUnit> SelectedUnits;

        public override void _Process(float delta)
        {
            Vector2 mousePos = GetViewport().GetMousePosition();
            CalculateMove(mousePos,delta);
            if(Input.IsActionJustPressed("action_command"))
                MoveSelectedUnits(mousePos);
            if(Input.IsActionJustPressed("alt_command"))
            {
                SelectionBox._startSelPos = mousePos;
                StartSelPos = mousePos;
            }
            if(Input.IsActionPressed("alt_command"))
            {
                SelectionBox._mousePos = mousePos;
                SelectionBox._isVisible = true;
            }
            else SelectionBox._isVisible = false;
            if(Input.IsActionJustReleased("alt_command"))
                SelectUnits(mousePos);
            if(Input.IsActionJustPressed("exit"))
                GetTree().Quit(); // тупо для дебага
        }
        public override void _Ready() // получение указателей на нужные ноды
        {
            Cam = GetNode<Godot.Camera>("Camera");
            SelectionBox = GetNode<SelectionBox>("SelectionBox");
            Input.SetMouseMode(Input.MouseMode.Confined);
            StartSelPos = new Vector2();
            SelectedUnits = new Godot.Collections.Array<testUnit>();
        }
        public void CalculateMove(Vector2 mousePos, float delta) // перемещение камеры
        {
            Vector2 vecSize = GetViewport().Size;
            Vector3 moveVec = Vector3.Zero;

            if(mousePos.x < MOVE_MARGIN)
                moveVec.x -= 1;
            if(mousePos.y < MOVE_MARGIN)
                moveVec.z -= 1;
            if(mousePos.x > vecSize.x - MOVE_MARGIN)
                moveVec.x += 1;
            if(mousePos.y > vecSize.y - MOVE_MARGIN)
                moveVec.z += 1;
            
            moveVec = moveVec.Rotated(Vector3.Up,RotationDegrees.y);
            GlobalTranslate(moveVec*delta*MOVE_SPEED);
        }
        public object GetUnitUnderMouse(Vector2 mousePos)
        {
            var result = RaycastFromMousePosition(mousePos,2);
            if(result != null && result.Count != 0 &&result["collider"] != null)
                return result["collider"];
            return null;
        }
        public void SelectUnits(Vector2 mousePos)
        {
            Godot.Collections.Array<testUnit> NewSelectedUnits = new Godot.Collections.Array<testUnit>();
            if(mousePos.DistanceSquaredTo(StartSelPos) < 16)
            {
                var unit = GetUnitUnderMouse(mousePos);
                if(unit != null)
                {
                    var debug_clone = unit;
                    NewSelectedUnits.Add((testUnit)unit);
                }
            }
            else NewSelectedUnits = GetUnitsInBox(StartSelPos,mousePos);
            if(NewSelectedUnits.Count != 0)
                {
                    foreach (var unit in SelectedUnits)
                    {
                        var clone = (Unit.testUnit)unit;
                        clone.Deselect();
                    }
                    foreach (var unit in NewSelectedUnits)
                    {
                        var clone = (Unit.testUnit)unit;
                        clone.Select();
                    }
                    SelectedUnits = NewSelectedUnits;
                }
        }
        public Godot.Collections.Array<testUnit> GetUnitsInBox(Vector2 TopLeft, Vector2 BottomRight)
        {
            if( TopLeft.x > BottomRight.x)
            {
                var temp = TopLeft.x;
                TopLeft.x = BottomRight.x;
                BottomRight.x = temp;
            }
            if(TopLeft.y > BottomRight.y)
            {
                var temp = TopLeft.y;
                TopLeft.y = BottomRight.y;
                BottomRight.y = temp;
            }
            var Box = new Rect2(TopLeft, BottomRight - TopLeft);
            Godot.Collections.Array<Unit.testUnit> BoxSelectedUnits = new Godot.Collections.Array<testUnit>();
            foreach (var unit in GetTree().GetNodesInGroup("Objects"))
            {
                Unit.testUnit clone = (Unit.testUnit)unit;
                if(Box.HasPoint(Cam.UnprojectPosition(clone.GlobalTransform.origin)))
                    BoxSelectedUnits.Add((testUnit)unit);
            }
            return BoxSelectedUnits;
        }
        public void MoveSelectedUnits(Vector2 mousePos)
        {
            var result = RaycastFromMousePosition(mousePos,1);
            //GD.Print(result["position"]);
            if(result != null && result.Count != 0)
                foreach (var unit in SelectedUnits)
                    unit.MoveTo((Vector3)result["position"]);
                // GetParent().GetTree().CallGroup("Objects","MoveTo",result["position"]);
        }
        public Godot.Collections.Dictionary RaycastFromMousePosition(Vector2 mousePos, uint CollisionMask) // определение положения курсора на карте
        {
            Vector3 RayStart = Cam.ProjectRayOrigin(mousePos);
            Vector3 RayEnd = RayStart + Cam.ProjectRayNormal(mousePos) * RAY_LENGTH;
            PhysicsDirectSpaceState SpaceState = GetWorld().DirectSpaceState;
            return SpaceState.IntersectRay(RayStart,RayEnd,new Godot.Collections.Array(),CollisionMask);
        }
    }
}