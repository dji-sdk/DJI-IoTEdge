using DroneRPC;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace DJI_IoTEdge
{
    public class MainPageViewModel : INotifyPropertyChanged
    {

        private CoreDispatcher Dispatcher;
        private WindowsSDKManager windowsSDKManager;
        private MainPage parent;
        private DroneRPCServer dronerpcserver = new DroneRPCServer();

        private BlobStorageOnEdge blobStorageOnEdge;
        private VisionAI visionAI;



        Task BlobStorageTask = null;
        Task AITask = null;



        const string DJI_DEV_KEY = "<Input your drone sdk key>";

        const string StorageAcount = "<Input your StorageAcount >";
        const string StorageAccessKey = "<Input your StorageAccessKey >";

        const string AIEndPointIP = "127.0.0.1";
        const string AIEndPointPort = "23114";


        public MainPageViewModel(CoreDispatcher dispatcher, MainPage page)
        {
            this.Dispatcher = dispatcher;
            parent = page;



            InitializeDrone();


            InitRpcServer();

            InitBlobStorageOnEdge();

            InitAI();
        }

        private void InitAI()
        {
            visionAI = new VisionAI(AIEndPointIP, AIEndPointPort);
            visionAI.SetLogFuntion(this);
        }

        private void InitBlobStorageOnEdge()
        {

            blobStorageOnEdge = new BlobStorageOnEdge(StorageAcount, StorageAccessKey);
            blobStorageOnEdge.SetLogFuntion(this);
            blobStorageOnEdge.BlobStorageInitAsync();
        }

        private void InitRpcServer()
        {
            var tasktmp = Task.Run(async () =>
            {
                await Task.Delay(2000);
                try
                {
                    dronerpcserver.SetLogFuntion(this);
                    dronerpcserver.StartRpcServer(13114);

                    while (true)
                    {
                        try
                        {

                            await Task.Delay(2000);


                            dronerpcserver.ConnectToIotEdge("127.0.0.1", 23113);

                            await Task.Delay(10000);


                        }
                        catch (Exception ex)
                        {


                        }
                    }

                }
                catch (Exception ex)
                {
                    OutputLog(ex.Message);
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisepropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (Dispatcher.HasThreadAccess)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            else
            {
                var ignore = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, (DispatchedHandler)(() =>
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }));
            }
        }

        ~MainPageViewModel()
        {

        }

        public void ShowVideo(byte[] data, int width, int height)
        {

            //DjiClient_FrameArivedAsync(WindowsRuntimeBufferExtensions.AsBuffer(data, 0, data.Length), (uint)width, (uint)height, 0);
            if (BlobStorageTask == null || BlobStorageTask.IsCompleted == true)
            {


                BlobStorageTask = Task.Run(async () =>
                {
                    await blobStorageOnEdge.SaveToBlobStorageAsync(data, width, height);
                });

            }

            if (AITask == null || AITask.IsCompleted == true)
            {
                AITask = Task.Run(async () =>
                {

                    var AIResult = await visionAI.ProcessStreamWithMLEdgeAsync(data, width, height);
                    await UpdateResultAsync(AIResult, width, height);

                });
            }



        }



        public async Task UpdateResultAsync(string res, int width, int height)
        {
            if (res == null)
                return;





            try
            {
                var jsonReturnStruct = JsonConvert.DeserializeObject<JsonStruct>(res);

                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                 {

                     var myCanvas = parent.GetAIResultCanvas();
                     myCanvas.Children.Clear();

                     if (jsonReturnStruct == null)
                     {
                            //todo:...
                        }
                     else
                     {

                         var pre = jsonReturnStruct.predictions;



                         try
                         {
                             foreach (var p in pre)
                             {

                                 string p_tagName = p.tagName;
                                 double p_probability = double.Parse(p.probability);
                                 string t_probability = Double.Parse(p.probability).ToString("F2");
                                 string showName;
                                 TextBlock floatText = new TextBlock()
                                 {
                                        //Foreground = new SolidColorBrush(Colors.Yellow)
                                    };
                                 if (p_probability < 0.45)
                                     continue;
                                 else
                                 {
                                     showName = p.tagName;

                                     floatText.FontSize = 30;

                                     try
                                     {
                                         var scal_ui = await GetUIScaleOutAsync(width, height);

                                         double top = 0;//double.Parse(p.boundingBox.top);
                                            double left = 0;// double.Parse(p.boundingBox.left);

                                            if (p.boundingBox != null)
                                         {
                                             top = double.Parse(p.boundingBox.top);
                                             left = double.Parse(p.boundingBox.left);
                                             var worgrid = new Grid()
                                             {
                                                 Height = double.Parse(p.boundingBox.height) * scal_ui.scaleH,
                                                 Width = double.Parse(p.boundingBox.width) * scal_ui.scaleW,
                                                 BorderThickness = new Thickness(2),
                                                 BorderBrush = new SolidColorBrush(Colors.Red)
                                             };

                                             Canvas.SetLeft(worgrid, left * scal_ui.scaleW + scal_ui.topPoint.X);
                                             Canvas.SetTop(worgrid, top * scal_ui.scaleH + scal_ui.topPoint.Y);
                                             myCanvas.Children.Add(worgrid);
                                         }
                                         floatText.Text = "Type: " + showName + "  Score: " + t_probability;
                                         Canvas.SetLeft(floatText, left * scal_ui.scaleW + scal_ui.topPoint.X);
                                         Canvas.SetTop(floatText, top * scal_ui.scaleH + scal_ui.topPoint.Y - 30);


                                         myCanvas.Children.Add(floatText);


                                     }
                                     catch (Exception ex)
                                     {
                                         OutputLog("ex" + ex);
                                     }

                                 }

                             }

                         }


                         catch (Exception ex)
                         {
                             OutputLog(ex.Message);

                         }


                     }
                 });


            }
            catch (Exception ex)
            {
                OutputLog(ex.Message);
            }


        }


        private async Task<UIScale> GetUIScaleOutAsync(int width, int hight)
        {
            UIScale ret = new UIScale();
            double screengridActualWidth = await parent.GetSwapChainPanelActualWidthAsync();
            double screengridActualHeight = await parent.GetSwapChainPanelActualHeightAsync();

            Windows.Foundation.Point TopPoint = new Windows.Foundation.Point(0, 0);



            double LiveActualWidth = 0;
            double LiveActualHeight = 0;
            double PicWidth = width;
            double PicHeight = hight;

            if ((screengridActualWidth / screengridActualHeight) > (PicWidth / PicHeight))
            {

                LiveActualHeight = screengridActualHeight;
                LiveActualWidth = screengridActualHeight * PicWidth / PicHeight;
                TopPoint.X = (screengridActualWidth - LiveActualWidth) / 2.0;
                TopPoint.Y = 0;

            }
            else
            {


                LiveActualWidth = screengridActualWidth;
                LiveActualHeight = screengridActualWidth * PicHeight / PicWidth;
                TopPoint.Y = (screengridActualHeight - LiveActualHeight) / 2.0;
                TopPoint.X = 0;

            }

            ret.topPoint = TopPoint;
            ret.scaleW = LiveActualWidth;
            ret.scaleH = LiveActualHeight;
            return ret;

        }


        class UIScale
        {
            public double scaleW { get; set; }
            public double scaleH { get; set; }
            public Windows.Foundation.Point topPoint { get; set; }
        }

        public class JsonStruct
        {
            public string id { get; set; }
            public string project { get; set; }
            public string iteration { get; set; }
            public string created { get; set; }

            public List<Predictions> predictions { get; set; }
        }

        public class Predictions
        {
            public string probability { get; set; }
            public string tagId { get; set; }
            public string tagName { get; set; }
            public BoundingBox boundingBox { get; set; }

        }


        public class BoundingBox
        {
            public string left { get; set; }
            public string top { get; set; }
            public string width { get; set; }
            public string height { get; set; }

        }

        private ObservableCollection<string> tipsList;
        public ObservableCollection<string> TipsList
        {
            get
            {
                if (tipsList == null)
                    tipsList = new ObservableCollection<string>();
                return tipsList;
            }
        }

        public void OutputLog(string text)
        {
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { TipsList.Add(text); });
        }

        private void InitializeDrone()
        {
            try
            {
                windowsSDKManager = new WindowsSDKManager();
                windowsSDKManager.RegisterAndInitializeWindowsSDK(DJI_DEV_KEY, this);
            }
            catch
            {
                windowsSDKManager = null;
            }
        }

        public SwapChainPanel GetSwapChainPanel()
        {
            return parent.GetSwapChainPanel();
        }

        public CoreDispatcher GetDispatcher()
        {
            return Dispatcher;
        }
    }
}
