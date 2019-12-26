using System.Threading.Tasks;

namespace BLL.Subscribers.Interfaces
{
    public interface ISubscriber
    {
        Task Start();
    }
}