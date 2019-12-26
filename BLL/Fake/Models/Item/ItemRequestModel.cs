using System.Collections.Generic;

namespace BLL.Fake.Models.Item
{
    public class ItemRequestModel
    {
        public IEnumerable<ItemModelDTO> Items { get; set; }
        public int OrderId { get; set; }
    }
}