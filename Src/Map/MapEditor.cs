using Godot;
using System;
namespace Map
{
    class MapEditor : Spatial
    {
        private CameraBase.CameraBase _editorCam;
        private MapManager _mapManager;
        private MapTile _selectedMapTile;
        public override void _Ready()
        {
            _editorCam = GetParent().GetNode("CameraBase")as CameraBase.CameraBase;
            _mapManager = GetParent().GetNode("MapManager")as MapManager;
        }
        private Vector3 PointToGrid()
        {
            var gridPosition = new Vector3();
            var point = ((StaticBody)_editorCam.RaycastFromMousePosition(GetViewport().GetMousePosition(),1)["collider"]);
            if (point == null) return new Vector3();
            gridPosition = _mapManager.Gridmap.WorldToMap(point.Transform.origin);
            return gridPosition;
        }
        public void PlaceTile(Vector3 position,MapTile tile)
        {
            _mapManager.GetMap().SetTile(tile,(int)position.x,(int)position.z);
            _mapManager.BuildLoadedMap();
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

        public override void _Process(float delta)
        {
            if(Input.IsActionJustPressed("editor_test"))
            {
                PointToGrid();
            }
        }
    }
}
