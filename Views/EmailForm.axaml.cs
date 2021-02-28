using System.Reactive.Disposables;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SendEmail.ViewModels;
using ReactiveUI;

namespace SendEmail.Views
{
    public partial class EmailForm : ReactiveUserControl<EmailFormViewModel>
    {
        public EmailForm()
        {
            InitializeComponent();
            SetupBinding();
        }

        private void SetupBinding()
        {
            this.WhenActivated(disposables =>
            {

                this.Bind(ViewModel, 
                        x=>x.From,
                        x=>x.From.Text)
                    .DisposeWith(disposables);
                this.Bind(ViewModel, 
                        x=>x.Password,
                        x=>x.Password.Text)
                    .DisposeWith(disposables);
                this.Bind(ViewModel, 
                        x=>x.To,
                        x=>x.To.Text)
                    .DisposeWith(disposables);
                
                this.Bind(ViewModel, 
                        x=>x.Subject,
                        x=>x.Subject.Text)
                    .DisposeWith(disposables);
                this.Bind(ViewModel, 
                        x=>x.Message,
                        x=>x.Message.Text)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, 
                        x=>x.SettingsForView, 
                        x=>x.Servers.Items)
                    .DisposeWith(disposables);
                this.Bind(ViewModel, 
                        x=>x.Server, 
                        x=>x.Servers.SelectedItem)
                    .DisposeWith(disposables);
                
                this.BindCommand(ViewModel, 
                        x=>x.SendEmailCommand, 
                        x=>x.SendEmail)
                    .DisposeWith(disposables);
                
                this.OneWayBind(ViewModel, 
                        x=>x.FilesForView, 
                        x=>x.Files.Items)
                    .DisposeWith(disposables);
                
                this.BindCommand(ViewModel, 
                        x=>x.AddFileCommand, 
                        x=>x.AddFileButton)
                    .DisposeWith(disposables);
                
                this.BindCommand(ViewModel, 
                        x=>x.DeleteSelectedFilesCommand, 
                        x=>x.DeleteSelectedButton)
                    .DisposeWith(disposables);
                
                this.BindCommand(ViewModel, 
                        x=>x.DeleteAllFilesCommand, 
                        x=>x.DeleteAllButton)
                    .DisposeWith(disposables);
                
               
               
            });

        }
    }
}