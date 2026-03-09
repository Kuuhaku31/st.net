using CommunityToolkit.Mvvm.ComponentModel;

namespace ST06
{
    public partial class ViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _volume = 50;
    }
}
