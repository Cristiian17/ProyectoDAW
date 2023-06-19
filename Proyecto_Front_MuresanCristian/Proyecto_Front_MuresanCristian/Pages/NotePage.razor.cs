using Microsoft.AspNetCore.Components;
using Proyecto_Front_MuresanCristian.Services.Interfaces;
using Proyecto_Front_MuresanCristian.Services;
using Proyecto_Front_MuresanCristian.Entities;
using Blazored.LocalStorage;
using System.Collections.ObjectModel;
using Blazorise;
using System.Xml.Serialization;
using Blazorise.Snackbar;
using Blazorise.RichTextEdit;
using Microsoft.AspNetCore.Components.Forms;
using Blazorise.Extensions;

namespace Proyecto_Front_MuresanCristian.Pages
{
    public partial class NotePage
    {
        private Note selectedNote { get; set; }
        private Modal AddNoteModal;
        private Modal EditNoteModal;
        private Modal SharedEmailModal;
        private Modal AboutMeModal;
        private string noteTitle = string.Empty;
        private string SharedEmail = string.Empty;
        //private string noteContent = string.Empty;
        #region DI
        [Inject]
        private IRestService MyRestService { get; set; }
        [Inject]
        protected NavigationManager Navigation { get; set; }
        [Inject]
        private DialogService DialogService { get; set; }
        [Inject]
        private ILocalStorageService LocalStorage { get; set; }
        #endregion

        private UserInfo user { get; set; }
        public ObservableCollection<Note> MyNotes { get; set; }
        public ObservableCollection<Note> SharedNotes { get; set; }
        public ObservableCollection<Note> FavNotes { get; set; }
        public bool MyNotesVisible { get; set; } = true;
        public bool SharedNotesVisible { get; set; }
        public bool FavNotesVisible { get; set; }
        public Snackbar MessageSnackbar;
        public Snackbar SharedOKSnackbar;
        public Snackbar SharedFailSnackbar;

        public RichTextEdit richTextAdd;
        public RichTextEdit richTextEdit;

        protected override Task OnInitializedAsync()
        {
            MyNotes = new ObservableCollection<Note>();
            SharedNotes = new ObservableCollection<Note>();
            FavNotes = new ObservableCollection<Note>();
            return base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                user = await LocalStorage.GetItemAsync<UserInfo>("user");
                if(user == null)
                {
                    Navigation.NavigateTo("/", true);
                    return;
                }

                var notes = await MyRestService.GetUserNotes(user);
                var sharedNotes = await MyRestService.GetSharedUserNotes(user);

                if (notes != null)
                {
                    MyNotes = new ObservableCollection<Note>(notes);
                    FavNotes = new ObservableCollection<Note>(MyNotes.Where(x => x.Favorite));
                    StateHasChanged();
                }
                if(sharedNotes != null)
                {
                    SharedNotes = new ObservableCollection<Note>(sharedNotes);
                }
                await base.OnInitializedAsync();
            }
        }

        private Task ShowModal()
        {
            return AddNoteModal.Show();
        }
        private Task ShowAboutMe()
        {
            return AboutMeModal.Show();
        }
        private Task HideAboutMeModal()
        {
            return AboutMeModal.Hide();
        }

        private async Task HideAddModal()
        {
            await richTextAdd.ClearAsync();
            noteTitle = string.Empty;
            await AddNoteModal.Hide();
        }
        private async Task HideEditModal()
        {
            await richTextEdit.ClearAsync();
            noteTitle = string.Empty;
            await EditNoteModal.Hide();
        }
        private async Task AddNote()
        {
            try
            {
                var plainText = (await richTextAdd.GetTextAsync()).Replace("\n", string.Empty);
                var htmlText = await richTextAdd.GetHtmlAsync();

                if (string.IsNullOrEmpty(noteTitle) || string.IsNullOrEmpty(plainText))
                {
                    await DialogService.AlertAsync("", "No puedes dejar campos vacios");
                    return;
                }
                var user = await LocalStorage.GetItemAsync<UserInfo>("user");
                Note newNote = new()
                {
                    Title = noteTitle,
                    Content = htmlText,
                    UserId = user.Id ?? 0

                };
                var addedNote = await MyRestService.AddNote(newNote, user);
                MyNotes.Insert(0,addedNote);

                await HideAddModal();

            } catch (Exception ex)
            {
                await DialogService.AlertAsync("", ex.Message);
            }
        }
        
