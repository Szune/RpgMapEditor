using System.Collections.Generic;
using RpgMapEditor.Modules.Entities;
using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor.Modules.Objects
{
    public class Backpack
    {
        private List<Item> _containedItems = new List<Item>();
        public Coordinates Position;
        public string Name = "";
        public bool Open = false;
        public int Id = -1;
        public int ItemId;
        public Coordinates ClosePosition;
        public int Height = 80;
        public int MaxHeight = 200;

        public bool AddItem(Item item)
        {
            if (_containedItems.Count < 20)
            {
                item.Parent = this;
                item.ParentId = this.Id;
                item.ParentItemId = this.ItemId;
                item.Slot = ItemSlot.InsideBag;
                _containedItems.Add(item);

                for (int i = 0; i < _containedItems.Count; i++)
                {
                    _containedItems[i].BagSlot = i;
                }
                return true;
            }
            return false;
        }

        public void RemoveItem(Item item)
        {
            item.BagSlot = -1;
            item.Parent = new Backpack();
            item.ParentId = -1;
            item.ParentItemId = -1;
            _containedItems.Remove(item);
        }


        public List<Item> GetItems()
        {
            return _containedItems;
        }

        public void Sort()
        {
            _containedItems.Sort((a, b) => a.BagSlot.CompareTo(b.BagSlot));
        }

        public List<Item> GetItemsSafe()
        {
            List<Item> newList = new List<Item>();
            newList.AddRange(_containedItems);
            return newList;
        }

        public Item GetItemBySlot(int slot)
        {
            if (slot < _containedItems.Count - 1)
                return _containedItems[slot];
            else
                return new Item();
        }

        public bool IsEmpty()
        {
            if (_containedItems.Count > 0)
            {
                return false;
            }
            return true;
        }

        public List<Item> GetAllItemsFromNestedBags(Item bag)
        {
            List<Item> allItems = new List<Item>();
            bool loopDone = false;
            if (this._containedItems.Count > 0)
            {
                List<Item> bagItems = new List<Item>();
                bagItems = bag.Container.GetItemsSafe();
                List<Backpack> moreBags = new List<Backpack>();


                while (!loopDone)
                {

                    for (int i = 0; i < bagItems.Count; i++)
                    {
                        if (bagItems[i].Container != null)
                        {
                            if (!bagItems[i].Container.IsEmpty())
                            {
                                allItems.Add(bagItems[i]);
                                moreBags.Add(bagItems[i].Container);
                            }
                            else
                            {
                                allItems.Add(bagItems[i]);
                            }
                        }
                        else
                        {
                            allItems.Add(bagItems[i]);
                        }
                    }
                    bagItems.Clear();
                    if (moreBags.Count > 0)
                    {
                        bagItems = moreBags[0].GetItemsSafe();
                        moreBags.RemoveAt(0);
                    }
                    else
                    {
                        loopDone = true;
                    }
                }
            }
            List<Item> newList = new List<Item>();
            newList.AddRange(allItems);
            return newList;
        }
    }
}
