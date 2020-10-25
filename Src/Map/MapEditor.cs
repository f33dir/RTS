using Godot;
using System;
namespace Map
{
    class MapEditor : Spatial
    {
        private CameraBase _EditorCam;
        public override void _Ready()
        {
            _EditorCam = GetParent().GetNode("CameraBase")as CameraBase;
        }
        private Vector3 PointToGrid()
        {
            Vector3 gridPosition = new Vector3();
            var point = _EditorCam.RaycastFromMousePosition(GetViewport().GetMousePosition(),1)["position"];
            return gridPosition;
        }
        public void PlaceTile(Vector3 position,MapTile tile)
        {

        }
        public void RotateTile(Vector3 position)
        {
            
        }
        public void DeleteTile(Vector3 position)
        {
            
        }
        public void PlaceStaticObject()
        {

        }
        public void DeleteStaticObject()
        {

        }
        public void ChooseTile()
        {

        }
        public void ChooseStaticObject()
        {

        }
    }
}
