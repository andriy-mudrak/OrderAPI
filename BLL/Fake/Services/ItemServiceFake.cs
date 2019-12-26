using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Fake.Models.Item;
using BLL.Fake.Services.Interfaces;

namespace BLL.Fake.Services
{
    public class ItemServiceFake : IItemService
    {
        private IEnumerable<ItemModelDTO> Items;
        private readonly IMapper _mapper;
        private Dictionary<int, IEnumerable<ItemModelDTO>> transactions = new Dictionary<int, IEnumerable<ItemModelDTO>>();

        public ItemServiceFake(IMapper mapper)
        {
            _mapper = mapper;
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

        public async Task<IEnumerable<ItemReservedModel>> Reservation(ItemRequestModel request)
        {
            var reservedItems = new List<ItemReservedModel>();
            var isValidReservation = true;

            foreach (var item in request.Items)
            {
                var itemModel = Items.Where(x => x.ItemId == item.ItemId).FirstOrDefault();
                var reservedItem = new ItemReservedModel();

                if (itemModel.Quantity < item.Quantity)
                {
                    isValidReservation = false;
                    reservedItem.Status = false;
                    reservedItem.Message = "Inventory does not have enough items";
                    //TODO: якщо впаде по кількості то треба повернутись до початкового стану
                    
                    break;
                }
                else
                {
                    itemModel.Quantity -= item.Quantity;
                    reservedItem = _mapper.Map<ItemReservedModel>(item);
                    reservedItem.Status = true;
                    reservedItems.Add(reservedItem);
                }
            }

            if (isValidReservation) transactions.Add(request.OrderId, request.Items);
            return reservedItems;
        }

        public async Task<IEnumerable<ItemModelDTO>> CancelReservation(int orderId)
        {
            var items = transactions[orderId];
            transactions.Remove(orderId);

            foreach (var item in transactions[orderId])
            {
                var itemModel = Items.Where(x => x.ItemId == item.ItemId).FirstOrDefault();
                itemModel.Quantity += item.Quantity;
            }

            return items;
        }
    }
}