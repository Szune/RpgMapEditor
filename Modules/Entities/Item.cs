using RpgMapEditor.Modules.Objects;
using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor.Modules.Entities
{
    public class Item : Entity
    {

        public int Strength;
        public int Defense;
        public ItemSlot Slot = ItemSlot.None;
        public ItemSlot WearSlot;
        public int WearingPlayerId;
        public Backpack Container;
        public Backpack Parent;
        public SpriteObject Sprite;
        public int ItemId = -1;
        public int ParentId = -1;
        public int ParentItemId = -1;
        public bool IsEmpty = true;
        public int BagSlot = -1;
        public int ZOrder = 0;
        public int RealId = 0;

        public bool WasReplaced;

        public Item()
        {
            Id = -1;
            Strength = 0;
            Defense = 0;
            ZOrder = -1;
        }
        public Item(SpriteObject sprite, string name, Coordinates pos, int zorder = 0, int id = 0, int strength = 0, int defense = 0, bool visible = true, int wearingplayerid = 0)
        {
            Sprite = sprite;
            ItemId = sprite.Id;
            Name = name;
            Id = sprite.Id;
            Position = pos;
            Visible = visible;
            Type = EntityType.ItemEntity;
            SpriteId = sprite.Id;
            Strength = strength;
            Defense = defense;
            WearingPlayerId = wearingplayerid;
            ZOrder = zorder;
            Parent = new Backpack();
        }

        public Item(SpriteObject sprite, string name, ItemSlot wearslot, Coordinates pos, int spriteid, int id = 0, int strength = 0, int defense = 0, bool visible = true, int wearingplayerid = 0, int zorder = 0)
        {
            Sprite = sprite;
            ItemId = spriteid;
            Name = name;
            Id = id;
            Position = pos;
            Visible = visible;
            Type = EntityType.ItemEntity;
            SpriteId = spriteid;
            Strength = strength;
            Defense = defense;
            WearSlot = wearslot;
            WearingPlayerId = wearingplayerid;
            if (wearslot == ItemSlot.Bag)
            {
                Container = new Backpack();
                Container.Id = this.Id;
                Container.ItemId = spriteid;
            }
            ZOrder = zorder;
            Parent = new Backpack();
        }
    }
}
