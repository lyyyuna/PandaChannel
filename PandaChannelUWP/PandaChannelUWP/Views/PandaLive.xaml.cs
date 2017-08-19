using System;
using System.Collections.Generic;
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
using Windows.Web.Http;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using Windows.UI.Popups;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace PandaChannelUWP.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PandaLive : Page
    {
        public PandaLive()
        {
            this.InitializeComponent();
            getLiveUri();
        }

        private async void getLiveUri()
        {
            var httpClient = new HttpClient();
            var requestUri = new Uri("http://vdn.live.cntv.cn/api2/liveHtml5.do?channel=pa://cctv_p2p_hdipanda&client=flash");
            try
            {
                var httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                var httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                httpResponseBody = httpResponseBody.Substring(20);
                httpResponseBody = httpResponseBody.Substring(0, httpResponseBody.Length - 36);

                JObject html5Res = JObject.Parse(httpResponseBody);
                var results = html5Res["flv_url"]["flv2"];
                var uriStr = results.ToObject<String>();
                pandaTVElement.Source = new Uri(uriStr);
                pandaTVElement.AutoPlay = true;
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog("获取直播链接失败。若重复出现，联系作者修复。");
                await dialog.ShowAsync();
            }
        }
    }
}
