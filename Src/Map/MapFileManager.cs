using Godot;
using System;
using System.IO;
using System.Collections.Generic;
namespace Map{
public class MapFileManager : Node
    {
        private List<DirectoryInfo> Maps;
        public  string MapPath{get; set;}
        public string CurrentMapPath{get;set;}

        public int ParseMaps()
        {
            if(System.IO.Directory.Exists(MapPath)){
                Maps.Clear();
                string[]paths = System.IO.Directory.GetFiles(MapPath,"mapinfo",SearchOption.AllDirectories);
                foreach(var pth in paths)
                {
                    DirectoryInfo dir = new DirectoryInfo(pth);
                    dir = dir.Parent;
                    Maps.Add(dir);
                }
                return 0;
            }
            return 1;
        }

        public int ChooseMap(int index)
        {
            if(Maps.Count>index)
            {
                CurrentMapPath = Maps[index].ToString();
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