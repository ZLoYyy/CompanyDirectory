using CompanyDirectory.Infrastructure.Commands;
using CompanyDirectory.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace CompanyDirectory.ViewModels
{
    internal class SprManageViewModel : BaseViewModel
    {
        public SprManageViewModel()
        {
            /*_sprCompanyPage = new Views.Pages.SubPages.SprCompanyPage();
            _sprDivisionPage = new Views.Pages.SubPages.SprDivisionPage();
            _sprEmployeePage = new Views.Pages.SubPages.SprEmployeePage();
            _sprPostPage = new Views.Pages.SubPages.SprPostPage();

            _currentPage = _sprCompanyPage;

            ButtonCompanyPageCommand = new GeneralComand(OnChangeCompanyPageCommandExecuted, CanChangeCompanyPageCommandExecute);
            ButtonDivisionPageCommand = new GeneralComand(OnChangeDivisionPageCommandExecuted, CanChangeDivisionPageCommandExecute);
            ButtonEmployeePageCommand = new GeneralComand(OnChangeEmployeePageCommandExecuted, CanChangeEmployeePageCommandExecute);
            ButtonPostPageCommand = new GeneralComand(OnChangePostPageCommandExecuted, CanChangePostPageCommandExecute);


            ButtonPageCommand = new GeneralComand(OnChangePageCommandExecuted, CanChangePageCommandExecute);



            ButtonAddCommand = new GeneralComand(OnChangeAddCommandExecuted, CanChangeAddCommandExecute);
            ButtonEditCommand = new GeneralComand(OnChangeEditCommandExecuted, CanChangeEditCommandExecute);
            ButtonDeleteCommand = new GeneralComand(OnChangeDeleteCommandExecuted, CanChangeDeleteCommandExecute);*/
        }


        #region ChangePages

        private Page _sprCompanyPage;
        private Page _sprDivisionPage;
        private Page _sprEmployeePage;
        private Page _sprPostPage;

        private Page _currentPage;
        public List<string> PullOfSelectedPages { get => new List<string>() { "Компании", "Подразделения", "Работники", "Должности" }; }
        public Page CurrentPage { get => _currentPage; set => Set(ref _currentPage, value); }

        public ICommand ButtonPageCommand { get; set; }
        private bool CanChangePageCommandExecute(object p) => _currentPage != _sprCompanyPage;

        private void OnChangePageCommandExecuted(object p)
        {
            CurrentPage = _sprCompanyPage;
        }


        public ICommand ButtonCompanyPageCommand { get; set; }
        private bool CanChangeCompanyPageCommandExecute(object p) => _currentPage != _sprCompanyPage;

        private void OnChangeCompanyPageCommandExecuted(object p)
        {
            CurrentPage = _sprCompanyPage;
        }
        public ICommand ButtonDivisionPageCommand { get; set; }
        private bool CanChangeDivisionPageCommandExecute(object p) => _currentPage != _sprDivisionPage;

        private void OnChangeDivisionPageCommandExecuted(object p)
        {
            CurrentPage = _sprDivisionPage;
        }
        public ICommand ButtonEmployeePageCommand { get; set; }
        private bool CanChangeEmployeePageCommandExecute(object p) => _currentPage != _sprEmployeePage;

        private void OnChangeEmployeePageCommandExecuted(object p)
        {
            CurrentPage = _sprEmployeePage;
        }
        public ICommand ButtonPostPageCommand { get; set; }
        private bool CanChangePostPageCommandExecute(object p) => _currentPage != _sprPostPage;

        private void OnChangePostPageCommandExecuted(object p)
        {
            CurrentPage = _sprPostPage;
        }

        #endregion

        #region ButtonsControls
        /// <summary>
        /// Создать запись
        /// </summary>
        public ICommand ButtonAddCommand { get; set; }
        private bool CanChangeAddCommandExecute(object p) => _currentPage != _sprCompanyPage;

        private void OnChangeAddCommandExecuted(object p)
        {
            CurrentPage = _sprCompanyPage;
        }
        /// <summary>
        /// Изменить запись
        /// </summary>
        public ICommand ButtonEditCommand { get; set; }
        private bool CanChangeEditCommandExecute(object p) => _currentPage != _sprCompanyPage;

        private void OnChangeEditCommandExecuted(object p)
        {
            CurrentPage = _sprCompanyPage;
        }
        /// <summary>
        /// Удалить запись
        /// </summary>
        public ICommand ButtonDeleteCommand { get; set; }
        private bool CanChangeDeleteCommandExecute(object p) => _currentPage != _sprCompanyPage;

        private void OnChangeDeleteCommandExecuted(object p)
        {
            CurrentPage = _sprCompanyPage;
        }
        #endregion


    }
}
