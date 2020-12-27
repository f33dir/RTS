using Godot;
using System;

public class Interface : Control
{
    private Label _CurrentTowerLabel;
    private Label _SelectedTowerLabel;
    private Player.Player _Player;
    public override void _Ready()
    {
        _CurrentTowerLabel = GetNode<Label>("SelectedTower/CurrentTowerLabel");
        _SelectedTowerLabel = GetNode<Label>("SelectedTower/SelectedTowerLabel");
        _Player = GetParent().GetNode<Player.Player>("Player");
        _SelectedTowerLabel.Hide();
        _CurrentTowerLabel.Hide();
    }
    public void UpdateStats()
    {
        if(_Player.SelectedTower != null)
        {
            _SelectedTowerLabel.Text = _Player.SelectedTower.Name.ToString();
            string Stats = "Attack Power: " + _Player.SelectedTower.AttackPower.ToString() + '\n'
                           + "Attack Range: " + _Player.SelectedTower.AttackRange.ToString() + '\n'
                           + "Upgrade cost: " + (_Player.SelectedTower.Cost*_Player.SelectedTower.LvL).ToString();
            _CurrentTowerLabel.Text = Stats;
            _SelectedTowerLabel.Show();
            _CurrentTowerLabel.Show();
            // (_SelectedTowerLabel.GetParent() as NinePatchRect).Show();
        }
        else
        {
            _SelectedTowerLabel.Hide();
            _CurrentTowerLabel.Hide();
        }
    }
}
