namespace NafanyaVPN.Entities.Outline;

public interface IOutlineKeysService
{
    Task<OutlineKey> CreateAsync(OutlineKey model);
}