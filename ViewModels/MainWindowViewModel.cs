using ReactiveUI.Fody.Helpers;

namespace SendEmail.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        [Reactive] public EmailFormViewModel EmailForm { get; set; } = new();
    }
}
