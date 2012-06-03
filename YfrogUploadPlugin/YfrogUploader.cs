using Acuerdo.External.Uploader;
using Dulcet.Twitter.Credential;

namespace YfrogUploader
{
    public class YfrogUploader : IUploader
    {
        //re4k 02
        private static string _appk = "019BHOXY3185ce37407b974330b77e48c47fcd4c";

        //commentは捨てる - APIにない
        public string UploadImage(OAuth credential, string path, string comment)
        {
            string url;
            if (!YfrogApi.UploadToYfrog(credential, _appk, path, out url))
            {
                return null;
            }
            else
            {
                return url;
            }
        }

        public string ServiceName
        {
            get { return "yfrog"; }
        }

        public bool IsResolvable(string url)
        {
            return false;
        }

        public string Resolve(string url)
        {
            return null;
        }
    }
}
