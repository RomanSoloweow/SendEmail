using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reactive;
using System.Reactive.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Controls;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SendEmail.Models;
using SendEmail.Views;

namespace SendEmail.ViewModels
{
    public class EmailFormViewModel:ViewModelBase
    {
        [Reactive] public string From { get; set; }
        [Reactive] public string Password { get; set; }
        [Reactive] public string To { get; set; } 
        [Reactive] public string Subject { get; set; }
        [Reactive] public string Message { get; set; }
        [Reactive] public string? Server { get; set; }
        public ReadOnlyObservableCollection<string> SettingsForView;
        protected SourceList<ServeSettings> Settings { get; set; } = new();
        
        protected SourceList<string> Files = new();
        public ReadOnlyObservableCollection<string> FilesForView;
        public ReactiveCommand<Unit, Unit> SendEmailCommand =>  ReactiveCommand.CreateFromTask(SendEmail);
        public ReactiveCommand<Unit, Unit> AddFileCommand =>  ReactiveCommand.CreateFromTask(AddFile);
        public ReactiveCommand<Unit, Unit> DeleteSelectedFilesCommand =>  ReactiveCommand.CreateFromTask(DeleteSelectedFiles);
        public ReactiveCommand<Unit, Unit> DeleteAllFilesCommand =>  ReactiveCommand.CreateFromTask(DeleteAllFiles);
        public EmailFormViewModel()
        {
            Files.Connect().ObserveOn(RxApp.MainThreadScheduler).Bind(out FilesForView).Subscribe();
            Settings.Connect().Transform(x=>x.Name)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out SettingsForView).Subscribe();

            this.WhenAnyValue(x => x.From)
                .Subscribe(UpdateServer);
            
            using (FileStream fs = new("Servers.json", FileMode.Open))
            {
                var result = JsonSerializer.DeserializeAsync<List<ServeSettings>>(fs).Result;
                Settings.AddRange(result);
            }
        }

        public async Task SendEmail()
        {
            var server = Settings.Items.First(x => x.Name == Server);

            using (var msg = new MailMessage(From, To, Subject, Message))
            {
                using (var smtpClient = new SmtpClient(server.Server, server.Port))
                {
                    if (!string.IsNullOrEmpty(From))
                        smtpClient.Credentials = new NetworkCredential(From, Password);

                    foreach (var file in Files.Items)
                    {
                        msg.Attachments.Add(new Attachment(file)); 
                    }
                  
                    smtpClient.EnableSsl = true;
                    await smtpClient.SendMailAsync(msg).ConfigureAwait(false);
                }
            }
        }

        private async Task AddFile()
        {
            var dialog = new OpenFileDialog();
            dialog.AllowMultiple = true;
            var result = await dialog.ShowAsync(MainWindow.Current);
            Files.AddRange(result);
        }

        private async Task DeleteSelectedFiles()
        {
            var selectedItems = MainWindow.Current.EmailForm.Files.Selection.SelectedIndexes;
            foreach (var select in selectedItems)
            {
                Files.RemoveAt(select);
            }
        }

        private async Task DeleteAllFiles()
        {
            Files.Clear();
        }

        private void UpdateServer(string email)
        {
            var key = email?.Split(new[] {'@'}).Last().Trim();

            Server = Settings.Items.FirstOrDefault(x => x.Patterns.Any(
                x=>x.IndexOf(key, StringComparison.OrdinalIgnoreCase) == 0))?.Name;
        }
        
    }
}