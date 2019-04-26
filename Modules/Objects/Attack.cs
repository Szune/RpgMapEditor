using System.Collections.Generic;

namespace RpgMapEditor.Modules.Objects
{
    public class Attack
    {
        public int Damage;
        public double[] Area;
        public int AreaCount;
        public double[] SpriteId;
        public int AnimationCount;
        public int Id;
        public int ManaCost;
        public string Name;
        public const int ExhaustionTime = 500; // Milliseconds
        public bool HealSpell;
        public bool TargetSpell;
        public bool AimSpell;
        public bool AreaSpell;
        public int Cooldown;
        public int MinDamage;
        public bool Animated;

        dynamic _lua = new DynamicLua.DynamicLua();
        string _pathToLua;

        public Attack()
        {
            Id = -1;
        }

        public Attack(string path)
        {
            LoadScript(path);
        }

        public void LoadScript(string path)
        {
            _pathToLua = path;
            _lua.DoFile(path);
            Name = (string)_lua.Name;
            if (_lua.Spell_Area == true)
            {
                AreaSpell = true;
                DynamicLua.DynamicLuaTable tab = _lua.Area;

                int i = 0;
                foreach (KeyValuePair<object, object> kvp in tab)
                {
                    i++;
                }

                Area = new double[i];
                AreaCount = i;
                i = 0;
                foreach (KeyValuePair<object, object> kvp in tab)
                {
                    Area[i] = (double)kvp.Value;
                    i++;
                }
            }
            AimSpell = _lua.Spell_Aim;
            Damage = (int)_lua.Damage;

            Animated = _lua.Animated;
            DynamicLua.DynamicLuaTable tabSprite = _lua.SpriteID;

            int j = 0;
            foreach (KeyValuePair<object, object> kvp in tabSprite)
            {
                j++;
            }

            SpriteId = new double[j];

            j = 0;
            foreach (KeyValuePair<object, object> kvp in tabSprite)
            {
                SpriteId[j] = (double)kvp.Value;
               j++;
            }

            AnimationCount = j;

            //SpriteID = (int)lua.SpriteID;
            ManaCost = (int)_lua.Mana_Cost;
            HealSpell = _lua.Spell_Heal;
            TargetSpell = _lua.Spell_RequireTarget;
            //MinDamage = (int)lua.minDamage;
            Id = (int)_lua.ID;
            Cooldown = (int)_lua.Cooldown;
        }

        public Attack(double[] area, int damage, double[] sprite, int manacost = 0, bool healspell = false, bool targetspell = false, int id = 0)
        {
            Area = area;
            Damage = damage;
            SpriteId = sprite;
            Id = id;
            ManaCost = manacost;
            HealSpell = healspell;
            TargetSpell = targetspell;
        }
    }
}