Sprites_Items = {
					{Name = 'Golden Armor', SpriteID = 1, SpriteName = 'Golden Armor', SpritesheetID = 1, X = 225, Y = 256, W = 15, H = 16, Show = true, ListCategory = 'Armor'},
					{Name = 'Golden Helmet', SpriteID = 2, SpriteName = 'Golden Helmet', SpritesheetID = 1, X = 208, Y = 256, W = 16, H = 16, Show = true, ListCategory = 'Armor'}
				}

Sprites_Items_Spritesheets = {
					{Name = 'Spritesheet_Items', SpriteID = 1, SpriteName = 'Items\\Spritesheet_Items', SpritesheetID = 1}
				}

-- TODO: Gör så att Sprites_Items har Spritesheet-variabel som säger vilken Spritesheet item:et finns i
-- + variabler för rektangeln i spritesheet:en som utgör item:et

-- W = width, H = height
Sprites_Tiles_SpriteCollections = {
	{ID = 1, Name = "WoodOven1", Columns = 3, Rows = 4},
	{ID = 2, Name = "Table1", Columns = 3, Rows = 3}
	--{ID = 2, Name = "Dirt1", Columns = 3, Rows = 1}
}

Sprites_Tiles = {
					-- Grass
					{Name = 'Grass1', SpriteID = 30001, SpriteName = 'Grass1', Walkable = true, MovePlayer = false, SpritesheetID = 1, X = 16, Y = 16, W = 16, H = 16, Show = true, ListCategory = 'Grass', RandomizeCategory = 'Grass1'},
					{Name = 'Grass2', SpriteID = 30002, SpriteName = 'Grass2', Walkable = true, MovePlayer = false, SpritesheetID = 1, X = 32, Y = 16, W = 16, H = 16, Show = true, ListCategory = 'Grass', RandomizeCategory = 'Grass1'},
					{Name = 'Grass3', SpriteID = 30003, SpriteName = 'Grass3', Walkable = true, MovePlayer = false, SpritesheetID = 1, X = 48, Y = 16, W = 16, H = 16, Show = true, ListCategory = 'Grass', RandomizeCategory = 'Grass1'},
					{Name = 'Grass4', SpriteID = 30004, SpriteName = 'Grass4', Walkable = true, MovePlayer = false, SpritesheetID = 1, X = 64, Y = 16, W = 16, H = 16, Show = true, ListCategory = 'Grass', RandomizeCategory = 'Grass1'},
					{Name = 'Grass5', SpriteID = 30005, SpriteName = 'Grass5', Walkable = true, MovePlayer = false, SpritesheetID = 1, X = 80, Y = 16, W = 16, H = 16, Show = true, ListCategory = 'Grass', RandomizeCategory = 'Grass3'},
					{Name = 'Grass6', SpriteID = 30006, SpriteName = 'Grass6', Walkable = true, MovePlayer = false, SpritesheetID = 1, X = 96, Y = 16, W = 16, H = 16, Show = true, ListCategory = 'Grass', RandomizeCategory = 'Grass2'},
					{Name = 'Grass7', SpriteID = 30007, SpriteName = 'Grass7', Walkable = true, MovePlayer = false, SpritesheetID = 1, X = 112, Y = 16, W = 16, H = 16, Show = true, ListCategory = 'Grass', RandomizeCategory = 'Grass2'},
					{Name = 'Grass8', SpriteID = 30008, SpriteName = 'Grass8', Walkable = true, MovePlayer = false, SpritesheetID = 1, X = 128, Y = 16, W = 16, H = 16, Show = true, ListCategory = 'Grass', RandomizeCategory = 'Grass3'},
					{Name = 'Grass9', SpriteID = 30009, SpriteName = 'Grass9', Walkable = true, MovePlayer = false, SpritesheetID = 1, X = 144, Y = 16, W = 16, H = 16, Show = true, ListCategory = 'Grass', RandomizeCategory = 'Grass2'},
					{Name = 'Grass10', SpriteID = 30010, SpriteName = 'Grass10', Walkable = true, MovePlayer = false, SpritesheetID = 1, X = 160, Y = 16, W = 16, H = 16, Show = true, ListCategory = 'Grass', RandomizeCategory = 'Grass2'},
					{Name = 'Grass11', SpriteID = 30011, SpriteName = 'Grass11', Walkable = true, MovePlayer = false, SpritesheetID = 1, X = 176, Y = 16, W = 16, H = 16, Show = true, ListCategory = 'Grass', RandomizeCategory = 'Grass2'},
					{Name = 'GrassFlower1', SpriteID = 30012, SpriteName = 'GrassFlower1', Walkable = true, MovePlayer = false, SpritesheetID = 2, X = 16, Y = 64, W = 16, H = 16, Show = true, ListCategory = 'Grass', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'GrassFlower2', SpriteID = 30013, SpriteName = 'GrassFlower2', Walkable = true, MovePlayer = false, SpritesheetID = 2, X = 32, Y = 64, W = 16, H = 16, Show = true, ListCategory = 'Grass', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'GrassFlower3', SpriteID = 30014, SpriteName = 'GrassFlower3', Walkable = true, MovePlayer = false, SpritesheetID = 2, X = 48, Y = 64, W = 16, H = 16, Show = true, ListCategory = 'Grass', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'GrassFlower4', SpriteID = 30015, SpriteName = 'GrassFlower4', Walkable = true, MovePlayer = false, SpritesheetID = 2, X = 64, Y = 64, W = 16, H = 16, Show = true, ListCategory = 'Grass', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'GrassMushroom1', SpriteID = 30016, SpriteName = 'GrassMushroom1', Walkable = true, MovePlayer = false, SpritesheetID = 2, X = 64, Y = 16, W = 16, H = 16, Show = true, ListCategory = 'Grass', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'GrassMushroom2', SpriteID = 30017, SpriteName = 'GrassMushroom2', Walkable = true, MovePlayer = false, SpritesheetID = 2, X = 80, Y = 16, W = 16, H = 16, Show = true, ListCategory = 'Grass', RandomizeCategory = '', SpriteCollection = ''},

					-- Dirt
					{Name = 'Dirt1', SpriteID = 30100, SpriteName = 'Dirt1', Walkable = true, MovePlayer = false, SpritesheetID = 1, X = 240, Y = 16, W = 16, H = 16, Show = true, ListCategory = 'Dirt', RandomizeCategory = 'Dirt1'},
					{Name = 'Dirt2', SpriteID = 30101, SpriteName = 'Dirt2', Walkable = true, MovePlayer = false, SpritesheetID = 1, X = 256, Y = 16, W = 16, H = 16, Show = true, ListCategory = 'Dirt', RandomizeCategory = 'Dirt1'},
					{Name = 'Dirt3', SpriteID = 30102, SpriteName = 'Dirt3', Walkable = true, MovePlayer = false, SpritesheetID = 1, X = 272, Y = 16, W = 16, H = 16, Show = true, ListCategory = 'Dirt', RandomizeCategory = 'Dirt1'},

					-- Objects
					{Name = 'Sign', SpriteID = 30200, SpriteName = 'Sign', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 608, Y = 16, W = 16, H = 16, Show = true, ListCategory = 'Objects'},

					-- Water
					{Name = 'Water1', SpriteID = 30400, SpriteName = 'Water1', Walkable = false, MovePlayer = false, SpritesheetID = 3, X = 24, Y = 56, W = 16, H = 16, Show = true, ListCategory = 'Water', RandomizeCategory = 'Water1', AnimationID = 1, AnimationOrder = 1},
					{Name = 'Water2', SpriteID = 30401, SpriteName = 'Water2', Walkable = false, MovePlayer = false, SpritesheetID = 3, X = 72, Y = 56, W = 16, H = 16, Show = true, ListCategory = 'Water', RandomizeCategory = 'Water1', AnimationID = 1, AnimationOrder = 2},
					{Name = 'Water3', SpriteID = 30402, SpriteName = 'Water3', Walkable = false, MovePlayer = false, SpritesheetID = 3, X = 120, Y = 56, W = 16, H = 16, Show = true, ListCategory = 'Water', RandomizeCategory = 'Water1', AnimationID = 1, AnimationOrder = 3},
					{Name = 'WaterCorner1', SpriteID = 30403, SpriteName = 'WaterCorner1', Walkable = false, MovePlayer = false, SpritesheetID = 3, X = 32, Y = 112, W = 16, H = 16, Show = true, ListCategory = 'Water'},
					{Name = 'WaterCorner2', SpriteID = 30404, SpriteName = 'WaterCorner2', Walkable = false, MovePlayer = false, SpritesheetID = 3, X = 48, Y = 112, W = 16, H = 16, Show = true, ListCategory = 'Water'},
					{Name = 'WaterCorner3', SpriteID = 30405, SpriteName = 'WaterCorner3', Walkable = false, MovePlayer = false, SpritesheetID = 3, X = 32, Y = 128, W = 16, H = 16, Show = true, ListCategory = 'Water'},
					{Name = 'WaterCorner4', SpriteID = 30406, SpriteName = 'WaterCorner4', Walkable = false, MovePlayer = false, SpritesheetID = 3, X = 48, Y = 128, W = 16, H = 16, Show = true, ListCategory = 'Water'},
					{Name = 'WaterCorner5', SpriteID = 30407, SpriteName = 'WaterCorner5', Walkable = false, MovePlayer = false, SpritesheetID = 3, X = 16, Y = 144, W = 16, H = 16, Show = true, ListCategory = 'Water'},
					{Name = 'WaterCorner6', SpriteID = 30408, SpriteName = 'WaterCorner6', Walkable = false, MovePlayer = false, SpritesheetID = 3, X = 16, Y = 160, W = 16, H = 16, Show = true, ListCategory = 'Water'},
					{Name = 'WaterCorner7', SpriteID = 30409, SpriteName = 'WaterCorner7', Walkable = false, MovePlayer = false, SpritesheetID = 3, X = 16, Y = 176, W = 16, H = 16, Show = true, ListCategory = 'Water'},
					{Name = 'WaterCorner8', SpriteID = 30410, SpriteName = 'WaterCorner8', Walkable = false, MovePlayer = false, SpritesheetID = 3, X = 32, Y = 144, W = 16, H = 16, Show = true, ListCategory = 'Water'},
					{Name = 'WaterCorner9', SpriteID = 30411, SpriteName = 'WaterCorner9', Walkable = false, MovePlayer = false, SpritesheetID = 3, X = 48, Y = 144, W = 16, H = 16, Show = true, ListCategory = 'Water'},
					{Name = 'WaterCorner10', SpriteID = 30412, SpriteName = 'WaterCorner10', Walkable = false, MovePlayer = false, SpritesheetID = 3, X = 48, Y = 160, W = 16, H = 16, Show = true, ListCategory = 'Water'},
					{Name = 'WaterCorner11', SpriteID = 30413, SpriteName = 'WaterCorner11', Walkable = false, MovePlayer = false, SpritesheetID = 3, X = 48, Y = 176, W = 16, H = 16, Show = true, ListCategory = 'Water'},
					{Name = 'WaterCorner12', SpriteID = 30414, SpriteName = 'WaterCorner12', Walkable = false, MovePlayer = false, SpritesheetID = 3, X = 32, Y = 176, W = 16, H = 16, Show = true, ListCategory = 'Water'},

					-- Houses
					{Name = 'HousePart1', SpriteID = 30600, SpriteName = 'HousePart1', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 32, Y = 288, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart2', SpriteID = 30601, SpriteName = 'HousePart2', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 48, Y = 288, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart3', SpriteID = 30602, SpriteName = 'HousePart3', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 64, Y = 288, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart4', SpriteID = 30603, SpriteName = 'HousePart4', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 80, Y = 288, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart5', SpriteID = 30604, SpriteName = 'HousePart5', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 96, Y = 288, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart6', SpriteID = 30605, SpriteName = 'HousePart6', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 112, Y = 288, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart7', SpriteID = 30606, SpriteName = 'HousePart7', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 128, Y = 288, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart8', SpriteID = 30607, SpriteName = 'HousePart8', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 32, Y = 272, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart9', SpriteID = 30608, SpriteName = 'HousePart9', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 48, Y = 272, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart10', SpriteID = 30609, SpriteName = 'HousePart10', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 64, Y = 272, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart11', SpriteID = 30610, SpriteName = 'HousePart11', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 80, Y = 272, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart12', SpriteID = 30611, SpriteName = 'HousePart12', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 96, Y = 272, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart13', SpriteID = 30612, SpriteName = 'HousePart13', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 112, Y = 272, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart14', SpriteID = 30613, SpriteName = 'HousePart14', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 128, Y = 272, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart15', SpriteID = 30614, SpriteName = 'HousePart15', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 32, Y = 256, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart16', SpriteID = 30615, SpriteName = 'HousePart16', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 48, Y = 256, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart17', SpriteID = 30616, SpriteName = 'HousePart17', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 64, Y = 256, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart18', SpriteID = 30617, SpriteName = 'HousePart18', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 80, Y = 256, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart19', SpriteID = 30618, SpriteName = 'HousePart19', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 96, Y = 256, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart20', SpriteID = 30619, SpriteName = 'HousePart20', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 112, Y = 256, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart21', SpriteID = 30620, SpriteName = 'HousePart21', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 128, Y = 256, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart22', SpriteID = 30621, SpriteName = 'HousePart22', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 32, Y = 240, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart23', SpriteID = 30622, SpriteName = 'HousePart23', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 48, Y = 240, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart24', SpriteID = 30623, SpriteName = 'HousePart24', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 64, Y = 240, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart25', SpriteID = 30624, SpriteName = 'HousePart25', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 80, Y = 240, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart26', SpriteID = 30625, SpriteName = 'HousePart26', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 96, Y = 240, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart27', SpriteID = 30626, SpriteName = 'HousePart27', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 112, Y = 240, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart28', SpriteID = 30627, SpriteName = 'HousePart28', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 128, Y = 240, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart29', SpriteID = 30628, SpriteName = 'HousePart29', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 32, Y = 224, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart30', SpriteID = 30629, SpriteName = 'HousePart30', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 48, Y = 224, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart31', SpriteID = 30630, SpriteName = 'HousePart31', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 64, Y = 224, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart32', SpriteID = 30631, SpriteName = 'HousePart32', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 80, Y = 224, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart33', SpriteID = 30632, SpriteName = 'HousePart33', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 96, Y = 224, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart34', SpriteID = 30633, SpriteName = 'HousePart34', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 112, Y = 224, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart35', SpriteID = 30634, SpriteName = 'HousePart35', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 128, Y = 224, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart36', SpriteID = 30635, SpriteName = 'HousePart36', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 32, Y = 208, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart37', SpriteID = 30636, SpriteName = 'HousePart37', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 48, Y = 208, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart38', SpriteID = 30637, SpriteName = 'HousePart38', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 64, Y = 208, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart39', SpriteID = 30638, SpriteName = 'HousePart39', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 80, Y = 208, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart40', SpriteID = 30639, SpriteName = 'HousePart40', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 96, Y = 208, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart41', SpriteID = 30640, SpriteName = 'HousePart41', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 112, Y = 208, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart42', SpriteID = 30641, SpriteName = 'HousePart42', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 128, Y = 208, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart43', SpriteID = 30642, SpriteName = 'HousePart43', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 32, Y = 192, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart44', SpriteID = 30643, SpriteName = 'HousePart44', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 48, Y = 192, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart45', SpriteID = 30644, SpriteName = 'HousePart45', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 64, Y = 192, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart46', SpriteID = 30645, SpriteName = 'HousePart46', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 80, Y = 192, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart47', SpriteID = 30646, SpriteName = 'HousePart47', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 96, Y = 192, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart48', SpriteID = 30647, SpriteName = 'HousePart48', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 112, Y = 192, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart49', SpriteID = 30648, SpriteName = 'HousePart49', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 128, Y = 192, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart50', SpriteID = 30649, SpriteName = 'HousePart50', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 32, Y = 176, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart51', SpriteID = 30650, SpriteName = 'HousePart51', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 48, Y = 176, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart52', SpriteID = 30651, SpriteName = 'HousePart52', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 64, Y = 176, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart53', SpriteID = 30652, SpriteName = 'HousePart53', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 80, Y = 176, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart54', SpriteID = 30653, SpriteName = 'HousePart54', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 96, Y = 176, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart55', SpriteID = 30654, SpriteName = 'HousePart55', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 112, Y = 176, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart56', SpriteID = 30655, SpriteName = 'HousePart56', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 128, Y = 176, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart57', SpriteID = 30656, SpriteName = 'HousePart57', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 64, Y = 160, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart58', SpriteID = 30657, SpriteName = 'HousePart58', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 80, Y = 160, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},
					{Name = 'HousePart59', SpriteID = 30658, SpriteName = 'HousePart59', Walkable = false, MovePlayer = false, SpritesheetID = 4, X = 96, Y = 160, W = 16, H = 16, Show = true, ListCategory = 'Houses', SpriteCollection = 'House1'},

					-- Interior
					{Name = 'WoodenFloor', SpriteID = 30800, SpriteName = 'WoodenFloor', Walkable = false, MovePlayer = false, SpritesheetID = 1, X = 448, Y = 32, W = 16, H = 16, Show = true, ListCategory = 'Interior'},

					-- HouseInside
					{Name = 'HouseBorder1', SpriteID = 31000, SpriteName = 'HouseBorder1', Walkable = false, MovePlayer = false, SpritesheetID = 6, X = 0, Y = 32, W = 16, H = 16, Show = true, ListCategory = 'HouseInside', RandomizeCategory = ''},
					{Name = 'HouseBorder2', SpriteID = 31001, SpriteName = 'HouseBorder2', Walkable = false, MovePlayer = false, SpritesheetID = 6, X = 0, Y = 48, W = 16, H = 16, Show = true, ListCategory = 'HouseInside', RandomizeCategory = ''},
					{Name = 'HouseBorder3', SpriteID = 31002, SpriteName = 'HouseBorder3', Walkable = false, MovePlayer = false, SpritesheetID = 6, X = 16, Y = 48, W = 16, H = 16, Show = true, ListCategory = 'HouseInside', RandomizeCategory = ''},
					{Name = 'HouseBorder4', SpriteID = 31003, SpriteName = 'HouseBorder4', Walkable = false, MovePlayer = false, SpritesheetID = 6, X = 0, Y = 64, W = 16, H = 16, Show = true, ListCategory = 'HouseInside', RandomizeCategory = ''},
					{Name = 'HouseBorder5', SpriteID = 31004, SpriteName = 'HouseBorder5', Walkable = false, MovePlayer = false, SpritesheetID = 6, X = 48, Y = 96, W = 16, H = 16, Show = true, ListCategory = 'HouseInside', RandomizeCategory = ''},
					{Name = 'HouseBorder6', SpriteID = 31005, SpriteName = 'HouseBorder6', Walkable = false, MovePlayer = false, SpritesheetID = 6, X = 64, Y = 96, W = 16, H = 16, Show = true, ListCategory = 'HouseInside', RandomizeCategory = ''},
					{Name = 'HouseBorder7', SpriteID = 31006, SpriteName = 'HouseBorder7', Walkable = false, MovePlayer = false, SpritesheetID = 6, X = 48, Y = 112, W = 16, H = 16, Show = true, ListCategory = 'HouseInside', RandomizeCategory = ''},
					{Name = 'HouseBorder8', SpriteID = 31007, SpriteName = 'HouseBorder8', Walkable = false, MovePlayer = false, SpritesheetID = 6, X = 64, Y = 112, W = 16, H = 16, Show = true, ListCategory = 'HouseInside', RandomizeCategory = ''},
					{Name = 'HouseWall1', SpriteID = 31100, SpriteName = 'HouseWall1', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 448, Y = 144, W = 16, H = 16, Show = true, ListCategory = 'HouseInside', RandomizeCategory = ''},
					{Name = 'HouseWall2', SpriteID = 31101, SpriteName = 'HouseWall2', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 464, Y = 144, W = 16, H = 16, Show = true, ListCategory = 'HouseInside', RandomizeCategory = ''},
					{Name = 'HouseWall3', SpriteID = 31102, SpriteName = 'HouseWall3', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 480, Y = 144, W = 16, H = 16, Show = true, ListCategory = 'HouseInside', RandomizeCategory = ''},
					{Name = 'HouseWall4', SpriteID = 31103, SpriteName = 'HouseWall4', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 496, Y = 144, W = 16, H = 16, Show = true, ListCategory = 'HouseInside', RandomizeCategory = ''},
					{Name = 'HouseWall5', SpriteID = 31104, SpriteName = 'HouseWall5', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 448, Y = 160, W = 16, H = 16, Show = true, ListCategory = 'HouseInside', RandomizeCategory = ''},
					{Name = 'HouseWall6', SpriteID = 31105, SpriteName = 'HouseWall6', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 464, Y = 160, W = 16, H = 16, Show = true, ListCategory = 'HouseInside', RandomizeCategory = ''},
					{Name = 'HouseWall7', SpriteID = 31106, SpriteName = 'HouseWall7', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 480, Y = 160, W = 16, H = 16, Show = true, ListCategory = 'HouseInside', RandomizeCategory = ''},
					{Name = 'HouseWall8', SpriteID = 31107, SpriteName = 'HouseWall8', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 496, Y = 160, W = 16, H = 16, Show = true, ListCategory = 'HouseInside', RandomizeCategory = ''},
					{Name = 'HouseWall9', SpriteID = 31108, SpriteName = 'HouseWall9', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 448, Y = 176, W = 16, H = 16, Show = true, ListCategory = 'HouseInside', RandomizeCategory = ''},
					{Name = 'HouseWall10', SpriteID = 31109, SpriteName = 'HouseWall10', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 464, Y = 176, W = 16, H = 16, Show = true, ListCategory = 'HouseInside', RandomizeCategory = ''},
					{Name = 'HouseWall11', SpriteID = 31110, SpriteName = 'HouseWall11', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 480, Y = 176, W = 16, H = 16, Show = true, ListCategory = 'HouseInside', RandomizeCategory = ''},
					{Name = 'HouseWall12', SpriteID = 31111, SpriteName = 'HouseWall12', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 496, Y = 176, W = 16, H = 16, Show = true, ListCategory = 'HouseInside', RandomizeCategory = ''},

					-- HouseDecoration
					{Name = 'WoodOven1', SpriteID = 35000, SpriteName = 'WoodOven1', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 16, Y = 368, W = 16, H = 16, Show = false, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = 'WoodOven1'},
					{Name = 'WoodOven2', SpriteID = 35001, SpriteName = 'WoodOven2', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 32, Y = 368, W = 16, H = 16, Show = false, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = 'WoodOven1'},
					{Name = 'WoodOven3', SpriteID = 35002, SpriteName = 'WoodOven3', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 48, Y = 368, W = 16, H = 16, Show = false, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = 'WoodOven1'},
					{Name = 'WoodOven4', SpriteID = 35003, SpriteName = 'WoodOven4', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 16, Y = 384, W = 16, H = 16, Show = false, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = 'WoodOven1'},
					{Name = 'WoodOven5', SpriteID = 35004, SpriteName = 'WoodOven5', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 32, Y = 384, W = 16, H = 16, Show = false, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = 'WoodOven1'},
					{Name = 'WoodOven6', SpriteID = 35005, SpriteName = 'WoodOven6', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 48, Y = 384, W = 16, H = 16, Show = false, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = 'WoodOven1'},
					{Name = 'WoodOven7', SpriteID = 35006, SpriteName = 'WoodOven7', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 16, Y = 400, W = 16, H = 16, Show = false, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = 'WoodOven1'},
					{Name = 'WoodOven8', SpriteID = 35007, SpriteName = 'WoodOven8', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 32, Y = 400, W = 16, H = 16, Show = false, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = 'WoodOven1'},
					{Name = 'WoodOven9', SpriteID = 35008, SpriteName = 'WoodOven9', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 48, Y = 400, W = 16, H = 16, Show = false, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = 'WoodOven1'},
					{Name = 'WoodOven10', SpriteID = 35009, SpriteName = 'WoodOven10', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 16, Y = 416, W = 16, H = 16, Show = false, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = 'WoodOven1'},
					{Name = 'WoodOven11', SpriteID = 35010, SpriteName = 'WoodOven11', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 32, Y = 416, W = 16, H = 16, Show = false, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = 'WoodOven1'},
					{Name = 'WoodOven12', SpriteID = 35011, SpriteName = 'WoodOven12', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 48, Y = 416, W = 16, H = 16, Show = false, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = 'WoodOven1'},
					
					{Name = 'Urn', SpriteID = 35012, SpriteName = 'Urn', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 128, Y = 384, W = 16, H = 16, Show = true, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'Urn2', SpriteID = 35013, SpriteName = 'Urn2', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 144, Y = 384, W = 16, H = 16, Show = true, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'Box', SpriteID = 35014, SpriteName = 'Box', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 160, Y = 400, W = 16, H = 16, Show = true, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'Box2', SpriteID = 35015, SpriteName = 'Box2', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 176, Y = 400, W = 16, H = 16, Show = true, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'Urn3', SpriteID = 35016, SpriteName = 'Urn3', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 160, Y = 496, W = 16, H = 16, Show = true, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = ''},
					
					{Name = 'Table1', SpriteID = 35017, SpriteName = 'Table1', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 464, Y = 496, W = 16, H = 16, Show = false, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = 'Table1'},
					{Name = 'Table2', SpriteID = 35018, SpriteName = 'Table2', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 480, Y = 496, W = 16, H = 16, Show = false, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = 'Table1'},
					{Name = 'Table3', SpriteID = 35019, SpriteName = 'Table3', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 496, Y = 496, W = 16, H = 16, Show = false, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = 'Table1'},
					{Name = 'Table4', SpriteID = 35020, SpriteName = 'Table4', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 464, Y = 512, W = 16, H = 16, Show = false, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = 'Table1'},
					{Name = 'Table5', SpriteID = 35021, SpriteName = 'Table5', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 480, Y = 512, W = 16, H = 16, Show = false, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = 'Table1'},
					{Name = 'Table6', SpriteID = 35022, SpriteName = 'Table6', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 496, Y = 512, W = 16, H = 16, Show = false, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = 'Table1'},
					{Name = 'Table7', SpriteID = 35023, SpriteName = 'Table7', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 464, Y = 528, W = 16, H = 16, Show = false, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = 'Table1'},
					{Name = 'Table8', SpriteID = 35024, SpriteName = 'Table8', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 480, Y = 528, W = 16, H = 16, Show = false, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = 'Table1'},
					{Name = 'Table9', SpriteID = 35025, SpriteName = 'Table9', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 496, Y = 528, W = 16, H = 16, Show = false, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = 'Table1'},

					{Name = 'Painting1', SpriteID = 35026, SpriteName = 'Painting1', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 160, Y = 464, W = 16, H = 16, Show = true, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'Painting2', SpriteID = 35027, SpriteName = 'Painting2', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 176, Y = 464, W = 16, H = 16, Show = true, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'Painting3', SpriteID = 35028, SpriteName = 'Painting3', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 192, Y = 464, W = 16, H = 16, Show = true, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = ''},

					{Name = 'ChairTop1', SpriteID = 35029, SpriteName = 'ChairTop1', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 384, Y = 432, W = 16, H = 16, Show = true, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'ChairBottom1', SpriteID = 35030, SpriteName = 'ChairBottom1', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 384, Y = 448, W = 16, H = 16, Show = true, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'ChairTop2', SpriteID = 35031, SpriteName = 'ChairTop2', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 416, Y = 432, W = 16, H = 16, Show = true, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'ChairBottom2', SpriteID = 35032, SpriteName = 'ChairBottom2', Walkable = false, MovePlayer = false, SpritesheetID = 5, X = 416, Y = 448, W = 16, H = 16, Show = true, ListCategory = 'HouseDecoration', RandomizeCategory = '', SpriteCollection = ''},

					-- Mountains
					{Name = 'DirtMountain1', SpriteID = 36000, SpriteName = 'DirtMountain1', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 464, Y = 16, W = 16, H = 16, Show = true, ListCategory = 'Mountains', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'DirtMountain2', SpriteID = 36001, SpriteName = 'DirtMountain2', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 480, Y = 16, W = 16, H = 16, Show = true, ListCategory = 'Mountains', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'DirtMountain3', SpriteID = 36002, SpriteName = 'DirtMountain3', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 496, Y = 16, W = 16, H = 16, Show = true, ListCategory = 'Mountains', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'DirtMountain4', SpriteID = 36003, SpriteName = 'DirtMountain4', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 464, Y = 32, W = 16, H = 16, Show = true, ListCategory = 'Mountains', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'DirtMountain5', SpriteID = 36004, SpriteName = 'DirtMountain5', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 480, Y = 32, W = 16, H = 16, Show = true, ListCategory = 'Mountains', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'DirtMountain6', SpriteID = 36005, SpriteName = 'DirtMountain6', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 496, Y = 32, W = 16, H = 16, Show = true, ListCategory = 'Mountains', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'DirtMountain7', SpriteID = 36006, SpriteName = 'DirtMountain7', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 464, Y = 48, W = 16, H = 16, Show = true, ListCategory = 'Mountains', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'DirtMountain8', SpriteID = 36007, SpriteName = 'DirtMountain8', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 480, Y = 48, W = 16, H = 16, Show = true, ListCategory = 'Mountains', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'DirtMountain9', SpriteID = 36008, SpriteName = 'DirtMountain9', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 496, Y = 48, W = 16, H = 16, Show = true, ListCategory = 'Mountains', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'DirtMountain10', SpriteID = 36009, SpriteName = 'DirtMountain10', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 464, Y = 64, W = 16, H = 16, Show = true, ListCategory = 'Mountains', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'DirtMountain11', SpriteID = 36010, SpriteName = 'DirtMountain11', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 480, Y = 64, W = 16, H = 16, Show = true, ListCategory = 'Mountains', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'DirtMountain12', SpriteID = 36011, SpriteName = 'DirtMountain12', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 496, Y = 64, W = 16, H = 16, Show = true, ListCategory = 'Mountains', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'DirtMountain13', SpriteID = 36012, SpriteName = 'DirtMountain13', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 464, Y = 80, W = 16, H = 16, Show = true, ListCategory = 'Mountains', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'DirtMountain14', SpriteID = 36013, SpriteName = 'DirtMountain14', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 480, Y = 80, W = 16, H = 16, Show = true, ListCategory = 'Mountains', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'DirtMountain15', SpriteID = 36014, SpriteName = 'DirtMountain1', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 496, Y = 80, W = 16, H = 16, Show = true, ListCategory = 'Mountains', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'DirtMountain16', SpriteID = 36015, SpriteName = 'DirtMountain16', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 464, Y = 96, W = 16, H = 16, Show = true, ListCategory = 'Mountains', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'DirtMountain17', SpriteID = 36016, SpriteName = 'DirtMountain17', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 480, Y = 96, W = 16, H = 16, Show = true, ListCategory = 'Mountains', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'DirtMountain18', SpriteID = 36017, SpriteName = 'DirtMountain18', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 496, Y = 96, W = 16, H = 16, Show = true, ListCategory = 'Mountains', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'DirtMountain19', SpriteID = 36018, SpriteName = 'DirtMountain19', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 464, Y = 112, W = 16, H = 16, Show = true, ListCategory = 'Mountains', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'DirtMountain20', SpriteID = 36019, SpriteName = 'DirtMountain20', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 480, Y = 112, W = 16, H = 16, Show = true, ListCategory = 'Mountains', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'DirtMountain221', SpriteID = 36020, SpriteName = 'DirtMountain21', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 496, Y = 112, W = 16, H = 16, Show = true, ListCategory = 'Mountains', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'DirtMountain22', SpriteID = 36021, SpriteName = 'DirtMountain22', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 464, Y = 128, W = 16, H = 16, Show = true, ListCategory = 'Mountains', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'DirtMountain23', SpriteID = 36022, SpriteName = 'DirtMountain23', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 480, Y = 128, W = 16, H = 16, Show = true, ListCategory = 'Mountains', RandomizeCategory = '', SpriteCollection = ''},
					{Name = 'DirtMountain24', SpriteID = 36023, SpriteName = 'DirtMountain24', Walkable = false, MovePlayer = false, SpritesheetID = 2, X = 496, Y = 128, W = 16, H = 16, Show = true, ListCategory = 'Mountains', RandomizeCategory = '', SpriteCollection = ''}
				}

Sprites_Tiles_Spritesheets = {
					{Name = 'Spritesheet_Terrain', SpriteID = 30000, SpriteName = 'Tiles\\Spritesheet_Terrain', SpritesheetID = 1},
					{Name = 'Spritesheet_Outside', SpriteID = 30001, SpriteName = 'Tiles\\Spritesheet_Outside', SpritesheetID = 2},
					{Name = 'Spritesheet_Water', SpriteID = 30002, SpriteName = 'Tiles\\Spritesheet_Water', SpritesheetID = 3},
					{Name = 'Spritesheet_House', SpriteID = 30003, SpriteName = 'Tiles\\Spritesheet_House', SpritesheetID = 4},
					{Name = 'Spritesheet_Inside', SpriteID = 30004, SpriteName = 'Tiles\\Spritesheet_Inside', SpritesheetID = 5},
					{Name = 'Spritesheet_Inside_Borders', SpriteID = 30005, SpriteName = 'Tiles\\Spritesheet_Inside_Borders', SpritesheetID = 6}
			 	}

Sprites_Creatures = {
					{Name = 'Snak', SpriteID = 50001, SpriteName = 'Snak', SpritesheetID = 1, X = 199, Y = 300, W = 18, H = 20, Show = true, ListCategory = 'Monsters'},
					{Name = 'Bat', SpriteID = 50002, SpriteName = 'Bat', SpritesheetID = 1, X = 544, Y = 288, W = 48, H = 32, Show = true, ListCategory = 'Monsters'}
				}

Sprites_Creatures_Spritesheets = {
					{Name = 'Spritesheet_Monster1', SpriteID = 50000, SpriteName = 'Creatures\\Spritesheet_Monster1', SpritesheetID = 1}
				}

Sprites_UI = { 
					{Name = 'UI_Black_Stroke', SpriteID = 100001, SpriteName = 'UI\\UI_Black_Stroke'},
					{Name = 'UI_White_Pixel', SpriteID = 100002, SpriteName = 'UI\\UI_White_Pixel'},
					{Name = 'UI_Arrow_Up', SpriteID = 100003, SpriteName = 'UI_Arrow_Up', SpritesheetID = 1, X = 306, Y = 549, W = 8, H = 7},
					{Name = 'UI_Arrow_Down', SpriteID = 100004, SpriteName = 'UI_Arrow_Down', SpritesheetID = 1, X = 306, Y = 557, W = 8, H = 7},
					{Name = 'UI_Scrollbar', SpriteID = 100005, SpriteName = 'UI_Scrollbar', SpritesheetID = 1, X = 306, Y = 534, W = 8, H = 14},
					{Name = 'UI_Big_Background', SpriteID = 100006, SpriteName = 'UI\\UI_Big_Background'},
					{Name = 'UI_Small_Background', SpriteID = 100007, SpriteName = 'UI\\UI_Small_Background'},
					{Name = 'UI_MapEditorBackground', SpriteID = 100009, SpriteName = 'UI\\UI_MapEditorBackground'},
					{Name = 'UI_MapEditorBackground2', SpriteID = 100010, SpriteName = 'UI\\UI_MapEditorBackground2'},
					{Name = 'UI_PlaceButton', SpriteID = 100011, SpriteName = 'UI_PlaceButton', SpritesheetID = 1, X = 306, Y = 517, W = 16, H = 16},
					{Name = 'UI_RemoveButton', SpriteID = 100012, SpriteName = 'UI_RemoveButton', SpritesheetID = 1, X = 323, Y = 517, W = 16, H = 16},
					{Name = 'UI_Textbox', SpriteID = 100013, SpriteName = 'UI\\UI_Textbox'},
					{Name = 'UI_ConfirmButton', SpriteID = 100014, SpriteName = 'UI_ConfirmButton', SpritesheetID = 1, X = 174, Y = 267, W = 9, H = 9},
					{Name = 'UI_PlusButton', SpriteID = 100015, SpriteName = 'UI_PlusButton', SpritesheetID = 1, X = 340, Y = 517, W = 16, H = 16},
					{Name = 'UI_MinusButton', SpriteID = 100016, SpriteName = 'UI_MinusButton', SpritesheetID = 1, X = 357, Y = 517, W = 16, H = 16},
					{Name = 'UI_SelectButton', SpriteID = 100017, SpriteName = 'UI_SelectButton', SpritesheetID = 1, X = 323, Y = 534, W = 16, H = 16}

				}

Sprites_UI_Spritesheets = {
					{Name = 'Spritesheet_UI', SpriteID = 100000, SpriteName = 'UI\\Spritesheet_UI', SpritesheetID = 1}
				}