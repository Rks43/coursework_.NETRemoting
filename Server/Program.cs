using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Lifetime;
using RemoteObject;
using System.Collections;
using System.Runtime.Serialization.Formatters;

namespace HelloServer
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Server is worked. 'Enter' to exit");
            RegisterTCP();
            RegisterHTTP();
            Console.ReadLine();
        }
        static void RegisterTCP()
        {
            // Запуск TCP сервера
            BinaryServerFormatterSinkProvider serverProv = new BinaryServerFormatterSinkProvider();
            serverProv.TypeFilterLevel = TypeFilterLevel.Full;
            IDictionary props = new Hashtable();
            props["port"] = 8086;
            props["name"] = "TCPChannel";
            props["typeFilterLevel"] = TypeFilterLevel.Full;
            props["impersonate"] = false;
            props["secure"] = true;
            props["protectionLevel"] = System.Net.Security.ProtectionLevel.EncryptAndSign;
            TcpChannel tcpChannel = new TcpChannel(props, null, serverProv);
            ChannelServices.RegisterChannel(tcpChannel, true);

            RemotingConfiguration.ApplicationName = "tcp";
            RemotingConfiguration.RegisterActivatedServiceType(typeof(ServerObject));
            LifetimeServices.LeaseTime = TimeSpan.FromMinutes(10);
            LifetimeServices.RenewOnCallTime = TimeSpan.FromMinutes(1);
            Console.WriteLine("TCP_Channel was created");
        }
        static void RegisterHTTP()
        {
            //  Запуск HTTP сервера
            RemotingConfiguration.Configure(@"C:\Users\User\Desktop\КАИ\3 курс\2 семак\ТП\Курсач\Моё\Код\Login\Server\App1.config", false);
            Console.WriteLine("HTTP_Channel was created");
        }
    }
}