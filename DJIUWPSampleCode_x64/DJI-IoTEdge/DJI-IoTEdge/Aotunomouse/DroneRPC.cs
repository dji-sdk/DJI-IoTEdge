using DJI.WindowsSDK;
using DJI.WindowsSDK.Components;
using DJI_IoTEdge;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DroneRPC
{
    public enum ErrorCodeList
    {
        DroneSDKUninitialized = 1,
        DroneSDKVerSionUnsupport,
        IotEdgeOffLine
    }
    public class DroneRPCServerException : Exception
    {
        public DroneRPCServerException() { }

        public DroneRPCServerException(string message, Exception inner) : base(message, inner) { }

        public DroneRPCServerException(string message, int ErrorCode) : base(message)
        {
            this.ErrorCode = ErrorCode;
        }

        public override string Message
        {
            get
            {
                return base.Message;
            }
        }

        public int ErrorCode { get; }
    }

    public sealed class DroneRPCServer
    {
        //var component = DJISDKManager.Instance.ComponentManager;
        int M_port;

        public delegate void _Outputlog(string text);
        _Outputlog WriteLogFunction = null;

        public DroneRPCServer()
        {
            M_port = 0;
        }

        public void SetLogFuntion(MainPageViewModel viewModel)
        {
            WriteLogFunction = viewModel.OutputLog;
        }

        void WriteLog(string text)
        {
            WriteLogFunction?.Invoke("Aotunomouse Log: " + text);
        }
        public void StartRpcServer(int Serverport)
        {
            //if (DJISDKManager.SDKVersion != "0.1.0")
            //{
            //    throw new DroneRPCServerException("DJIWINDOWSSDK's vertion is unsupported ", (int)ErrorCodeList.DroneSDKVerSionUnsupport);
            //}

            if (DJISDKManager.Instance.ComponentManager == null || DJISDKManager.Instance.ComponentManager.GetFlightAssistantHandler(0, 0) == null)
            {
                WriteLog("DJIWINDOWSSDK is not initialized or is not unregistered");
                return;
            }

            M_port = Serverport;


            Task.Run(() =>
            {
                TcpListener listener = new TcpListener(IPAddress.Any, Serverport);

                listener.Start();

                while (true)
                {
                    WriteLog("等待连接");

                    var mysocket = listener.AcceptSocket();
                    Task.Run(async () =>
                    {
                        var ip = (IPEndPoint)mysocket.RemoteEndPoint;
                        string RemoteMachineInfo = ip.Address.ToString() + ":" + ip.Port.ToString();
                        try
                        {
                            WriteLog("主机 ：" + RemoteMachineInfo + "已经连接");

                            var myNetworkStream = new NetworkStream(mysocket, true);

                            JsonRpc rpc = JsonRpc.Attach(myNetworkStream, new DroneRpcFuntions());


                            await rpc.Completion;

                            WriteLog("主机 ：" + RemoteMachineInfo + "断开连接");
                        }
                        catch (Exception ex)
                        {
                            WriteLog("主机：" + RemoteMachineInfo + "异常退出! 原因为：" + ex.Message);
                        }

                    });

                }

            });

        }



        public void ConnectToIotEdge(string TagertIP, int TagertPort)
        {

            IPAddress address = IPAddress.Parse(TagertIP);

            TcpClient client = new TcpClient();

            client.Connect(address, TagertPort);

            NetworkStream ns = client.GetStream();

            string Sendmessage = M_port.ToString();


            Byte[] sendBytes = Encoding.UTF8.GetBytes(Sendmessage);
            ns.Write(sendBytes, 0, sendBytes.Length);

            ns.Close();
            client.Close();
        }



    }
}
