using TinyMessenger;

namespace RofiSdk.Models
{
    public class AdsCallback : TinyMessageBase
    {
        public int requestCode;

        public AdsCallback(object sender) : base(sender)
        {
            
        }
    }
}