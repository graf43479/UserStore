
namespace UserStore.BLL.Infrastructure
{
    public class OperationDetails
    {
        public OperationDetails(bool succedeed, string[] messages, string prop)
        {
            Succedeed = succedeed;
            Messages = messages;
            Property = prop;
        }

        public OperationDetails(bool succedeed, string message, string prop)
        {
            Succedeed = succedeed;
            Messages =new string[] { message };
            Property = prop;
        }

        public bool Succedeed { get; private set; }
        public string[] Messages { get; private set; }
        public string Property { get; private set; }

        
    }
}
