using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constant
{
    //info for fish
    public const string FISH_INFO_ROOT 			= "FishInfoRoot.dat";
    public const string FISH_INFO_PROGRESS 		= "FishInfoProgress.dat";

	// SCREEN size info
	public const float HALF_HEIGHT 				= 7;
	public static float HALF_WIDTH				= 0;

    public const float FISH_MOVEMENT_OFFSETY    = 1F;

    public const float FISH_UI_OFFSETY          = 0.5F;

    public static Vector2 m_TargetSceneSize = new Vector2(1080, 1920);

    public const string JS_LISTTANK             = "ListTank";
    public const string JS_TANK1                = "Tank1";
    public const string TANK                    = "Tank";
}

public static class AssetPathConstant
{
    // DataBase
    public const string FOLDER_DB_PATH = "Data";
    public const string FILE_RAW_DB_TOPIC = "Data/TopicDB";
    public const string FILE_RAW_DB_VOCA = "Data/VocasDB";
    public const string FILE_RAW_DB_MAP = "Data/MapDB";
    public const string FILE_RAW_DB_FISHES = "Data/FishesDB";
    public const string FILE_RAW_DB_SCENE = "Data/SceneDB";
    public const string FILE_RAW_DB_HUD = "Data/HUDDB";
    public const string FILE_RAW_DB_KEYBOARD_TEMPLATE = "Data/KbTemplateDB";
    public const string FILE_FISH_INFO_SAVE = "FishInfoRoot";
    // Story Game Play
    public const string FOLDER_ANSWER_LETTERS_PATH = "Textures/Letters";
    public const string FOLDER_KEYBOARD_LETTERS_PATH = "Textures/Letters";
    public const string FOLDER_KEYBOARD_PRESSED_LETTERS_PATH = "Textures/Letters";
    public const string FOLDER_PROGRESS_NO_PATH = "Textures/Number";
    public const string FOLDER_PROGRESS_TRASH_PATH = "Textures/Trash";
    // Story Map
    public const string FOLDER_HIGHLIGHT_PROGRESS_NO_PATH = "Textures/Number";
    public const string FOLDER_HIGHLIGHT_NODE_PATH = "Textures/Map/Obj/MapImage";
    public const string FOLDER_MAP_PATH = "Prefab/HUD/Element/HUD_StoryMap/Maps";
    // Fish
    public const string FOLDER_FISH_PATH = "Textures/Fishes";
    public const string FOLDER_FISH_CARD_PATH = "Textures/FishCard";
}
