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
    var script_templates_path = ProjectSettings.get_setting("editor/script/templates_search_path")
    
    # Check script templates path to see if all files are appropriately written to the correct directoray
    var addon_template_path = "res://addons/Finex/script_templates/State/"
    var addon_template_dir = DirAccess.open(addon_template_path)

    if !DirAccess.dir_exists_absolute(script_templates_path + "/State/"):
        print("Finex: Script Template path doesn't exist, creating it now.")
        DirAccess.make_dir_recursive_absolute(script_templates_path + "/State/")

    #To Do: Add error handling 
    for file in addon_template_dir.get_files():
        if !FileAccess.file_exists(script_templates_path + "/State/" + file):
            print("Finex: Missing template " + file + " in script template path, copying it now")
            DirAccess.copy_absolute(addon_template_path + file, script_templates_path + "/State/" + file)
    EditorInterface.get_resource_filesystem().scan()

    # Write template exlcusion into .csproj file if it isn't there
    var old_csproj = FileAccess.get_file_as_string("res://" + project_name + ".csproj")
    var entry = "\t<ItemGroup>\n\t\t<Compile Remove=\"addons/Finex/script_templates/**\" />\n\t\t<Compile Remove=\"" + script_templates_path.trim_prefix("res://") + "/**\" />\n\t</ItemGroup>\n"
    
    if !old_csproj.contains(entry):
        print("Finex: Templates not excluded from compilation, udpading .csproj")
        var csproj = FileAccess.open("res://" + project_name + ".csproj", FileAccess.WRITE)

        if csproj == null:
            print("Finex: Error locating " + project_name + ".csproj")
        else:
            var insertion_point = old_csproj.find("</Project>")
            var new_csproj = old_csproj.insert(insertion_point, entry)
            csproj.store_string(new_csproj)
        csproj.close()
