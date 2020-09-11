using System;
using System.Collections.Generic;
using Prism.Mvvm;

namespace TreeViewControlWpfApp.TreeViewControl {
    public abstract class BaseClass: BindableBase
    {
        public Folder Parent {get; set;}

        public Type Type => GetType();

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (!value)
                {
                    IsEdit = false;
                    if (string.IsNullOrWhiteSpace(Name))
                    {
                        Name = GetDefaultName(Parent);
                    }
                }
                SetProperty(ref _isSelected, value);
            }
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }

        private bool _isEdit;
        public bool IsEdit
        {
            get => _isEdit;
            set => SetProperty(ref _isEdit, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        protected abstract string GetDefaultName(Folder parent);

        public static string GetPath(Folder parent)
        {
            var folder = parent;
            var paths = new List<string> {folder.Name};
            while (folder.Parent != null)
            {
                folder = folder.Parent;
                paths.Add(folder.Name);
            }
            paths.Reverse();
            var path = string.Join("\\", paths);
            return path;
        }
    }
}