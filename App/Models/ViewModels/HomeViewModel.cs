namespace App.Models;

public class HomeViewModel(string initialRequestUrl)
{
    public string InitialRequestUrl => initialRequestUrl;
}