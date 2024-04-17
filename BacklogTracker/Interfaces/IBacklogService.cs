using BacklogTracker.Data.DTOs;

namespace BacklogTracker.Interfaces
{
    public interface IBacklogService
    {
        void AddToBacklog(UserDto userDto);
        void RemoveFromBacklog(UserDto userDto);
    }
}
