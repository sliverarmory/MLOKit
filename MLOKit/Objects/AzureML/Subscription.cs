using System;

namespace MLOKit.Objects.AzureML
{
    class Subscription
    {


        public string subscriptionID { get; set; }
        public string subscriptionDisplayName { get; set; }
        public string subscriptionState { get; set; }

        public Subscription(string subscriptionID, string subscriptionDisplayName, string subscriptionState)
        {
            this.subscriptionID = subscriptionID;
            this.subscriptionDisplayName = subscriptionDisplayName;
            this.subscriptionState = subscriptionState;


        }


    }
}
