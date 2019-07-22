using Android.App;

namespace Ultz.XNFC.Android
{
    public class XNFCPlatform
    {
        public static ICrossNearFieldCommunication Init(Activity activity)
        {
            XNFC.GloballyAssociate(activity);
            XNFC.Register<AndroidNfc>();
            var api= XNFC.GetApi();
            return api;
        }
    }
}