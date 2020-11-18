using Godot;
using System.Collections.Generic;
using System;
namespace BuildingManager
{
    public class BuilderManager : Spatial
    {
        private Map.MapManager _mapmanager;
        private Map.Map _map;
        private Map.Map _otherworldmap;
        private Dictionary<Player.Player, PlayerBuildingManager> _playerBuilders;  
        public override void _Ready()
        {
            _playerBuilders = new Dictionary<Player.Player, PlayerBuildingManager>();
            _mapmanager = GetParent().GetNode<Map.MapManager>("MapManager");
            _map = _mapmanager.GetMap();
            //debug
            var testplayer =  GetTree().CurrentScene.GetNode<Player.Player>("Player");
            AddPlayerBuildingManager(testplayer);
            var res  = ResourceLoader.Load<PackedScene>("res://Scenes/Units/BuildingUnit.tscn");
            Unit.BuildingUnit k = (Unit.BuildingUnit)res.Instance();
            SetBuilding(ref testplayer, ref k);
            build(new Vector2(0,0),testplayer);
        }
        public override void _PhysicsProcess(float delta)
        {
            foreach(var a in _playerBuilders.Values)
            {
                var dic =  a.player.GetCamera().RaycastFromMousePosition(a.player.GetViewport().GetMousePosition(),1);
                if(dic.Contains("Collider"))
                {
                    var col = dic["Collider"] as StaticBody;
                    a._cursorPos = PointToGrid(a.player,col.GlobalTransform.origin);
                }
            }
        }
        public void SetBuilding(ref Player.Player player,ref Unit.BuildingUnit building)
        {
            _playerBuilders[player]._currentBuilding = building;
        }
        public void ShowSilhouette(ref Player.Player player)
        {
            _playerBuilders[player]._showSilhouette = true;
        }
        public void HideSilhouette(ref Player.Player player)
        {   
            _playerBuilders[player]._showSilhouette = false;
        }
        public void build(Vector2 GridPosition,Player.Player player)
        {
            var pos = getRealPosition(GridPosition);
            var building = _playerBuilders[player]._currentBuilding;
            AddChild(_playerBuilders[player]._currentBuilding);
            building.Translate(pos);
        }
        private Vector2 PointToGrid(Player.Player player,Vector3 position)
        {
            var gridPosition3 = new Vector3();
            var gridPosition = new Vector2(-1000,-1000);
            var point = player.GetCamera().RaycastFromMousePosition(GetViewport().GetMousePosition(),1);
            if(point.Contains("collider"))
            {
                gridPosition3 = _mapmanager.Gridmap.WorldToMap(position);
                gridPosition.x = gridPosition3.x;
                gridPosition.y = gridPosition3.z;
            }
            return gridPosition;
        }
        public void AddPlayerBuildingManager(Player.Player player)
        {
            _playerBuilders.Add(player,new PlayerBuildingManager(ref player));
        }
        private Vector3 getRealPosition(Vector2 GridPosition)
        {
            var height = _map.Matrix[(int)GridPosition.x,(int)GridPosition.y].Height;
            Vector3 position = new Vector3(GridPosition.x,height,GridPosition.y);
            return position;
        }
    }
}   
