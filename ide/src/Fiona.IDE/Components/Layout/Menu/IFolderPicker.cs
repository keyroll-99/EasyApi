﻿using System.Threading.Tasks;

namespace Fiona.IDE.Components.Layout.Menu
{
    public interface IFolderPicker
    {
        Task<string> PickFolder();
    }
}