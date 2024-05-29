using BacklogTracker.Data.DTOs;

namespace BacklogTracker.Interfaces
{
    public interface IBacklogService
    {
        void AddToBacklog(UserDto userDto);
        void RemoveFromBacklog(UserDto userDto);
        List<string>? GetBacklog(string email);
        void AddToCompleted(UserDto userDto);
		List<string>? GetCompleted(string email);
		void RemoveFromCompleted(UserDto userDto);
	}
}
