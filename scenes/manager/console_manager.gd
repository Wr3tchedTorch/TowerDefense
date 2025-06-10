extends Node

signal spawn_enemy_signal

func _ready():
	Console.add_command("hello", console_hello, ["person_name"], 1, "Says Hello to whoever typed this command.")
	Console.add_command("spawn_enemies", spawn_enemies, ["number_of_enemies"], 1, "Spawns the specified number of enemies in the beginning of the map.")	
	Console.add_command("moneeey", give_money, null, 0, "Spawns the specified number of enemies in the beginning of the map.")	

func console_hello(param: String):
	Console.print_line("Hello, " + param + ", how are you doing?")

func spawn_enemies(param: String):
	Console.print_line("Spawning " + param + " enemies! ")
	emit_signal("spawn_enemy_signal", int(param))

func give_money():
	Console.print_line("Giving you 999999 money!")
