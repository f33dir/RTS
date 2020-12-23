using Godot;
using System;
using System.IO;
using System.Collections.Generic;
namespace Map{
public class MapFileManager : Node
    {
        private List<String> _maps = new List<string>();
        public  string MapPath{get; set;}
        public string CurrentMapPath{get;set;}
        public int ParseMaps()
        {
            _maps.Clear();
            _maps.Add("res://Maps/Map1/");
            _maps.Add("res://Maps/Map2/");
            if(System.IO.Directory.Exists(MapPath)){
                
                var paths = System.IO.Directory.GetFiles(MapPath,"mapfile",SearchOption.AllDirectories);
                foreach(var pth in paths)
                {
                    DirectoryInfo dir = new DirectoryInfo(pth);
                    dir = dir.Parent;
                    _maps.Add(dir.ToString());
                }
                return 0;
            }
            return 1;
        }

        public void ChooseMap()
        {
            var list = GetTree().CurrentScene.GetNode("MapList") as   ItemList;
            var items = list.GetSelectedItems();
            var item = items[0];
                CurrentMapPath = _maps[item]; 
        }

        public override void _Ready()
        {
            //debug 
            MapPath = "/home/f33dir/Temp/";
            CurrentMapPath = "/home/f33dir/Temp/editor/";
        }
    }
}