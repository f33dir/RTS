using Godot;
using System;
namespace Map
{
    class MapEditor : Node
    {
        private Camera _EditorCam;
        public override void _Ready()
        {
            _EditorCam = (Camera)GetParent();
        }
        private Vector3 PointToGrid()
        {
            Vector3 gridPosition = new Vector3();
            return gridPosition;
        }
        public void PlaceTile(Vector3 position,MapTile tile)
        {

        }
        public void RotateTile(){
            
        }
        public void DeleteTile()
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
