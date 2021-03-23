using ImaginationOverflow.AndroidInstallReferrer;
using ImaginationOverflow.AndroidInstallReferrer.JavaInterop;
using UnityEngine;
using UnityEngine.UI;

public class DeferralDeepLinkDemo : MonoBehaviour
{
    public Text InstallReferer;
    public Text PlayInstant;
    public Text InstallTime;
    public Text ReferrerClickTime;
    public Text Error;
    public Text IsException;

    public Button Button;



    void Start()
    {
        InstallTime.text =
            InstallReferer.text =
                PlayInstant.text =
                    ReferrerClickTime.text =
                        Error.text =
                            IsException.text = string.Empty;
    }

    // Use this for initialization
    public void ButtonClicked()
    {
        Button.interactable = false;

        //
        //  Enables caching, by default its set to true
        // 
        // InstallReferrerManager.Instance.CacheReferrerInformation = true;

        InstallReferrerManager.Instance.ReferrerInformationCollected += Instance_ReferrerInformationCollected;

        //
        //  Set triggerUniversalDeepLinkEvent to true in order to activate universal deep link integration
        //
        InstallReferrerManager.Instance.FetchInformationCollected(triggerUniversalDeepLinkEvent:false);

    }

    
    private void Instance_ReferrerInformationCollected(InstallReferrerInfo data)
    {
        Button.interactable = true;
        if (string.IsNullOrEmpty(data.Error) == false)
        {
            Debug.LogErrorFormat("Deferred Deep Link Error!\n{0}\nIs Exception: {1}", data.Error, data.IsException);
            Error.text = data.Error;
            IsException.text = data.IsException.ToString();
            return;
        }

        InstallTime.text = data.InstallTime.ToString("G");
        InstallReferer.text = data.InstallReferrer;
        PlayInstant.text = data.GooglePlayInstant.ToString();
        ReferrerClickTime.text = data.ReferrerClickTime.ToString("G");

    }

}
