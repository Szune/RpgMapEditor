using System;
using System.Collections.Generic;
using System.IO;
using DynamicLua;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RpgMapEditor.Modules.Controls.HelperObjects;
using RpgMapEditor.Modules.Controls.NonStaticControls;
using RpgMapEditor.Modules.Controls.StaticControls;
using RpgMapEditor.Modules.Entities;
using RpgMapEditor.Modules.EventHandlers;
using RpgMapEditor.Modules.Objects;
using RpgMapEditor.Modules.Objects.ControlStyles;
using RpgMapEditor.Modules.Utilities;
using MessageBox = RpgMapEditor.Modules.Controls.StaticControls.MessageBox;
using Dialog =  RpgMapEditor.Modules.Controls.StaticControls.Dialog;

namespace RpgMapEditor
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 

    public enum MapAction
    {
        Place,
        Remove,
        Select
    }

    public class Editor : Game
    {
        public class ZoomScale
        {
            public float X;
            public float Y;

            public ZoomScale(float x, float y)
            {
                X = x;
                Y = y;
            }
        }

        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        private SpriteFont _font;
        private SpriteFont _bigFont;

        private int _gameWidth = 1280;
        private int _gameHeight = 928;

        private static int _screenGameWidth = 960;
        private static int _screenGameHeight = 704;

        private Vector3 _mouseScale = new Vector3(1, 1, 1);

        private readonly Storage _storage = Storage.Instance;

        private Screen _gameScreen;
        private Screen _menuScreen;
        private EViewport _screenGame;
        private EViewport _screenUi;
        private EViewport _screenFull;
        private readonly int _pixelGridSize;

        private string _luaCurrentScreen = "";

        private bool _inGame;
        private TextBox _focusedTextbox = new TextBox();
        private ClickEventObject _tmpClicked;

        private GameTime _currentTime;

        private readonly Map _map = Map.Instance;

        private bool _reloading = false;
        private Keys _mostRecentKey = Keys.None;
        private Direction _walkingDirection = Direction.None;
        private Keys _lastPressedKey = Keys.None;
        private bool _walking = false;
        private int _maxZ;
        private bool _inHouse;
        private bool _clickEventFired;
        private readonly Color _selectColor = Color.BlueViolet;

        private bool _showGrid = false;


        private readonly ZoomScale _zoom = new ZoomScale(1f, 1f);

        private Coordinates _currentLocation = new Coordinates(0,0,0);

        private ImageListItem _lastClickedItem = new ImageListItem();

        private Size _tileAmount = new Size();

        private Keys _shortcutPressed = Keys.None;

        private MapAction _currentAction = MapAction.Place;
        private Vector2 _oldCursorPos = Vector2.Zero;
        private bool _movingMap = false;
        private int _currentZ = 0;
        private MouseState _oldScroll = new MouseState();
        private ComboBox _zorder;
        private List<SelectedObjects> _currentlySelectedObjects = new List<SelectedObjects>();
        private List<SelectedObjects> _copyBuffer = new List<SelectedObjects>();

        private EQueue<ChangeAction> _undoActions = new EQueue<ChangeAction>();
        private EQueue<ChangeAction> _redoActions = new EQueue<ChangeAction>();
        private int _maxChangeActions;
        private bool _doClose;
        private Vector2 _selectStartPoint;
        private Vector2 _selectEndPoint;
        private Vector2 _drawStartPoint;
        private Vector2 _drawEndPoint;

        private Vector2 _getMouse = new Vector2();
        private bool _selecting = false;
        private SpriteObject _gridSprite;
        private Coordinates _mouseLocation = new Coordinates();

        public Editor()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = _gameWidth;
            _graphics.PreferredBackBufferHeight = _gameHeight;
            _graphics.SynchronizeWithVerticalRetrace = true;
            _graphics.ApplyChanges();


            _pixelGridSize = 8;
            Content.RootDirectory = "Content";

            _maxChangeActions = 50;
            IsFixedTimeStep = false; // This removes the 60 fps limit
            IsMouseVisible = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;

            EventInput.Initialize(Window);
            EventInput.CharEntered += new CharEnteredHandler(EventInput_CharEntered);
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            _mouseScale.X = (float)Window.ClientBounds.Width / _gameWidth;
            _mouseScale.Y = (float)Window.ClientBounds.Height / _gameHeight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        private void EventInput_CharEntered(object sender, CharacterEventArgs e)
        {
            if (!_focusedTextbox.ReadOnly)
            {
                if ((int)e.Character == 9)
                {
                    // Tab keypress
                }
                if ((int)e.Character == 13)
                {
                    // Enter keypress
                }
                if (e.Character == '\b' && _focusedTextbox.Text.Length > 0)
                {
                    _focusedTextbox.Text = _focusedTextbox.Text.Substring(0, _focusedTextbox.Text.Length - 1);
                }
                else
                {
                    if (e.Character != '\r' && e.Character != '\b' && e.Character != '\t' && (int)e.Character != 13)
                    {
                        _focusedTextbox.Text += e.Character;
                    }
                }
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            System.Windows.Forms.Form f = System.Windows.Forms.Control.FromHandle(Window.Handle) as System.Windows.Forms.Form;
            if(f != null)
            {
                f.FormClosing += F_FormClosing;
            }
            // Create a new SpriteBatch, which can be used to draw textures.
            Window.Title = "RPGay Map Editor";
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _font = Content.Load<SpriteFont>("EFont");
            _bigFont = Content.Load<SpriteFont>("BEFont");
            _storage.SmallFont = _font;
            _storage.Font = _font;
            _storage.BigFont = _bigFont;

            dynamic luaSprites = new DynamicLua.DynamicLua();
            string path = "Content\\scripts\\sprites.lua";
            luaSprites.DoFile(path);

           DynamicLuaTable spritesUi = luaSprites.Sprites_UI;
            LoadSprites(spritesUi, SpriteCategory.Ui);
            DynamicLuaTable spritesUiSpritesheets = luaSprites.Sprites_UI_Spritesheets;
            LoadSpritesheets(spritesUiSpritesheets, SpriteCategory.Ui);

            DynamicLuaTable spritesTilesSpriteCollections = luaSprites.Sprites_Tiles_SpriteCollections;
            LoadSpriteCollections(spritesTilesSpriteCollections, SpriteCategory.Tiles);
            DynamicLuaTable spritesTiles = luaSprites.Sprites_Tiles;
            LoadSprites(spritesTiles, SpriteCategory.Tiles);
            DynamicLuaTable spritesTilesSpritesheets = luaSprites.Sprites_Tiles_Spritesheets;
            LoadSpritesheets(spritesTilesSpritesheets, SpriteCategory.Tiles);

            DynamicLuaTable spritesCreatures = luaSprites.Sprites_Creatures;
            LoadSprites(spritesCreatures, SpriteCategory.Creatures);
            DynamicLuaTable spritesCreaturesSpritesheets = luaSprites.Sprites_Creatures_Spritesheets;
            LoadSpritesheets(spritesCreaturesSpritesheets, SpriteCategory.Creatures);

            DynamicLuaTable spritesItems = luaSprites.Sprites_Items;
            LoadSprites(spritesItems, SpriteCategory.Items);
            DynamicLuaTable spritesItemsSpritesheets = luaSprites.Sprites_Items_Spritesheets;
            LoadSpritesheets(spritesItemsSpritesheets, SpriteCategory.Items);


            SetupSpriteCollections();

            Styles.Init();

            LoadMapName();

            _gameScreen = CreateGameScreen();
            _inGame = true;
            _focusedTextbox = new TextBox();
            _gameScreen.Visible = true;
            //MenuScreen.Visible = true;

            _map.InitializeMap();

            _gridSprite = _storage.GetSpriteByName("UI_White_Pixel");
            //LoadMonsters();

            //map.CreateTileFromTileList(30000, new Coordinates(0, 0, 7));



            //map.CreateCreatureFromCreatureList(2, new Coordinates(0, 0, 7));
            //map.CreateCreatureFromCreatureList(3, new Coordinates(3, 2, 7));

            //LoadAttacks();

            //LoadScripts();

            //QuestDialog.Show(1);

            //lua_ui.ui_init();
        }

        private void F_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            e.Cancel = true;
            Close();
        }

        private void LoadSpriteCollections(DynamicLuaTable tab, SpriteCategory category)
        {
            dynamic dataTable;

            foreach (KeyValuePair<object, object> kvp in tab)
            {
                string tmpName = "";
                int tmpRows = -1;
                int tmpColumns = -1;
                int tmpId = -1;

                if (kvp.Value.ToString() == "table")
                {
                    dataTable = kvp.Value;
                    var enumer = dataTable.GetEnumerator();
                    while (enumer.MoveNext())
                    {
                        if (enumer.Current.Key.ToString() == "ID")
                        {
                            tmpId = int.Parse(enumer.Current.Value.ToString());
                        }
                        else if (enumer.Current.Key.ToString() == "Name")
                        {
                            tmpName = enumer.Current.Value.ToString();
                        }
                        else if (enumer.Current.Key.ToString() == "Columns")
                        {
                            tmpColumns = int.Parse(enumer.Current.Value.ToString());
                        }
                        else if (enumer.Current.Key.ToString() == "Rows")
                        {
                            tmpRows = int.Parse(enumer.Current.Value.ToString());
                        }
                    }
                    enumer.Dispose();
                    _storage.SpriteCollections.Add(new SpriteCollection(tmpId, tmpName, tmpColumns, tmpRows, category));
                    /*if (loadFromSpritesheet)
                    {
                        storage.AddSprite(tmpSpriteID, Category, tmpName, tmpSpritesheetPosition, tmpSpritesheetID, tmpRandomizeCategory, walkable);
                    }
                    else
                    {
                        storage.AddSprite(Content.Load<Texture2D>("Graphics\\" + tmpSpriteName), tmpSpriteID, Category, tmpName);
                        //map.TileList.Add(new Tile(tmpName, tmpSpriteID, null, 0, true, walkable));
                    }*/
                }
            }
        }

        private void SetupSpriteCollections()
        {
            foreach(SpriteCollection sc in _storage.SpriteCollections)
            {
                int x = 0;
                int y = 0;
                for(int i = 0; i < _storage.SpriteList.Count; i++)
                {
                    if(_storage.SpriteList[i].SpriteCollection == sc.Name)
                    {
                        sc.AddSprite(_storage.SpriteList[i], x, y);
                        if (x < sc.Columns - 1)
                        {
                            x++;
                        }
                        else
                        {
                            x = 0;
                            y++;
                        }
                    }
                }
            }
        }

        private Screen CreateGameScreen()
        {
            _gameScreen = _storage.AddScreen(2, "GameScreen", false);
            _screenGame = _gameScreen.AddViewport(1, "ScreenGame", 0, 0, 960, 704, true, 423);
            _screenGame.Click += GameScreen_Click;
            _screenGame.MouseOver += GameScreen_MouseOver;
            _screenGame.MouseDownMove += GameScreen_MouseDownMove;
            _screenGame.ParentScreen = _gameScreen;
            _screenUi = _gameScreen.AddViewport(2, "ScreenUI", 960, 0, 320, 928);
            _screenUi.Click += ScreenUI_Click;
            _screenUi.MouseOver += ScreenUI_MouseOver;
            _screenUi.ParentScreen = _gameScreen;

            EViewport screenChat = _gameScreen.AddViewport(3, "ScreenChat", 0, 704, 960, 224);
            _gameScreen.AddViewport(4, "ScreenFull", 0, 0, _gameWidth, _gameHeight);

            Panel uiPanel = _gameScreen.AddPanel(1, "UIPanel", _storage.GetSpriteByName("UI_MapEditorBackground").Sprite, 320, 928, new Vector2(0, 0), _screenUi, false, null, true);
            Button closeButton = uiPanel.AddButton(1, "Exit", ButtonStyles.LightSmooth, 128, 32, new Vector2(170, 20), "Close", true);
            closeButton.Click += CloseButton_Click;


            Panel toolPanel = _gameScreen.AddPanel(2, "ToolPanel", _storage.GetSpriteByName("UI_MapEditorBackground2").Sprite, 960, 224, new Vector2(0, 0), screenChat, false, null, true);
            Button hideGrid = toolPanel.AddButton(1, "HideGrid", ButtonStyles.LightSmooth, 128, 32, new Vector2(32, 32), "Show Grid", true);
            hideGrid.Click += HideGrid_Click;
            Label positionLabel = toolPanel.AddLabel(1, "Location", 200, 20, new Vector2(screenChat.MaxWidth - 240, 32), "World Location: ", true);
            Label mousePositionLabel = toolPanel.AddLabel(2, "MouseLocation", 200, 20, new Vector2(screenChat.MaxWidth - 240, 52), "Mouse Location: ", true);
            Label tileIdLabel = toolPanel.AddLabel(3, "TileID", 200, 20, new Vector2(screenChat.MaxWidth - 240, 72), "TileID: ", true);
            Label toolsLabel = toolPanel.AddLabel(4, "ToolsLabel", 200, 20, new Vector2(240, 32), "Tools\n-----------------------------\nPlace tiles:\nRemove tiles:\nSelect tiles:", true);
            Label actionLabel = toolPanel.AddLabel(5, "Action", 200, 20, new Vector2(screenChat.MaxWidth - 240, 92), "Action: ", true);
            Label zoomLabel = toolPanel.AddLabel(6, "Zoom", 200, 20, new Vector2(screenChat.MaxWidth - 240, 112), "Zoom: ", true);
            Label floorLabel = toolPanel.AddLabel(7, "Floor", 200, 20, new Vector2(440, 32), "Floor: ", true);
            Label zorderLabel = toolPanel.AddLabel(8, "Zorder", 200, 20, new Vector2(440, 62), "Z-order: ", true);
            Label filePathLabel = toolPanel.AddLabel(9, "Path", 200, 20, new Vector2(440, 82), "Map file name (*.map):", true);

            TextBox txtFilePath = toolPanel.AddTextbox(1, "txtFilePath", _storage.GetSpriteByName("UI_Textbox").Sprite, 160, 20, new Vector2(440, 102), _map.MapFileName, true);

            ComboBox zorderCombo = toolPanel.AddComboBox(1, "ZorderCombo", 40, 20, new Vector2(520, 62), _storage.GetSpriteByName("UI_MapEditorBackground2"), true);
            for(int i = 0; i < Utility.MaxZOrder + 1; i++)
            {
                zorderCombo.Items.Add(i.ToString());
            }
            zorderCombo.UpdateClicked(0);

            ComboBox floorCombo = toolPanel.AddComboBox(2, "FloorCombo", 40, 20, new Vector2(520, 32), _storage.GetSpriteByName("UI_MapEditorBackground2"), true);
            for(int i = 0; i < Utility.MaxZ + 1; i++)
            {
                floorCombo.Items.Add(i.ToString());
            }
            floorCombo.UpdateClicked(Utility.GroundZ);

            Button placeButton = toolPanel.AddButton(2, "Place", _storage.GetSpriteByName("UI_PlaceButton"), 16, 16, new Vector2(370, 66), null, true);
            placeButton.OriginalColor = Color.BlueViolet;
            placeButton.Click += PlaceButton_Click;
            Button removeButton = toolPanel.AddButton(3, "Remove", _storage.GetSpriteByName("UI_RemoveButton"), 16, 16, new Vector2(370, 83), null, true);
            removeButton.Click += RemoveButton_Click;
            Button showTips = toolPanel.AddButton(4, "ShowTips", ButtonStyles.LightSmooth, 128, 32, new Vector2(32, 70), "Show Tips", true);
            showTips.Click += ShowTips_Click;
            Button loadButton = toolPanel.AddButton(5, "Load", ButtonStyles.LightSmooth, 128, 32, new Vector2(32, 108), "Load Map", true);
            loadButton.Click += LoadButton_Click;
            Button saveButton = toolPanel.AddButton(6, "Save", ButtonStyles.LightSmooth, 128, 32, new Vector2(32, 146), "Save Map", true);
            saveButton.Click += SaveButton_Click;
            Button floorMinusButton = toolPanel.AddButton(7, "FloorMinus", _storage.GetSpriteByName("UI_MinusButton"), 16, 16, new Vector2(502, 34), null, true);
            floorMinusButton.Click += FloorMinusButton_Click;
            Button floorPlusButton = toolPanel.AddButton(7, "FloorPlus", _storage.GetSpriteByName("UI_PlusButton"), 16, 16, new Vector2(582, 34), null, true);
            floorPlusButton.Click += FloorPlusButton_Click;
            Button zorderMinusButton = toolPanel.AddButton(7, "ZorderMinus", _storage.GetSpriteByName("UI_MinusButton"), 16, 16, new Vector2(502, 64), null, true);
            zorderMinusButton.Click += ZorderMinusButton_Click;
            Button zorderPlusButton = toolPanel.AddButton(7, "ZorderPlus", _storage.GetSpriteByName("UI_PlusButton"), 16, 16, new Vector2(582, 64), null, true);
            zorderPlusButton.Click += ZorderPlusButton_Click;
            Button saveMapPathButton = toolPanel.AddButton(8, "SaveMapPath", ButtonStyles.LightSmooth, 128, 32, new Vector2(440, 126), "Set map name", true);
            saveMapPathButton.Click += SaveMapPathButton_Click;
            Button toggleFullScreenButton = toolPanel.AddButton(9, "ToggleFullScreen", ButtonStyles.LightSmooth, 128, 32, new Vector2(440, 164), "Toggle fullscreen", true);
            toggleFullScreenButton.Click += ToggleFullScreenButton_Click;
            Button selectButton = toolPanel.AddButton(10, "Select", _storage.GetSpriteByName("UI_SelectButton"), 16, 16, new Vector2(370, 100), null, true);
            selectButton.Click += SelectButton_Click;

            uiPanel.AddLabel(1, "TileTypeLabel", 100, 20, new Vector2(26, 26), "Type of tile\n----------------", true);
            uiPanel.AddLabel(2, "TileLabel", 100, 20, new Vector2(26, 166), "", true);
            uiPanel.AddLabel(3, "Separator", 100, 20, new Vector2(26, 182), "-------------------------", true);


            ComboBox tileCategoryList = uiPanel.AddComboBox(1, "TileCategoryList", 140, 32, new Vector2(26, 96), _storage.GetSpriteByName("UI_MapEditorBackground2"), false);
            PopulateTileCategoryList();
            tileCategoryList.ClickedId = 0;
            tileCategoryList.SelectedItemText = tileCategoryList.Items[tileCategoryList.ClickedId];
            tileCategoryList.Click += TileCategoryList_Click;

            ComboBox creatureCategoryList = uiPanel.AddComboBox(2, "CreatureCategoryList", 140, 32, new Vector2(26, 96), _storage.GetSpriteByName("UI_MapEditorBackground2"), false);
            PopulateCreatureCategoryList();
            creatureCategoryList.ClickedId = 0;
            creatureCategoryList.SelectedItemText = creatureCategoryList.Items[creatureCategoryList.ClickedId];
            creatureCategoryList.Click += CreatureCategoryList_Click;

            ComboBox itemCategoryList = uiPanel.AddComboBox(3, "ItemCategoryList", 140, 32, new Vector2(26, 96), _storage.GetSpriteByName("UI_MapEditorBackground2"), false);
            PopulateItemCategoryList();
            itemCategoryList.ClickedId = 0;
            itemCategoryList.SelectedItemText = itemCategoryList.Items[itemCategoryList.ClickedId];
            itemCategoryList.Click += ItemCategoryList_Click;

            ComboBox tileTypeList = uiPanel.AddComboBox(4, "TileTypeList", 140, 32, new Vector2(26, 60), _storage.GetSpriteByName("UI_MapEditorBackground2"), true);
            tileTypeList.Click += TileTypeList_Click;
            tileTypeList.Items.Add("Sprite Collections");
            tileTypeList.Items.Add("Tiles");
            tileTypeList.Items.Add("Creatures");
            tileTypeList.Items.Add("Items");
            tileTypeList.ClickedId = 0;
            tileTypeList.SelectedItemText = tileTypeList.Items[tileTypeList.ClickedId];
            uiPanel.GetLabelByName("TileLabel").Text = tileTypeList.SelectedItemText;

            ListBox tileSizeList = uiPanel.AddListBox(1, "TileSizeList", 80, 80, new Vector2(240, 60), true);
            tileSizeList.Click += TileSizeList_Click;
            tileSizeList.Items.Add("1x1");
            tileSizeList.Items.Add("2x2");
            tileSizeList.Items.Add("4x4");
            tileSizeList.Items.Add("8x8");
            tileSizeList.Items.Add("16x16");
            tileSizeList.ClickedId = 0;
            tileSizeList.SelectedItemText = tileSizeList.Items[tileSizeList.ClickedId];

            ImageListBox spriteCollectionList = uiPanel.AddImageListBox(1, "SpriteCollectionsList", 300, 700, new Vector2(26, 210), true);
            spriteCollectionList.Click += SpriteCollectionList_Click;

            int c = 1;
            for(int i = 0; i < _storage.SpriteCollections.Count; i++)
            {
                if (_storage.SpriteCollections[i].Category == SpriteCategory.Tiles)
                {
                    if(_storage.SpriteCollections[i].Sprite[0, 0] == null)
                    {
                        throw new Exception("Couldn't load SpriteCollection, make sure the spritecollection attribute name in the sprites is the same as in the spritecollection part of the lua");
                    }
                    spriteCollectionList.Items.Add(new ImageListItem(c, _storage.SpriteCollections[i].Name, _storage.SpriteCollections[i].Sprite[0, 0], _storage.SpriteCollections[i]));
                }
            }

            ImageListBox tileList = uiPanel.AddImageListBox(2, "TilesList", 300, 700, new Vector2(26, 210), false);
            tileList.Click += TileList_Click;

            PopulateTileList(tileCategoryList.Items[0]);

            ImageListBox creatureList = uiPanel.AddImageListBox(3, "CreaturesList", 300, 700, new Vector2(26, 210), false);
            creatureList.Click += CreatureList_Click;

            PopulateCreatureList(creatureCategoryList.Items[0]);

            ImageListBox itemList = uiPanel.AddImageListBox(4, "ItemsList", 300, 700, new Vector2(26, 210), false);
            itemList.Click += ItemList_Click;

            PopulateItemList(itemCategoryList.Items[0]);

            /*
             * GameScreen.AddLabel(1, "TileTypeLabel", 100, 20, new Vector2(10, 10), ScreenUI, "Type of tile", true);
            GameScreen.AddLabel(2, "TileLabel", 100, 20, new Vector2(10, 150), ScreenUI, "", true);

            ListBox TileTypeList = GameScreen.AddListBox(1, "TileTypeList", 150, 100, new Vector2(10, 40), ScreenUI, true);
            TileTypeList.Click += TileTypeList_Click;
            TileTypeList.Items.Add("Sprite Collections");
            TileTypeList.Items.Add("Tiles");
            TileTypeList.Items.Add("Creatures");
            TileTypeList.Items.Add("Items");
            TileTypeList.ClickedID = 0;
            TileTypeList.SelectedItemText = TileTypeList.Items[TileTypeList.ClickedID];
            GameScreen.GetLabelByName("TileLabel").Text = TileTypeList.SelectedItemText;

            ListBox TileSizeList = GameScreen.AddListBox(1, "TileSizeList", 150, 100, new Vector2(160, 40), ScreenUI, true);
            TileSizeList.Click += TileSizeList_Click;
            TileSizeList.Items.Add("1x1");
            TileSizeList.Items.Add("2x2");
            TileSizeList.Items.Add("4x4");
            TileSizeList.ClickedID = 0;
            TileSizeList.SelectedItemText = TileSizeList.Items[TileSizeList.ClickedID];


            ImageListBox SpriteCollectionList = GameScreen.AddImageListBox(1, "SpriteCollectionsList", 300, 700, new Vector2(10, 190), ScreenUI, true);
            SpriteCollectionList.Click += SpriteCollectionList_Click;

            int c = 1;
            for(int i = 0; i < storage.spriteCollections.Count; i++)
            {
                if (storage.spriteCollections[i].Category == SpriteCategory.Tiles)
                {
                    SpriteCollectionList.Items.Add(new ImageListItem(c, storage.spriteCollections[i].Name, storage.spriteCollections[i].Sprite[0, 0], storage.spriteCollections[i]));
                }
            }

            ImageListBox TileList = GameScreen.AddImageListBox(2, "TilesList", 300, 700, new Vector2(10, 190), ScreenUI, false);
            TileList.Click += TileList_Click;

            for(int i = 0; i < storage.spriteList.Count; i++)
            {
                if(storage.spriteList[i].Category == SpriteCategory.Tiles && storage.spriteList[i].Show)
                {
                    TileList.Items.Add(new ImageListItem(c, storage.spriteList[i].SpriteName, storage.spriteList[i]));
                    c++;
                }
            }*/

            return _gameScreen;
        }

        private void CloseButton_Click(object sender, ButtonEventArgs e)
        {
            Close();
        }

        private void ToggleFullScreenButton_Click(object sender, ButtonEventArgs e)
        {
            _graphics.ToggleFullScreen();
        }

        private void SaveMapPathButton_Click(object sender, ButtonEventArgs e)
        {
            string tmpName = _gameScreen.GetPanelByName("ToolPanel").GetTextboxByName("txtFilePath").Text;
            if (tmpName.EndsWith(".map"))
            {
                _map.MapFileName = tmpName;
                _map.MapFilePath = _map.MapFileDir + _map.MapFileName;
                MessageBox.Show("Now editing map:\n" + _map.MapFileName);
            }
            else
            {
                MessageBox.Show("Map names have to end with '.map'");
            }
        }

        private void ZorderPlusButton_Click(object sender, ButtonEventArgs e)
        {
            NextZorder();
        }

        private void ZorderMinusButton_Click(object sender, ButtonEventArgs e)
        {
            PreviousZorder();
        }

        private void FloorPlusButton_Click(object sender, ButtonEventArgs e)
        {
            NextFloor();
        }

        private void FloorMinusButton_Click(object sender, ButtonEventArgs e)
        {
            PreviousFloor();
        }

        private void ItemCategoryList_Click(object sender, ComboBox.ComboBoxEventArgs e)
        {
            PopulateItemList(e.ComboBoxSelectedItemText);
        }

        private void ItemList_Click(object sender, ImageListBox.ImageListBoxEventArgs e)
        {
            _lastClickedItem = e.ImageListBoxSelectedItem;
        }

        private void PopulateItemList(string category)
        {
            ImageListBox items = _gameScreen.GetPanelByName("UIPanel").GetImageListBoxByName("ItemsList");
            items.Items.Clear();
            items.ResetScrollPosition();
            for (int i = 0; i < _storage.SpriteList.Count; i++)
            {
                SpriteObject tmpSpr = _storage.SpriteList[i];
                if (tmpSpr.ListCategory == category && tmpSpr.Show)
                {
                    items.Items.Add(new ImageListItem(items.Items.Count + 1, tmpSpr.SpriteName, tmpSpr));
                }
            }
        }

        private void PopulateItemCategoryList()
        {
            List<string> categories = new List<string>();
            for (int i = 0; i < _storage.SpriteList.Count; i++)
            {
                if (_storage.SpriteList[i].Category == SpriteCategory.Items)
                {
                    if (!categories.Contains(_storage.SpriteList[i].ListCategory))
                    {
                        categories.Add(_storage.SpriteList[i].ListCategory);
                    }
                }
            }
            _gameScreen.GetPanelByName("UIPanel").GetComboBoxByName("ItemCategoryList").Items.AddRange(categories);
        }

        private void PopulateCreatureList(string category)
        {
            ImageListBox creatures = _gameScreen.GetPanelByName("UIPanel").GetImageListBoxByName("CreaturesList");
            creatures.Items.Clear();
            creatures.ResetScrollPosition();
            for (int i = 0; i < _storage.SpriteList.Count; i++)
            {
                SpriteObject tmpSpr = _storage.SpriteList[i];
                if (tmpSpr.ListCategory == category && tmpSpr.Show)
                {
                    creatures.Items.Add(new ImageListItem(creatures.Items.Count + 1, tmpSpr.SpriteName, tmpSpr));
                }
            }
        }

        private void CreatureCategoryList_Click(object sender, ComboBox.ComboBoxEventArgs e)
        {
            PopulateCreatureList(e.ComboBoxSelectedItemText);
        }

        private void PopulateCreatureCategoryList()
        {
            List<string> categories = new List<string>();
            for (int i = 0; i < _storage.SpriteList.Count; i++)
            {
                if (_storage.SpriteList[i].Category == SpriteCategory.Creatures)
                {
                    if (!categories.Contains(_storage.SpriteList[i].ListCategory))
                    {
                        categories.Add(_storage.SpriteList[i].ListCategory);
                    }
                }
            }
            _gameScreen.GetPanelByName("UIPanel").GetComboBoxByName("CreatureCategoryList").Items.AddRange(categories);
        }

        private void CreatureList_Click(object sender, ImageListBox.ImageListBoxEventArgs e)
        {
            _lastClickedItem = e.ImageListBoxSelectedItem;
        }

        private void TileCategoryList_Click(object sender, ComboBox.ComboBoxEventArgs e)
        {
            PopulateTileList(e.ComboBoxSelectedItemText);
        }

        private void PopulateTileList(string category)
        {
            ImageListBox tiles = _gameScreen.GetPanelByName("UIPanel").GetImageListBoxByName("TilesList");
            tiles.Items.Clear();
            tiles.ResetScrollPosition();
            for (int i = 0; i < _storage.SpriteList.Count; i++)
            {
                SpriteObject tmpSpr = _storage.SpriteList[i];
                if (tmpSpr.ListCategory == category && tmpSpr.Show)
                {
                    tiles.Items.Add(new ImageListItem(tiles.Items.Count + 1, tmpSpr.SpriteName, tmpSpr));
                }
            }
        }

        private void PopulateTileCategoryList()
        {
            List<string> categories = new List<string>();
            for(int i = 0; i < _storage.SpriteList.Count; i++)
            {
                if (_storage.SpriteList[i].Category == SpriteCategory.Tiles)
                {
                    if (!categories.Contains(_storage.SpriteList[i].ListCategory))
                    {
                        categories.Add(_storage.SpriteList[i].ListCategory);
                    }
                }
            }
            _gameScreen.GetPanelByName("UIPanel").GetComboBoxByName("TileCategoryList").Items.AddRange(categories);
        }

        private void LoadButton_Click(object sender, ButtonEventArgs e)
        {
            LoadMap();
        }

        private void LoadMap()
        {
            Dialog.Show("Are you sure you want to load\n" + _map.MapFileName + "?", "CTRL + D to load map");
            Dialog.Click += LoadMapDialog;
        }

        private void LoadMapDialog(object sender, EventArgs args)
        {
            Dialog.DialogButton b = (Dialog.DialogButton)sender;
            if (b == Dialog.DialogButton.Yes)
            {
                if (_map.LoadMap())
                {
                    MessageBox.Show("Map loaded.");
                }
                else
                {
                    MessageBox.Show("Couldn't load map.");
                }
            }
            else if (b == Dialog.DialogButton.No)
            {
                MessageBox.Show("Didn't load map.");
            }
            Dialog.Click -= LoadMapDialog;
        }

        private void SaveButton_Click(object sender, ButtonEventArgs e)
        {
            SaveMap();
        }

        private void SaveMapDialog(object sender, EventArgs args)
        {
            Dialog.DialogButton b = (Dialog.DialogButton)sender;

            if (b == Dialog.DialogButton.Yes)
            {
                if (_map.SaveMap(true))
                {
                    MessageBox.Show("Map saved.");
                }
                else
                {
                    MessageBox.Show("Couldn't save map.");
                }
            }
            else if (b == Dialog.DialogButton.No)
            {
                MessageBox.Show("Didn't save map.");
            }
            Dialog.Click -= SaveMapDialog;
        }

        private void SaveMap()
        {
            FileInfo f = new FileInfo(_map.MapFilePath);
            if (f.Exists)
            {
                Dialog.Show("Do you want to overwrite\n" + f.Name + "?", "CTRL + S to save map");
                Dialog.Click += SaveMapDialog;
            }
            else
            {
                if (_map.SaveMap(false))
                {
                    MessageBox.Show("Map saved.");
                }
                else
                {
                    MessageBox.Show("Couldn't save.");
                }
            }
        }

        private void ShowTips_Click(object sender, ButtonEventArgs e)
        {
            MessageBox.Show("Shortcuts:\nPress A to start pl(a)cing tiles\nPress R to start (r)emoving tiles\nPress E to go to n(e)xt tile size\nPress space to show/hide grid\nScroll to zoom in/out on the map\nQ & W changes floors, + CTRL: z-order");
        }

        private void RemoveButton_Click(object sender, ButtonEventArgs e)
        {
            ChangeActionToRemove();
        }

        private void ChangeActionToRemove()
        {
            _currentAction = MapAction.Remove;
            ClearSelectedObjects();
            ClearSelectedTool();
            _gameScreen.GetPanelByName("ToolPanel").GetButtonByName("Remove").OriginalColor = Color.BlueViolet;
        }

        private void PlaceButton_Click(object sender, ButtonEventArgs e)
        {
            ChangeActionToPlace();
        }

        private void ChangeActionToPlace()
        {
            _currentAction = MapAction.Place;
            ClearSelectedObjects();
            ClearSelectedTool();
            _gameScreen.GetPanelByName("ToolPanel").GetButtonByName("Place").OriginalColor = Color.BlueViolet;
        }

        private void SelectButton_Click(object sender, ButtonEventArgs e)
        {
            ChangeActionToSelect();
        }

        private void ChangeActionToSelect()
        {
            _currentAction = MapAction.Select;
            ClearSelectedTool();
            _gameScreen.GetPanelByName("ToolPanel").GetButtonByName("Select").OriginalColor = Color.BlueViolet;
        }

        private void ClearSelectedTool()
        {
            _gameScreen.GetPanelByName("ToolPanel").GetButtonByName("Place").OriginalColor = Color.White;
            _gameScreen.GetPanelByName("ToolPanel").GetButtonByName("Select").OriginalColor = Color.White;
            _gameScreen.GetPanelByName("ToolPanel").GetButtonByName("Remove").OriginalColor = Color.White;
        }

        private void HideGrid_Click(object sender, ButtonEventArgs e)
        {
            ToggleGrid();
        }

        private void ToggleGrid()
        {
            Button b = _gameScreen.GetPanelByName("ToolPanel").GetButtonByName("HideGrid");
            if (_showGrid)
            {
                b.Text = "Show Grid";
                _showGrid = false;
            }
            else
            {
                b.Text = "Hide Grid";
                _showGrid = true;
            }
        }

        private void TileList_Click(object sender, ImageListBox.ImageListBoxEventArgs e)
        {
            _lastClickedItem = e.ImageListBoxSelectedItem;
        }

        private void TileSizeList_Click(object sender, ListBox.ListBoxEventArgs e)
        {
            
        }

        private void SpriteCollectionList_Click(object sender, ImageListBox.ImageListBoxEventArgs e)
        {
            _lastClickedItem = e.ImageListBoxSelectedItem;
        }

        private void TileTypeList_Click(object sender, ComboBox.ComboBoxEventArgs e)
        {
            string oldName = _gameScreen.GetPanelByName("UIPanel").GetLabelByName("TileLabel").Text.Replace(" ", null) + "List";
            _gameScreen.GetPanelByName("UIPanel").GetImageListBoxByName(oldName).Hide();
            _gameScreen.GetPanelByName("UIPanel").GetLabelByName("TileLabel").Text = e.ComboBoxSelectedItemText;
            string newName = _gameScreen.GetPanelByName("UIPanel").GetLabelByName("TileLabel").Text.Replace(" ", null) + "List";
            _gameScreen.GetPanelByName("UIPanel").GetImageListBoxByName(newName).Show();
            if(_gameScreen.GetPanelByName("UIPanel").GetLabelByName("TileLabel").Text == "Tiles")
            {
                _gameScreen.GetPanelByName("UIPanel").GetComboBoxByName("CreatureCategoryList").Hide();
                _gameScreen.GetPanelByName("UIPanel").GetComboBoxByName("ItemCategoryList").Hide();
                _gameScreen.GetPanelByName("UIPanel").GetComboBoxByName("TileCategoryList").Show();
            }
            else if(_gameScreen.GetPanelByName("UIPanel").GetLabelByName("TileLabel").Text == "Creatures")
            {
                _gameScreen.GetPanelByName("UIPanel").GetComboBoxByName("ItemCategoryList").Hide();
                _gameScreen.GetPanelByName("UIPanel").GetComboBoxByName("TileCategoryList").Hide();
                _gameScreen.GetPanelByName("UIPanel").GetComboBoxByName("CreatureCategoryList").Show();
            }
            else if(_gameScreen.GetPanelByName("UIPanel").GetLabelByName("TileLabel").Text == "Items")
            {
                _gameScreen.GetPanelByName("UIPanel").GetComboBoxByName("TileCategoryList").Hide();
                _gameScreen.GetPanelByName("UIPanel").GetComboBoxByName("CreatureCategoryList").Hide();
                _gameScreen.GetPanelByName("UIPanel").GetComboBoxByName("ItemCategoryList").Show();
            }
            else
            {
                _gameScreen.GetPanelByName("UIPanel").GetComboBoxByName("TileCategoryList").Hide();
                _gameScreen.GetPanelByName("UIPanel").GetComboBoxByName("CreatureCategoryList").Hide();
                _gameScreen.GetPanelByName("UIPanel").GetComboBoxByName("ItemCategoryList").Hide();
            }
        }

        private void ScreenUI_MouseOver(object sender, ViewportEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void ScreenUI_Click(object sender, ViewportEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void GameScreen_MouseOver(object sender, ViewportEventArgs e)
        {
            if (_inGame)
            {
                /*int X = e.ClickedX / (int)(16 * Zoom.X);
                int Y = e.ClickedY / (int)(16 * Zoom.X);
                int nx = X + currentLocation.X; // - currentLocation.X;
                int ny = Y + currentLocation.Y; // - currentLocation.Y;*/

                /*int X = e.ClickedX / 64;
                int Y = e.ClickedY / 64;
                X = (int)(X + map.player.Position.X - Utility.ScreenX);
                Y = (int)(Y + map.player.Position.Y - Utility.ScreenY);

                if (e.ClickedInScreen)
                {
                    Creature c = lua.GetCreatureFromTile(X, Y, map.player.Position.Z);
                    if (c.ID != -1)
                    {
                        //MessageBox.Show(c.Name);
                        InfoPanel.Show(c);
                    }
                    else
                    {
                        InfoPanel.Visible = false;
                    }
                }*/
        }
    }

        private SpriteObject GetRandomizedSprite(SpriteObject sprite, int seed = 0)
        {
            List<SpriteObject> randomSprites = new List<SpriteObject>();

            for(int i = 0; i < _storage.SpriteList.Count; i++)
            {
                if(_storage.SpriteList[i].RandomizeCategory == sprite.RandomizeCategory)
                {
                    randomSprites.Add(_storage.SpriteList[i]);
                }
            }

            if (randomSprites.Count > 0)
            {
                Random rand;
                if (seed == 0)
                    rand = new Random();
                else
                    rand = new Random(seed);

                int spriteN = rand.Next(0, randomSprites.Count - 1);
                return randomSprites[spriteN];
            }
            else
            {
                return new SpriteObject();
            }
        }

        private void GameScreen_Click(object sender, ViewportEventArgs e)
        {
            int x = e.ClickedX / (int)(16 * _zoom.X);
            int y = e.ClickedY / (int)(16 * _zoom.X);
            int nx = x + _currentLocation.X; // - currentLocation.X;
            int ny = y + _currentLocation.Y; // - currentLocation.Y;

            int nz = int.Parse(_gameScreen.GetPanelByName("ToolPanel").GetComboBoxByName("FloorCombo").SelectedItemText);
            int zOrder = int.Parse(_gameScreen.GetPanelByName("ToolPanel").GetComboBoxByName("ZorderCombo").SelectedItemText);

            if (e.ClickedButton == ClickedMouseButton.Left && e.ClickedInScreen && !StaticControlsManager.IsAnyVisible())
            {
                if (_currentAction != MapAction.Select)
                {
                    if (_lastClickedItem.Id != -1)
                    {
                        if (_lastClickedItem.Sprite.Category == SpriteCategory.Tiles)
                        {
                            PlaceOrRemoveTile(nx, ny, nz, zOrder);
                        }
                        else if (_lastClickedItem.Sprite.Category == SpriteCategory.Creatures)
                        {
                            PlaceOrRemoveCreature(nx, ny, nz);
                        }
                        else if (_lastClickedItem.Sprite.Category == SpriteCategory.Items)
                        {
                            PlaceOrRemoveItem(nx, ny, nz, zOrder);
                        }
                    }
                }
            }

            //X = (int)(X + map.player.Position.X - Utility.ScreenX);
            //Y = (int)(Y + map.player.Position.Y - Utility.ScreenY);
        }

        private void GameScreen_MouseDownMove(object sender, ViewportEventArgs e)
        {
            int x = e.ClickedX / (int)(16 * _zoom.X);
            int y = e.ClickedY / (int)(16 * _zoom.X);
            int nx = x + _currentLocation.X; // - currentLocation.X;
            int ny = y + _currentLocation.Y; // - currentLocation.Y;

            int nz = int.Parse(_gameScreen.GetPanelByName("ToolPanel").GetComboBoxByName("FloorCombo").SelectedItemText);
            int zOrder = int.Parse(_gameScreen.GetPanelByName("ToolPanel").GetComboBoxByName("ZorderCombo").SelectedItemText);

            if (e.ClickedButton == ClickedMouseButton.Left && e.ClickedInScreen && !StaticControlsManager.IsAnyVisible())
            {
                if (_currentAction != MapAction.Select)
                {
                    if (_lastClickedItem.Id != -1)
                    {
                        if (_lastClickedItem.Sprite.Category == SpriteCategory.Tiles)
                        {
                            PlaceOrRemoveTile(nx, ny, nz, zOrder);
                        }
                        else if (_lastClickedItem.Sprite.Category == SpriteCategory.Creatures)
                        {
                            PlaceOrRemoveCreature(nx, ny, nz);
                        }
                        else if (_lastClickedItem.Sprite.Category == SpriteCategory.Items)
                        {
                            PlaceOrRemoveItem(nx, ny, nz, zOrder);
                        }
                    }
                }
            }
        }

        private void SelectObjects(Coordinates startPos, Coordinates endPos)
        {
            ClearSelectedObjects();
            SelectedObjects sos = new SelectedObjects();
            for (int y = startPos.Y; y < endPos.Y; y++)
            {
                for(int x = startPos.X; x < endPos.X; x++)
                {
                    sos = GetSelectableObjects(x, y, _currentZ);
                    sos.StartPos = startPos;
                    _currentlySelectedObjects.Add(sos);
                    if (sos.SelectedCreature != null)
                    { 
                        sos.SelectedCreature.Selected = true;
                    }
                    if (sos.SelectedItems != null)
                    {
                        foreach(Item i in sos.SelectedItems)
                        {
                            i.Selected = true;
                        }
                    }
                    if(sos.SelectedTiles != null)
                    {
                        foreach (Tile t in sos.SelectedTiles)
                        {
                            t.Selected = true;
                        }
                    }
                }
            }

            if(_currentlySelectedObjects.Count == 1)
            {
                SelectedObject so = GetSelectableObject(startPos.X, startPos.Y, _currentZ);
                if (so.SelectedType == SpriteCategory.Creatures)
                {
                    if (so.SelectedCreature != null)
                    {
                        _lastClickedItem = new ImageListItem(1, so.SelectedCreature.Sprite.SpriteName, so.SelectedCreature.Sprite);
                    }
                }
                else if (so.SelectedType == SpriteCategory.Items)
                {
                    if (so.SelectedItem != null)
                    {
                        _lastClickedItem = new ImageListItem(1, so.SelectedItem.Sprite.SpriteName, so.SelectedItem.Sprite);
                    }
                }
                else if (so.SelectedType == SpriteCategory.Tiles)
                {
                    if (so.SelectedTile != null)
                    {
                        _lastClickedItem = new ImageListItem(1, so.SelectedTile.Sprite.SpriteName, so.SelectedTile.Sprite);
                    }
                }
            }
            else
            {
                _lastClickedItem = new ImageListItem();
            }
        }

        private SelectedObjects GetSelectableObjects(int nx, int ny, int nz)
        {
            /* Make it possible to select multiple objects */
            SelectedObjects selectableObjects = new SelectedObjects();
            Coordinates selectPos = new Coordinates(nx, ny, nz);
            List<Tile> tiles = _map.GetTilesFromPos(selectPos);
            List<Item> items = _map.GetItemsFromPos(selectPos);
            Creature topCreature = _map.GetCreatureFromTile(selectPos);
            if (topCreature.Id != -1)
            {
                selectableObjects.IsSet = true;
                selectableObjects.SelectedCreature = topCreature;
            }
            if (items.Count > 0)
            {
                selectableObjects.IsSet = true;
                selectableObjects.SelectedItems = items;
            }
            if (tiles.Count > 0)
            {
                selectableObjects.IsSet = true;
                selectableObjects.SelectedTiles = tiles;
            }
            return selectableObjects;
        }

        private SelectedObject GetSelectableObject (int nx, int ny, int nz)
        {
            SelectedObject selectableObject = new SelectedObject();
            Coordinates selectPos = new Coordinates(nx, ny, nz);
            Tile topTile = _map.GetTopTile(selectPos);
            Item topItem = _map.GetTopItem(selectPos);
            Creature topCreature = _map.GetCreatureFromTile(selectPos);
            if (topCreature.Id != -1)
            {
                selectableObject.IsSet = true;
                selectableObject.SelectedType = SpriteCategory.Creatures;
                selectableObject.SelectedCreature = topCreature;
            }
            else if (topItem.Id != -1)
            {
                selectableObject.IsSet = true;
                selectableObject.SelectedType = SpriteCategory.Items;
                selectableObject.SelectedItem = topItem;
            }
            else if (topTile.Id != -1)
            {
                selectableObject.IsSet = true;
                selectableObject.SelectedType = SpriteCategory.Tiles;
                selectableObject.SelectedTile = topTile;
            }
            return selectableObject;
        }

        //private void SelectObject(int nx, int ny, int nz)
        //{
        //    ClearSelectedObjects();
        //    SelectedObjects so = GetSelectableObject(nx, ny, nz);
        //    if (so.IsSet)
        //    {
        //        if (so.SelectedType == SpriteCategory.Creatures)
        //        {
        //            lastClickedItem = new ImageListItem(1, so.SelectedCreature.Sprite.SpriteName, so.SelectedCreature.Sprite);
        //        }
        //        else if (so.SelectedType == SpriteCategory.Items)
        //        {
        //            lastClickedItem = new ImageListItem(1, so.SelectedItems[0].Sprite.SpriteName, so.SelectedItems[0].Sprite);
        //        }
        //        else if (so.SelectedType == SpriteCategory.Tiles)
        //        {
        //            lastClickedItem = new ImageListItem(1, so.SelectedTiles[0].Sprite.SpriteName, so.SelectedTiles[0].Sprite);
        //        }
        //    }
        //    currentlySelectedObjects.Add(so);
        //}

        private void DeleteSelectedObjects()
        {
            foreach (SelectedObjects so in _currentlySelectedObjects)
            {
                if (so.SelectedTiles.Count > 0)
                {
                    foreach (Tile t in so.SelectedTiles)
                    {
                        _map.Tiles.Remove(t);
                    }
                }
                if (so.SelectedItems.Count > 0)
                {
                    foreach (Item i in so.SelectedItems)
                    {
                        _map.Items.Remove(i);
                    }
                }
                if (so.SelectedCreature.Id != -1)
                {
                    _map.Creatures.Remove(so.SelectedCreature);
                }
            }
        }

        private void PasteBuffer()
        {
            foreach (SelectedObjects so in _copyBuffer)
            {
                if (so.SelectedTiles.Count > 0)
                {
                    foreach (Tile t in so.SelectedTiles)
                    {
                        _map.AddTile(t.Sprite, new Coordinates(t.Position.X - so.StartPos.X + _mouseLocation.X, t.Position.Y - so.StartPos.Y + _mouseLocation.Y, _currentZ), t.ZOrder);
                    }
                }
                if (so.SelectedItems.Count > 0)
                {
                    foreach (Item i in so.SelectedItems)
                    {
                        _map.AddItem(i.Sprite, new Coordinates(i.Position.X - so.StartPos.X + _mouseLocation.X, i.Position.Y - so.StartPos.Y + _mouseLocation.Y, _currentZ), i.ZOrder);
                    }
                }
                if (so.SelectedCreature.Id != -1)
                {
                    _map.AddCreature(so.SelectedCreature.Sprite, new Coordinates(so.SelectedCreature.Position.X - so.StartPos.X + _mouseLocation.X, so.SelectedCreature.Position.Y - so.StartPos.Y + _mouseLocation.Y, _currentZ));
                }
            }
        }

        private void ClearSelectedObjects()
        {
            foreach (SelectedObjects so in _currentlySelectedObjects)
            {
                if (so.SelectedTiles.Count > 0)
                {
                    foreach (Tile t in so.SelectedTiles)
                    {
                        t.Selected = false;
                    }
                }
                if (so.SelectedItems.Count > 0)
                {
                    foreach (Item i in so.SelectedItems)
                    {
                        i.Selected = false;
                    }
                }
                if (so.SelectedCreature.Id != -1)
                {
                    so.SelectedCreature.Selected = false;
                }
            }

            _currentlySelectedObjects.Clear();
        }

        private Item PlaceOrRemoveFixedItem(int nx, int ny, int nz, int zOrder, MapAction action, Item placeItem)
        {
            Item returnItem = new Item();
            if (action == MapAction.Place)
            {
                if (placeItem.SpriteId != -1)
                {
                    Item newItem = new Item(placeItem.Sprite, placeItem.Sprite.SpriteName, new Coordinates(nx, ny, nz), zOrder);
                    returnItem = _map.AddItem(newItem.Sprite, newItem.Position, zOrder);
                    if (returnItem.SpriteId < 1)
                    {
                        returnItem = newItem;
                    }
                }
            }
            else if(action == MapAction.Remove)
            {
                Item r = _map.RemoveItem(new Coordinates(nx, ny, nz), zOrder);
                returnItem = r;

                if(placeItem.SpriteId != -1 && placeItem.WasReplaced)
                {
                    Item newItem = new Item(placeItem.Sprite, placeItem.Sprite.SpriteName, new Coordinates(nx, ny, nz), zOrder);
                    _map.AddItem(newItem.Sprite, newItem.Position, zOrder);
                }
            }


            return returnItem;
        }

        private void PlaceOrRemoveItem(int nx, int ny, int nz, int zOrder)
        {
            List<Item> itemsChanged = new List<Item>();

            if (_currentAction == MapAction.Place)
            {
                ImageListItem cSprite = _lastClickedItem;

                if (cSprite.Id != -1)
                {
                    for (int y = 0; y < _tileAmount.Height; y++)
                    {
                        for (int x = 0; x < _tileAmount.Width; x++)
                        {
                            Item newItem = new Item(cSprite.Sprite, cSprite.Sprite.SpriteName, new Coordinates(nx + x, ny + y, nz), zOrder);
                            Item replacedItem = _map.AddItem(newItem.Sprite, newItem.Position, newItem.ZOrder);
                            if (replacedItem.SpriteId < 1)
                            {
                                replacedItem = newItem;
                            }
                            itemsChanged.Add(replacedItem);
                        }
                    }
                    if (_undoActions.Count < _maxChangeActions)
                    {
                        _undoActions.Add(new ChangeAction(new Coordinates(nx, ny, nz), zOrder, MapAction.Remove, cSprite.Sprite, _tileAmount, SpriteCategory.Items, null, null, itemsChanged));
                    }
                    else
                    {
                        _undoActions.Pop(PeekAt.FirstAdded);
                        _undoActions.Add(new ChangeAction(new Coordinates(nx, ny, nz), zOrder, MapAction.Remove, cSprite.Sprite, _tileAmount, SpriteCategory.Items, null, null, itemsChanged));
                    }
                }
            }
            else if (_currentAction == MapAction.Remove)
            {
                for (int y = 0; y < _tileAmount.Height; y++)
                {
                    for (int x = 0; x < _tileAmount.Width; x++)
                    {
                        Item i = _map.RemoveTopItem(new Coordinates(nx + x, ny + y, nz));
                        itemsChanged.Add(i);
                    }
                }

                if (_undoActions.Count < _maxChangeActions)
                {
                    _undoActions.Add(new ChangeAction(new Coordinates(nx, ny, nz), zOrder, MapAction.Place, itemsChanged[0].Sprite, _tileAmount, SpriteCategory.Items, null, null, itemsChanged));
                }
                else
                {
                    _undoActions.Pop(PeekAt.FirstAdded);
                    _undoActions.Add(new ChangeAction(new Coordinates(nx, ny, nz), zOrder, MapAction.Place, itemsChanged[0].Sprite, _tileAmount, SpriteCategory.Items, null, null, itemsChanged));
                }
            }
            if(itemsChanged.Count > 0)
            {
                _redoActions.Clear();
            }
        }

        private void LoadMapName()
        {
            FileInfo f = new FileInfo("Content\\settings.dat");
            if (f.Exists)
            {
                string tmpName = null;
                using (var sr = new StreamReader("Content\\settings.dat"))
                {
                    tmpName = sr.ReadLine();
                }

                if (tmpName?.EndsWith(".map") ?? false)
                {
                    _map.MapFileName = tmpName;
                    _map.MapFilePath = _map.MapFileDir + _map.MapFileName;
                }
            }

        }

        private void SaveMapName()
        {
            using (var sw = new StreamWriter("Content\\settings.dat"))
            {
                sw.WriteLine(_map.MapFileName);
            }
        }

        private Creature PlaceOrRemoveFixedCreature(int nx, int ny, int nz, MapAction action, Creature placeCreature)
        {
            Creature returnCreature = new Creature();
            if (action == MapAction.Place)
            {
                if (placeCreature.SpriteId != -1)
                {
                    Creature newCreature = new Creature(placeCreature.Sprite.SpriteName, placeCreature.Sprite, new Coordinates(nx, ny, nz));
                    returnCreature = _map.AddCreature(newCreature.Sprite, newCreature.Position);
                    if (returnCreature.SpriteId < 1)
                    {
                        returnCreature = newCreature;
                    }
                }
            }
            else if (action == MapAction.Remove)
            {
                Creature r = _map.RemoveCreature(new Coordinates(nx, ny, nz));
                returnCreature = r;

                if(placeCreature.SpriteId != -1 && placeCreature.WasReplaced)
                {
                    Creature newCreature = new Creature(placeCreature.Sprite.SpriteName, placeCreature.Sprite, new Coordinates(nx, ny, nz));
                    _map.AddCreature(newCreature.Sprite, newCreature.Position);
                }
            }

            return returnCreature;
        }

        private void PlaceOrRemoveCreature(int nx, int ny, int nz)
        {
            List<Creature> creaturesChanged = new List<Creature>();

            if (_currentAction == MapAction.Place)
            {
                ImageListItem cSprite = _lastClickedItem;

                if (cSprite.Id != -1)
                {
                    for (int y = 0; y < _tileAmount.Height; y++)
                    {
                        for (int x = 0; x < _tileAmount.Width; x++)
                        {
                            Creature newCreature = new Creature(cSprite.Sprite.SpriteName, cSprite.Sprite, new Coordinates(nx + x, ny + y, nz));
                            Creature replacedCreature = _map.AddCreature(cSprite.Sprite, new Coordinates(nx + x, ny + y, nz));
                            if (replacedCreature.SpriteId < 1)
                            {
                                replacedCreature = newCreature;
                            }
                            creaturesChanged.Add(replacedCreature);
                        }
                    }

                    if (_undoActions.Count < _maxChangeActions)
                    {
                        _undoActions.Add(new ChangeAction(new Coordinates(nx, ny, nz), 0, MapAction.Remove, cSprite.Sprite, _tileAmount, SpriteCategory.Creatures, null, creaturesChanged));
                    }
                    else
                    {
                        _undoActions.Pop(PeekAt.FirstAdded);
                        _undoActions.Add(new ChangeAction(new Coordinates(nx, ny, nz), 0, MapAction.Remove, cSprite.Sprite, _tileAmount, SpriteCategory.Creatures, null, creaturesChanged));
                    }
                }
            }
            else if (_currentAction == MapAction.Remove)
            {
                for (int y = 0; y < _tileAmount.Height; y++)
                {
                    for (int x = 0; x < _tileAmount.Width; x++)
                    {
                        Creature r = _map.RemoveCreature(new Coordinates(nx + x, ny + y, nz));
                        creaturesChanged.Add(r);
                    }
                }

                if (_undoActions.Count < _maxChangeActions)
                {
                    _undoActions.Add(new ChangeAction(new Coordinates(nx, ny, nz), 0, MapAction.Place, creaturesChanged[0].Sprite, _tileAmount, SpriteCategory.Creatures, null, creaturesChanged));
                }
                else
                {
                    _undoActions.Pop(PeekAt.FirstAdded);
                    _undoActions.Add(new ChangeAction(new Coordinates(nx, ny, nz), 0, MapAction.Place, creaturesChanged[0].Sprite, _tileAmount, SpriteCategory.Creatures, null, creaturesChanged));
                }
            }
            if(creaturesChanged.Count > 0)
            {
                _redoActions.Clear();
            }
        }

        private Tile PlaceOrRemoveFixedTile(int nx, int ny, int nz, int zOrder, MapAction action, Tile placeTile)
        {
            Tile returnTile = new Tile();
            if (action == MapAction.Place)
            {
                if (placeTile.SpriteId != -1)
                {
                    Tile newTile = new Tile(placeTile.Sprite.SpriteName, placeTile.Sprite, new Coordinates(nx, ny, nz), _map.Tiles.Count, true, placeTile.Sprite.Walkthrough, zOrder);
                    returnTile = _map.AddTile(newTile.Sprite, newTile.Position, zOrder);
                    if (returnTile.SpriteId < 1)
                    {
                        returnTile = newTile;
                    }
                }
            }
            else if (action == MapAction.Remove)
            {
                Tile r = _map.RemoveTile(_map.Tiles, new Coordinates(nx, ny, nz), zOrder);
                returnTile = r;

                if (placeTile.SpriteId != -1 && placeTile.WasReplaced)
                {
                    Tile newTile = new Tile(placeTile.Sprite.SpriteName, placeTile.Sprite, new Coordinates(nx, ny, nz), _map.Tiles.Count, true, placeTile.Sprite.Walkthrough, zOrder);
                    _map.AddTile(newTile.Sprite, newTile.Position, zOrder);
                }
            }

            return returnTile;
        }

        private void PlaceOrRemoveTile(int nx, int ny, int nz, int zOrder)
        {
            List<Tile> tilesChanged = new List<Tile>();

            if (_currentAction == MapAction.Place)
            {
                ImageListItem cSprite = _lastClickedItem;

                if (cSprite.Id != -1)
                {
                    if (!string.IsNullOrEmpty(cSprite.Sprite.RandomizeCategory)) // place randomized tiles
                    {
                        Random rand = new Random();
                        if (_tileAmount.Height == 1 && _tileAmount.Width == 1)
                        {
                            SpriteObject randSprite = GetRandomizedSprite(cSprite.Sprite, rand.Next(1547353));
                            Tile newTile = new Tile(cSprite.Sprite.SpriteName, randSprite, new Coordinates(nx, ny, nz), _map.Tiles.Count, true, cSprite.Sprite.Walkthrough, zOrder);
                            Tile replacedTile = _map.AddTile(newTile.Sprite, newTile.Position, newTile.ZOrder);
                            if (replacedTile.SpriteId < 1)
                            {
                                replacedTile = newTile;
                            }
                            tilesChanged.Add(replacedTile);
                        }
                        else
                        {
                            for (int y = 0; y < _tileAmount.Height; y++) // place multiple randomized tiles (e.g. 2x2, 4x4 etc)
                            {
                                for (int x = 0; x < _tileAmount.Width; x++)
                                {
                                    SpriteObject randSprite = GetRandomizedSprite(cSprite.Sprite, x + y + rand.Next(15003959));
                                    Tile newTile = new Tile(randSprite.SpriteName, randSprite, new Coordinates(nx + x, ny + y, nz), _map.Tiles.Count, true, randSprite.Walkthrough, zOrder);
                                    Tile replacedTile = _map.AddTile(newTile.Sprite, newTile.Position, newTile.ZOrder);
                                    if (replacedTile.SpriteId < 1)
                                    {
                                        replacedTile = newTile;
                                    }
                                    tilesChanged.Add(replacedTile);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (cSprite.SpriteCollection != null) // place sprite collection
                        {
                            for (int tAy = 0; tAy < _tileAmount.Height; tAy++)
                            {
                                for (int tAx = 0; tAx < _tileAmount.Width; tAx++)
                                {
                                    for (int y = 0; y < cSprite.SpriteCollection.Rows; y++)
                                    {
                                        for (int x = 0; x < cSprite.SpriteCollection.Columns; x++)
                                        {
                                            Tile newTile = new Tile(cSprite.SpriteCollection.Sprite[y, x].SpriteName, cSprite.SpriteCollection.Sprite[y, x], new Coordinates(nx + x + (tAy * cSprite.SpriteCollection.Columns), ny + y + (tAx * cSprite.SpriteCollection.Rows), nz), _map.Tiles.Count, true, cSprite.SpriteCollection.Sprite[y, x].Walkthrough, zOrder);
                                            Tile replacedTile = _map.AddTile(newTile.Sprite, newTile.Position, newTile.ZOrder);
                                            if (replacedTile.SpriteId < 1)
                                            {
                                                replacedTile = newTile;
                                            }
                                            tilesChanged.Add(replacedTile);
                                        }
                                    }
                                }
                            }
                        }
                        else // place regular tile
                        {
                            for (int y = 0; y < _tileAmount.Height; y++)
                            {
                                for (int x = 0; x < _tileAmount.Width; x++)
                                {
                                    Tile newTile = new Tile(cSprite.Sprite.SpriteName, cSprite.Sprite, new Coordinates(nx + x, ny + y, nz), _map.Tiles.Count, true, cSprite.Sprite.Walkthrough, zOrder);
                                    Tile replacedTile = _map.AddTile(newTile.Sprite, newTile.Position, newTile.ZOrder);
                                    if (replacedTile.SpriteId < 1)
                                    {
                                        replacedTile = newTile;
                                    }
                                    tilesChanged.Add(replacedTile);
                                }
                            }
                        }
                    }
                    /* If the current MapAction is 'Place', to reverse it when undoing, we need to change it to 'Remove' */
                    if (_undoActions.Count < _maxChangeActions)
                    {
                        _undoActions.Add(new ChangeAction(new Coordinates(nx, ny, nz), zOrder, MapAction.Remove, cSprite.Sprite, _tileAmount, SpriteCategory.Tiles, tilesChanged));
                    }
                    else
                    {
                        _undoActions.Pop(PeekAt.FirstAdded);
                        _undoActions.Add(new ChangeAction(new Coordinates(nx, ny, nz), zOrder, MapAction.Remove, cSprite.Sprite, _tileAmount, SpriteCategory.Tiles, tilesChanged));
                    }
                }
            }
            else if (_currentAction == MapAction.Remove)
            {
                for (int y = 0; y < _tileAmount.Height; y++)
                {
                    for (int x = 0; x < _tileAmount.Width; x++)
                    {
                        Tile r = _map.RemoveTopTile(new Coordinates(nx + x, ny + y, nz));
                        tilesChanged.Add(r);
                    }
                }
                /* If the current MapAction is 'Remove', to reverse it when undoing, we need to change it to 'Place' */
                if (_undoActions.Count < _maxChangeActions)
                {
                    _undoActions.Add(new ChangeAction(new Coordinates(nx, ny, nz), zOrder, MapAction.Place, tilesChanged[0].Sprite, _tileAmount, SpriteCategory.Tiles, tilesChanged));
                }
                else
                {
                    _undoActions.Pop(PeekAt.FirstAdded);
                    _undoActions.Add(new ChangeAction(new Coordinates(nx, ny, nz), zOrder, MapAction.Place, tilesChanged[0].Sprite, _tileAmount, SpriteCategory.Tiles, tilesChanged));
                }
            }
            if (tilesChanged.Count > 0)
            {
                _redoActions.Clear();
            }
        }

        private void LoadSpritesheets(DynamicLuaTable tab, SpriteCategory category)
        {
            dynamic dataTable;

            string tmpName = "";
            int tmpSpritesheetId = 0;
            string tmpSpriteName = "";

            foreach (KeyValuePair<object, object> kvp in tab)
            {
                if (kvp.Value.ToString() == "table")
                {
                    dataTable = kvp.Value;
                    var enumer = dataTable.GetEnumerator();
                    while (enumer.MoveNext())
                    {
                        if (enumer.Current.Key.ToString() == "Name")
                        {
                            tmpName = enumer.Current.Value.ToString();
                        }
                        else if (enumer.Current.Key.ToString() == "SpriteName")
                        {
                            tmpSpriteName = enumer.Current.Value.ToString();
                        }
                        else if (enumer.Current.Key.ToString() == "SpritesheetID")
                        {
                            tmpSpritesheetId = int.Parse(enumer.Current.Value.ToString());
                        }
                    }
                    enumer.Dispose();
                    _storage.AddSpritesheet(tmpSpritesheetId, tmpName, Content.Load<Texture2D>("Graphics\\" + tmpSpriteName), category);
                    //map.TileList.Add(new Tile(tmpName, tmpSpriteID, null, 0, true));
                }
            }
        }

        private void LoadSprites(DynamicLuaTable tab, SpriteCategory category)
        {
            dynamic dataTable;

            foreach (KeyValuePair<object, object> kvp in tab)
            {
                string tmpName = "";
                int tmpSpriteId = 0;
                string tmpSpriteName = "";
                bool walkable = false;
                bool loadFromSpritesheet = false;
                int tmpSpritesheetId = -1;
                Rectangle tmpSpritesheetPosition = new Rectangle();
                string tmpRandomizeCategory = null;
                string tmpSpriteCollection = "";
                string tmpListCategory = null;
                bool tmpShow = true;

                if (kvp.Value.ToString() == "table")
                {
                    dataTable = kvp.Value;
                    var enumer = dataTable.GetEnumerator();
                    while (enumer.MoveNext())
                    {
                        if (enumer.Current.Key.ToString() == "Name")
                        {
                            tmpName = enumer.Current.Value.ToString();
                        }
                        else if (enumer.Current.Key.ToString() == "SpriteName")
                        {
                            tmpSpriteName = enumer.Current.Value.ToString();
                        }
                        else if (enumer.Current.Key.ToString() == "SpriteID")
                        {
                            tmpSpriteId = int.Parse(enumer.Current.Value.ToString());
                        }
                        else if (enumer.Current.Key.ToString() == "SpritesheetID")
                        {
                            tmpSpritesheetId = int.Parse(enumer.Current.Value.ToString());
                            if(tmpSpritesheetId != -1)
                            {
                                loadFromSpritesheet = true;
                            }
                        }
                        else if (enumer.Current.Key.ToString() == "X")
                        {
                            tmpSpritesheetPosition.X = int.Parse(enumer.Current.Value.ToString());
                        }
                        else if (enumer.Current.Key.ToString() == "Y")
                        {
                            tmpSpritesheetPosition.Y = int.Parse(enumer.Current.Value.ToString());
                        }
                        else if (enumer.Current.Key.ToString() == "W")
                        {
                            tmpSpritesheetPosition.Width = int.Parse(enumer.Current.Value.ToString());
                        }
                        else if (enumer.Current.Key.ToString() == "H")
                        {
                            tmpSpritesheetPosition.Height = int.Parse(enumer.Current.Value.ToString());
                        }
                        else if (enumer.Current.Key.ToString() == "SpriteCollection")
                        {
                            if (enumer.Current.Value.ToString() != "")
                            {
                                tmpSpriteCollection = enumer.Current.Value.ToString();
                            }
                        }
                        else if (enumer.Current.Key.ToString() == "RandomizeCategory")
                        {
                            if (enumer.Current.Value.ToString() != "")
                            {
                                tmpRandomizeCategory = enumer.Current.Value.ToString();
                            }
                        }
                        else if (enumer.Current.Key.ToString() == "Show")
                        {
                            tmpShow = bool.Parse(enumer.Current.Value.ToString());
                        }
                        else if (enumer.Current.Key.ToString() == "ListCategory")
                        {
                            tmpListCategory = enumer.Current.Value.ToString();
                        }
                        else if (category == SpriteCategory.Tiles && enumer.Current.Key.ToString() == "Walkable")
                        {
                            walkable = bool.Parse(enumer.Current.Value.ToString());
                        }
                    }
                    enumer.Dispose();
                    if (loadFromSpritesheet)
                    {
                        _storage.AddSprite(tmpSpriteId, category, tmpName, tmpSpritesheetPosition, tmpSpritesheetId, tmpRandomizeCategory, walkable, tmpSpriteCollection, tmpShow, tmpListCategory);
                    }
                    else
                    {
                        _storage.AddSprite(Content.Load<Texture2D>("Graphics\\" + tmpSpriteName), tmpSpriteId, category, tmpName, walkable, tmpRandomizeCategory, tmpSpriteCollection, tmpShow, tmpListCategory);
                        //map.TileList.Add(new Tile(tmpName, tmpSpriteID, null, 0, true, walkable));
                    }
                }
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Close();
            }

            if(Window != null)
            {
                Window.Title = "RPGay Map Editor: Editing \"" + _map.MapFileName + "\"";
            }

            if (IsActive)
            {
                MouseState currentState = Mouse.GetState();
                Vector2 scaledMousePos = GetScalableMousePosition(currentState.Position.ToVector2());

                DoKeyboardEvents(gameTime.TotalGameTime.TotalMilliseconds);
                DoClickEvents(currentState, scaledMousePos);

                DoShortcuts();
                DoScrollWheelEvents(currentState, scaledMousePos);
                //DoOnMouseOverEvents(currentState, mousePos);
            }



            UpdateToolPanel();
            _zorder = _gameScreen.GetPanelByName("ToolPanel").GetComboBoxByName("ZorderCombo");
            //InHouse = map.TileAbove(map.player.Position);
            /*if (InHouse)
            {
                MaxZ = map.player.Position.Z;
            }
            else
            {
                MaxZ = Utility.MaxZ;
            }*/

            //map.DoMove(gameTime.TotalGameTime.TotalMilliseconds);

            if (_inGame)
            {
                //focusedTextbox = storage.GetScreenByName("GameScreen").GetTextboxByName("Chat");
            }
            base.Update(gameTime);
        }

        private void Close()
        {
            _doClose = false;
            Dialog.Show("Are you sure you want to quit?", "Warning!");
            Dialog.Click += CloseDialog;
        }

        private void CloseDialog(object sender, EventArgs args)
        {
            Dialog.DialogButton b = (Dialog.DialogButton)sender;
            if (b == Dialog.DialogButton.Yes)
            {
                _doClose = true;
                SaveMapName();
                Exit();
            }
            Dialog.Click -= CloseDialog;
        }

        private void DoScrollWheelEvents(MouseState state, Vector2 mousePos)
        {
            if (_screenGame.Viewport.Bounds.Contains(mousePos))
            {
                ZoomMap(state, mousePos);
            }
        }

        private void ZoomMap(MouseState state, Vector2 mousePos)
        {
            if (state.ScrollWheelValue > _oldScroll.ScrollWheelValue)
            {
                // Scroll up
                int wheel = (state.ScrollWheelValue - _oldScroll.ScrollWheelValue) / 120;
                if (wheel == 1)
                {
                    if (_zoom.X < 4f)
                    {
                        _zoom.X = _zoom.X * 2;
                        _zoom.Y = _zoom.Y * 2;
                    }
                }
                _oldScroll = state;
            }
            else if (state.ScrollWheelValue < _oldScroll.ScrollWheelValue)
            {
                // Scroll down
                int wheel = (_oldScroll.ScrollWheelValue - state.ScrollWheelValue) / 120;
                if (wheel == 1)
                {
                    if (_zoom.X > 0.0625f)
                    {
                        _zoom.X = _zoom.X / 2;
                        _zoom.Y = _zoom.Y / 2;
                    }
                }
                _oldScroll = state;
            }
        }

        public class MapObject
        {
            public List<Tile> Tiles = new List<Tile>();
            public List<Creature> Creatures = new List<Creature>();
        }

        private void DoShortcuts()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !Keyboard.GetState().IsKeyDown(Keys.LeftAlt) && !Keyboard.GetState().IsKeyDown(Keys.LeftControl) && _shortcutPressed == Keys.None)
            {
                StaticControlsManager.OnEnterKeyPress();
                _shortcutPressed = Keys.Enter;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Delete) && _shortcutPressed == Keys.None)
            {
                DeleteSelectedObjects();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A) && _shortcutPressed == Keys.None)
            {
                ChangeActionToPlace();
                _shortcutPressed = Keys.A;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.R) && _shortcutPressed == Keys.None)
            {
                ChangeActionToRemove();
                _shortcutPressed = Keys.R;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.E) && _shortcutPressed == Keys.None)
            {
                NextTileAmount();
                _shortcutPressed = Keys.E;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Space) && _shortcutPressed == Keys.None)
            {
                ToggleGrid();
                _shortcutPressed = Keys.Space;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Z) && Keyboard.GetState().IsKeyDown(Keys.LeftControl) && _shortcutPressed == Keys.None)
            {
                UndoLastMapAction();
                _shortcutPressed = Keys.Z;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Y) && Keyboard.GetState().IsKeyDown(Keys.LeftControl) && _shortcutPressed == Keys.None)
            {
                RedoLastMapAction();
                _shortcutPressed = Keys.Y;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Q) && !Keyboard.GetState().IsKeyDown(Keys.LeftControl) && _shortcutPressed == Keys.None)
            {
                PreviousFloor();
                _shortcutPressed = Keys.Q;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.W) && !Keyboard.GetState().IsKeyDown(Keys.LeftControl) && _shortcutPressed == Keys.None)
            {
                NextFloor();
                _shortcutPressed = Keys.W;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Q) && Keyboard.GetState().IsKeyDown(Keys.LeftControl) && _shortcutPressed == Keys.None)
            {
                PreviousZorder();
                _shortcutPressed = Keys.Q;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.W) && Keyboard.GetState().IsKeyDown(Keys.LeftControl) && _shortcutPressed == Keys.None)
            {
                NextZorder();
                _shortcutPressed = Keys.W;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S) && !Keyboard.GetState().IsKeyDown(Keys.LeftControl) && _shortcutPressed == Keys.None)
            {
                ChangeActionToSelect();
                _shortcutPressed = Keys.S;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S) && Keyboard.GetState().IsKeyDown(Keys.LeftControl) && _shortcutPressed == Keys.None)
            {
                SaveMap();
                _shortcutPressed = Keys.S;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D) && Keyboard.GetState().IsKeyDown(Keys.LeftControl) && _shortcutPressed == Keys.None)
            {
                LoadMap();
                _shortcutPressed = Keys.D;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && Keyboard.GetState().IsKeyDown(Keys.LeftAlt) && Keyboard.GetState().IsKeyDown(Keys.LeftControl) && _shortcutPressed == Keys.None)
            {
                _graphics.ToggleFullScreen();
                _shortcutPressed = Keys.Enter;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.C) && Keyboard.GetState().IsKeyDown(Keys.LeftControl) && _shortcutPressed == Keys.None)
            {
                _copyBuffer.Clear();
                _copyBuffer.AddRange(_currentlySelectedObjects);
                _shortcutPressed = Keys.C;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.V) && Keyboard.GetState().IsKeyDown(Keys.LeftControl) && _shortcutPressed == Keys.None)
            {
                PasteBuffer();
                _shortcutPressed = Keys.V;
            }



            if (Keyboard.GetState().IsKeyUp(Keys.Enter) && _shortcutPressed == Keys.Enter)
            {
                _shortcutPressed = Keys.None;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.A) && _shortcutPressed == Keys.A)
            {
                _shortcutPressed = Keys.None;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.R) && _shortcutPressed == Keys.R)
            {
                _shortcutPressed = Keys.None;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.E) && _shortcutPressed == Keys.E)
            {
                _shortcutPressed = Keys.None;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Space) && _shortcutPressed == Keys.Space)
            {
                _shortcutPressed = Keys.None;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Q) && _shortcutPressed == Keys.Q)
            {
                _shortcutPressed = Keys.None;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.W) && _shortcutPressed == Keys.W)
            {
                _shortcutPressed = Keys.None;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Z) && _shortcutPressed == Keys.Z)
            {
                _shortcutPressed = Keys.None;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Y) && _shortcutPressed == Keys.Y)
            {
                _shortcutPressed = Keys.None;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.S) && _shortcutPressed == Keys.S)
            {
                _shortcutPressed = Keys.None;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.D) && _shortcutPressed == Keys.D)
            {
                _shortcutPressed = Keys.None;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.C) && _shortcutPressed == Keys.C)
            {
                _shortcutPressed = Keys.None;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.V) && _shortcutPressed == Keys.V)
            {
                _shortcutPressed = Keys.None;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Delete) && _shortcutPressed == Keys.Delete)
            {
                _shortcutPressed = Keys.None;
            }
        }

        private void RedoLastMapAction()
        {
            if (_redoActions.Count > 0)
            {
                ChangeAction a = _redoActions.Pop(PeekAt.LastAdded);

                MapAction nMapAction;
                if (a.MapAction == MapAction.Place)
                {
                    nMapAction = MapAction.Remove;
                }
                else // (a.mapAction == MapAction.Remove)
                {
                    nMapAction = MapAction.Place;
                }

                if (a.Category == SpriteCategory.Tiles)
                {
                    List<Tile> tilesChanged = new List<Tile>();
                    for (int i = 0; i < a.TilesChanged.Count; i++)
                    {
                        if (a.TilesChanged[i].SpriteId > 0)
                        {
                            Tile cTile = a.TilesChanged[i];
                            tilesChanged.Add(PlaceOrRemoveFixedTile(cTile.Position.X, cTile.Position.Y, cTile.Position.Z, cTile.ZOrder, nMapAction, cTile));
                        }
                    }
                    _undoActions.Add(new ChangeAction(a.Pos, a.Zorder, a.MapAction, a.Sprite, a.TileAmount, a.Category, tilesChanged));
                }
                else if (a.Category == SpriteCategory.Creatures)
                {
                    List<Creature> creaturesChanged = new List<Creature>();
                    for(int i = 0; i < a.CreaturesChanged.Count; i++)
                    {
                        if(a.CreaturesChanged[i].Sprite.Id != -1)
                        {
                            Creature cCreature = a.CreaturesChanged[i];
                            creaturesChanged.Add(PlaceOrRemoveFixedCreature(cCreature.Position.X, cCreature.Position.Y, cCreature.Position.Z, nMapAction, cCreature));
                        }
                    }
                    _undoActions.Add(new ChangeAction(a.Pos, a.Zorder, a.MapAction, a.Sprite, a.TileAmount, a.Category, null, creaturesChanged));
                }
                else if (a.Category == SpriteCategory.Items)
                {
                    List<Item> itemsChanged = new List<Item>();
                    for(int i = 0; i < a.ItemsChanged.Count; i++)
                    {
                        if(a.ItemsChanged[i].Sprite.Id != -1)
                        {
                            Item cItem = a.ItemsChanged[i];
                            itemsChanged.Add(PlaceOrRemoveFixedItem(cItem.Position.X, cItem.Position.Y, cItem.Position.Z, cItem.ZOrder, nMapAction, cItem));
                        }
                    }
                    _undoActions.Add(new ChangeAction(a.Pos, a.Zorder, a.MapAction, a.Sprite, a.TileAmount, a.Category, null, null, itemsChanged));
                }
            }
        }

        private void UndoLastMapAction()
        {
            if (_undoActions.Count > 0)
            {
                ChangeAction a = _undoActions.Pop(PeekAt.LastAdded);
                //redoActions.Add(a);

                if (a.Category == SpriteCategory.Tiles)
                {
                    List<Tile> tilesChanged = new List<Tile>();
                    for (int i = 0; i < a.TilesChanged.Count; i++)
                    {
                        if (a.TilesChanged[i].SpriteId != -1)
                        {
                            Tile cTile = a.TilesChanged[i];
                            tilesChanged.Add(PlaceOrRemoveFixedTile(cTile.Position.X, cTile.Position.Y, cTile.Position.Z, cTile.ZOrder, a.MapAction, cTile));
                        }
                    }
                    _redoActions.Add(new ChangeAction(a.Pos, a.Zorder, a.MapAction, a.Sprite, a.TileAmount, a.Category, tilesChanged));
                }
                else if (a.Category == SpriteCategory.Creatures)
                {
                    List<Creature> creaturesChanged = new List<Creature>();
                    for(int i = 0; i < a.CreaturesChanged.Count; i++)
                    {
                        if (a.CreaturesChanged[i].SpriteId != -1)
                        {
                            Creature cCreature = a.CreaturesChanged[i];
                            creaturesChanged.Add(PlaceOrRemoveFixedCreature(cCreature.Position.X, cCreature.Position.Y, cCreature.Position.Z, a.MapAction, cCreature));
                        }
                    }
                    _redoActions.Add(new ChangeAction(a.Pos, a.Zorder, a.MapAction, a.Sprite, a.TileAmount, a.Category, null, creaturesChanged));
                }
                else if (a.Category == SpriteCategory.Items)
                {
                    List<Item> itemsChanged = new List<Item>();
                    for(int i = 0; i < a.ItemsChanged.Count; i++)
                    {
                        if (a.ItemsChanged[i].SpriteId != -1)
                        {
                            Item cItem = a.ItemsChanged[i];
                            itemsChanged.Add(PlaceOrRemoveFixedItem(cItem.Position.X, cItem.Position.Y, cItem.Position.Z, cItem.ZOrder, a.MapAction, cItem));
                        }
                    }
                    _redoActions.Add(new ChangeAction(a.Pos, a.Zorder, a.MapAction, a.Sprite, a.TileAmount, a.Category, null, null, itemsChanged));
                }
            }
        }

        private void PreviousZorder()
        {
            ComboBox zorder = _gameScreen.GetPanelByName("ToolPanel").GetComboBoxByName("ZorderCombo");
            if (zorder.ClickedId > 0)
            {
                zorder.UpdateClicked(zorder.ClickedId - 1);
            }
            else
            {
                zorder.UpdateClicked(zorder.Items.Count - 1);
            }
        }

        private void NextZorder()
        {
            ComboBox zorder = _gameScreen.GetPanelByName("ToolPanel").GetComboBoxByName("ZorderCombo");
            if (zorder.ClickedId < zorder.Items.Count - 1)
            {
                zorder.UpdateClicked(zorder.ClickedId + 1);
            }
            else
            {
                zorder.UpdateClicked(0);
            }
        }


        private void PreviousFloor()
        {
            ComboBox floor = _gameScreen.GetPanelByName("ToolPanel").GetComboBoxByName("FloorCombo");
            if (floor.ClickedId > 0)
            {
                floor.UpdateClicked(floor.ClickedId - 1);
            }
            else
            {
                floor.UpdateClicked(floor.Items.Count - 1);
            }
        }

        private void NextFloor()
        {
            ComboBox floor = _gameScreen.GetPanelByName("ToolPanel").GetComboBoxByName("FloorCombo");
            if (floor.ClickedId < floor.Items.Count - 1)
            {
                floor.UpdateClicked(floor.ClickedId + 1);
            }
            else
            {
                floor.UpdateClicked(0);
            }
        }

        private void NextTileAmount()
        {
            ListBox tileSize = _gameScreen.GetPanelByName("UIPanel").GetListBoxByName("TileSizeList");
            if(tileSize.ClickedId < tileSize.Items.Count - 1)
            {
                tileSize.UpdateClicked(tileSize.ClickedId + 1);
            }
            else
            {
                tileSize.UpdateClicked(0);
            }
        }

        private void UpdateToolPanel()
        {
            UpdateTileId();
            UpdateLocation();
            UpdateMouseLocation();
            UpdateAction();
            UpdateZoom();
        }

        private void UpdateZoom()
        {
            _gameScreen.GetPanelByName("ToolPanel").GetLabelByName("Zoom").Text = "Zoom: " + (int)(_zoom.X * 100) + "%";
        }

        private void UpdateAction()
        {
            _gameScreen.GetPanelByName("ToolPanel").GetLabelByName("Action").Text = "Action: " + _currentAction.ToString();
        }

        private void UpdateLocation()
        {
            _gameScreen.GetPanelByName("ToolPanel").GetLabelByName("Location").Text = "World Location: " + _currentLocation.X + ", " + _currentLocation.Y + ", " + _currentLocation.Z;
        }

        private void UpdateMouseLocation()
        {
            _gameScreen.GetPanelByName("ToolPanel").GetLabelByName("MouseLocation").Text = "Mouse Location: " + _mouseLocation.X + ", " + _mouseLocation.Y + ", " + _mouseLocation.Z;
        }

        private void UpdateTileId()
        {
            if (_lastClickedItem.Id != -1)
            {
                _gameScreen.GetPanelByName("ToolPanel").GetLabelByName("TileID").Text = "TileID: " + _lastClickedItem.Sprite.Id;
            }
            else
            {
                _gameScreen.GetPanelByName("ToolPanel").GetLabelByName("TileID").Text = "TileID: ";
            }
        }

        private void DoKeyboardEvents(double gameTime)
        {
            if (gameTime - _map.TimeOfLastMovement > _map.Player.Speed && _map.Player.MovementAnimation.Count == 0)
            {
                UpdateRecentlyPressedKey();
                UpdateWalkingState();
                Keyboard_MovePlayer(gameTime);
            }
        }

        private void UpdateRecentlyPressedKey()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _mostRecentKey = Keys.Right;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _mostRecentKey = Keys.Left;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                _mostRecentKey = Keys.Up;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                _mostRecentKey = Keys.Down;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                _mostRecentKey = Keys.Space;
            }
        }

        private void UpdateWalkingState()
        {
            if (!_walking && _mostRecentKey != _lastPressedKey)
            {
                if (_mostRecentKey == Keys.Right)
                {
                    _walkingDirection = Direction.East;
                    _walking = true;
                }
                else if (_mostRecentKey == Keys.Left)
                {
                    _walkingDirection = Direction.West;
                    _walking = true;
                }
                else if (_mostRecentKey == Keys.Up)
                {
                    _walkingDirection = Direction.North;
                    _walking = true;
                }
                else if (_mostRecentKey == Keys.Down)
                {
                    _walkingDirection = Direction.South;
                    _walking = true;
                }
                _lastPressedKey = _mostRecentKey;
            }
            _mostRecentKey = Keys.None;
        }

        private void Keyboard_MovePlayer(double gameTime)
        {
            if (_walking)
            {
                MoveAroundMap(_walkingDirection, gameTime);
                ResetWalkingState(gameTime);
            }
        }

        private void MoveAroundMap(Direction walkingDirection, double gameTime)
        {
            if (walkingDirection != Direction.None)
            {
                Coordinates newLoc = new Coordinates();
                if (walkingDirection == Direction.East)
                {
                    newLoc = new Coordinates(_currentLocation.X + 1, _currentLocation.Y, _currentZ);
                }
                else if (walkingDirection == Direction.West)
                {
                    newLoc = new Coordinates(_currentLocation.X - 1, _currentLocation.Y, _currentZ);
                }
                else if (walkingDirection == Direction.South)
                {
                    newLoc = new Coordinates(_currentLocation.X, _currentLocation.Y + 1, _currentZ);
                }
                else if (walkingDirection == Direction.North)
                {
                    newLoc = new Coordinates(_currentLocation.X, _currentLocation.Y - 1, _currentZ);
                }
                if (newLoc.X >= 0 && newLoc.Y >= 0)
                {
                    _currentLocation = newLoc;
                }
                    /*Animation newAnimation = new Animation(gameTime, gameTime + player.Speed, 0.1);
                    player.MovementAnimation.Add(new Movement(dir, _Destination, newAnimation));
                    TimeOfLastMovement = (int)gameTime;*/
            }
        }

        private void ResetWalkingState(double gameTime)
        {
            _walking = false;
            _mostRecentKey = Keys.None;
            _lastPressedKey = Keys.None;
        }

        /*private void ResetPath()
        {
            if (map.player.hasPath())
            {
                map.player.ResetPath();
            }
        }*/

        private Vector2 GetScalableMousePosition(Vector2 cursorPos)
        {
            Vector2 newPos;
            newPos.X = cursorPos.X / _mouseScale.X;
            newPos.Y = cursorPos.Y / _mouseScale.Y;
            return newPos;
        }

        private void DoClickEvents(MouseState state, Vector2 scaledMousePos)
        {
            bool clickedTextbox = false;

            if (_clickEventFired)
            {
                _clickEventFired = false;
            }

            for (int i = 0; i < _storage.ScreenList.Count; i++)
            {
                _tmpClicked = _storage.ScreenList[i].DoEvents(state, scaledMousePos);
                if (_tmpClicked.ClickedTextbox.Id > 0)
                {
                    _focusedTextbox = _tmpClicked.ClickedTextbox;
                    clickedTextbox = true;
                    _clickEventFired = true;
                }
                _storage.ComboBoxOpen = _tmpClicked.ComboBoxOpen;
            }

            if(StaticControlsManager.DoEvents(state, scaledMousePos))
            {
                _clickEventFired = true;
            }

            if (!_clickEventFired)
            {
                if (state.LeftButton == ButtonState.Pressed && !clickedTextbox)
                {
                    _focusedTextbox = new TextBox();
                }
            }

            MoveMap(state, scaledMousePos);

            DoSelectObjects(state, scaledMousePos);
        }

        private void DoSelectObjects(MouseState state, Vector2 scaledMousePos)
        {
            if (_currentAction == MapAction.Select)
            {
                if (state.LeftButton == ButtonState.Pressed && !_selecting)
                {
                    if (_screenGame.Viewport.Bounds.Contains(scaledMousePos))
                    {
                        _selectStartPoint = scaledMousePos;
                        _selectStartPoint.X /= (_zoom.X);
                        _selectStartPoint.Y /= (_zoom.Y);
                        _drawStartPoint = _selectStartPoint;
                        _selecting = true;
                    }
                }
                else if (state.LeftButton == ButtonState.Released && _selecting)
                {
                    if (_screenGame.Viewport.Bounds.Contains(scaledMousePos))
                    {
                        _selectEndPoint = scaledMousePos;
                        _selectEndPoint.X /= (_zoom.X);
                        _selectEndPoint.Y /= (_zoom.Y);
                        Vector2 tmpPos = new Vector2(_selectEndPoint.X, _selectEndPoint.Y);
                        if (GetLength_X(_selectStartPoint, _selectEndPoint) < 0)
                        {
                            tmpPos.X = _selectEndPoint.X;
                            _selectEndPoint.X = _selectStartPoint.X;
                            _selectStartPoint.X = tmpPos.X;
                        }
                        if (GetLength_Y(_selectStartPoint, _selectEndPoint) < 0)
                        {
                            tmpPos.Y = _selectEndPoint.Y;
                            _selectEndPoint.Y = _selectStartPoint.Y;
                            _selectStartPoint.Y = tmpPos.Y;
                        }
                        _selectStartPoint = ConvertScreenToGridLocation(_selectStartPoint);
                        _selectEndPoint = ConvertScreenToGridLocation(_selectEndPoint);
                        _selectEndPoint.X += 1;
                        _selectEndPoint.Y += 1;
                        SelectObjects(new Coordinates((int)_selectStartPoint.X, (int)_selectStartPoint.Y, _currentZ), new Coordinates((int)_selectEndPoint.X, (int)_selectEndPoint.Y, _currentZ));
                    }
                    _selecting = false;
                }

                if (state.LeftButton == ButtonState.Pressed && _selecting)
                {
                    _selectEndPoint = scaledMousePos;
                    _selectEndPoint.X /= (_zoom.X);
                    _selectEndPoint.Y /= (_zoom.Y);
                    _drawEndPoint = _selectEndPoint;
                    Vector2 tmpPos = new Vector2(_selectEndPoint.X, _selectEndPoint.Y);
                    if (GetLength_X(_selectStartPoint, _selectEndPoint) < 0)
                    {
                        tmpPos.X = _selectEndPoint.X;
                        _drawEndPoint.X = _selectStartPoint.X;
                        _drawStartPoint.X = tmpPos.X;
                    }
                    if (GetLength_Y(_selectStartPoint, _selectEndPoint) < 0)
                    {
                        tmpPos.Y = _selectEndPoint.Y;
                        _drawEndPoint.Y = _selectStartPoint.Y;
                        _drawStartPoint.Y = tmpPos.Y;
                    }
                }
            }
        }

        private void MoveMap(MouseState state, Vector2 mousePos)
        {
            if (state.RightButton == ButtonState.Pressed && !_movingMap)
            {
                if (_screenGame.Viewport.Bounds.Contains(mousePos))
                {
                    _movingMap = true;
                }
            }
            else if (state.RightButton == ButtonState.Released && _movingMap)
            {
                _movingMap = false;
            }

            if (_movingMap)
            {
                if (Math.Abs(mousePos.X - _oldCursorPos.X) > Coordinates.Step * _zoom.X || Math.Abs(mousePos.Y - _oldCursorPos.Y) > Coordinates.Step * _zoom.Y)
                {
                    float add = 2f;
                    //int newX = _currentLocation.X + (int)(((_oldCursorPos.X - mousePos.X) / 16) * add);
                    //int newY = _currentLocation.Y + (int)(((_oldCursorPos.Y - mousePos.Y) / 16) * add);
                    int newX = (int)(_currentLocation.X + (_oldCursorPos.X - mousePos.X) / (Coordinates.Step * _zoom.X));
                    int newY = (int)(_currentLocation.Y + ((_oldCursorPos.Y - mousePos.Y) / (Coordinates.Step * _zoom.Y)));
                    if (newX >= 0 && newY >= 0)
                    {
                        _currentLocation = new Coordinates(newX, newY, _currentZ);
                    }
                    else if (newX >= 0 && newY <= 5)
                    {
                        _currentLocation = new Coordinates(newX, 0, _currentZ);
                    }
                    else if (newY >= 0 && newX <= 5)
                    {
                        _currentLocation = new Coordinates(0, newY, _currentZ);
                    }

                    _oldCursorPos = mousePos; //GetScalableMousePosition(state.Position.ToVector2());
                }
            }
            else
            {
                _oldCursorPos = mousePos; //GetScalableMousePosition(state.Position.ToVector2());
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _getMouse = GetScalableMousePosition(Mouse.GetState().Position.ToVector2());

            int curZ = int.Parse(_gameScreen.GetPanelByName("ToolPanel").GetComboBoxByName("FloorCombo").SelectedItemText);

            int x = (int)_getMouse.X / (int)(Coordinates.Step * _zoom.X);
            int y = (int)_getMouse.Y / (int)(Coordinates.Step * _zoom.X);
            int nx = x + _currentLocation.X;
            int ny = y + _currentLocation.Y;

            _mouseLocation = new Coordinates(nx, ny, curZ);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            //_spriteBatch.Draw(storage.GetSpriteByName("UI_Background"), new Vector2(0, 0), Color.White);
            Screen currentScreen;

            for (int i = 0; i < _storage.ScreenList.Count; i++)
            {
                currentScreen = _storage.ScreenList[i];
                if (currentScreen.Visible)
                {
                    _storage.CurrentScreen = currentScreen;
                    if (currentScreen.Name == "MenuScreen")
                    {
                        //currentScreen.SetupViewport(_spriteBatch, currentScreen.GetViewportByName("ScreenFull").Viewport);
                        //DrawBackground();
                    }
                    else if (currentScreen.Name == "GameScreen")
                    {
                        StaticControlsManager.SetUpScreens(currentScreen);

                        Viewport cView = currentScreen.GetViewportByName("ScreenGame").Viewport;
                        currentScreen.SetupViewport(_spriteBatch, cView);
                        int startZ = GetStartZ(curZ);

                        int maxZ = curZ;

                        _currentZ = curZ;

                        Coordinates mousePos = new Coordinates(nx, ny, _currentZ);

                        for (int z = startZ; z <= maxZ; z++)
                        {
                            for (int p = 0; p < Utility.MaxZOrder; p++)
                            {
                                DrawTiles(currentScreen.GetViewportByName("ScreenGame"), z, p, mousePos);
                                DrawItems(currentScreen.GetViewportByName("ScreenGame"), z, p, mousePos);
                            }
                        }

                        if (_showGrid && _zoom.X >= 0.5f)
                        {
                            DrawTileGrid(currentScreen.GetViewportByName("ScreenGame"));
                        }

                        string size = _gameScreen.GetPanelByName("UIPanel").GetListBoxByName("TileSizeList").SelectedItemText;
                        UpdateTileAmount(size);

                        UpdateScale(1);
                        DrawHoverSprite(nx, ny);

                        for (int z = startZ; z <= maxZ; z++)
                        {
                            DrawCreatures(currentScreen.GetViewportByName("ScreenGame"), z);
                        }

                        DrawSelecting();
                        //_spriteBatch.Draw(storage.GetSpriteByName("UI_Game_Background"), new Rectangle(0, 0, cView.Width, cView.Height), Color.White);
                        //DrawGame(currentScreen.GetViewportByName("ScreenGame"));
                    }
                    currentScreen.Draw(_spriteBatch, _font, _bigFont, gameTime, _focusedTextbox);
                }
            }
            
            _storage.CurrentScreen.SetupViewport(_spriteBatch, _storage.CurrentScreen.GetViewportByName("ScreenFull").Viewport);
            StaticControlsManager.Draw(_spriteBatch, gameTime, _focusedTextbox);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private Vector2 ConvertScreenToGridLocation(Vector2 screenLocation)
        {
            Vector2 gridLocation = new Vector2();
            gridLocation.X = (int)((screenLocation.X / Coordinates.Step) + _currentLocation.X);
            gridLocation.Y = (int)((screenLocation.Y / Coordinates.Step) + _currentLocation.Y);
            return gridLocation;
        }

        private void DrawSelecting()
        {
            if (_selecting)
            {
                Matrix matrix = Matrix.CreateScale(_zoom.X, _zoom.Y, 1f);
                matrix *= Matrix.CreateRotationZ(_storage.Rotation); // Rotate the game screen for whatever reason
                _spriteBatch.End();
                _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, matrix);
                _spriteBatch.GraphicsDevice.Viewport = _screenGame.Viewport;
                int horizontalLength = GetLength_X(_drawStartPoint, _drawEndPoint);
                int verticalLength = GetLength_Y(_drawStartPoint, _drawEndPoint);

                //Vector2 gridStart = ConvertScreenToGridLocation(drawStartPoint);
                //Vector2 gridEnd = ConvertScreenToGridLocation(drawEndPoint);
                //int startX = (int)((drawStartPoint.X / (Zoom.X * 16))) + currentLocation.X;
                //int startY = (int)((drawStartPoint.Y / (Zoom.Y * 16))) + currentLocation.Y;
                //int endX = (int)((drawEndPoint.X / (Zoom.X * 16))) + currentLocation.X;
                //int endY = (int)((drawEndPoint.Y / (Zoom.Y * 16))) + currentLocation.Y;
                //_spriteBatch.DrawString(font, "startX: " + gridStart.X + " startY: " + gridStart.Y + " endX: " + gridEnd.X + " endY:" + gridEnd.Y, new Vector2(0, 0), Color.Red);

                // Draw the upper horizontal line
                Sprite.Draw(_spriteBatch, _gridSprite, new Rectangle((int)_drawStartPoint.X, (int)_drawStartPoint.Y, Math.Abs(horizontalLength), 2), Color.White);
                // Draw the lower horizontal line
                Sprite.Draw(_spriteBatch, _gridSprite, new Rectangle((int)_drawStartPoint.X, (int)_drawEndPoint.Y, Math.Abs(horizontalLength), 2), Color.White);
                // Draw the left vertical line
                Sprite.Draw(_spriteBatch, _gridSprite, new Rectangle((int)_drawStartPoint.X, (int)_drawStartPoint.Y, 2, Math.Abs(verticalLength)), Color.White);
                // Draw the right vertical line
                Sprite.Draw(_spriteBatch, _gridSprite, new Rectangle((int)_drawEndPoint.X, (int)_drawStartPoint.Y, 2, Math.Abs(verticalLength)), Color.White);
                _spriteBatch.End();
                _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            }
        }

        private int GetLength_X(Vector2 startPos, Vector2 endPos)
        {
            return (int)endPos.X - (int)startPos.X;
        }
        private int GetLength_Y(Vector2 startPos, Vector2 endPos)
        {
            return (int)endPos.Y - (int)startPos.Y;
        }

        private int GetStartZ(int curZ)
        {
            int startZ = curZ;
            if (curZ >= Utility.GroundZ)
            {
                startZ = Utility.GroundZ;
            }
            else
            {
                startZ = Utility.MinZ;
            }

            return startZ;
        }

        private void DrawHoverSprite(int nx, int ny)
        {
            ImageListItem cSprite = _lastClickedItem;

            Matrix matrix = Matrix.CreateScale(_zoom.X, _zoom.Y, 1f);
            matrix *= Matrix.CreateRotationZ(_storage.Rotation); // Rotate the game screen for whatever reason
            _spriteBatch.End();
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, matrix);
            _spriteBatch.GraphicsDevice.Viewport = _screenGame.Viewport;

            if (cSprite.Id != -1 && _zoom.X >= 0.5f)
            {
                for (int y = 0; y < _tileAmount.Height; y++)
                {
                    for (int x = 0; x < _tileAmount.Width; x++)
                    {
                        Size spriteSize = Sprite.GetSize(cSprite.Sprite);
                        Sprite.Draw(_spriteBatch, cSprite.Sprite, new Rectangle(((nx + x) - _currentLocation.X) * Coordinates.Step, ((ny + y) - _currentLocation.Y) * Coordinates.Step, spriteSize.Width, spriteSize.Height), Color.White);
                    }
                }
            }
            _spriteBatch.End();
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
        }

        private void DrawItems(EViewport eViewport, int z, int p, Coordinates mouse)
        {
            Matrix matrix = Matrix.CreateScale(_zoom.X, _zoom.Y, 1f);
            matrix *= Matrix.CreateRotationZ(_storage.Rotation); // Rotate the game screen for whatever reason
            _spriteBatch.End();
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, matrix);
            _spriteBatch.GraphicsDevice.Viewport = _screenGame.Viewport;
            Vector2 gameScale = new Vector2((float)_screenGame.Viewport.Width / (float)_screenGameWidth, (float)_screenGame.Viewport.Height / (float)_screenGameHeight);
            for (int i = 0; i < _map.Items.Count; i++)
            {
                //if (map.Tiles[i].Position.Z == z)
                if (_map.Items[i].Position.Z == z && _map.Items[i].ZOrder == p)
                {
                    if (z == _currentZ && !(_map.SamePosition(_map.Items[i].Position, mouse, true) && int.Parse(_zorder.SelectedItemText) == p))
                    {
                        if (_map.Items[i].Selected)
                        {
                            DrawScaledSprite(_map.Items[i].Sprite, new Vector2((float)((_map.Items[i].Position.X - _currentLocation.X) * Coordinates.Step), (float)((_map.Items[i].Position.Y - _currentLocation.Y) * Coordinates.Step)), gameScale, _selectColor);
                        }
                        else
                        {
                            DrawScaledSprite(_map.Items[i].Sprite, new Vector2((float)((_map.Items[i].Position.X - _currentLocation.X) * Coordinates.Step), (float)((_map.Items[i].Position.Y - _currentLocation.Y) * Coordinates.Step)), gameScale, Color.White);
                        }
                    }
                    else if(z != _currentZ && !(_map.SamePosition(_map.Items[i].Position, mouse, true) && int.Parse(_zorder.SelectedItemText) == p))
                    {
                        DrawScaledSprite(_map.Items[i].Sprite, new Vector2((float)((_map.Items[i].Position.X - _currentLocation.X) * Coordinates.Step), (float)((_map.Items[i].Position.Y - _currentLocation.Y) * Coordinates.Step)), gameScale, Color.Gray);
                    }
                }
                //DrawScaledSprite(map.Tiles[i].Sprite, new Vector2((float)((map.Tiles[i].Position.X - map.player.MoveCoordinates.X + Utility.ScreenX) * Coordinates.Step), (float)((map.Tiles[i].Position.Y - map.player.MoveCoordinates.Y + Utility.ScreenY) * Coordinates.Step)), gameScale, Color.White);
            }
        }

        private void UpdateScale(float scale)
        {
            Matrix matrix = Matrix.CreateScale(scale, scale, 1f);
            matrix *= Matrix.CreateRotationZ(_storage.Rotation); // Rotate the game screen for whatever reason
            _spriteBatch.End();
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, matrix);
            _spriteBatch.GraphicsDevice.Viewport = _screenGame.Viewport;
        }

        private void UpdateTileAmount(string size)
        {
            if (size == null || size == "1x1")
            {
                _tileAmount = new Size(1, 1);
            }
            else if (size == "2x2")
            {
                _tileAmount = new Size(2, 2);
            }
            else if (size == "4x4")
            {
                _tileAmount = new Size(4, 4);
            }
            else if (size == "8x8")
            {
                _tileAmount = new Size(8, 8);
            }
            else if (size == "16x16")
            {
                _tileAmount = new Size(16, 16);
            }
        }

        private void DrawCreatures(EViewport eViewport, int z)
        {
            Matrix matrix = Matrix.CreateScale(_zoom.X, _zoom.Y, 1f);
            matrix *= Matrix.CreateRotationZ(_storage.Rotation); // Rotate the game screen for whatever reason
            _spriteBatch.End();
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, matrix);
            _spriteBatch.GraphicsDevice.Viewport = _screenGame.Viewport;
            Vector2 gameScale = new Vector2((float)_screenGame.Viewport.Width / (float)_screenGameWidth, (float)_screenGame.Viewport.Height / (float)_screenGameHeight);
            for (int i = 0; i < _map.Creatures.Count; i++)
            {
                if (_map.Creatures[i].Position.Z == z)
                {
                    if (z == _currentZ)
                    {
                        Size spriteSize = Sprite.GetSize(_map.Creatures[i].Sprite);
                        float x = ((_map.Creatures[i].Position.X + ((spriteSize.Width / Coordinates.Step) / 2) - _currentLocation.X) * Coordinates.Step);
                        float y = ((_map.Creatures[i].Position.Y - _currentLocation.Y) * Coordinates.Step);
                        if (_map.Creatures[i].Selected)
                        {
                            DrawScaledSprite(_map.Creatures[i].Sprite, new Vector2(x, y), gameScale, _selectColor);
                        }
                        else
                        {
                            DrawScaledSprite(_map.Creatures[i].Sprite, new Vector2(x, y), gameScale, Color.White);
                        }
                    }
                    else
                    {
                        Size spriteSize = Sprite.GetSize(_map.Creatures[i].Sprite);
                        float x = ((_map.Creatures[i].Position.X + ((spriteSize.Width / Coordinates.Step) / 2) - _currentLocation.X) * Coordinates.Step);
                        float y = ((_map.Creatures[i].Position.Y - _currentLocation.Y) * Coordinates.Step);
                        DrawScaledSprite(_map.Creatures[i].Sprite, new Vector2(x, y), gameScale, Color.Gray);
                    }
                }
                //if (map.Tiles[i].Position.Z == z && map.Tiles[i].Z_order == p)
                //DrawScaledSprite(map.Tiles[i].Sprite, new Vector2((float)((map.Tiles[i].Position.X - map.player.MoveCoordinates.X + Utility.ScreenX) * Coordinates.Step), (float)((map.Tiles[i].Position.Y - map.player.MoveCoordinates.Y + Utility.ScreenY) * Coordinates.Step)), gameScale, Color.White);
            }
        }

        private void DrawTiles(EViewport eViewport, int z, int p, Coordinates mouse)
        {

            Matrix matrix = Matrix.CreateScale(_zoom.X, _zoom.Y, 1f);
            matrix *= Matrix.CreateRotationZ(_storage.Rotation); // Rotate the game screen for whatever reason
            _spriteBatch.End();
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, matrix);
            _spriteBatch.GraphicsDevice.Viewport = _screenGame.Viewport;
            Vector2 gameScale = new Vector2((float)_screenGame.Viewport.Width / (float)_screenGameWidth, (float)_screenGame.Viewport.Height / (float)_screenGameHeight);
            for (int i = 0; i < _map.Tiles.Count; i++)
            {
                //if (map.Tiles[i].Position.Z == z)
                if (_map.Tiles[i].Position.Z == z && _map.Tiles[i].ZOrder == p)
                {
                    if (z == _currentZ && !HoverTiles(p, mouse, i))
                    {
                        if (_map.Tiles[i].Selected)
                        {
                            DrawScaledSprite(_map.Tiles[i].Sprite, new Vector2((float)((_map.Tiles[i].Position.X - _currentLocation.X) * Coordinates.Step), (float)((_map.Tiles[i].Position.Y - _currentLocation.Y) * Coordinates.Step)), gameScale, _selectColor);
                        }
                        else
                        {
                            DrawScaledSprite(_map.Tiles[i].Sprite, new Vector2((float)((_map.Tiles[i].Position.X - _currentLocation.X) * Coordinates.Step), (float)((_map.Tiles[i].Position.Y - _currentLocation.Y) * Coordinates.Step)), gameScale, Color.White);
                        }
                    }
                    else if (z != _currentZ && !(_map.SamePosition(_map.Tiles[i].Position, mouse, true) && int.Parse(_zorder.SelectedItemText) == p))
                    {
                        DrawScaledSprite(_map.Tiles[i].Sprite, new Vector2((float)((_map.Tiles[i].Position.X - _currentLocation.X) * Coordinates.Step), (float)((_map.Tiles[i].Position.Y - _currentLocation.Y) * Coordinates.Step)), gameScale, Color.Gray);
                    }
                }
                //DrawScaledSprite(map.Tiles[i].Sprite, new Vector2((float)((map.Tiles[i].Position.X - map.player.MoveCoordinates.X + Utility.ScreenX) * Coordinates.Step), (float)((map.Tiles[i].Position.Y - map.player.MoveCoordinates.Y + Utility.ScreenY) * Coordinates.Step)), gameScale, Color.White);
            }

        }

        private bool HoverTiles(int p, Coordinates mouse, int i)
        {
            bool hoverTile = false;
            if (_currentAction != MapAction.Select)
            {
                Coordinates oldPos = _map.Tiles[i].Position;
                for (int y = 0; y < _tileAmount.Height; y++)
                {
                    for (int x = 0; x < _tileAmount.Width; x++)
                    {

                        if (_map.SamePosition(new Coordinates(oldPos.X - x, oldPos.Y - y, oldPos.Z), mouse, true) && int.Parse(_zorder.SelectedItemText) == p)
                        {
                            hoverTile = true;
                        }
                    }
                }
            }
            return hoverTile;
        }

        private void DrawScaledSprite(SpriteObject sprite, Vector2 position, Vector2 scale, Color color)
        {
            float rotation = 0f;
            //_spriteBatch.Draw(Sprite, new Vector2(), null, Color.White, rotation, Vector2.Zero, Scale, SpriteEffects.None, 0f);

            Rectangle destinationRectangle = new Rectangle();

            if (!sprite.LoadFromSpritesheet)
            {
                destinationRectangle.X = (int)(scale.X * position.X - (sprite.Sprite.Width - Coordinates.Step));
                destinationRectangle.Y = (int)(scale.Y * position.Y - (sprite.Sprite.Height - Coordinates.Step));
                destinationRectangle.Width = sprite.Sprite.Width;
                destinationRectangle.Height = sprite.Sprite.Height;

                _spriteBatch.Draw(sprite.Sprite, destinationRectangle, null, color, rotation, Vector2.Zero, SpriteEffects.None, 0f);
            }
            else
            {
                //destinationRectangle.X = (int)(Scale.X * Position.X - (Sprite.SpritesheetPosition.Width - Coordinates.Step));
                //destinationRectangle.Y = (int)(Scale.Y * Position.Y - (Sprite.SpritesheetPosition.Height - Coordinates.Step));
                destinationRectangle.X = (int)(scale.X * position.X - (sprite.SpritesheetPosition.Width - Coordinates.Step));
                destinationRectangle.Y = (int)(scale.Y * position.Y - (sprite.SpritesheetPosition.Height - Coordinates.Step));
                destinationRectangle.Width = sprite.SpritesheetPosition.Width;
                destinationRectangle.Height = sprite.SpritesheetPosition.Height;

                Texture2D spritesheet = Storage.Instance.GetSpritesheetById(sprite.SpritesheetId, sprite.Category).Texture;
                _spriteBatch.Draw(spritesheet, destinationRectangle, sprite.SpritesheetPosition, color, rotation, Vector2.Zero, SpriteEffects.None, 0f);
            }
        }

        private void DrawTileGrid(EViewport eViewport)
        {
            Matrix matrix = Matrix.CreateScale(_zoom.X * 2, _zoom.Y * 2, 1f);
            matrix *= Matrix.CreateRotationZ(_storage.Rotation); // Rotate the game screen for whatever reason
            _spriteBatch.End();
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, matrix);
            _spriteBatch.GraphicsDevice.Viewport = _screenGame.Viewport;
            Vector2 gameScale = new Vector2((float)_screenGame.Viewport.Width / (float)_screenGameWidth, (float)_screenGame.Viewport.Height / (float)_screenGameHeight);
            for (int y = 0; y < ((_screenGame.Viewport.Height) / _pixelGridSize) / _zoom.X; y++)
            {
                for(int x = 0; x < ((_screenGame.Viewport.Width) / _pixelGridSize) / _zoom.Y; x++)
                {
                    Sprite.Draw(_spriteBatch, _gridSprite, new Rectangle((x * _pixelGridSize) - 1, 0, 1, _screenGame.Viewport.Height), Color.White);
                }
                Sprite.Draw(_spriteBatch, _gridSprite, new Rectangle(0, (y * _pixelGridSize) - 1, _screenGame.Viewport.Width, 1), Color.White);
            }
            _spriteBatch.End();
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

        }
    }
}
