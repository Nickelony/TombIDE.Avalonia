namespace ScriptLib.ClassicScript.Data
{
	public static class Keywords
	{
		public static readonly string[] Directives = new string[]
		{
			"DEFINE",
			"FIRST_ID",
			"INCLUDE"
		};

		public static readonly string[] Sections = new string[]
		{
			"Language",
			"Level",
			"Options",
			"PCExtensions",
			"PSXExtensions",
			"Title",
			"Strings",
			"PSXStrings",
			"PCStrings",
			"ExtraNG"
		};

		public static readonly string[] NewCommands = new string[]
		{
			"AddEffect",
			"Animation",
			"AnimationSlot",
			"AssignSlot",
			"ColorRGB",
			"CombineItems",
			"CRS",
			"Customize",
			"CutScene",
			"Damage",
			"DefaultWindowsFont",
			"Demo",
			"Detector",
			"Diagnostic",
			"DiagnosticType",
			"Diary",
			"Elevator",
			"Enemy",
			"Equipment",
			"FogRange",
			"ForceBumpMapping",
			"ForceVolumetricFX",
			"GlobalTrigger",
			"Image",
			"ImportFile",
			"ItemGroup",
			"KeyPad",
			"LaraStartPos",
			"LevelFarView",
			"LogItem",
			"MirrorEffect",
			"MultEnvCondition",
			"NewSoundEngine",
			"Organizer",
			"Parameters",
			"Plugin",
			"PreserveInventory",
			"Rain",
			"SavegamePanel",
			"Settings",
			"ShowLaraInTitle",
			"Snow",
			"SoundSettings",
			"StandBy",
			"StaticMIP",
			"Switch",
			"TestPosition",
			"TextFormat",
			"TextureSequence",
			"TriggerGroup",
			"Turbo",
			"WindowsFont",
			"WindowTitle",
			"WorldFarView"
		};

		public static readonly string[] OldCommands = new string[]
		{
			"AnimatingMIP",
			"ColAddHorizon",
			"DemoDisc",
			"Examine",
			"FlyCheat",
			"Fog",
			"Horizon",
			"InputTimeout",
			"Key",
			"KeyCombo",
			"Layer1",
			"Legend",
			"LensFlare",
			"Level",
			"Lightning",
			"LoadCamera",
			"LoadSave",
			"Mirror",
			"Name",
			"Pickup",
			"PickupCombo",
			"PlayAnyLevel",
			"Puzzle",
			"PuzzleCombo",
			"RemoveAmulet",
			"ResetHUB",
			"ResidentCut",
			"Timer",
			"Title",
			"Train",
			"UVRotate",
			"YoungLara",

			// Dunno where these belong :P
			"Cut",
			"File",
			"FMV",

			// Unknown
			"Layer2",
			"NoLevel",
			"Pulse",
			"Security",
			"StarField"
		};

		public static readonly string[] Boolean = new string[]
		{
			"ENABLED",
			"DISABLED"
		};
	}
}
