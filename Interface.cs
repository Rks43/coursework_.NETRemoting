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
        List<List<string>> AddInfoUserList(string str);
        List<string> GetData(string str);
        string GetCategory(string str);
        string AddService(string service, string category, string paymenttype, int cost);
        string AddReview(string login, int service, int mark, int employee, int cost, string review);
        ILease MyInitializeLifetimeService();
    }
}