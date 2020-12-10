using Godot;
using System.Collections.Generic;
using System;
namespace BuildingManager
{
    public class BuilderManager : Spatial
    {
        private Map.MapManager _mapmanager;
        private Spatial Arrow;
        private Map.Map _map;
        private Map.Map _otherworldmap;
        private Dictionary<Player.Player, PlayerBuildingManager> _playerBuilders;  
        public override void _Ready()
        {
            var arrowscene = ResourceLoader.Load<PackedScene>("res://Scenes/Units/arrow.tscn");
            Arrow = (Spatial)arrowscene.Instance();
            AddChild(Arrow);
            _playerBuilders = new Dictionary<Player.Player, PlayerBuildingManager>();
            _mapmanager = GetParent().GetNode<Map.MapManager>("MapManager");
            _map = _mapmanager.GetMap();
            var testplayer =  GetTree().CurrentScene.GetNode<Player.Player>("Player");
            AddPlayerBuildingManager(testplayer);
            var res  = ResourceLoader.Load<PackedScene>("res://Scenes/Units/Portal.tscn");
            Unit.BuildingUnit portal = (Unit.BuildingUnit)res.Instance();
            SetBuilding(ref testplayer, ref portal);
            _playerBuilders[testplayer]._cursorPos = _map.PortalPos;
            build(testplayer);
            res  = ResourceLoader.Load<PackedScene>("res://Scenes/Units/Base.tscn");
            Unit.BuildingUnit BaseBuilding; 
            BaseBuilding = (Unit.BuildingUnit)res.Instance();
            SetBuilding(ref testplayer, ref BaseBuilding);
            _playerBuilders[testplayer]._cursorPos = _map.BasePos;
            build(testplayer);
            HideArrow();
        }
        public override void _PhysicsProcess(float delta)
        {
            foreach(var a in _playerBuilders.Values)
            {
                var dic =  a.player.GetCamera().RaycastFromMousePosition(a.player.GetViewport().GetMousePosition(),1);
                if(dic.Contains("collider"))
                {
                    var col = dic["collider"] as StaticBody;
                    a._cursorPos = PointToGrid(a.player,col.GlobalTransform.origin);
                    Arrow.Translation = getRealPosition(a._cursorPos);
                }
            }
        }
        public void SetBuilding(ref Player.Player player,ref Unit.BuildingUnit building)
        {
            _playerBuilders[player]._currentBuilding = building;
        }
        public void ShowArrow()
        {
            // _playerBuilders[player]._showSilhouette = true;
            Arrow.Visible = true;
        }
        public void HideArrow()
        {   
            // _playerBuilders[player]._showSilhouette = false;
            Arrow.Visible = false;
        }
        public void build(Player.Player player)
        {
            var building = _playerBuilders[player]._currentBuilding;
            var pos = getRealPosition(_playerBuilders[player]._cursorPos);
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
            Vector3 position = new Vector3(GridPosition.x*2+1f,height+1,GridPosition.y*2+1f);
            return position;
        }
        public bool IsBuildable(Player.Player player)
        {
            bool Possible = true;
            var building = _playerBuilders[player]._currentBuilding;
            var pointer = _playerBuilders[player]._cursorPos;
            var cursortile = _map.GetTile(_playerBuilders[player]._cursorPos);
            for(int i = 0;i<building._GridSizeX;i++)
            {
                for(int j = 0;j<building._GridSizeY;j++)
                {
                    if((_map.Matrix[(int)pointer.x+i,(int)pointer.x+j].Height != cursortile.Height)
                    ||(_map.Matrix[(int)pointer.x+i,(int)pointer.x+j].Type != Map.TileType.Plain))
                    {
                        Possible = false;
                    };
                }
            }
            return Possible;
        }
    }
}   
