using Godot;
using System.Collections.Generic;
using System;

public class BuilderManager : Spatial
{
    private Map.MapManager _mapmanager;
    private Map.Map _map;
    private Map.Map _otherworldmap;
    private Dictionary<Player,Unit.BuildingUnit> _currentBuildings;
    private bool _silhouete = false;
    public override void _Ready()
    {
        _mapmanager = (GetNode<Map.MapManager>("DetourNavigation/DetourNavigationMesh/MapManager"));
        _map = _mapmanager.GetMap();
    }
    public override void _PhysicsProcess(float delta)
    {
        
    }
    public void SetBuilding(ref Player player,Unit.BuildingUnit building)
    {
        if(_currentBuildings.ContainsKey(player))
        {
            _currentBuildings.Remove(player);
        }
        _currentBuildings.Add(player,building);
    }
    public void ShowSilhouette()
    {
        _silhouete = true;
    }
    public void HideSilhouette()
    {
        _silhouete = false;
    }
    public bool IsBuildable(Vector3 position,Player player){
        bool possibility = true;
        Vector2 realPosition = PointToGrid(player,position);
        Unit.BuildingUnit building  = _currentBuildings[player];
        realPosition.x -= (building._GridSizeX/2);
        realPosition.y -= (building._GridSizeY/2);
        Map.MapTile startile;
        startile = _map.GetTile(realPosition);
        Map.MapTile currentile;
        for(int i = 0;i<building._GridSizeX;i++)
        {
            for(int j = 0;j<building._GridSizeY;j++)
            {
                var currentpos = realPosition+(new Vector2(i,j));
                currentile = _map.GetTile(currentpos);
                if(!((currentile.Height ==startile.Height)&&(currentile.Type == startile.Type)))
                {
                    possibility = false;
                }

            }
        }
        return possibility;
    }
    public void build(Vector3 GridPosition,Unit.BuildingUnit building)
    {

    }
    private Vector2 PointToGrid(Player player,Vector3 position)
        {
            var gridPosition3 = new Vector3();
            var gridPosition = new Vector2();
            var point = GetNode<CameraBase.CameraBase>("CameraBase").RaycastFromMousePosition(GetViewport().GetMousePosition(),1);
            if(point.Contains("collider"))
            {
                gridPosition3 = _mapmanager.Gridmap.WorldToMap(((StaticBody)point["collider"]).GlobalTransform.origin);
            }
            gridPosition.x =gridPosition3.x;
            gridPosition.y =gridPosition3.z;
            return gridPosition;
        }
}
