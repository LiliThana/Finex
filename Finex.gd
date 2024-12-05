@tool
extends EditorPlugin

func _enter_tree():
    add_custom_type("FSM", "Node", preload("finex/FSM.cs"), preload("icon.png"))
    add_custom_type("State", "Node", preload("finex/State.cs"), preload("icon.png"))

func _exit_tree():
    remove_custom_type("FSM")
    remove_custom_type("State")
