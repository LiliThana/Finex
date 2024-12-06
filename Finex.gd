@tool
extends EditorPlugin

func _enter_tree():
    add_custom_type("FSM", "Node", preload("finex/FSM.cs"), preload("icon.png"))
    add_custom_type("State", "Node", preload("finex/State.cs"), preload("icon.png"))

    add_script_templates()

func _exit_tree():
    remove_custom_type("FSM")
    remove_custom_type("State")

func add_script_templates():
    var project_name = ProjectSettings.get_setting("application/config/name")

    # Copy script templates folder from addon to res://script_templates if it hasn't been copied yet
    var script_templates_path = ProjectSettings.get_setting("editor/script/templates_search_path")+ "/State/"
    
    # Check script templates path to see if all files are appropriately written to the correct directoray
    var addon_template_path = "res://addons/Finex/script_templates/State/"
    var addon_template_dir = DirAccess.open(addon_template_path)

    if !DirAccess.dir_exists_absolute(script_templates_path):
        print("Finex: Script Template path doesn't exist, creating it now.")
        DirAccess.make_dir_recursive_absolute(script_templates_path)
    
    for file in addon_template_dir.get_files():
        if !FileAccess.file_exists(script_templates_path + file):
            print("Finex: Missing template " + file + " in script template path, copying it now")
            DirAccess.copy_absolute(addon_template_path + file, script_templates_path + file)
    EditorInterface.get_resource_filesystem().scan()


    # Write template exlcusion into .csproj file
