using Newtonsoft.Json.Linq;

namespace MMContest.Models
{
    public class Signed_Request
    {
        public string code { get; set; }
        public string algorithm { get; set; }
        public string issued_at { get; set; }
        public string fbuid { get; set; }
        public JObject user { get; set; }
        public string country { get; set; }
        public string locale { get; set; }
        public string oauth_token { get; set; }
        public string expires { get; set; }
        public string app_data { get; set; }
        public JObject page { get; set; }
        public string pageId { get; set; }
        public string pageLiked { get; set; }
    }
}
