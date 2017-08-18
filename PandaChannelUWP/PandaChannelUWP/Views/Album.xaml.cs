using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace PandaChannelUWP.Views
{

    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Album : Page
    {
        private ObservableCollection<Photo> photos { get; set; }

        public Album()
        {
            photos = new ObservableCollection<Photo>();
            this.InitializeComponent();
            getBatchPhotos();
        }

        private void getBatchPhotos()
        {
            var items = new List<Photo>();
            var item = new Photo();
            item.Cover = "http://p1.img.cctvpic.com/photoworkspace/2017/08/16/2017081617324770369.jpg";
            item.Title = "【图集】盼盼园雨后凉爽的日子";
            item.Urls = new List<string>();
            item.Urls.Add("http://p1.img.cctvpic.com/photoAlbum/photo/2017/08/16/PHOT0C06M1ytVixCnrgULuat170816_920x700.jpg");
            item.Urls.Add("http://p1.img.cctvpic.com/photoAlbum/photo/2017/08/16/PHOTAg74o7xG45vs7aj0Bh0m170816_920x700.jpg");

            items.Add(item);
            items.ForEach(p => photos.Add(p));
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var items = new List<Photo>();
            var item = new Photo();
            item.Cover = "http://p1.img.cctvpic.com/photoworkspace/2017/08/14/2017081415204475204.jpg";
            item.Title = "【图集】大熊猫“福豹”盛大的生日会";
            item.Urls = new List<string>();
            item.Urls.Add("http://p1.img.cctvpic.com/photoAlbum/photo/2017/08/14/PHOThPZwbCjxOD4s4WOrR2Hh170814_920x700.jpg");
            item.Urls.Add("http://p1.img.cctvpic.com/photoAlbum/photo/2017/08/14/PHOTh3CKb5BcDQLKXlH8qPbk170814_920x700.jpg");

            items.Add(item);
            items.ForEach(p => photos.Add(p));
        }
    }

    public class Photo
    {
        public string Cover { get; set; }

        public string Title { get; set; }

        public List<String> Urls { get; set; }
    }

}
