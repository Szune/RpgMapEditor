using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgMapEditor.Modules.Controls.NonStaticControls;
using RpgMapEditor.Modules.Objects;

namespace RpgMapEditor.Modules.Utilities
{
    public sealed class Storage
    {
        internal List<SpriteObject> SpriteList = new List<SpriteObject>();

        internal List<Screen> ScreenList = new List<Screen>();

        internal Dictionary<string, dynamic> GlobalVars = new Dictionary<string, dynamic>();

        internal SpriteFont Font;
        internal SpriteFont SmallFont;
        internal SpriteFont BigFont;

        internal List<SpriteCollection> SpriteCollections = new List<SpriteCollection>();

        internal float Rotation = 0f;

        internal bool ComboBoxOpen = false;

        internal Screen CurrentScreen;

        internal List<Spritesheet> Spritesheets = new List<Spritesheet>();
        internal Spritesheet SpritesheetUi => Instance.GetSpritesheetByName("Spritesheet_UI", SpriteCategory.Ui);

        private Storage()
        {

        }

        public static Storage Instance { get; } = new Storage();

        internal void AddSprite(int id, SpriteCategory category, string spriteName, Rectangle spritesheetPosition, int spritesheetId, string randomizeCategory = null, bool walkthrough = true, string spriteCollection = null, bool show = true, string tileCategory = null)
        {
            SpriteList.Add(new SpriteObject(id, category, spriteName, walkthrough, spritesheetPosition, spritesheetId, randomizeCategory, spriteCollection, show, tileCategory));
        }

        internal void AddSprite(Texture2D sprite, int id, SpriteCategory category, string name, bool walkthrough = true, string randomizeCategory = null, string spriteCollection = null, bool show = true, string tileCategory = null)
        {
            SpriteList.Add(new SpriteObject(sprite, id, category, name, walkthrough, randomizeCategory, spriteCollection, show, tileCategory));
        }

        internal void AddSpritesheet(int id, string name, Texture2D spritesheet, SpriteCategory category)
        {
            Spritesheets.Add(new Spritesheet(id, name, spritesheet, category));
        }

        internal Screen AddScreen(int id, string name, bool visible)
        {
            ScreenList.Add(new Screen(id, name, visible));
            Screen getScreen = ScreenList[ScreenList.Count - 1];
            return getScreen;
        }

        internal Spritesheet GetSpritesheetById(int id, SpriteCategory category)
        {
            for (int i = 0; i < Spritesheets.Count; i++)
            {
                if (Spritesheets[i].Id == id && Spritesheets[i].Category == category)
                {
                    return Spritesheets[i];
                }

            }
            return new Spritesheet();
        }

        internal Spritesheet GetSpritesheetByName(string name, SpriteCategory category)
        {
            for (int i = 0; i < Spritesheets.Count; i++)
            {
                if (Spritesheets[i].Name.ToLower() == name.ToLower() && Spritesheets[i].Category == category)
                {
                    return Spritesheets[i];
                }

            }
            return new Spritesheet();
        }

        internal SpriteObject GetSpriteById(int id)
        {
            for (int i = 0; i < SpriteList.Count; i++)
            {
                if (SpriteList[i].Id == id)
                {
                    return SpriteList[i];
                }

            }
            return new SpriteObject();
        }

        internal SpriteObject GetSpriteByName(string name)
        {
            for (int i = 0; i < SpriteList.Count; i++)
            {
                if (SpriteList[i].SpriteName.ToLower() == name.ToLower())
                {
                    return SpriteList[i];
                }

            }
            return new SpriteObject();
        }

        internal int GetSpriteIdByName(string name)
        {
            for (int i = 0; i < SpriteList.Count; i++)
            {
                if (SpriteList[i].SpriteName.ToLower() == name.ToLower())
                {
                    return SpriteList[i].Id;
                }

            }
            return SpriteList[0].Id;
        }

        internal SpriteObject GetSpriteObjectById(int id)
        {
            for (int i = 0; i < SpriteList.Count; i++)
            {
                if (SpriteList[i].Id == id)
                {
                    return SpriteList[i];
                }

            }
            return SpriteList[0];
        }

        internal Screen GetScreenByName(string name)
        {
            for (int i = 0; i < ScreenList.Count; i++)
            {
                if (ScreenList[i].Name.ToLower() == name.ToLower())
                {
                    return ScreenList[i];
                }
            }
            return new Screen();
        }

        internal Screen GetScreenById(int id)
        {
            for (int i = 0; i < ScreenList.Count; i++)
            {
                if (ScreenList[i].Id == id)
                {
                    return ScreenList[i];
                }
            }
            return new Screen();
        }

        // Store sprites and stuff like that here
    }
}