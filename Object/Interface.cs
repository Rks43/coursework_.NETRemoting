using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;

namespace Proginterface
{
    public interface Interface
    {
        void Connecting();
        string Autorization(string login, string password);
        string Registration(string login, string password, string fio, string phone);
        int GetCodeUser(string login);
        List<string> GetReviews();
        List<string> GetProducts();
        string AddReview(int codeUser, string Feedback);
        string AddRequest(string service,string login, int cost);
        List<string> GetRequests();
        ILease MyInitializeLifetimeService();
        object InitializeLifetimeService();
        string RemoteMethod();
        string RemoveService(string name);
        string RemoveFeedback(string nameUser, string feedback);
        string AddService(string serviceName, int servicePrice);
        string DeleteRequest(int price);
    }
}