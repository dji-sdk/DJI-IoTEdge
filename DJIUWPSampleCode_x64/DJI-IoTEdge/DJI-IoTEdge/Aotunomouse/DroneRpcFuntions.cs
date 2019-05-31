using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DJI.WindowsSDK;
using DJI.WindowsSDK.Components;

namespace DroneRPC
{
    public class DroneRpcFuntions
    {
        FlightControllerHandler flightControllerHandler = null;
       
        FlightAssistantHandler flightAssistantHandler = null;
        VirtualRemoteController virtualRemoteController = null;
        public DroneRpcFuntions()
        {

            var componentManager = DJISDKManager.Instance.ComponentManager;

            virtualRemoteController = DJISDKManager.Instance.VirtualRemoteController;

            flightAssistantHandler = componentManager.GetFlightAssistantHandler(0, 0);

            flightControllerHandler = componentManager.GetFlightControllerHandler(0, 0);

            flightControllerHandler.ConnectionChanged += (object sender, BoolMsg? value) => { OnflightControllerHandler_ConnectionChanged(sender, value); };
            flightControllerHandler.IsFlyingChanged += (object sender, BoolMsg? value) => { OnflightControllerHandler_IsFlyingChanged(sender, value); };
            componentManager.GetBatteryHandler(0, 0).CurrentChanged += (object sender, IntMsg? value) => { OnbatteryHandler_CurrentChanged(sender, value); };


        }

        //Functions
        public Task<string> ComfirmAsync() => Task.FromResult("I'm Drone.");
        public Task<ResultValue<StringMsg?>> GetDroneNameAsync() => flightControllerHandler.GetAircraftNameAsync();
        public Task UpdateJoystickValueAsync(float throttle, float roll, float pitch, float yaw) => Task.Run(() => { virtualRemoteController.UpdateJoystickValue(throttle, roll, pitch, yaw); });
        public Task<SDKError> flightControllerHandler_StartTakeoffAsync() => flightControllerHandler.StartTakeoffAsync();
        public Task<SDKError> flightControllerHandler_StartAutoLandingAsync() => flightControllerHandler.StartAutoLandingAsync();


        //Events
        public event EventHandler<BoolMsgEventArgs> flightControllerHandler_ConnectionChanged;
        public event EventHandler<BoolMsgEventArgs> flightControllerHandler_IsFlyingChanged;
        public event EventHandler<IntMsgEventArgs> batteryHandler_CurrentChanged;


        //internal EventHandler<CustomEventArgs> ServerEventWithCustomArgsAccessor => this.ServerEventWithCustomArgs;
        // public event EventHandler<CustomEventArgs> ServerEventWithCustomArgs;
        //  protected virtual void OnServerEventWithCustomArgs(CustomEventArgs args) => this.ServerEventWithCustomArgs?.Invoke(this, args);

        //Internal
        internal void OnflightControllerHandler_ConnectionChanged(object sender, BoolMsg? BoolMsgvalue) => flightControllerHandler_ConnectionChanged?.Invoke(sender, new BoolMsgEventArgs { value = BoolMsgvalue });
        internal void OnflightControllerHandler_IsFlyingChanged(object sender, BoolMsg? BoolMsgvalue) => flightControllerHandler_IsFlyingChanged?.Invoke(sender, new BoolMsgEventArgs { value = BoolMsgvalue });
        internal void OnbatteryHandler_CurrentChanged(object sender, IntMsg? IntMsgvalue) => batteryHandler_CurrentChanged?.Invoke(sender, new IntMsgEventArgs { value = IntMsgvalue });




    }

    public class BoolMsgEventArgs : EventArgs
    {
        public BoolMsg? value { get; set; }
    }
    public class IntMsgEventArgs : EventArgs
    {
        public IntMsg? value { get; set; }
    }
}
