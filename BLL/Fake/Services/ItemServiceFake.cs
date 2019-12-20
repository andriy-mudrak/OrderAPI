using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Fake.Models.Item;
using BLL.Fake.Services.Interfaces;

namespace BLL.Fake.Services
{
    public class ItemServiceFake : IItemService
    {
        private IEnumerable<ItemModelDTO> Items;

        public ItemServiceFake()
        {
            Items = new List<ItemModelDTO>()
                {
                    new ItemModelDTO()
                    {
                        ItemId = 1,
                        Price = 100,
                        Quantity = 20,
                        ProductName = "Frying pan"
                    },

                    new ItemModelDTO()
                    {
                        ItemId = 2,
                        Price = 1,
                        Quantity = 450,
                        ProductName = "Fork"
                    },

                    new ItemModelDTO()
                    {
                        ItemId = 3,
                        Price = 1,
                        Quantity = 500,
                        ProductName = "Spoon"
                    },

                    new ItemModelDTO()
                    {
                        ItemId = 4,
                        Price = 10,
                        Quantity = 700,
                        ProductName = "Plate"
                    },

                    new ItemModelDTO()
                    {
                        ItemId = 5,
                        Price = 120,
                        Quantity = 0,
                        ProductName = "Kettle"
                    }
                };
        }

        public async Task<IEnumerable<ItemModelDTO>> GetAll()
        {
            return Items;
        }

        public async Task<IEnumerable<ItemReservedModel>> Reservation(IEnumerable<ItemModelDTO> items)
        {
            var reservedItems = new List<ItemReservedModel>();

            foreach (var item in items) 
            {
                var itemModel = Items.Where(x=>x.ItemId == item.ItemId).FirstOrDefault();
                var reservedItem = new ItemReservedModel(){Status = true};

                if (itemModel.Quantity < item.Quantity)
                {
                    reservedItem.Status = false;
                    reservedItem.Message = "Inventory does not have enough items";
                    itemModel.Quantity -= item.Quantity;
                }
                else itemModel.Quantity -= item.Quantity;

                reservedItems.Add(reservedItem);
            }

            return reservedItems;
        }

        public async Task<IEnumerable<ItemModelDTO>> CancelReservation(IEnumerable<ItemModelDTO> items)
        {
            foreach (var item in items)
            {
                var itemModel = Items.Where(x => x.ItemId == item.ItemId).FirstOrDefault();
                itemModel.Quantity += item.Quantity;
            }

            return items;
        }
    }
}