        private void ShowMyNotes()
        {
            MyNotesVisible = true;
            SharedNotesVisible = false;
            FavNotesVisible = false;
        }

        private void ShowSharedNotes()
        {
            MyNotesVisible = false;
            SharedNotesVisible = true;
            FavNotesVisible = false;
        }

        private void ShowFavNotes()
        {
            MyNotesVisible = false;
            SharedNotesVisible = false;
            FavNotesVisible = true;
        }

        private async Task EditNote(Note note)
        {
            noteTitle = note.Title;
            await richTextEdit.SetHtmlAsync(note.Content);
            await EditNoteModal.Show();
            selectedNote = note;  
        }

        private async Task ShareNoteModal()
        {
            await SharedEmailModal.Show();

        }
        private async Task ShareNote()
        {
            if (SharedEmail.IsNullOrEmpty())
            {
                await SharedEmailModal.Hide();
                await DialogService.AlertAsync("", "No puedes dejar campos vacios");
                SharedEmail = string.Empty;
                return;
            }
            var userInfo = await LocalStorage.GetItemAsync<UserInfo>("user");
            if(SharedEmail == userInfo.Email)
            {
                await SharedEmailModal.Hide();
                await DialogService.AlertAsync("", "No puedes compartir la nota contigo");
                SharedEmail = string.Empty;
                return;
            }
            if (!await MyRestService.GetUserByEmail(SharedEmail, userInfo))
            {
                await SharedEmailModal.Hide();
                await DialogService.AlertAsync("", "El email no esta registrado");
                SharedEmail = string.Empty;
                return;
            }
            var result = await MyRestService.AddSharedNote(SharedEmail, selectedNote, userInfo);
            SharedEmail = string.Empty;
            if (result)
            {
                await SharedEmailModal.Hide();
                await SharedOKSnackbar.Show();
            }
            else
            {
                await SharedEmailModal.Hide();
                await SharedFailSnackbar.Show();
            }

        }
        private async Task EditNote()
        {
            try
            {
                var plainText = (await richTextEdit.GetTextAsync()).Replace("\n", string.Empty);
                var htmlText = await richTextEdit.GetHtmlAsync();
                if (string.IsNullOrEmpty(noteTitle) || string.IsNullOrEmpty(plainText))
                {
                    await DialogService.AlertAsync("", "No puedes dejar campos vacios");
                    return;
                }
                var user = await LocalStorage.GetItemAsync<UserInfo>("user");
                selectedNote.Content = htmlText;
                selectedNote.Title = noteTitle;
                var addedNote = await MyRestService.EditNote(selectedNote, user);
                await MessageSnackbar.Show();
                await HideEditModal();
            }
            catch (Exception ex)
            {
                await DialogService.AlertAsync("", ex.Message);
            }
        }
        
        private async Task DeleteNote(Note note)
        {
            try
            {
                var user = await LocalStorage.GetItemAsync<UserInfo>("user");
                var deleted = await MyRestService.DeleteNote(note, user);
                if (!deleted)
                {
                    await DialogService.AlertAsync("", "No se ha pidido borrar la nota");
                    return;
                }
                MyNotes.Remove(note);
            }
            catch (Exception ex) 
            {
                await DialogService.AlertAsync("", ex.Message);
            }

        }
        private async Task AddFav(Note note)
        {
            try
            {
                var user = await LocalStorage.GetItemAsync<UserInfo>("user");
                var addedNote = await MyRestService.AddFav(note, user);
                if (addedNote)
                {
                    note.Favorite = true;
                    FavNotes.Add(note);
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await DialogService.AlertAsync("", ex.Message);
            }
        }
        private async Task RemoveFav(Note note)
        {
            try
            {
                var user = await LocalStorage.GetItemAsync<UserInfo>("user");
                var removed = await MyRestService.RemoveFav(note, user);
                if (!removed)
                {
                    await DialogService.AlertAsync("", "No se ha pidido eliminar la nota de favoritos");
                    return;
                }
                note.Favorite = false;
                FavNotes.Remove(note);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await DialogService.AlertAsync("", ex.Message);
            }

        }
        private async Task Logout()
        {
            await LocalStorage.ClearAsync();
            Navigation.NavigateTo("/");
        }
    }
}
