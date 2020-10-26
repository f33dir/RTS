using Godot;
using System;
using System.IO;
using System.Collections.Generic;
namespace Map{
public class MapFileManager : Node
    {
        private List<DirectoryInfo> _maps;
        public  string MapPath{get; set;}
        public string CurrentMapPath{get;set;}

        public int ParseMaps()
        {
            if(System.IO.Directory.Exists(MapPath)){
                _maps.Clear();
                var paths = System.IO.Directory.GetFiles(MapPath,"mapinfo",SearchOption.AllDirectories);
                foreach(var pth in paths)
                {
                    DirectoryInfo dir = new DirectoryInfo(pth);
                    dir = dir.Parent;
                    _maps.Add(dir);
                }
                return 0;
            }
            return 1;
        }

        public int ChooseMap(int index)
        {
            if(_maps.Count>index)
            {
                CurrentMapPath = _maps[index].ToString();
                return 0;   
            }
            else
            {
                return 1;
            } 
        }

        public override void _Ready()
        {
            //debug 
            MapPath = "/home/f33dir/Temp/";
            CurrentMapPath = "/home/f33dir/Temp/testMap/";
        }
    }
}