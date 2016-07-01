using Microsoft.Bot.Connector.DirectLine;
using Microsoft.Bot.Connector.DirectLine.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace HuntTheWumpus
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            KeyDown += (s, e) =>
            {
                if (e.Key == Windows.System.VirtualKey.Enter)
                {
                    SendButton_Click(s, e);
                }
            };
        }

        public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();

        private DirectLineClient _directLine;
        private string _conversationId;

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            _directLine = new DirectLineClient("xkzv4Vz_910.cwA.y3w.Y1bblOqVLBqf6mJDvGejaIvI6i2QhjF7I1-ZadnJpo4");
            var conversation = await _directLine.Conversations.NewConversationWithHttpMessagesAsync();
            _conversationId = conversation.Body.ConversationId;
        }

        private async Task GetMessages()
        {
            var httpMessages = await _directLine.Conversations.GetMessagesWithHttpMessagesAsync(_conversationId);
            var messages = httpMessages.Body.Messages;

            foreach (var message in messages.Where(httpMessage => !Messages.Any(existingMessage => existingMessage.Id == httpMessage.Id)))
            {
                Messages.Add(message);
            }
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            var message = MessageToSendTextBox.Text;
            MessageToSendTextBox.Text = string.Empty;

            await _directLine.Conversations.PostMessageAsync(_conversationId, new Message { Text = message });
            await GetMessages();
        }
    }
}
