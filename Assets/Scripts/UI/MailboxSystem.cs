using UnityEngine;
using UnityEngine.UI;

public class MailboxSystem : MonoBehaviour
{
    [SerializeField] Canvas inboxCanvas;
    [SerializeField] Canvas messageCanvas;

    [SerializeField] Text luluMessage;
    [SerializeField] Text juanjuanMessage;

    public static MailboxSystem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CloseMailbox();
        CloseMessageView();
    }

    public void OpenMailbox()
    {
        inboxCanvas.gameObject.SetActive(true);
    }

    public void CloseMailbox()
    {
        inboxCanvas.gameObject.SetActive(false);
    }

    private void OpenMessageView()
    {
        messageCanvas.gameObject.SetActive(true);
    }

    private void CloseMessageView()
    {
        luluMessage.gameObject.SetActive(false);
        juanjuanMessage.gameObject.SetActive(false);
        messageCanvas.gameObject.SetActive(false);
    }

    public void GoBackToMailbox()
    {
        CloseMessageView();
        OpenMailbox();
    }

    public void OpenLuluMessage()
    {
        CloseMailbox();
        OpenMessageView();
        luluMessage.gameObject.SetActive(true);
    }

    public void OpenJuanJuanMessage()
    {
        CloseMailbox();
        OpenMessageView();
        juanjuanMessage.gameObject.SetActive(true);
    }
}
