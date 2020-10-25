using Godot;
using System;
using CameraBase;

namespace CameraBase
{    
    public class SelectionBox : Control
    {
        public bool _isVisible = false;
        public Vector2 _mousePos;
        public Vector2 _startSelPos;
        
        private Godot.Color selBoxCol = Color.Color8(0,1,0);
        private float selBoxLineWidth = 2;

        public override void _Draw()
        {
            if(_isVisible && _startSelPos != _mousePos)
            {
                DrawLine(_startSelPos,new Vector2(_mousePos.x,_startSelPos.y),selBoxCol,selBoxLineWidth);
                DrawLine(_startSelPos,new Vector2(_startSelPos.x,_mousePos.y),selBoxCol,selBoxLineWidth);
                DrawLine(_mousePos,new Vector2(_mousePos.x,_startSelPos.y),selBoxCol,selBoxLineWidth);
                DrawLine(_mousePos,new Vector2(_startSelPos.x,_mousePos.y),selBoxCol,selBoxLineWidth);
            }
        }
        public override void _Process(float delta)
        {
            Update();
        }
        // public void SetStartSelPos(Vector2 SelPos)
        // {
        //     this._startSelPos = SelPos;
        // }
        // public void SetMousePos(Vector2 MousePos)
        // {
        //     this._mousePos = MousePos;
        // }
        // public void SetVisibility(bool isVisible)
        // {
        //     this._isVisible = isVisible;
        // }
    }
}