using NafanyaVPN.Database.Abstract;
using NafanyaVPN.Models;
using NafanyaVPN.Services.Abstractions;

namespace NafanyaVPN.Services;

public class OutlineKeysService : IOutlineKeysService
{
    private readonly IBaseRepository<OutlineKey> _outlineKeysRepository;

    public OutlineKeysService(IBaseRepository<OutlineKey> outlineKeysRepository)
    {
        _outlineKeysRepository = outlineKeysRepository;
    }

    public async Task<OutlineKey> CreateAsync(OutlineKey model)
    {
        return await _outlineKeysRepository.CreateAsync(model);
    }
}