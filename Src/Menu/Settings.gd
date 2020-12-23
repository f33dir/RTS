extends Control
onready var manager = $"/root/MapFileManager"
onready var button =$Button
onready var line = $LineEdit
func _ready():
	button.connect("button_down",self,"accept")
	pass
func accept():
	manager.MapPath = line.get_text();
	get_tree().change_scene("res://Scenes/MainMenu/MainMenu.tscn")
