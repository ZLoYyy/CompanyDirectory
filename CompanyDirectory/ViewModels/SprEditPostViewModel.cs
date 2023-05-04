using CompanyDirectory.Server.Entities;
using CompanyDirectory.ViewModels.Base;
using MathCore.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CompanyDirectory.ViewModels
{
    internal class SprEditPostViewModel : BaseViewModel
    {
        private Post _currentPost;

        public Post CurrentPost { get => _currentPost; set => Set(ref _currentPost, value); }

        public SprEditPostViewModel()
            : this(new Post())
        {
        }

        public SprEditPostViewModel(Post post)
        {
            _currentPost = post;
        }
    }
}
