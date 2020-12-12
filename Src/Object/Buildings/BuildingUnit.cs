using Godot;
using System;

namespace Unit
{
    public class BuildingUnit : Unit
    {
        protected bool _IsMovable = false; // для юнитов вроде Ancient'ов из Warcraft III (night elves)
        public uint _GridSizeX{get;set;} = 1;
        public uint _GridSizeY{get;set;} = 1;
        public void Build(){}
        virtual public void Upgrade(){}
    }
}