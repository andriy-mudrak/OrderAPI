using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Fake.Models.Item;
using BLL.Fake.Services.Interfaces;
using MicroserviceMessages;
using RawRabbit;

namespace BLL.Fake.Services
{
    public class ItemServiceFake : IItemService
    {
        private static IEnumerable<ItemModelDTO> Items;
        private readonly IMapper _mapper;
        private static Dictionary<int, IEnumerable<ItemModelDTO>> transactions = new Dictionary<int, IEnumerable<ItemModelDTO>>();
        private static IBusClient _client;

        public ItemServiceFake(IMapper mapper, IBusClient client)
        {
            _client = client;
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

                    transactions.Add(request.OrderId, reservedItems);
                    await _client.PublishAsync(new ResponseBasic {OrderId = request.OrderId, Status = false});
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

            foreach (var item in items)
            {
                var itemModel = Items.Where(x => x.ItemId == item.ItemId).FirstOrDefault();
                itemModel.Quantity += item.Quantity;
            }

            return items;
        }
    }
}