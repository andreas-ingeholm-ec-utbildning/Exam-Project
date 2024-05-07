namespace App.Services;

public static class Service
{
    static IServiceProvider serviceProvider = null!;
    public static void Initialize(IServiceProvider serviceProvider) =>
        Service.serviceProvider = serviceProvider;

    public static T Get<T>() =>
        serviceProvider.GetService<T>()!;
}