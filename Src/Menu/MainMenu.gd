extends Control

# Called when the node enters the scene tree for the first time.
func _ready():
	var play = $Button1
	var settings = $Button3
	var exit = $Button2
	play.connect("pressed",self,"_button_play_pressed")
	settings.connect("pressed",self,"_button_settings_pressed")
	exit.connect("pressed",self,"_button_exit_pressed")
	pass

func _button_exit_pressed():
	get_tree().quit(0)
	pass
func _button_settings_pressed():
	get_tree().change_scene("res://Scenes/MainMenu/Settings.tscn")
	pass
func _button_play_pressed():
	get_tree().change_scene("res://Scenes/MainMenu/SelectLevel.tscn")
	pass
func _process(delta):
	pass
