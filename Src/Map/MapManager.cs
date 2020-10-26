using System;
using System.Collections.Generic;
using Godot;

namespace Map{
    class MapManager : Spatial
    {
        private Map _loadedMap;
        private ColliderBuilder _colliderBuilder;
        public GridMap Gridmap{get;set;}
        MapFileManager _mapfilemanager;
        List<PackedScene> _loadedStaticObjects;
        public Map GetLoadedMap(){
            return _loadedMap;
        }
        public override void _Process(float delta)
        {
            if(Input.IsActionJustPressed("debug")){
                LoadMap();
                BuildLoadedMap();
            }
        }
        public void BuildLoadedMap()
        {
            BuildCurrentMap(_loadedMap);
        }

        private void BuildCurrentMap(Map inputMap)
        {
            Gridmap.Clear();
            MeshLibrary tileset = (MeshLibrary)ResourceLoader.Load(_mapfilemanager.CurrentMapPath+"tileset.meshlib");
            Gridmap.MeshLibrary = tileset;
            for(int i = 0;i<inputMap.GetSizeX();++i)
            {
                for(int j = 0;j<inputMap.GetSizeY();++j)
                {
                    MapTile current = inputMap.Matrix[i,j];
                    if(current!=null)
                    {
                        Gridmap.SetCellItem(i,current.Height,j,(int)current.Type);
                    }
                }
            }
            Gridmap.MakeBakedMeshes(); 
            _colliderBuilder.Clear();
            _colliderBuilder.SlowFill();
        }

        public void LoadMap()
        {
            _loadedMap = GetMap();
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

        public void BuildMap(string mapPath)
        {
            Map currentMap = GetMap(mapPath);
            BuildCurrentMap(currentMap);
        }

        public MapTile[,] GetMatrix()
        {
            return _loadedMap.Matrix;
        }
        public void CreateMap(int wight,int height)
        {
            Map output = new Map(wight,height);
            MapTile tile = new MapTile();
            tile.Type = TileType.Basement;
            for(int i = 0;i< wight;i++)
            {
                for(int j = 0;j<height;j++)
                {
                    Gridmap.SetCellItem(i,-1,j,(int)tile.Type,0);
                }
            }
            _loadedMap = output;
        }
        public void SaveMap()
        {
            
        }
        public void SetTestMap()
        {
            Map output = new Map(2,2);
            MapTile tile = new MapTile();
            tile.Height = 20;
            output.SetTile(tile, 0, 0);
            output.SetTile(tile, 1, 1);
            output.SetTile(tile, 1, 0);
            output.SetTile(tile, 0, 1);
            _loadedMap = output;
            String str;
            str = Newtonsoft.Json.JsonConvert.SerializeObject(output);
            GD.Print("beep");
            String testMapPath = (_mapfilemanager.MapPath+"mapfile");
            System.IO.File.WriteAllText(testMapPath,str);
        }
        public override void _Ready()
        {
            _mapfilemanager = (MapFileManager)GetNode("/root/MapFileManager");
            Gridmap = GetNode("Map")as GridMap;
            //debug
            LoadMap();
            BuildLoadedMap();
            _colliderBuilder = (ColliderBuilder)GetNode("ColliderBuilder");
            _colliderBuilder.SlowFill();
            GetParent().Call("clear_navmesh");
            GetParent().Call("bake_navmesh");
        }
        public void PlaceStaticObject(Vector3 position,Vector3 direction,string name)
        {
            MapStaticObject newObject = new MapStaticObject();
            Transform tr = new Transform();
            tr.origin = position;
            direction.y = position.y;
            tr.SetLookAt(new Vector3(0,0,1),direction,new Vector3(0,1,0));
            newObject.Transform = tr;
            newObject.Name = name;
            _loadedMap._staticObjects.Add(newObject);
        }

        private PackedScene LoadStaticObjectScene(string name)
        {
            PackedScene output = ResourceLoader.Load<PackedScene>("res://Resources/MapStaticObjects"+name);
            if (output != null) 
                return output;
            output = ResourceLoader.Load<PackedScene>(_mapfilemanager.CurrentMapPath+name);
            return output;
            return null;
        }
        public void LoadStaticObjects()
        {
            foreach(var currentObject in _loadedMap._staticObjects)
            {
                PackedScene scene = LoadStaticObjectScene(currentObject.Name);
                if (scene != null)
                {
                    Spatial spatial = scene.Instance() as Spatial;
                    if (spatial == null) continue;
                    spatial.Transform = currentObject.Transform;
                    this.AddChild(spatial);
                }
            }
        }
    }
}