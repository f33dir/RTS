using System;
using System.Collections.Generic;
using Godot;

namespace Map{
    class MapManager : Spatial
    {
        private Map _loadedMap;
        public ColliderBuilder _colliderBuilder;
        public GridMap Gridmap{get;set;}
        MapFileManager _mapfilemanager;
        public MeshLibrary _tileset;
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
            _tileset =ResourceLoader.Load(_mapfilemanager.CurrentMapPath+"tileset.meshlib")as MeshLibrary;
            if(_tileset==null)
            {
                _tileset = ResourceLoader.Load("res://Resources/tilesets/defaulttileset.meshlib")as MeshLibrary;
            }
            Gridmap.MeshLibrary = _tileset;
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
            return GetMap(_mapfilemanager.CurrentMapPath);
        }
        public Map GetMap(String path)
        {
            Map output = new Map(40,40,TileType.Basement);
            if(System.IO.File.Exists(path+"/mapfile"))
            {
                output  = Newtonsoft.Json.JsonConvert.DeserializeObject<Map>(System.IO.File.ReadAllText(path+ "mapfile"));
            }
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
            for(int i = 0;i<_loadedMap.GetSizeX();i++)
            {
                for(int j = 0;j<_loadedMap.GetSizeY();j++)
                {
                    int Height = 20;
                    while(Gridmap.GetCellItem(i,Height,j) == -1)
                    {
                        Height--;
                    }
                    var tile = new MapTile(Gridmap.GetCellItem(i,Height,j),Gridmap.GetCellItemOrientation(i,Height,j),Height,1);
                    _loadedMap.Matrix[i,j] = tile;
                }
            }
            var map = Newtonsoft.Json.JsonConvert.SerializeObject(_loadedMap);
            if(!System.IO.Directory.Exists(_mapfilemanager.CurrentMapPath))
            {
                System.IO.Directory.CreateDirectory(_mapfilemanager.CurrentMapPath);
            }
            System.IO.File.WriteAllText(_mapfilemanager.CurrentMapPath+"mapfile",map);
            if(!System.IO.File.Exists(_mapfilemanager.CurrentMapPath+"tileset.meshlib"))
            {
                var meshlib = new Godot.File();
                meshlib.Open("res://Resources/tilesets/defaulttileset.meshlib",File.ModeFlags.Read);
                string transfer = meshlib.GetPascalString();
                System.IO.File.WriteAllText(_mapfilemanager.CurrentMapPath+"tileset.meshlib",transfer);
            }
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
            _colliderBuilder = (ColliderBuilder)GetNode("ColliderBuilder");
            LoadMap();
            BuildLoadedMap();
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
        public void NewEmptyMap(int x,int y)
        {
            _loadedMap = new Map(x,y,TileType.Basement);
            BuildLoadedMap();
        }
    }
}