namespace App.Models.ViewModels;

public class ListResultViewModel
{
    public ListResultViewModel(params (string name, object viewModel)[] partials)
    {
        Partials.AddRange(partials);
    }

    public ListResultViewModel(IEnumerable<(string name, object viewModel)> partials)
    {
        Partials.AddRange(partials);
    }

    public List<(string name, object viewModel)> Partials { get; } = [];
}