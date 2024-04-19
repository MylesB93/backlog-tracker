using BacklogTracker.Data.DTOs;
using BacklogTracker.Models.UserBacklog;

namespace BacklogTracker.Interfaces
{
    public interface IBacklogService
    {
        void AddToBacklog(UserDto userDto);
        void RemoveFromBacklog(UserDto userDto);
        List<string>? GetBacklog(string email);
    }
}
