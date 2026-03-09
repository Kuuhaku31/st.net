using CommunityToolkit.Mvvm.ComponentModel;

namespace st06
{
    public partial class ViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _volume = 50;

        [ObservableProperty]
        private string _name = "";
    }
}
