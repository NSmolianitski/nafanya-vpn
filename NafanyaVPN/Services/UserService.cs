using Microsoft.EntityFrameworkCore;
using NafanyaVPN.Constants;
using NafanyaVPN.Database.Abstract;
using NafanyaVPN.Models;
using NafanyaVPN.Services.Abstractions;

namespace NafanyaVPN.Services;

public class UserService : IUserService
{
    private readonly IBaseRepository<User> _userRepository;
    private readonly ISubscriptionService _subscriptionService;

    public UserService(IBaseRepository<User> userRepository, ISubscriptionService subscriptionService)
    {
        _userRepository = userRepository;
        _subscriptionService = subscriptionService;
    }

    public async Task<User> AddAsync(long telegramUserId, string telegramUserName)
    {
        var userModel = await CreateNewUserModel(telegramUserId, telegramUserName);
        return await _userRepository.CreateAsync(userModel);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _userRepository.GetAll().ToListAsync();
    }
    
    public async Task<User> GetAsync(long telegramUserId)
    {
        var user = await TryGetAsync(telegramUserId);
        if (user is null)
            throw new NullReferenceException($"No user with such telegramUserId: {telegramUserId}");
        
        return user;
    }

    public async Task<User?> TryGetAsync(long telegramUserId)
    {
        var user = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.TelegramUserId == telegramUserId);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        await _userRepository.UpdateAsync(user);
    }

    public async Task UpdateAllAsync(IEnumerable<User> users)
    {
        await _userRepository.UpdateAllAsync(users);
    }
    
    private async Task<User> CreateNewUserModel(long telegramUserId, string telegramUserName)
    {
        var defaultSubscription = await _subscriptionService.GetAsync(DatabaseConstants.Default);
        
        var user = new User
        {
            TelegramUserId = telegramUserId,
            TelegramUserName = telegramUserName,
            MoneyInRoubles = 0, 
            Subscription = defaultSubscription,
            TelegramState = ""
        };
        
        return user;
    }
}