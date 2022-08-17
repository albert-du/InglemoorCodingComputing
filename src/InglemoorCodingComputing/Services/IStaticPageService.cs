﻿namespace InglemoorCodingComputing.Services;

public interface IStaticPageService
{
    Task CreateAsync(StaticPage page);
    Task<StaticPage> ReadAsync(Guid id);
    Task<StaticPage?> FindAsync(string path);
    Task UpdateAsync(StaticPage page);
    Task DeleteAsync(Guid id);
}