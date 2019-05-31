namespace Aotunomouse
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Loader;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Devices.Client;
    using Microsoft.Azure.Devices.Client.Transport.Mqtt;
    using StreamJsonRpc;
    using System.Net;
    using System.Net.Sockets;
    class Program
    {
        static int counter;
        static ModuleClient ioTHubModuleClient = null;
        static IRPCFuntions RpcClient = null;       
        static void Main(string[] args)
        {
            try
            {
                WaitDroneConnect();
            }
            catch (SystemException ex)
            {


                Console.WriteLine(DateTime.Now.ToString() + ":" + ex.Message);

            }

            var cts = new CancellationTokenSource();
            AssemblyLoadContext.Default.Unloading += (ctx) => cts.Cancel();
            Console.CancelKeyPress += (sender, cpe) => cts.Cancel();
            WhenCancelled(cts.Token).Wait();
        }

        /// <summary>
        /// Handles cleanup operations when app is cancelled or unloads
        /// </summary>
        public static Task WhenCancelled(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
            return tcs.Task;
        }

        static void WaitDroneConnect()
        {
            try
            {


                TcpListener listener = new TcpListener(IPAddress.Any, 23113);

                listener.Start();
                Console.WriteLine(DateTime.Now.ToString() + ":" + "Listening:23113");
                while (true)
                {

                    //线程会挂在这里，直到客户端发来连接请求
                    var TargetClientSocket = listener.AcceptSocket();

                    Task.Run(async () =>
                    {
                        var ip = (IPEndPoint)TargetClientSocket.RemoteEndPoint;

                        string RemoteMachineInfo = ip.Address.ToString() + ":" + ip.Port.ToString();

                        try
                        {
                            Console.WriteLine(DateTime.Now.ToString() + ":" + "Host ：" + RemoteMachineInfo + "Connected");


                            var ClientNetworkStream = new NetworkStream(TargetClientSocket, true);

                            byte[] recvbuffer = new byte[100];
                            int recvsize = await ClientNetworkStream.ReadAsync(recvbuffer, 0, 100, new CancellationToken());
                            var strrecv = Encoding.UTF8.GetString(recvbuffer);
                            var strport = strrecv.Substring(0, recvsize);
                            Console.WriteLine(DateTime.Now.ToString() + ":" + "Get DroneRPC Server   ：" + ip.Address.ToString() + ":" + strport);
                            Task.Run(async () =>
                            {
                                try
                                {
                                    await ConnectToServerAsync(ip.Address.ToString(), strport);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(DateTime.Now.ToString() + ":" + ex.ToString());
                                }

                            });
                            ClientNetworkStream.Dispose();
                            Console.WriteLine(DateTime.Now.ToString() + ":" + "Disconnected，Host ：" + RemoteMachineInfo + "");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(DateTime.Now.ToString() + ":" + "Host：" + RemoteMachineInfo + "Disconnected with errror! Because：" + ex.Message);
                        }

                    });

                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString() + ":" + ex.Message);
            }

        }
        static async Task ConnectToServerAsync(string strIP, string strPort)
        {

            IPAddress address = IPAddress.Parse(strIP);
            //创建TcpClient对象实现与服务器的连接
            TcpClient client = new TcpClient();
            //连接服务器
            await client.ConnectAsync(address, int.Parse(strPort));


            string RemoteMachineInfo = "";

            var stream = new NetworkStream(client.Client, true);

            RpcClient = JsonRpc.Attach<IRPCFuntions>(stream);


            var namereturn = await RpcClient.GetDroneNameAsync();

            if (namereturn.error == SDKError.NO_ERROR)
            {
                Console.WriteLine(DateTime.Now.ToString() + ":" + namereturn.value.Value.value);

            }
            else
            {
                Console.WriteLine(DateTime.Now.ToString() + ":" + client.Client.ToString() + ":" + namereturn.error);

            }

            RpcClient.flightControllerHandler_ConnectionChanged += async (object sender, BoolMsgEventArgs value) =>
            {
                Console.WriteLine(DateTime.Now.ToString() + ":" + "flightControllerHandler Connected");

                if (value.value.Value.value)
                {
                    Console.WriteLine(DateTime.Now.ToString() + ":" + RemoteMachineInfo + ":" + "flightControllerHandler Connected");

                }
                else
                {
                    Console.WriteLine(DateTime.Now.ToString() + ":" + "flightControllerHandler Disconnected");

                }
            };


            RpcClient.flightControllerHandler_IsFlyingChanged += async (object sender, BoolMsgEventArgs value) =>
             {


                 if (value.value.Value.value)
                 {
                     Console.WriteLine(DateTime.Now.ToString() + ":" + RemoteMachineInfo + ":" + "Drone Flying");

                 }
                 else
                 {
                     Console.WriteLine(DateTime.Now.ToString() + ":" + RemoteMachineInfo + ":" + "Drone Landed");

                 }

             };

            var takeoffret = await RpcClient.flightControllerHandler_StartTakeoffAsync();
            if (takeoffret != SDKError.NO_ERROR)
            {
                Console.WriteLine(DateTime.Now.ToString() + ":" + "flightControllerHandler_StartTakeoffAsync:" + takeoffret.ToString());
                
            }
            else
            {
                await Task.Delay(8000);
            }
           

            await RpcClient.flightControllerHandler_StartAutoLandingAsync();
            ((IDisposable)RpcClient).Dispose();
        }
    }
}
