using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DJIVideoParser;
using DJI.WindowsSDK;
using DJI.WindowsSDK.Components;

namespace DJI_IoTEdge
{
    class WindowsSDKManager
    {

       
        Parser videoParser = null;
       
        static MainPageViewModel viewModel = null;
        public void RegisterAndInitializeWindowsSDK(String app_key, MainPageViewModel viewmodel)
        {
            viewModel = viewmodel;
            DJISDKManager.Instance.SDKRegistrationStateChanged += Instance_SDKRegistrationStateChanged;
            DJISDKManager.Instance.RegisterApp(app_key);
        }

        private void Instance_SDKRegistrationStateChanged(SDKRegistrationState state, SDKError errorCode)
        {
            if (state == SDKRegistrationState.Succeeded)
            {
                //InitializeWindowsSDK();
                InitializeParser();
            }
            else
            {
                viewModel.OutputLog("SDKRegistrationState = " + state.ToString());
            }

        }
        private void InitializeParser()
        {
            if (videoParser == null)
            {
                videoParser = new DJIVideoParser.Parser();
                videoParser.Initialize(delegate (byte[] data)
                {
                    //Note: This function must be called because we need DJI Windows SDK to help us to parse frame data.
                    return DJISDKManager.Instance.VideoFeeder.ParseAssitantDecodingInfo(0, data);
                });
                var viewTask = viewModel.GetDispatcher().RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    videoParser.SetSurfaceAndVideoCallback(0, 0, viewModel.GetSwapChainPanel(), viewModel.ShowVideo);
                });

                DJISDKManager.Instance.VideoFeeder.GetPrimaryVideoFeed(0).VideoDataUpdated += OnVideoPush;
                DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0).CameraTypeChanged += OnCameraTypeChanged;
                Task<ResultValue<CameraTypeMsg?>> res = Task.Run(async () =>
                {
                    var type = await DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0).GetCameraTypeAsync();
                    OnCameraTypeChanged(this, type.value);
                    return type;
                });

            }
        }

        void OnVideoPush(VideoFeed sender, byte[] bytes)
        {

            videoParser.PushVideoData(0, 0, bytes, bytes.Length);
        }

        private void OnCameraTypeChanged(object sender, CameraTypeMsg? value)
        {
            if (value.HasValue)
            {
                switch (value.Value.value)
                {
                    case CameraType.MAVIC_2_ZOOM:
                        this.videoParser.SetCameraSensor(AircraftCameraType.Mavic2Zoom);
                        break;
                    case CameraType.MAVIC_2_PRO:
                        this.videoParser.SetCameraSensor(AircraftCameraType.Mavic2Pro);
                        break;
                    default:
                        this.videoParser.SetCameraSensor(AircraftCameraType.Others);
                        break;
                }

            }
        }



    }
}
