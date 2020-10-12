using System;
using System.Collections.Generic;
using Godot;

namespace Map{
    class MapManager : Spatial
    {
        private Map _LoadedMap;
        private GridMap _gridmap;
        MapFileManager _mapfilemanager;
        List<PackedScene> _loadedStaticObjects;
        public override void _Process(float delta)
        {
            if(Input.IsActionJustPressed("debug")){
                LoadMap();
                BuildLoadedMap();
            }
        }
        public void BuildLoadedMap()
        {
            BuildCurrentMap(_LoadedMap);
        }

        public void BuildCurrentMap(Map InputMap)
        {
            _gridmap.Clear();
            MeshLibrary tileset = (MeshLibrary)ResourceLoader.Load(_mapfilemanager.CurrentMapPath+"tileset.meshlib");
            for(int i = 0;i<InputMap.GetSizeX();++i)
            {
                for(int j = 0;j<InputMap.GetSizeY();++j)
                {
                    MapTile current = InputMap.Matrix[i,j];
                    if(current!=null)
                    {
                        _gridmap.SetCellItem(i,current.Height,j,(int)current.Type);
                    }
                }
            }
            _gridmap.MakeBakedMeshes();
        }

        public void LoadMap()
        {
            _LoadedMap = GetMap();
        }

        public Map GetMap()
        {
            _mapfilemanager = (MapFileManager)GetNode("/root/MapFileManager");
            return GetMap(_mapfilemanager.CurrentMapPath);
        }

        public Map GetMap(String path)
        {
            Map output  = Newtonsoft.Json.JsonConvert.DeserializeObject<Map>(System.IO.File.ReadAllText(path+ "mapfile"));
            return output;
        }

        public void BuildMap(string MapPath)
        {
            Map CurrentMap = GetMap(MapPath);
            BuildCurrentMap(CurrentMap);
        }

        public MapTile[,] getMatrix()
        {
            return _LoadedMap.Matrix;
        }

        public void setTestMap()
        {
            Map output = new Map(10,10);
            MapTile tile = new MapTile();
            tile.Height = 20;
            output.SetTile(tile, 0, 0);
            output.SetTile(tile, 1, 1);
            output.SetTile(tile, 1, 0);
            output.SetTile(tile, 0, 1);
            _LoadedMap = output;
            String str;
            str = Newtonsoft.Json.JsonConvert.SerializeObject(output);
            GD.Print("beep");
            String testMapPath = (_mapfilemanager.MapPath+"mapfile");
            // System.IO.File.Create(testMapPath);
            System.IO.File.WriteAllText(testMapPath,str);
        }
        public override void _Ready()
        {
            _mapfilemanager = (MapFileManager)GetNode("/root/MapFileManager");
            _gridmap = GetNode("Map")as GridMap;
        }
        public void PlaceStaticObject(MapStaticObject mapStaticObject,Vector3 place,int index)
        {
            
        }
        public void LoadStaticObjects()
        {
            foreach(var item in _LoadedMap._staticObjects)
            {
                PackedScene next = ResourceLoader.Load<PackedScene>("res://Resources/MapStaticObjects"+item._name);
                if(next == null)
                {
                    next = ResourceLoader.Load<PackedScene>(_mapfilemanager.CurrentMapPath+item._name);
                }
                if(next != null)
                {
                    _loadedStaticObjects.Add(next);
                }
            }
            if(_loadedStaticObjects.Count!=_LoadedMap._staticObjects.Count)
            {
                GD.Print("Map Static resources not found");
            }
        }

    }
}
 