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
            
            _selectedMapTile = new MapTile();
            //debug
            _selectedMapTile.Type = TileType.Pit;
            _editorCam = GetParent().GetNode("CameraBase")as CameraBase.CameraBase;
            _mapManager = GetParent().GetNode("MapManager")as MapManager;
            _mapManager.EditorMode = true;

        }
        private Vector3 PointToGrid()
        {
            var gridPosition = new Vector3();
            var point = _editorCam.RaycastFromMousePosition(GetViewport().GetMousePosition(),1);
            if(point.Contains("collider"))
            {
                gridPosition = _mapManager.Gridmap.WorldToMap(((StaticBody)point["collider"]).GlobalTransform.origin);
            }
            return gridPosition;
        }
        public void PlaceTile(Vector3 position,MapTile tile)
        {
            position.y += 1;
            _mapManager.GetMap().SetTile(tile,(int)position.x,(int)position.z);
            _mapManager.Gridmap.SetCellItem((int)position.x,(int)position.y,(int)position.z,(int)tile.Type);
            _mapManager.Gridmap.MakeBakedMeshes();
            _mapManager._colliderBuilder.PlaceColliderBox(1,1,position);
        }
        public void RotateTile(Vector3 position)
        {
            var orient = _mapManager.Gridmap.GetCellItemOrientation((int)position.x,(int)position.y,(int)position.z);
            orient = (orient+1);
            var item = _mapManager.Gridmap.GetCellItem((int)position.x,(int)position.y,(int)position.z);
            _mapManager.Gridmap.SetCellItem((int)position.x,(int)position.y,(int)position.z,item,orient);
        }
        public void DeleteTile(Vector3 position)
        {
            _mapManager.Gridmap.SetCellItem((int)position.x,(int)position.y,(int)position.z,-1);
            var point = _editorCam.RaycastFromMousePosition(GetViewport().GetMousePosition(),1);
            if(point.Contains("collider")){
            var obj =  point["collider"] as StaticBody;
                obj.QueueFree();
            }
        }
        public void PlaceStaticObject()
        {

        }
        public void DeleteStaticObject()
        {

        }
        public void ChangeTile()
        {
            int currentType = (int)_selectedMapTile.Type;
            currentType = (currentType+1)%_mapManager._tileset.GetItemList().GetLength(0);
            _selectedMapTile.Type = (TileType)currentType;
        }
        public void ChooseStaticObject()
        {

        }
        public void NewMap()
        {
            _mapManager.NewEmptyMap(25,25);
        }
        public void SetPortal2(Vector3 pos3d){
            // var pos3d = PointToGrid();
            _mapManager.SetPortal2(new Vector2(pos3d.x,pos3d.z));
        }
        public void SetPortal(Vector3 pos3d){
            // var pos3d = PointToGrid();
            _mapManager.SetPortal(new Vector2(pos3d.x,pos3d.z));
        }
        public void SetBase(Vector3 pos3d){
            // var pos3d = PointToGrid();
            _mapManager.SetBase(new Vector2(pos3d.x,pos3d.z));
        }
        public override void _Process(float delta)
        {
            if(Input.IsActionJustPressed("editor_placetile"))
            {
                PlaceTile(PointToGrid(),_selectedMapTile);
            }
            if(Input.IsActionJustPressed("editor_deletetile"))
            {
                DeleteTile(PointToGrid());
            }
            if(Input.IsActionJustPressed("editor_rotate"))
            {
                RotateTile(PointToGrid());
            }
            if(Input.IsActionJustPressed("editor_save"))
            {
                _mapManager.SaveMap();
            };
            if(Input.IsActionJustPressed("editor_new"))
            {
                NewMap();
            }
            if(Input.IsActionJustPressed("editor_changetile"))
            {
                ChangeTile();
            }
            if(Input.IsActionJustPressed("editor_placeportal"))
            {
                SetPortal(PointToGrid());
            }
            if(Input.IsActionJustPressed("editor_placebase"))
            {
                SetBase(PointToGrid());
            }
            if(Input.IsActionJustPressed("editor_placeportal2")){
                SetPortal2(PointToGrid());
            }
            if(Input.IsActionJustPressed("editor_resetbuildings"))
            {
                SetBase(new Vector3(0,0,0));
                SetPortal(new Vector3(0,0,0));
                SetPortal2(new Vector3(0,0,0));
            }
        }
    }

}