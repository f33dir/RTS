using Godot;
using System;
using Unit;

namespace CameraBase
{
    public class CameraBase : Spatial
    {
        // Data is in units (godot measure)
        private const float MOVE_MARGIN = 30;
        private const float MOVE_SPEED = 30;
        private const float RAY_LENGTH = 10000;
        private const float ZOOM_SPEED = 25;
        private const float MAX_ZOOM_IN = 13;   // basically just y limitations
        private const float MAX_ZOOM_OUT = 25; // currently doesn't work (saddly)
        private const float MAX_X_MOVE_VALUE = 40;
        private const float MAX_Z_MOVE_VALUE = 50;
        private const float MIN_MOVE_VALUE = 10;
        //Godot nodes
        private Godot.Camera Cam;
        private Vector2 StartSelPos;
        private SelectionBox SelectionBox;
        //Camera movement & user input
        public override void _Process(float delta)
        {
            Vector2 mousePos = GetViewport().GetMousePosition();
            CalculateMove(mousePos,delta);
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
            if(Input.IsActionJustReleased("zoom_in"))
            {
                Vector3 moveVec = GlobalTransform.origin.Normalized();
                moveVec.y -= ZOOM_SPEED;
                moveVec.z -= MOVE_SPEED;
                moveVec = moveVec.Rotated(Vector3.Up,RotationDegrees.y);
                GlobalTranslate(moveVec.Normalized());
                GD.Print(GlobalTransform.origin);
            }
            if(Input.IsActionJustReleased("zoom_out"))
            {
                Vector3 moveVec = GlobalTransform.origin.Normalized();
                moveVec.y += ZOOM_SPEED;
                moveVec.z += MOVE_SPEED;
                moveVec = moveVec.Rotated(Vector3.Up,RotationDegrees.y);
                GlobalTranslate(moveVec.Normalized());
                GD.Print(GlobalTransform.origin);
            }
        }
        // Getting references to needed nodes & basic setup
        public override void _Ready()
        {
            Cam = GetNode<Godot.Camera>("Camera");
            SelectionBox = GetNode<SelectionBox>("SelectionBox");
            // Input.SetMouseMode(Input.MouseMode.Confined); // это зло
            StartSelPos = new Vector2();
        }
        // Calculate camera movement if cursor is at the window border
        public void CalculateMove(Vector2 mousePos, float delta)
        {
            GD.Print("x -> " + GlobalTransform.origin.x + " z -> " + GlobalTransform.origin.z);
            // if(Mathf.Abs(GlobalTransform.origin.x) <= 15 || Mathf.Abs(GlobalTransform.origin.z) <= 25 )
            // {
                // public static int X_MOVE_MARGIN = 0;
                
                Vector2 vecSize = GetViewport().Size;
                Vector3 moveVec = Vector3.Zero;

                if(mousePos.x < MOVE_MARGIN && GlobalTransform.origin.x >= MIN_MOVE_VALUE)
                    moveVec.x -= 1;
                if(mousePos.y < MOVE_MARGIN  && GlobalTransform.origin.z >= MIN_MOVE_VALUE)
                    moveVec.z -= 1;
                if(mousePos.x > vecSize.x - MOVE_MARGIN  && GlobalTransform.origin.x <= MAX_X_MOVE_VALUE)
                    moveVec.x += 1;
                if(mousePos.y > vecSize.y - MOVE_MARGIN  && GlobalTransform.origin.z <= MAX_Z_MOVE_VALUE)
                    moveVec.z += 1;
                
                moveVec = moveVec.Rotated(Vector3.Up,RotationDegrees.y);
                GlobalTranslate(moveVec*delta*MOVE_SPEED);
            // }
        }
        //Get single unit right under mouse
        public Unit.Tower GetUnitUnderMouse(Vector2 mousePos)
        {
            var result = RaycastFromMousePosition(mousePos,4);
            if(result != null && result.Count != 0 && result["collider"] != null)
                return (Unit.Tower)result["collider"];
            return null;
        }
        // Select "selected" units
        public Godot.Collections.Array<Unit.Tower> SelectUnits(Vector2 mousePos, Godot.Collections.Array<Unit.Tower> Units)
        {
            Godot.Collections.Array<Unit.Tower> NewSelectedUnits = new Godot.Collections.Array<Unit.Tower>();
            if(mousePos.DistanceSquaredTo(StartSelPos) < 9)
            {
                var unit = GetUnitUnderMouse(mousePos);
                if(unit != null)
                {
                    // if(unit.Team == Team.Player)
                        NewSelectedUnits.Add((Unit.Tower)unit);
                }
            }
            // else NewSelectedUnits = GetUnitsInBox(StartSelPos,mousePos);
            if(NewSelectedUnits.Count != 0)
            {
                foreach (var unit in Units)
                {
                    if(unit != null)
                        unit.Deselect();
                }
                foreach (var unit in NewSelectedUnits)
                {
                    unit.Select();
                }
                return NewSelectedUnits;
            }
            return NewSelectedUnits;
        }
        // Get units in SelectionBox
        // public Godot.Collections.Array<Unit.Tower> GetUnitsInBox(Vector2 TopLeft, Vector2 BottomRight)
        // {
        //     if( TopLeft.x > BottomRight.x)
        //     {
        //         var temp = TopLeft.x;
        //         TopLeft.x = BottomRight.x;
        //         BottomRight.x = temp;
        //     }
        //     if(TopLeft.y > BottomRight.y)
        //     {
        //         var temp = TopLeft.y;
        //         TopLeft.y = BottomRight.y;
        //         BottomRight.y = temp;
        //     }
        //     var Box = new Rect2(TopLeft, BottomRight - TopLeft);
        //     Godot.Collections.Array<Unit.Tower> BoxSelectedUnits = new Godot.Collections.Array<Unit.Tower>();
        //     foreach (var unit in GetTree().GetNodesInGroup("Units"))
        //     {
        //         // Unit.Tower clone = unit as Unit.Tower;
        //         if(unit as Unit.Tower != null && Box.HasPoint(Cam.UnprojectPosition((unit as Unit.Tower).GlobalTransform.origin)))
        //             BoxSelectedUnits.Add((Unit.Tower)unit);
        //     }
        //     return BoxSelectedUnits;
        // }
        //Project a RayCast form camera to cursor position check for collision with smth
        public Godot.Collections.Dictionary RaycastFromMousePosition(Vector2 mousePos, uint CollisionMask) // определение положения курсора на карте
        {
            Vector3 RayStart = Cam.ProjectRayOrigin(mousePos);
            Vector3 RayEnd = RayStart + Cam.ProjectRayNormal(mousePos) * RAY_LENGTH;
            PhysicsDirectSpaceState SpaceState = GetWorld().DirectSpaceState;
            return SpaceState.IntersectRay(RayStart,RayEnd,new Godot.Collections.Array(),CollisionMask);
        }
    }
}