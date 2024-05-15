namespace App.Services;

public interface IIdService
{
    string Generate();
}

public class IdService : IIdService
{

    public string Generate() =>
        shortid.ShortId.Generate(new(useSpecialCharacters: false, length: 12));

}
