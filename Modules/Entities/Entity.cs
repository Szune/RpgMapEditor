using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor.Modules.Entities
{
    public class Entity
    {
        public string Name { get; set; }
        public bool Selected { get; set; }

        public int Id { get; set; }

        public bool Visible { get; set; }

        public Coordinates Position { get; set; }

        public int Experience { get; set; }

        public int SpriteId { get; set; }

        public EntityType Type { get; set; }
    }

    public enum EntityType
    {
        CreatureEntity,
        PlayerEntity,
        TileEntity,
        ItemEntity,
        UnknownEntity,
        SpellEntity,
        UiEntity
    }
}
