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
using Microsoft.Toolkit.Uwp;
using System.Threading.Tasks;
using System.Threading;
using Windows.UI.Popups;
using Windows.Web.Http;
using Newtonsoft.Json.Linq;
using System.Text;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace PandaChannelUWP.Views
{

    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Album : Page
    {
        IncrementalLoadingCollection<PhotosSource, Photo> photos { get; set; }

        public Album()
        {
            photos = new IncrementalLoadingCollection<PhotosSource, Photo>();
            this.InitializeComponent();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            photos.RefreshAsync();
        }
    }

    public class Photo
    {
        public String Cover { get; set; }

        public String Title { get; set; }

        public List<String> Urls { get; set; }
    }

    public class PhotosSource : IIncrementalSource<Photo>
    {
        private readonly List<Photo> photos;

        public PhotosSource()
        {
            photos = new List<Photo>();
        }


        public async Task<IEnumerable<Photo>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            var photos = new List<Photo>();
            /*var item = new Photo();
            item.Cover = "http://p1.img.cctvpic.com/photoworkspace/2017/08/14/2017081415204475204.jpg";
            item.Title = "【图集】大熊猫“福豹”盛大的生日会";
            item.Urls = new List<string>();
            item.Urls.Add("http://p1.img.cctvpic.com/photoAlbum/photo/2017/08/14/PHOThPZwbCjxOD4s4WOrR2Hh170814_920x700.jpg");
            item.Urls.Add("http://p1.img.cctvpic.com/photoAlbum/photo/2017/08/14/PHOTh3CKb5BcDQLKXlH8qPbk170814_920x700.jpg");
            photos.Add(item);*/
            var httpClient = new HttpClient();
            var requestStr = "http://pandachannel.lihulab.net/news/photos?page=" + (pageIndex+1);
            var requestUri = new Uri(requestStr);

            try
            {
                //var httpClient = new HttpClient();
                var httpResponse = await httpClient.GetAsync(requestUri);

                if (httpResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    return photos;
                }

                //httpResponse.EnsureSuccessStatusCode();
                var bytes = await httpResponse.Content.ReadAsBufferAsync();
                var properEncodedString = Encoding.UTF8.GetString(bytes.ToArray());

                JObject res = JObject.Parse(properEncodedString);
                var results = res["results"].Children().ToList();
                var serverResults = new List<ServerResult>();
                foreach (var result in results)
                {
                    ServerResult serverResult = result.ToObject<ServerResult>();
                    serverResults.Add(serverResult);
                }

                foreach (var result in serverResults)
                {
                    var item = new Photo();
                    item.Cover = result.cover;
                    item.Title = result.title;
                    var imgurls = result.imgurls.Split(' ');
                    item.Urls = new List<string>();
                    foreach (var url in imgurls)
                    {
                        if (url.Length > 10)
                            item.Urls.Add(url);
                    }
                    photos.Add(item);
                }

            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog("发生网络错误。若重复出现，联系作者修复。");
                await dialog.ShowAsync();
            }

            return photos;
        }

        public class ServerResult
        {
            public string title { get; set; }
            public string cover { get; set; }
            public string imgurls { get; set; }
        }
    }

}
