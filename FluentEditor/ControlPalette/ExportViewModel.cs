﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FluentEditor.ControlPalette.Export
{
    public class ExportViewModel : INotifyPropertyChanged
    {
        public ExportViewModel()
            : this(null) { }

        public ExportViewModel(string exportText)
        {
            _exportText = exportText;
        }

        private string _exportText;
        public string ExportText
        {
            get { return _exportText; }
            set
            {
                if(_exportText != value)
                {
                    _exportText = value;
                    RaisePropertyChangedFromSource();
                    RaisePropertyChanged("ReadyToCopy");
                }
            }
        }

        public bool ReadyToCopy
        {
            get
            {
                return !string.IsNullOrEmpty(_exportText);
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void RaisePropertyChangedFromSource([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
