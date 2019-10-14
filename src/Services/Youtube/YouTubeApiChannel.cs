using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace downr.Services.Youtube
{
    public class YouTubeApiChannel
    {
        private string Url => "https://www.googleapis.com/youtube/v3/search?key=AIzaSyCWRC9J27FKBDsFUEVXAyPFxiugRNBX_TM&channelId=UCP5Ons7fK3yKhX6lhc9XcfQ&part=snippet";

        private string Тoken => "AIzaSyCWRC9J27FKBDsFUEVXAyPFxiugRNBX_TM";

        public string Channel { get; set; }

        public async Task<string> GetVideos()
        {
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(this.Url);
            var strContent = await response.Content.ReadAsStringAsync();

            dynamic json = JObject.Parse(strContent);

         

            return strContent;
        }
    }
}
