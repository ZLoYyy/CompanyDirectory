using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using CompanyDirectory.Infrastructure.Commands;
using CompanyDirectory.Interfaces;
using CompanyDirectory.Server.Entities;
using CompanyDirectory.ViewModels.Base;
using CompanyDirectory.Views.Windows.SprWondows;
using MathCore.WPF.Commands;
using Microsoft.EntityFrameworkCore;

namespace CompanyDirectory.ViewModels
{
    internal class SprPostViewModel : BaseViewModel
    {
        IRepository<Post> _repositoryPost;

        /// <summary>
        /// Выбранная запись
        /// </summary>
        private Post _selectedPost;
        public Post SelectedPost { get => _selectedPost; set => Set(ref _selectedPost, value); }

        #region Список должностей
        private CollectionViewSource _postsViewSource;
        private ObservableCollection<Post> _posts;
        public ICollectionView PostsView => _postsViewSource?.View;
        public ObservableCollection<Post> Posts
        {
            get => _posts;
            set
            {
                if (Set(ref _posts, value))
                {
                    _postsViewSource = new CollectionViewSource
                    {
                        Source = value,
                        SortDescriptions =
                        {
                            new SortDescription(nameof(Post.Caption), ListSortDirection.Ascending)
                        }
                    };

                    _postsViewSource.View.Refresh();

                    OnPropertyChanged(nameof(PostsView));
                }
            }
        }        
        #endregion

        #region Кнопки
        /// <summary>
        /// Создать
        /// </summary>
        private ICommand _buttonAddCommand;
        public ICommand ButtonAddCommand => _buttonAddCommand
            ??= new LambdaCommand(OnChangeAddCommandExecuted, CanChangeAddCommandExecute);
        private bool CanChangeAddCommandExecute(object p) => true;

        private void OnChangeAddCommandExecuted(object p)
        {
            var postEditorModel = new SprEditPostViewModel();

            var postEditorWindow = new SprEditPostWindow
            {
                DataContext = postEditorModel
            };

            if (postEditorWindow.ShowDialog() == true)
                _posts.Add(_repositoryPost.Add(postEditorModel.CurrentPost));

            SelectedPost = postEditorModel.CurrentPost;
        }

        /// <summary>
        /// Изменить
        /// </summary>
        private ICommand _buttonEditCommand;
        public ICommand ButtonEditCommand => _buttonEditCommand
            ??= new LambdaCommand(OnChangeEditCommandExecuted, CanChangeEditCommandExecute);
        private bool CanChangeEditCommandExecute(object p) => SelectedPost != null;

        private void OnChangeEditCommandExecuted(object p)
        {
            var postEditorModel = new SprEditPostViewModel(SelectedPost);

            var postEditorWindow = new SprEditPostWindow
            {
                DataContext = postEditorModel
            };

            if (postEditorWindow.ShowDialog() == true)
                _repositoryPost.Update(postEditorModel.CurrentPost);
        }
        /// <summary>
        /// Удалить
        /// </summary>
        private ICommand _buttonDeleteCommand;
        public ICommand ButtonDeleteCommand => _buttonDeleteCommand
            ??= new LambdaCommand<Post>(OnChangeDeleteCommandExecuted, CanChangeDeleteCommandExecute);
        private bool CanChangeDeleteCommandExecute(Post p) => p != null || SelectedPost != null;

        private void OnChangeDeleteCommandExecuted(Post p)
        {
            var postToRemove = p ?? SelectedPost;
            if(MessageBox.Show($"Вы хотите удалить должность {postToRemove.Caption}?", "Удаление должности", 
                MessageBoxButton.YesNo,MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;
            
            _repositoryPost.Remove(postToRemove.Id);

            Posts.Remove(postToRemove);
            if (ReferenceEquals(SelectedPost, postToRemove))
                SelectedPost = null;
        }
        #endregion

        #region Загрузка

        private ICommand _loadDataCommand;

        public ICommand LoadDataCommand => _loadDataCommand
            ??= new LambdaCommandAsync(OnLoadDataCommandExecuted);

        private async Task OnLoadDataCommandExecuted()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            if (Posts == null)
                Posts = new ObservableCollection<Post>();
            Posts.Clear();

            foreach(Post post in await _repositoryPost.Items.ToArrayAsync())
                Posts.Add(post);
        }
        #endregion

        public SprPostViewModel(IRepository<Post> repositoryPost)
        {
            _repositoryPost = repositoryPost;
        }
    }
}
