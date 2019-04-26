using System.Collections.Generic;
using System.IO;
using RpgMapEditor.Modules.Controls.StaticControls;
using RpgMapEditor.Modules.Entities;
using RpgMapEditor.Modules.Objects;
using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor
{
    public sealed class Map
    {
        private readonly Storage _storage = Storage.Instance;
        internal List<Tile> Tiles = new List<Tile>();
        internal List<Creature> Creatures = new List<Creature>();
        internal List<Creature> CreatureList = new List<Creature>();
        internal List<MapTile> NTiles = new List<MapTile>();
        internal List<Tile> TileList = new List<Tile>();
        internal List<Attack> Attacks = new List<Attack>();
        internal List<Item> Items = new List<Item>();

        internal bool WalkBlocked = false;

        internal string MapFileDir = "Content\\";
        internal string MapFileName = "Game.map";
        internal string MapFilePath = "Content\\Game.map";

        internal Player Player = new Player("Player");

        private Map() { }

        public static Map Instance { get; } = new Map();

        public int TimeOfLastMovement = -1;

        internal void InitializeMap()
        {
            for (int i = 0; i < Utility.MaxZ + 1; i++)
            {
                NTiles.Add(new MapTile()); // adds each floor to the map
            }
        }

        internal void Move(Direction dir, double gameTime)
        {
            if (dir != Direction.None && !WalkBlocked)
            {
                Coordinates destination = new Coordinates();
                if (dir == Direction.East)
                {
                    destination = new Coordinates(Player.Position.X + 1, Player.Position.Y, Player.Position.Z);
                }
                else if (dir == Direction.West)
                {
                    destination = new Coordinates(Player.Position.X - 1, Player.Position.Y, Player.Position.Z);
                }
                else if (dir == Direction.South)
                {
                    destination = new Coordinates(Player.Position.X, Player.Position.Y + 1, Player.Position.Z);
                }
                else if (dir == Direction.North)
                {
                    destination = new Coordinates(Player.Position.X, Player.Position.Y - 1, Player.Position.Z);
                }

                if (IsTileWalkable(destination))
                {
                    Animation newAnimation = new Animation(gameTime, gameTime + Player.Speed, 0.1);
                    Player.MovementAnimation.Add(new Movement(dir, destination, newAnimation));
                    TimeOfLastMovement = (int)gameTime;
                }
            }
        }

        internal Attack GetAttackById(int id)
        {
            Attack tmp = new Attack();
            foreach(Attack a in Attacks)
            {
                if(a.Id == id)
                {
                    tmp = a;
                }
            }
            return tmp;
        }

        internal Creature GetCreatureFromTile(Coordinates tile)
        {
            Creature tileC = new Creature();
            for (int i = 0; i < Creatures.Count; i++)
            {
                if (SamePosition(tile, Creatures[i].Position))
                {
                    return Creatures[i];
                }
            }

            return tileC;
        }

        internal bool IsTileWalkable(Coordinates tile)
        {
            bool tileWalkable = true;

            /*if (IsTile(Tile))
            {
                Tile cTile = GetTile(Tile);
                tileWalkable = !cTile.Walkable || cTile.ExternalThingsBlockingTile < 1;
            }
            else
            {
                tileWalkable = false;
            }*/

            tileWalkable = GetTileWalkable(tile);
            /*if(IsTile(Tile))
            {
                tileWalkable = GetTile(Tile).Walkable;
            }
            else
            {
                return false;
            }*/

            if (tileWalkable) { tileWalkable = !IsTileCreature(tile); }

            /*            if (tileWalkable) { tileWalkable = !IsTilePlayer(Tile); }
                        if (tileWalkable) { tileWalkable = !IsTileCreature(Tile); }
                        if (tileWalkable) { tileWalkable = !IsTileNPC(Tile); }
                        if (tileWalkable) { tileWalkable = !OutOfBoundaries(Tile); }
                        */
            return tileWalkable;
        }

        internal bool IsTileCreature(Coordinates tile)
        {
            bool tileCreature = false;
            for (int i = 0; i < Creatures.Count; i++)
            {
                if (SamePosition(tile, Creatures[i].Position) && Creatures[i].Health > 0)
                {
                    tileCreature = true;
                    break;
                }
            }
            return tileCreature;
        }

        internal Creature CreateCreatureFromCreatureList(int id, Coordinates pos = null)
        {
            Creature newCreature;
            if (pos == null) { pos = new Coordinates(0, 0, 0); }
            for (int i = 0; i < CreatureList.Count; i++)
            {
                if (CreatureList[i].Id == id)
                {
                    int count = Creatures.Count;
                    newCreature = new Creature(CreatureList[i].Name, CreatureList[i].Sprite, pos, CreatureList[i].SpriteId, CreatureList[i].MagicStrength, CreatureList[i].Strength, CreatureList[i].Health, count, CreatureList[i].Defense, CreatureList[i].Experience, CreatureList[i].AttackList, CreatureList[i].Id);
                    Creatures.Add(newCreature);

                    AddBlocking(pos);

                    return newCreature;
                }
            }
            return new Creature();
        }

        internal void DoMove(double gameTime)
        {
            if (Player.MovementAnimation.Count > 0)
            {
                Movement cMovement = Player.MovementAnimation.Peek();
                Direction dir = cMovement.MoveDirection;
                if (cMovement.Destination.Y != -1 && cMovement.Destination.X != -1)
                {
                    double move = cMovement.CurrentAnimation.GetStep(gameTime);
                    double cMove = cMovement.CurrentAnimation.CurrentStep * cMovement.CurrentAnimation.OneStep;
                    if (move > 0.00)
                    {
                        if (dir == Direction.East)
                        {
                            Player.MoveCoordinates = new AnimationCoordinates(Player.MoveCoordinates.X + move, Player.MoveCoordinates.Y, Player.MoveCoordinates.Z);
                            Player.WalkDirection = "E";
                        }
                        else if (dir == Direction.West)
                        {
                            Player.MoveCoordinates = new AnimationCoordinates(Player.MoveCoordinates.X - move, Player.MoveCoordinates.Y, Player.MoveCoordinates.Z);
                            Player.WalkDirection = "W";
                        }
                        else if (dir == Direction.South)
                        {
                            Player.MoveCoordinates = new AnimationCoordinates(Player.MoveCoordinates.X, Player.MoveCoordinates.Y + move, Player.MoveCoordinates.Z);
                            Player.WalkDirection = "S";
                        }
                        else if (dir == Direction.North)
                        {
                            Player.MoveCoordinates = new AnimationCoordinates(Player.MoveCoordinates.X, Player.MoveCoordinates.Y - move, Player.MoveCoordinates.Z);
                            Player.WalkDirection = "N";
                        }
                    }

                    if (cMove <= 0.4)
                    {
                        Player.WalkState = "Walking1";
                    }
                    else if (cMove >= 0.5)
                    {
                        Player.WalkState = "Walking2";
                    }

                    if (cMovement.CurrentAnimation.Finished)
                    {
                        if (!SamePosition(new Coordinates((int)Player.MoveCoordinates.X, (int)Player.MoveCoordinates.Y, (int)Player.MoveCoordinates.Z), cMovement.Destination))
                        {
                            Player.MoveCoordinates = new AnimationCoordinates(cMovement.Destination.X, cMovement.Destination.Y, cMovement.Destination.Z);
                        }
                        Player.Position = new Coordinates(cMovement.Destination.X, cMovement.Destination.Y, cMovement.Destination.Z);
                        Player.WalkState = "Standing";
                        Player.MovementAnimation.Pop();
                    }
                }
                else
                {
                    Player.MovementAnimation.Pop();
                }
            }
            /*
            foreach (Creature c in Creatures)
            {
                if (c.MovementAnimation.Count > 0)
                {
                    Movement cMov = c.MovementAnimation.Peek();
                    Direction dir = cMov.MoveDirection;
                    if (cMov.Destination.Y != -1)
                    {
                        double move = cMov.CurrentAnimation.GetStep(ElapsedMilliseconds);
                        if (move > 0.00)
                        {
                            if (dir == Direction.East)
                            {
                                c.Position = new Coordinates(c.Position.X + move, c.Position.Y, c.Position.Z);
                            }
                            else if (dir == Direction.West)
                            {
                                c.Position = new Coordinates(c.Position.X - move, c.Position.Y, c.Position.Z);
                            }
                            else if (dir == Direction.South)
                            {
                                c.Position = new Coordinates(c.Position.X, c.Position.Y + move, c.Position.Z);
                            }
                            else if (dir == Direction.North)
                            {
                                c.Position = new Coordinates(c.Position.X, c.Position.Y - move, c.Position.Z);
                            }
                        }
                    }
                    if (cMov.CurrentAnimation.Finished)
                    {
                        if (!SamePosition(c.Position, cMov.Destination))
                        {
                            c.Position = new Coordinates(cMov.Destination.X, cMov.Destination.Y, cMov.Destination.Z);
                        }
                        c.MovementAnimation.Pop();
                    }
                }
            }*/
        }

        internal void MoveBlocking(Coordinates source, Coordinates destination)
        {
            RemoveBlocking(source);

            AddBlocking(destination);
        }

        internal void AddBlocking(Coordinates tile)
        {
            if (IsTile(tile))
            {
                GetTile(tile).ExternalThingsBlockingTile++;
            }
        }

        internal void RemoveBlocking(Coordinates tile)
        {
            if (IsTile(tile))
            {
                GetTile(tile).ExternalThingsBlockingTile--;
            }
        }

        internal Tile GetTile(Coordinates tile)
        {
            return NTiles[tile.Z].Tiles[tile.X, tile.Y];
        }

        internal bool IsTile(Coordinates tile)
        {
            // TODO: Make IsTile() check if the tile is out of the bounds of the array.
            if (tile.X > -1 && tile.X < Utility.MaxX && tile.Y > -1 && tile.Y < Utility.MaxY && tile.Z > -1 && tile.Z <= Utility.MaxZ)
            {
                return (NTiles[tile.Z].Tiles[tile.X, tile.Y] != null);
            }
            return false;
        }

        internal bool GetTileWalkable(Coordinates tile)
        {
            List<Tile> tiles = GetTilesFromPos(tile);

            if (tiles.Count < 1)
            {
                return false;
            }

            foreach(Tile t in tiles)
            {
                if(!t.Walkable)
                {
                    MessageBox.Show(t.SpriteId.ToString());
                    return false;
                }
            }

            return true;
            /*if (IsTile(Tile))
            {
                return GetTile(Tile).Walkable;
            }
            return true;*/
        }

        internal int GetTileIdFromTile(Coordinates tile)
        {
            int tileId = -1;
            if (IsTile(tile))
            {
                return GetTile(tile).Id;
            }
            return tileId;

        }

        internal Tile AddTile(SpriteObject sprite, Coordinates pos, int zorder = 0)
        {
            var newTile = new Tile();
            Tile returnTile;
            if (pos == null) { pos = new Coordinates(0, 0, 0); }
            if (sprite == null) { return newTile; }

            newTile = new Tile(sprite.SpriteName, sprite, pos, Tiles.Count, true, sprite.Walkthrough, zorder);
            if (!ContainsTile(GetTilesFromPos(pos), newTile))
            {
                Tiles.Add(newTile);
                returnTile = newTile;
            }
            else
            {
                var t = ReplaceTile(GetTilesFromPos(pos), newTile, zorder);
                t.WasReplaced = true;
                returnTile = t;
            }

            return returnTile;
        }

        internal Creature AddCreature(SpriteObject sprite, Coordinates pos)
        {
            Creature newCreature = new Creature();
            Creature returnCreature = new Creature();
            if (pos == null) { pos = new Coordinates(0, 0, 0); }

            newCreature = new Creature(sprite.SpriteName, sprite, pos);
            if (!ContainsCreature(pos))
            {
                Creatures.Add(newCreature);
                returnCreature = newCreature;
            }
            else
            {
                Creature c = ReplaceCreature(pos, newCreature);
                c.WasReplaced = true;
                returnCreature = c;
            }

            return returnCreature;
        }

        internal Item AddItem(SpriteObject sprite, Coordinates pos, int zorder = 0)
        {
            Item newItem = new Item();
            Item returnItem = new Item();
            if (pos == null) { pos = new Coordinates(0, 0, 0); }

            newItem = new Item(sprite, sprite.SpriteName, pos, zorder);
            if (!ContainsItem(pos, zorder))
            {
                Items.Add(newItem);
                returnItem = newItem;
            }
            else
            {
                Item i = ReplaceItem(pos, newItem);
                i.WasReplaced = true;
                returnItem = i;
            }

            return returnItem;
        }

        private Item ReplaceItem(Coordinates pos, Item newItem)
        {
            Item replacedItem = RemoveItem(pos, newItem.ZOrder);
            Items.Add(newItem);

            return replacedItem;
        }

        internal Item RemoveItem(Coordinates pos, int zorder)
        {
            Item removedItem = new Item();
            for (int i = 0; i < Items.Count; i++)
            {
                if (SamePosition(Items[i].Position, pos) && Items[i].ZOrder == zorder) // && tileList[i].SpriteID == tile.SpriteID)
                {
                    Item item = Items[i];
                    removedItem = new Item(item.Sprite, item.Name, item.Position, item.ZOrder);
                    Items.Remove(item);
                }
            }
            return removedItem;
        }

        internal Item RemoveTopItem(Coordinates pos)
        {
            Item topItem = GetTopItem(pos);
            Item removedItem = new Item();

            if(topItem.Id != -1)
            {
                removedItem = new Item(topItem.Sprite, topItem.Name, topItem.Position, topItem.ZOrder);
                Items.Remove(topItem);
            }

            return removedItem;
        }

        internal bool ContainsItem(Coordinates pos, int zorder)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (SamePosition(Items[i].Position, pos) && Items[i].ZOrder == zorder) // && tileList[i].SpriteID == tile.SpriteID)
                {
                    return true;
                }
            }
            return false;
        }

        private Creature ReplaceCreature(Coordinates pos, Creature newC)
        {
            Creature replacedCreature = RemoveCreature(pos);
            Creatures.Add(newC);

            return replacedCreature;
        }

        internal Creature RemoveCreature(Coordinates pos)
        {
            Creature removedCreature = new Creature();
            for (int i = 0; i < Creatures.Count; i++)
            {
                if (SamePosition(Creatures[i].Position, pos)) // && tileList[i].SpriteID == tile.SpriteID)
                {
                    Creature c = Creatures[i];
                    removedCreature = new Creature(c.Name, c.Sprite, c.Position, c.SpriteId);
                    Creatures.Remove(c);
                }
            }
            return removedCreature;
        }

        internal bool ContainsCreature(Coordinates pos)
        {
            for (int i = 0; i < Creatures.Count; i++)
            {
                if (SamePosition(Creatures[i].Position, pos)) // && tileList[i].SpriteID == tile.SpriteID)
                {
                    return true;
                }
            }
            return false;
        }

        internal void CreateTileFromTileList(int spriteId, Coordinates pos = null, int zorder = 0)
        {
            Tile newTile;
            if (pos == null) { pos = new Coordinates(0, 0, 0); }
            SpriteObject spr = _storage.GetSpriteById(spriteId);
            newTile = new Tile(spr.SpriteName, spr, pos, Tiles.Count, true, spr.Walkthrough, zorder);

            if (!ContainsTile(GetTilesFromPos(pos), newTile))
            {
                Tiles.Add(newTile);
            }
            else
            {
                ReplaceTile(GetTilesFromPos(pos), newTile, zorder);
            }
        }

        private Tile ReplaceTile(List<Tile> tileList, Tile tile, int zorder)
        {
            Tile replacedTile = RemoveTile(tileList, tile.Position, zorder);
            Tiles.Add(tile);

            return replacedTile;
        }

        internal Tile RemoveTile(List<Tile> tileList, Coordinates tile, int zorder)
        {
            Tile removedTile = new Tile();
            for (int i = 0; i < tileList.Count; i++)
            {
                if (SamePosition(tileList[i].Position, tile) && tileList[i].ZOrder == zorder) // && tileList[i].SpriteID == tile.SpriteID)
                {
                    Tile r = tileList[i];
                    removedTile = new Tile(r.Name, r.Sprite, r.Position, r.Id, r.Visible, r.Walkable, r.ZOrder);
                    Tiles.Remove(r);
                }
            }

            return removedTile;
        }

        internal Tile RemoveTopTile(Coordinates tile)
        {
            Tile removedTile = new Tile();
            Tile r = GetTopTile(tile);
            if (r.Id != -1)
            {
                removedTile = new Tile(r.Name, r.Sprite, r.Position, r.Id, r.Visible, r.Walkable, r.ZOrder);
                Tiles.Remove(r);
            }

            return removedTile;
        }

        public void ClearMap()
        {
            Tiles.Clear();
        }

        public bool LoadMap()
        {
            string mapn = "Content\\Game.map";
            /*int SpawnX = 0;
            int SpawnY = 0;*/
            FileInfo file = new FileInfo(mapn);

            if (file.Exists)
            {
                using (var read = new StreamReader(mapn))
                {
                    Tiles.Clear();

                    /* Load Tiles */

                    string[] coordinateList = read.ReadLine()?.Split(",".ToCharArray());
                    if (coordinateList == null)
                        return false;
                    for (int i = 0; i < coordinateList.Length; i++)
                    {
                        string[] coordinates = coordinateList[i].Split("|".ToCharArray());
                        if (coordinates[0].Length < 1)
                        {
                            read.Close();
                            return false;
                        }

                        int x = int.Parse(coordinates[0]);
                        int y = int.Parse(coordinates[1]);
                        int z = int.Parse(coordinates[2]);
                        int spriteId = int.Parse(coordinates[3]);
                        int zOrder = int.Parse(coordinates[4]);

                        AddTile(_storage.GetSpriteById(spriteId), new Coordinates(x, y, z), zOrder);
                    }

                    Creatures.Clear();

                    /* Load Creatures */

                    if (!read.EndOfStream)
                    {
                        coordinateList = read.ReadLine()?.Split(",".ToCharArray());
                        if (coordinateList == null)
                            return false;
                        if (coordinateList[0] != string.Empty)
                        {
                            for (int i = 0; i < coordinateList.Length; i++)
                            {
                                string[] coordinates = coordinateList[i].Split("|".ToCharArray());
                                int x = int.Parse(coordinates[0]);
                                int y = int.Parse(coordinates[1]);
                                int z = int.Parse(coordinates[2]);
                                int spriteId = int.Parse(coordinates[3]);

                                AddCreature(_storage.GetSpriteById(spriteId), new Coordinates(x, y, z));
                            }
                        }
                    }

                    Items.Clear();

                    /* Load Items */

                    if (!read.EndOfStream)
                    {
                        coordinateList = read.ReadLine()?.Split(",".ToCharArray());
                        if (coordinateList == null)
                            return false;
                        if (coordinateList[0] != string.Empty)
                        {
                            for (int i = 0; i < coordinateList.Length; i++)
                            {
                                string[] coordinates = coordinateList[i].Split("|".ToCharArray());
                                var x = int.Parse(coordinates[0]);
                                var y = int.Parse(coordinates[1]);
                                var z = int.Parse(coordinates[2]);
                                var spriteId = int.Parse(coordinates[3]);
                                var zOrder = int.Parse(coordinates[4]);

                                AddItem(_storage.GetSpriteById(spriteId), new Coordinates(x, y, z), zOrder);
                            }
                        }
                    }

                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SaveMap(bool overwrite)
        {
            bool canSave = false;
            if(overwrite)
            {
                canSave = true;
            }
            else
            {
                FileInfo f = new FileInfo(MapFilePath);
                if (!f.Exists)
                {
                    canSave = true;
                }
            }

            if (canSave)
            {
                string saveString = "";
                for (int i = 0; i < Tiles.Count; i++)
                {
                    if (i == 0)
                        saveString = Tiles[i].Position.X + "|" + Tiles[i].Position.Y + "|" + Tiles[i].Position.Z + "|" + Tiles[i].SpriteId + "|" + Tiles[i].ZOrder;
                    else
                        saveString += "," + Tiles[i].Position.X + "|" + Tiles[i].Position.Y + "|" + Tiles[i].Position.Z + "|" + Tiles[i].SpriteId + "|" + Tiles[i].ZOrder;
                }

                using (var writer = new StreamWriter(MapFilePath))
                {
                    writer.WriteLine(saveString);

                    saveString = "";

                    for (int i = 0; i < Creatures.Count; i++)
                    {
                        if (i == 0)
                        {
                            saveString = Creatures[i].Position.X + "|" + Creatures[i].Position.Y + "|" +
                                         Creatures[i].Position.Z + "|" + Creatures[i].Sprite.Id;
                        }
                        else
                        {
                            saveString += "," + Creatures[i].Position.X + "|" + Creatures[i].Position.Y + "|" +
                                          Creatures[i].Position.Z + "|" + Creatures[i].Sprite.Id;
                        }
                    }

                    writer.WriteLine(saveString);

                    saveString = "";

                    for (int i = 0; i < Items.Count; i++)
                    {
                        if (i == 0)
                        {
                            saveString = Items[i].Position.X + "|" + Items[i].Position.Y + "|" + Items[i].Position.Z +
                                         "|" + Items[i].Sprite.Id + "|" + Items[i].ZOrder;
                        }
                        else
                        {
                            saveString += "," + Items[i].Position.X + "|" + Items[i].Position.Y + "|" +
                                          Items[i].Position.Z + "|" + Items[i].Sprite.Id + "|" + Items[i].ZOrder;
                        }
                    }

                    writer.WriteLine(saveString);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool ContainsTile(List<Tile> tileList, Tile tile)
        {
            for (int i = 0; i < tileList.Count; i++)
            {
                if (SamePosition(tileList[i].Position, tile.Position)) // && tileList[i].SpriteID == tile.SpriteID)
                {
                    return true;
                }
            }
            return false;
        }

        internal bool SamePosition(Coordinates source, Coordinates destination, bool checkZ = true)
        {
            if (checkZ)
                return (source.X == destination.X && source.Y == destination.Y && source.Z == destination.Z);
            else
                return (source.X == destination.X && source.Y == destination.Y);
        }

        internal Tile GetTopTile(Coordinates tilePos)
        {
            List<Tile> tilesOnPos = GetTilesFromPos(tilePos);
            Tile topTile = new Tile();
            for(int i = 0; i < tilesOnPos.Count; i++)
            {
                if (tilesOnPos[i].ZOrder > topTile.ZOrder)
                {
                    topTile = tilesOnPos[i];
                }
            }

            return topTile;
        }

        internal List<Tile> GetTilesFromPos(Coordinates tilePos)
        {
            List<Tile> tileItems = new List<Tile>();
            for (int i = 0; i < Tiles.Count; i++)
            {
                if (SamePosition(Tiles[i].Position, tilePos))
                {
                    tileItems.Add(Tiles[i]);
                }
            }
            return tileItems;
        }

        internal bool TileAbove(Coordinates position)
        {
            for (int i = 0; i < Tiles.Count; i++)
            {
                if (IsAbove(Tiles[i].Position, position))
                {
                    return true;
                }
            }
            return false;
        }

        internal bool IsAbove(Coordinates destination, Coordinates source)
        {
            return (source.X == destination.X && source.Y == destination.Y && source.Z < destination.Z);
        }

        internal Item GetTopItem(Coordinates selectPos)
        {
            List<Item> tileItems = GetItemsFromPos(selectPos);
            Item topItem = new Item();
            for(int i = 0; i < tileItems.Count; i++)
            {
                if(tileItems[i].ZOrder > topItem.ZOrder)
                {
                    topItem = tileItems[i];
                }
            }

            return topItem;
        }

        internal List<Item> GetItemsFromPos(Coordinates tilePos)
        {
            List<Item> tileItems = new List<Item>();
            for (int i = 0; i < Items.Count; i++)
            {
                if (SamePosition(Items[i].Position, tilePos))
                {
                    tileItems.Add(Items[i]);
                }
            }
            return tileItems;
        }
    }
}
