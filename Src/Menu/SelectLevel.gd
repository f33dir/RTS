extends Control
onready var FileManager = $"/root/MapFileManager"
onready var play = $Play
onready var editor = $Editor
onready var List = $MapList
func _ready():
	play.connect("button_down",self,"_start")
	editor.connect("pressed",self,"_start_editor")
	FileManager.ParseMaps()
	for i in FileManager._maps:
		List.add_item(i,null,true)
	pass 
func _select_map(index):
	FileManager.ChooseMap();
	pass
func _start():
	get_tree().change_scene("res://Scenes/GridMapTestEnviroment.tscn");
	pass
func _start_editor():
	get_tree().change_scene("res://Scenes/Map/Editor.tscn")
	pass
