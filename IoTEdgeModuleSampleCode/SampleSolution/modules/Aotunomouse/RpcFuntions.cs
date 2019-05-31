namespace Aotunomouse
{
    using System.Threading;
    using System.Threading.Tasks;
    using System;
    public interface IRPCFuntions    //right
    {

        //Functions
        Task<string> ComfirmAsync();
        Task<ResultValue<StringMsg?>> GetDroneNameAsync();
        Task UpdateJoystickValueAsync(float throttle, float roll, float pitch, float yaw);
        Task<SDKError> flightControllerHandler_StartTakeoffAsync();
        Task<SDKError> flightControllerHandler_StartAutoLandingAsync();
       
        //Events
        event EventHandler<BoolMsgEventArgs> flightControllerHandler_ConnectionChanged;
        event EventHandler<BoolMsgEventArgs> flightControllerHandler_IsFlyingChanged;
        event EventHandler<IntMsgEventArgs> batteryHandler_CurrentChanged;



    }


}