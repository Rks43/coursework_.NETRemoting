using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Lifetime;
using RemoteHello;
namespace HelloServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpServerChannel channel = new TcpServerChannel(8086);
            ChannelServices.RegisterChannel(channel, false);
            RemotingConfiguration.ApplicationName = "Hello";
            RemotingConfiguration.RegisterActivatedServiceType(typeof(Hello));
            LifetimeServices.LeaseTime = TimeSpan.FromMinutes(10);
            LifetimeServices.RenewOnCallTime = TimeSpan.FromMinutes(3);
            System.Console.WriteLine("press enter to exit");
            System.Console.ReadLine();
        }
    }
}