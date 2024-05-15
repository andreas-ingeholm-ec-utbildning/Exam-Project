namespace App.Services;

public interface IMediaService
{
    Task<string> Upload(IFormFile file, MediaKind kind);
}

public enum MediaKind
{
    Video, Image
}

public class MediaService(IIdService idService, IWebHostEnvironment environment) : IMediaService
{
    public async Task<string> Upload(IFormFile file, MediaKind kind)
    {
        var id = idService.Generate();
        var path = GetPath(id, kind);

        using var fs = new FileStream(path, FileMode.CreateNew);
        await file.CopyToAsync(fs);
        return id;
    }

    readonly Dictionary<MediaKind, string> paths = new()
    {
        { MediaKind.Video, "video" },
        { MediaKind.Image, "image" },
    };

    string GetPath(string id, MediaKind kind)
    {
        var basePath = Path.Combine(environment.WebRootPath, paths[kind]);
        Directory.GetParent(basePath)!.Create();
        return Path.Combine(basePath, $"{id}.png");
    }
}