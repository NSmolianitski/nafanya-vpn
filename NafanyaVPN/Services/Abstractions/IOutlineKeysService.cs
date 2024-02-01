using NafanyaVPN.Models;

namespace NafanyaVPN.Services.Abstractions;

public interface IOutlineKeysService
{
    Task<OutlineKey> CreateAsync(OutlineKey model);
}