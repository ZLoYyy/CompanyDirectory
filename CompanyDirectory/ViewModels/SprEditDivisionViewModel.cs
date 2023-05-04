using CompanyDirectory.Interfaces;
using CompanyDirectory.Server.Entities;
using CompanyDirectory.Server.Entities.Base;
using CompanyDirectory.ViewModels.Base;
using CompanyDirectory.Views.Windows;
using MathCore.WPF.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace CompanyDirectory.ViewModels
{
    internal class SprEditDivisionViewModel : BaseViewModel
    {
        private Division _division;
        private IRepository<Post> _repositoryPost;
        public Division CurrentDivision { get => _division; set => Set(ref _division, value); }

        public SprEditDivisionViewModel()
        : this(new Division(), null) { }
        
        public SprEditDivisionViewModel(IRepository<Post> repositoryPost)
            : this(new Division(), repositoryPost)
        {
        }

        public SprEditDivisionViewModel(Division division, IRepository<Post> repositoryPost)
        {
            _division = division;
            _repositoryPost = repositoryPost;

            if (_division != null)
            {
                if (Posts == null)
                    Posts = new ObservableCollection<Post>();
                foreach (Post post in _division.Posts)
                    Posts.Add(post);
            }
        }        
        #region Список должностей
        /// <summary>
        /// общий список
        /// </summary>
        private List<Post> MainPostList { get; set; }

        /// <summary>
        /// Для отображения
        /// </summary>
        private CollectionViewSource _postViewSource;
        private ObservableCollection<Post> _posts;
        public ICollectionView PostsView => _postViewSource?.View;
        public ObservableCollection<Post> Posts
        {
            get => _posts;
            set
            {
                if (Set(ref _posts, value))
                {
                    _postViewSource = new CollectionViewSource
                    {
                        Source = value,
                        SortDescriptions =
                        {
                            new SortDescription(nameof(Post.Caption), ListSortDirection.Ascending)
                        }
                    };

                    _postViewSource.View.Refresh();

                    OnPropertyChanged(nameof(PostsView));
                }
            }
        }
        #endregion

        #region Кнопки
        /// <summary>
        /// Добавить
        /// </summary>
        private ICommand _buttonAddCommand;
        public ICommand ButtonAddCommand => _buttonAddCommand
            ??= new LambdaCommand(OnChangeAddCommandExecuted, CanChangeAddCommandExecute);
        private bool CanChangeAddCommandExecute(object p) => true;

        private void OnChangeAddCommandExecuted(object p)
        {
            var postSelectModel = new SelectedItemViewModel(MainPostList.ToList<NameEntity>());

            var postSelectWindow = new SelectedItemWindow
            {
                DataContext = postSelectModel
            };

            if (postSelectWindow.ShowDialog() != true || postSelectModel.SelectedItem == null)
                return;

            CurrentDivision.Posts.Add((Post)postSelectModel.SelectedItem);

            Posts.Add((Post)postSelectModel.SelectedItem);
            SelectedPost = (Post)postSelectModel.SelectedItem;
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
            if (MessageBox.Show($"Вы хотите удалить должность {postToRemove.Caption} из списка?", "Удаление должности",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            if(CurrentDivision.Posts.Select(P=>P.Id == postToRemove.Id) != null)
                CurrentDivision.Posts.Remove(postToRemove);
            if (Posts.Select(P => P.Id == postToRemove.Id) != null)
                Posts.Remove(postToRemove);

            if (ReferenceEquals(SelectedPost, postToRemove))
                SelectedPost = null;
        }
        #endregion

        #region Загрузка

        private ICommand _loadDataCommand;

        public ICommand LoadDataCommand => _loadDataCommand
            ??= new LambdaCommandAsync(OnLoadEmployeeCommandExecuted);

        private async Task OnLoadEmployeeCommandExecuted()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {            
            if (MainPostList == null)
                MainPostList = new List<Post>();
            MainPostList.Clear();

            foreach (Post post in await _repositoryPost.Items.Where(X => !CurrentDivision.Posts.Contains(X)).ToArrayAsync())
                MainPostList.Add(post);
        }
        #endregion

        private Post _selectedPost;
        public Post SelectedPost
        {
            get => _selectedPost;
            set => Set(ref _selectedPost, value);
        }
    }
}
