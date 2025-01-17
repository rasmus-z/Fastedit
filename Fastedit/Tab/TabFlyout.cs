﻿using Fastedit.Dialogs;
using Fastedit.Helper;
using Fastedit.Storage;
using Microsoft.UI.Xaml.Controls;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Fastedit.Tab
{
    public class TabFlyout
    {
        public static MenuFlyout CreateFlyout(TabPageItem tab, TabView tabView)
        {
            var flyout = new MenuFlyout();
            flyout.Items.Add(CreateItem(tab, tabView, "Close", "\uE8BB", TabPageFlyoutItem.Close, VirtualKeyModifiers.Control, VirtualKey.W));
            flyout.Items.Add(new MenuFlyoutSeparator());
            flyout.Items.Add(CreateItem(tab, tabView, "Info", "\uE946", TabPageFlyoutItem.FileInfo, VirtualKeyModifiers.Control, VirtualKey.J));
            flyout.Items.Add(CreateItem(tab, tabView, "Share", Symbol.Share, TabPageFlyoutItem.Share));
            flyout.Items.Add(CreateItem(tab, tabView, "Save", Symbol.Save, TabPageFlyoutItem.Save, VirtualKeyModifiers.Control, VirtualKey.S));

            return flyout;
        }
        private static MenuFlyoutItem CreateItem(TabPageItem tab, TabView tabView, string Text, Symbol symbol, TabPageFlyoutItem type, VirtualKeyModifiers modifier = VirtualKeyModifiers.None, VirtualKey key = VirtualKey.None)
        {
            var item = CreateItem(tab, tabView, Text, type);
            item.Icon = new SymbolIcon { Symbol = symbol };

            //prevent adding a keyboard accelerator
            if (key == VirtualKey.None)
                return item;

            item.KeyboardAccelerators.Add(new KeyboardAccelerator
            {
                Key = key,
                Modifiers = modifier,
                IsEnabled = false
            });

            return item;
        }
        private static MenuFlyoutItem CreateItem(TabPageItem tab, TabView tabView, string Text, string glyph, TabPageFlyoutItem type, VirtualKeyModifiers modifier, VirtualKey key = VirtualKey.None)
        {
            var item = CreateItem(tab, tabView, Text, type);
            item.Icon = new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe MDL2 Assets"), Glyph = glyph };

            //prevent adding a keyboard accelerator
            if (key == VirtualKey.None)
                return item;

            item.KeyboardAccelerators.Add(new KeyboardAccelerator
            {
                Key = key,
                Modifiers = modifier,
                IsEnabled = false
            });
            return item;
        }

        private static MenuFlyoutItem CreateItem(TabPageItem tab, TabView tabView, string Text, TabPageFlyoutItem type)
        {
            var item = new MenuFlyoutItem
            {
                Text = Text,
                Tag = new TabFlyoutItemData(tab, tabView, type),
            };
            item.Click += Item_Click;
            return item;
        }

        private static async void Item_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem item && item.Tag is TabFlyoutItemData data)
            {
                switch (data.Item)
                {
                    case TabPageFlyoutItem.Save:
                        await SaveFileHelper.Save(data.Tab);
                        break;
                    case TabPageFlyoutItem.Close:
                        await TabPageHelper.CloseTab(data.TabView, data.Tab);
                        break;
                    case TabPageFlyoutItem.FileInfo:
                        await FileInfoDialog.Show(data.Tab);
                        break;
                    case TabPageFlyoutItem.Share:
                        ShareFileHelper.ShowShareUI(data.Tab);
                        break;
                }
            }
        }
    }
    public enum TabPageFlyoutItem
    {
        Save, Close, Share, FileInfo
    }
    public class TabFlyoutItemData
    {
        public TabFlyoutItemData(TabPageItem tab, TabView tabView, TabPageFlyoutItem item)
        {
            this.Item = item;
            this.Tab = tab;
            this.TabView = tabView;
        }
        public TabPageFlyoutItem Item { get; set; }
        public TabPageItem Tab { get; set; }
        public TabView TabView { get; set; }
    }
}
