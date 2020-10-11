using System;
using Godot;

namespace Map{
    class MapManager : Spatial
    {
        private Map LoadedMap;
        private GridMap gridmap;
        MapFileManager mapfilemanager;
        public override void _Process(float delta)
        {
            if(Input.IsActionJustPressed("debug")){
                LoadMap();
                BuildLoadedMap();
            }
        }
        public void BuildLoadedMap()
        {
            BuildCurrentMap(LoadedMap);
        }

        public void BuildCurrentMap(Map InputMap)
        {
            gridmap.Clear();
            MeshLibrary tileset = (MeshLibrary)ResourceLoader.Load(mapfilemanager.CurrentMapPath+"tileset.meshlib");
            for(int i = 0;i<InputMap.GetSizeX();++i)
            {
                for(int j = 0;j<InputMap.GetSizeY();++j)
                {
                    MapTile current = InputMap.Matrix[i,j];
                    if(current!=null)
                    {
                        gridmap.SetCellItem(i,current.Height,j,(int)current.Type);
                    }
                }
            }
            gridmap.MakeBakedMeshes();
        }

        public void LoadMap()
        {
            LoadedMap = GetMap();
        }

        public Map GetMap()
        {
            mapfilemanager = (MapFileManager)GetNode("/root/MapFileManager");
            return GetMap(mapfilemanager.CurrentMapPath);
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
            return LoadedMap.Matrix;
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
            LoadedMap = output;
            String str;
            str = Newtonsoft.Json.JsonConvert.SerializeObject(output);
            GD.Print("beep");
            String testMapPath = (mapfilemanager.MapPath+"mapfile");
            // System.IO.File.Create(testMapPath);
            System.IO.File.WriteAllText(testMapPath,str);
        }
        public override void _Ready()
        {
            mapfilemanager = (MapFileManager)GetNode("/root/MapFileManager");
            gridmap = GetNode("Map")as GridMap;
        }
    }
}
 