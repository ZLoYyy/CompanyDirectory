using CompanyDirectory.Interfaces;
using CompanyDirectory.Server.Entities;
using CompanyDirectory.Server.Entities.Base;
using CompanyDirectory.ViewModels.Base;
using MathCore.WPF.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace CompanyDirectory.ViewModels
{
    class SelectedItemViewModel : BaseViewModel
    {

        public string _title;

        public string Title { get => _title; set => Set(ref _title, value); }

        private List<NameEntity> _dataList;
        public List<NameEntity> DataList { get => _dataList; set => Set(ref _dataList, value); }
        public SelectedItemViewModel(List<NameEntity> dataList, string title)
        {
            _dataList = dataList;
        }

        /// <summary>
        /// Подразделение
        /// </summary>
        private NameEntity _selectedItem;
        public NameEntity SelectedItem { get => _selectedItem; set => Set(ref _selectedItem, value); }
        

    }
}
