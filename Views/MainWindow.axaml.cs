using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SendEmail.ViewModels;
using ReactiveUI;

namespace SendEmail.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    { 
       
         public static MainWindow Current { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Current = this;
            SetupBinding();
        }

        private void SetupBinding()
        {

            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, 
                        x=>x.EmailForm,
                        x=>x.EmailForm.DataContext)
                    .DisposeWith(disposables);
            });
        }
    }
}