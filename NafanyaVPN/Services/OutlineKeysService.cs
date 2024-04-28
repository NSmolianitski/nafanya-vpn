using NafanyaVPN.Database.Abstract;
using NafanyaVPN.Models;
using NafanyaVPN.Services.Abstractions;

namespace NafanyaVPN.Services;

public class OutlineKeysService(IBaseRepository<OutlineKey> outlineKeysRepository) : IOutlineKeysService
{
    public async Task<OutlineKey> CreateAsync(OutlineKey model)
    {
        return await outlineKeysRepository.CreateAsync(model);
    }
}