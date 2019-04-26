using System.Collections.Generic;
using RpgMapEditor.Modules.Objects;
using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor.Modules.Entities
{
    public class Creature : Entity
    {
        public int Health { get; set; }
        public int MaxHealth { get; set; }   
        public int Level { get; set; }

        public int Speed = 250;

        public int RealId { get; set; }

        public AnimationCoordinates MoveCoordinates = new AnimationCoordinates();

        public int MagicStrength { get; set; }
        public int Strength { get; set; }
        public int Defense { get; set; }

        public string WalkDirection { get; set; }
        public string WalkState { get; set; }
        public bool WasReplaced;

        public List<Attack> AttackList = new List<Attack>();

        public EQueue<Movement> MovementAnimation = new EQueue<Movement>();

        public SpriteObject Sprite;


        public Creature()
        {
            Id = -1;
        }

        public Creature(string name, SpriteObject sprite, Coordinates coordinates, int spriteid = 0, int magicstr = 0, int strength = 1, int health = 1, int id = 0, int defense = 0, int experience = 1, List<Attack> attacks = null, int realId = -1, bool wasReplaced = false)
        {
            Sprite = sprite;
            Name = name;
            Position = coordinates;
            MaxHealth = health;
            Health = health;
            Id = id;
            Visible = true;
            Type = EntityType.CreatureEntity;
            Strength = strength;
            Defense = defense;
            Experience = experience;
            MagicStrength = magicstr;
            WalkState = "Standing";
            WalkDirection = "S";
            if (attacks != null)
            {
                AttackList.AddRange(attacks);
            }
            if (sprite != null)
            {
                SpriteId = sprite.Id;
            }
            else
            {
                SpriteId = -1;
            }
            RealId = realId;
            WasReplaced = wasReplaced;
        }

        public void Attack(int damage)
        {
            Health -= damage;
            if(Health < 0)
            {
                Health = 0;
            }
            if(Health > MaxHealth)
            {
                Health = MaxHealth;
            }
        }
    }
}
