using System;
using System.Collections.Generic;
using System.Text;

namespace Aotunomouse
{
    //
    // Summary:
    //     A generic class that encapsulates the execution result of a request.
    public struct ResultValue<T>
    {
        //
        // Summary:
        //     The encountered error if any. It is `NO_ERROR` if the request succeeds.
        public SDKError error;
        //
        // Summary:
        //     The execution result value. It is `null` if the request fails.
        public T value;
    }
    //
    // Summary:
    //     Error codes that represents errors that may occur when using SDK.
    public enum SDKError
    {
        //
        // Summary:
        //     Unknown error.
        UNKNOWN = -65535,
        //
        // Summary:
        //     Placeholder for errors that not handled by SDK.
        PLACEHOLDER = -65534,
        //
        // Summary:
        //     Authorization failed during login.
        ACCOUNT_MANAGER_AUTHROIZED_FAILED = -1794,
        //
        // Summary:
        //     The network request encountered errors.
        ACCOUNT_MANAGER_NETWORK_REQUEST_ERROR = -1793,
        //
        // Summary:
        //     Applications under basic develop account could only have the maximum 20 unqiue
        //     REGISTRATIONs. Visit DJI Developer Center to upgrade the membership.
        SDK_REGISTRATION_OVER_MAX_REGISTRATION_COUNT = -1298,
        //
        // Summary:
        //     The SDK version is not accessible with the APP key.
        SDK_REGISTRATION_VERSION_NOT_ACCESSIBLE = -1297,
        //
        // Summary:
        //     The APP key is empty.
        SDK_REGISTRATION_EMPTY_SDK_KEY = -1296,
        //
        // Summary:
        //     The respond for the REGISTRATION is in invalid format.
        SDK_REGISTRATION_INVALID_META_DATA = -1294,
        //
        // Summary:
        //     The data returned by the server is abnormal.
        SDK_REGISTRATION_SERVER_DATA_ABNORMAL = -1293,
        //
        // Summary:
        //     Write error occurs in the server.
        SDK_REGISTRATION_SERVER_WRITE_ERROR = -1292,
        //
        // Summary:
        //     The server failed to parse the request data.
        SDK_REGISTRATION_SERVER_PARSE_FAILURE = -1291,
        //
        // Summary:
        //     Deprecated.
        SDK_REGISTRATION_SDK_KEY_LEVEL_NOT_PERMITTED = -1290,
        //
        // Summary:
        //     The application key does not exist. Please make sure the application key is correct.
        SDK_REGISTRATION_SDK_KEY_DOES_NOT_EXIST = -1289,
        //
        // Summary:
        //     This error occurs when an application key was given for a specific platform and
        //     is trying to be used to activate an application for another platform.
        SDK_REGISTRATION_SDK_KEY_INVALID_PLATFORM = -1288,
        //
        // Summary:
        //     Deprecated.
        SDK_REGISTRATION_MAX_REGISTRATION_COUNT_REACHED = -1287,
        //
        // Summary:
        //     The application key is prohibited. This occurs when an application key taht has
        //     already been released by DJI is revoked.
        SDK_REGISTRATION_SDK_KEY_PROHIBITED = -1286,
        //
        // Summary:
        //     The package ID of your application does not match the one you registered on the
        //     developer website when you applied to obtain an application key.
        SDK_REGISTRATION_PACKAGE_ID_DOES_NOT_MATCH = -1285,
        //
        // Summary:
        //     The attempt to copy data from another registered device to a device that is currently
        //     connected is not permitted.
        SDK_REGISTRATION_DEVICE_DOES_NOT_MATCH = -1284,
        //
        // Summary:
        //     The network you are trying to reach is busy, or the server is unreachable.
        SDK_REGISTRATION_HTTP_TIMEOUT = -1283,
        //
        // Summary:
        //     The application key you provided is incorrect.
        SDK_REGISTRATION_INVALID_SDK_KEY = -1282,
        //
        // Summary:
        //     The application is not able to connect to the Internet the first time it tries
        //     to register the key.
        SDK_REGISTRATION_COULD_NOT_CONNECT_TO_INTERNET = -1281,
        //
        // Summary:
        //     The server failed to parse the JSON data.
        FLY_SAFE_SERVER_JSON_DATA_PARSE_ERROR = -1056,
        //
        // Summary:
        //     The request is rejected by the server because there are too many licenses.
        FLY_SAFE_SERVER_TOO_MANY_LICENSES = -1055,
        //
        // Summary:
        //     The request is rejected by the server because the user is trying to unlock too
        //     many areas at the same time.
        FLY_SAFE_SERVER_UNLOCK_TO_MANY_AREAS = -1054,
        //
        // Summary:
        //     The area cannot be unlocked.
        FLY_SAFE_SERVER_NOT_UNLOCKABLE_AREA = -1053,
        //
        // Summary:
        //     The phone number is not identified by the server.
        FLY_SAFE_SERVER_UNIDENTIFIED_PHONE = -1052,
        //
        // Summary:
        //     Used invalid token.
        FLY_SAFE_SERVER_INVALID_TOKEN = -1051,
        //
        // Summary:
        //     The license ID does not exist.
        FLY_SAFE_NO_LICENSE_ID = -1050,
        //
        // Summary:
        //     Key version error.
        FLY_SAFE_INVALID_KEY_VERSION = -1049,
        //
        // Summary:
        //     The license is out-of-date.
        FLY_SAFE_OLD_LICENSE_DATA_ERROR = -1048,
        //
        // Summary:
        //     Unlock version error.
        FLY_SAFE_UNLOCK_VERSION_ERROR = -1047,
        //
        // Summary:
        //     The firmware version is too old.
        FLY_SAFE_LOW_FIRMWARE_VERSION_ERROR = -1046,
        //
        // Summary:
        //     The license does not contain data.
        FLY_SAFE_NO_LICENSE_DATA = -1045,
        //
        // Summary:
        //     The license does not exist.
        FLY_SAFE_LICENSE_NOT_EXIST = -1044,
        //
        // Summary:
        //     Cannot enable the license because of user ID error.
        FLY_SAFE_LICENSE_ENABLE_USER_ID_ERROR = -1043,
        //
        // Summary:
        //     Failed to enable the license.
        FLY_SAFE_FC_OP_SET_ENABLE_FAILED = -1042,
        //
        // Summary:
        //     The query failed.
        FLY_SAFE_FC_QUERY_FAILED = -1041,
        //
        // Summary:
        //     The area ID is invalid.
        FLY_SAFE_INVALID_AREA_IDS = -1040,
        //
        // Summary:
        //     The database is invalid.
        FLY_SAFE_DB_NOT_VALID = -1039,
        //
        // Summary:
        //     Passed invalid database query arguments.
        FLY_SAFE_DB_INVALID_PARAMS = -1038,
        //
        // Summary:
        //     The database file may be corrupted.
        FLY_SAFE_FILE_ERROR = -1037,
        //
        // Summary:
        //     Serial number mismatches.
        FLY_SAFE_WRONG_SERIAL_NUMBER = -1036,
        //
        // Summary:
        //     Device ID mismatches.
        FLY_SAFE_PACK_MANAGER_WRONG_DEVICE_ID = -1035,
        //
        // Summary:
        //     Unlock version mismatches.
        FLY_SAFE_PACK_MANAGER_WRONG_UNLOCK_VERSION = -1034,
        //
        // Summary:
        //     The communication between the aircraft and SDK timeouted.
        FLY_SAFE_PACK_MANAGER_TIMEOUT = -1033,
        //
        // Summary:
        //     The user is not logged in yet.
        FLY_SAFE_USER_IS_NOT_LOGIN = -1032,
        //
        // Summary:
        //     Received 404 respond.
        FLY_SAFE_PAGE_NOT_FOUND = -1031,
        //
        // Summary:
        //     The request is invalid.
        FLY_SAFE_INVALID_REQUEST = -1030,
        //
        // Summary:
        //     Signature error.
        FLY_SAFE_CHECK_SIGNATURE_ERROR = -1029,
        //
        // Summary:
        //     Signature error.
        FLY_SAFE_SIGNATURE_ERROR = -1028,
        //
        // Summary:
        //     Invalid data from the server.
        FLY_SAFE_SERVER_DATA_ERROR = -1027,
        //
        // Summary:
        //     The network is not reachable.
        FLY_SAFE_NETWORK_INVALID = -1026,
        //
        // Summary:
        //     The user token is invalid.
        FLY_SAFE_LOCAL_USER_TOKEN_INVALID = -1025,
        //
        // Summary:
        //     The mission ID is invalid.
        MISSION_WAYPOINT_MISSION_ID_INVALID = -590,
        //
        // Summary:
        //     The command ID is wrong.
        MISSION_WAYPOINT_WRONG_CMD = -589,
        //
        // Summary:
        //     The aircraft is already tring to start the motors.
        MISSION_WAYPOINT_AIRCRAFT_STARTING_MOTOR = -588,
        //
        // Summary:
        //     The aircraft is already going home.
        MISSION_WAYPOINT_AIRCRAFT_GOING_HOME = -587,
        //
        // Summary:
        //     The aircraft is already landing.
        MISSION_WAYPOINT_AIRCRAFT_LANDING = -586,
        //
        // Summary:
        //     The aircraft is already taking off.
        MISSION_WAYPOINT_AIRCRAFT_TAKING_OFF = -585,
        //
        // Summary:
        //     Internal error 0xEE.
        MISSION_WAYPOINT_WAYPOINT_IDLE_VELOCITY_INVALID = -584,
        //
        // Summary:
        //     Internal error 0xED.
        MISSION_WAYPOINT_WAYPOINT_REQUEST_IS_RUNNING = -583,
        //
        // Summary:
        //     The waypoint mission is not uploaded completely.
        MISSION_WAYPOINT_WAYPOINT_UPLOAD_NOT_COMPLETE = -582,
        //
        // Summary:
        //     The waypoint mission summary is not uploaded.
        MISSION_WAYPOINT_WAYPOINT_MISSION_INFO_NOT_UPLOADED = -581,
        //
        // Summary:
        //     The waypoint mission action parameter is invalid.
        MISSION_WAYPOINT_WAYPOINT_ACTION_PARAMETER_INVALID = -580,
        //
        // Summary:
        //     The corner radius is invalid.
        MISSION_WAYPOINT_WAYPOINT_INVALID_CORNER_RADIUS = -579,
        //
        // Summary:
        //     The distance of two adjacent waypoints is too large.
        MISSION_WAYPOINT_WAYPOINT_DISTANCE_TOO_LONG = -578,
        //
        // Summary:
        //     The distance of two adjacent waypoints is too close.
        MISSION_WAYPOINT_WAYPOINT_DISTANCE_TOO_CLOSE = -577,
        //
        // Summary:
        //     Waypoint index error.
        MISSION_WAYPOINT_WAYPOINT_INDEX_OVER_RANGE = -576,
        //
        // Summary:
        //     The total distance of the waypoint mission is too large.
        MISSION_WAYPOINT_WAYPOINT_TOTAL_TRACE_TOO_LONG = -575,
        //
        // Summary:
        //     The total distance of waypoints is too large.
        MISSION_WAYPOINT_WAYPOINT_TRACE_TOO_LONG = -574,
        //
        // Summary:
        //     Invalid parameters in waypoints.
        MISSION_WAYPOINT_WAYPOINT_INFO_INVALID = -573,
        //
        // Summary:
        //     Invalid parameters in waypoint mission.
        MISSION_WAYPOINT_MISSION_INFO_INVALID = -572,
        //
        // Summary:
        //     No waypoint mission is being executed.
        MISSION_WAYPOINT_WAYPOINT_NOT_RUNNING = -571,
        //
        // Summary:
        //     The home point location is not refreshed.
        MISSION_WAYPOINT_HOME_POINT_NOT_RECORDED = -570,
        //
        // Summary:
        //     The waypoint mission is invalid because it will cross a no fly-zone.
        MISSION_WAYPOINT_MISSION_ACROSS_NO_FLY_ZONE = -569,
        //
        // Summary:
        //     The waypoint mission request cannot be executed.
        MISSION_WAYPOINT_MISSION_CONDITION_NOT_SATISFIED = -568,
        //
        // Summary:
        //     Invalid parameters.
        MISSION_WAYPOINT_MISSION_PARAMETERS_INVALID = -567,
        //
        // Summary:
        //     The aircraft is not in the air. Takeoff first.
        MISSION_WAYPOINT_AIRCRAFT_NOT_IN_THE_AIR = -566,
        //
        // Summary:
        //     The remaining charge is too low.
        MISSION_WAYPOINT_LOW_BATTERY = -565,
        //
        // Summary:
        //     The GPS signal is weak.
        MISSION_WAYPOINT_GPS_SIGNAL_WEAK = -564,
        //
        // Summary:
        //     High priority mission is running.
        MISSION_WAYPOINT_HIGH_PRIORITY_MISSION_EXECUTING = -563,
        //
        // Summary:
        //     Mission will consume too much time.
        MISSION_WAYPOINT_MISSION_ESTIMATE_TIME_TOO_LONG = -562,
        //
        // Summary:
        //     Other mission is running.
        MISSION_WAYPOINT_MISSION_CONFLICT = -561,
        //
        // Summary:
        //     No mission is running.
        MISSION_WAYPOINT_MISSION_NOT_EXIST = -560,
        //
        // Summary:
        //     Aircraft's mission is not initialized.
        MISSION_WAYPOINT_MISSION_NOT_INITIALIZED = -559,
        //
        // Summary:
        //     The aircraft is in IOC mode.
        MISSION_WAYPOINT_IOC_WORKING = -558,
        //
        // Summary:
        //     The aircraft is inside a no fly-zone.
        MISSION_WAYPOINT_AIRCRAFT_IN_NO_FLY_ZONE = -557,
        //
        // Summary:
        //     RC mode error.
        MISSION_WAYPOINT_RC_MODE_ERROR = -556,
        //
        // Summary:
        //     RTK is not ready.
        MISSION_WAYPOINT_RTK_IS_NOT_READY = -555,
        //
        // Summary:
        //     The multiple mode is disabled.
        MISSION_WAYPOINT_MULTI_MODE_IS_OFF = -554,
        //
        // Summary:
        //     The novice mode is enabled.
        MISSION_WAYPOINT_IN_NOVICE_MODE = -553,
        //
        // Summary:
        //     Mission start point is too far away from the aircraft's current location.
        MISSION_WAYPOINT_DISTANCE_FROM_MISSION_TARGET_TOO_LONG = -552,
        //
        // Summary:
        //     Waypoint mission is not supported by the aircraft.
        MISSION_WAYPOINT_NAVIGATION_MODE_NOT_SUPPORTED = -551,
        //
        // Summary:
        //     Over max flight radius.
        MISSION_WAYPOINT_MISSION_RADIUS_OVER_LIMIT = -550,
        //
        // Summary:
        //     Failed to resume the mission, because the aircraft is far away from the mission.
        MISSION_WAYPOINT_MISSION_RESUME_FAILED = -549,
        //
        // Summary:
        //     The waypoint has invalid heading.
        MISSION_WAYPOINT_MISSION_HEADING_MODE_INVALID = -548,
        //
        // Summary:
        //     Entry point invalid.
        MISSION_WAYPOINT_MISSION_ENTRY_POINT_INVALID = -547,
        //
        // Summary:
        //     The circular velocity is too large.
        MISSION_WAYPOINT_MISSION_SPEED_TOO_HIGH = -546,
        //
        // Summary:
        //     Invalid surrounding radius.
        MISSION_WAYPOINT_MISSION_RADIUS_INVALID = -545,
        //
        // Summary:
        //     Aircraft's altitude is too low.
        MISSION_WAYPOINT_ALTITUDE_TOO_LOW = -544,
        //
        // Summary:
        //     Aircraft's altitude is too high.
        MISSION_WAYPOINT_ALTITUDE_TOO_HIGH = -543,
        //
        // Summary:
        //     Gimbal's pitch angle is too large.
        MISSION_FOLLOW_ME_GIMBAL_PITCH_ERROR = -542,
        //
        // Summary:
        //     The following targer is lost.
        MISSION_FOLLOW_ME_DISCONNECT_TIME_TOO_LONG = -541,
        //
        // Summary:
        //     The distance between the aircraft and the target is too far.
        MISSION_FOLLOW_ME_DISTANCE_TOO_LARGE = -540,
        //
        // Summary:
        //     The hotpoint mission is already resumed.
        MISSION_HOTPOINT_MISSION_NOT_PAUSED = -539,
        //
        // Summary:
        //     The hotpoint mission is already paused.
        MISSION_HOTPOINT_MISSION_PAUSED = -538,
        //
        // Summary:
        //     Invalid direction.
        MISSION_HOTPOINT_DIRECTION_UNKNOWN = -537,
        //
        // Summary:
        //     Invalid location as a hotpoint.
        MISSION_HOTPOINT_LOCATION_INVALID = -536,
        //
        // Summary:
        //     Invalid parameters.
        MISSION_HOTPOINT_VALUE_INVALID = -535,
        //
        // Summary:
        //     Invalid IOC type.
        MISSION_IOC_TYPE_UNKNOWN = -534,
        //
        // Summary:
        //     The aircraft is too close to the home point.
        MISSION_IOC_TOO_CLOSE_TO_HOME_POINT = -533,
        //
        // Summary:
        //     The ground station mode is not enabled.
        MISSION_WAYPOINT_NAVIGATION_MODE_DISABLED = -532,
        //
        // Summary:
        //     Internal error 0x0D.
        MISSION_WAYPOINT_KEY_LEVEL_LOW = -531,
        //
        // Summary:
        //     The handler is uploading waypoints.
        MISSION_WAYPOINT_UPLOADING_WAYPOINT = -530,
        //
        // Summary:
        //     The number of waypoints exceeded the max count.
        MISSION_WAYPOINT_MAX_NUMBER_OF_WAYPOINTS_UPLOAD_LIMIT_REACHED = -529,
        //
        // Summary:
        //     Internal error 0x0A.
        MISSION_WAYPOINT_NOT_AUTO_MODE = -528,
        //
        // Summary:
        //     Internal error 0x09.
        MISSION_WAYPOINT_IS_FLYING = -527,
        //
        // Summary:
        //     Internal error 0x08.
        MISSION_WAYPOINT_TAKE_OFF = -526,
        //
        // Summary:
        //     Internal error 0x07.
        MISSION_WAYPOINT_MOTORS_DID_NOT_START = -525,
        //
        // Summary:
        //     The waypoint mission cannot be executed because of invalid GPS location of the
        //     aircraft.
        MISSION_WAYPOINT_GPS_NOT_READY = -524,
        //
        // Summary:
        //     The waypoint mission cannot be executed because of the flight mode error.
        MISSION_WAYPOINT_MODE_ERROR = -523,
        //
        // Summary:
        //     The waypoint mission execution timeouted.
        MISSION_WAYPOINT_TIMEOUT = -522,
        //
        // Summary:
        //     The waypoint mission execution failed.
        MISSION_WAYPOINT_FAILED = -521,
        //
        // Summary:
        //     The request for a waypoint mission is cancelled.
        MISSION_WAYPOINT_CANCELLED = -520,
        //
        // Summary:
        //     The waypoint mission is already started.
        MISSION_WAYPOINT_ALREADY_STARTED = -519,
        //
        // Summary:
        //     The local information of the waypoint mission is complete. It is not necessary
        //     to download it from the aircraft.
        MISSION_WAYPOINT_DOWNLOAD_UNNECESSARY = -518,
        //
        // Summary:
        //     The waypoint handler is in disconnected state. The request is rejected.
        MISSION_WAYPOINT_DISCONNECTED = -517,
        //
        // Summary:
        //     The waypoint count is less than 2.
        MISSION_WAYPOINT_COUNT_INVALID = -516,
        //
        // Summary:
        //     The repeat times are less than 0.
        MISSION_WAYPOINT_REPEAT_TIMES_INVALID = -515,
        //
        // Summary:
        //     The max flight speed exceeds the valid range.
        MISSION_WAYPOINT_MAX_FLIGHT_SPEED_INVALID = -514,
        //
        // Summary:
        //     The mission is null.
        MISSION_WAYPOINT_NULL_MISSION = -513,
        //
        // Summary:
        //     The execution is failed.
        EXECUTION_FAILED = -14,
        //
        // Summary:
        //     The request is invalid in the current state.
        INVALID_REQUEST_IN_CURRENT_STATE = -13,
        //
        // Summary:
        //     The parameter is out of the valid range.
        PARAM_OUT_OF_RANGE = -12,
        //
        // Summary:
        //     The respond data from the firmware is either empty or corrupted.
        INVALID_RESPOND = -11,
        //
        // Summary:
        //     Error occurs when trying to set the data.
        PARAMETERS_SET_ERROR = -10,
        //
        // Summary:
        //     Error occurs when trying to get the data.
        PARAMETERS_GET_ERROR = -9,
        //
        // Summary:
        //     The command is interrupted.
        COMMAND_INTERRUPTED = -8,
        //
        // Summary:
        //     Internal system error occurs.
        SYSTEM_ERROR = -7,
        //
        // Summary:
        //     The parameter is invalid.
        INVALID_PARAM = -6,
        //
        // Summary:
        //     The handler disconnects during the execution.
        DISCONNECTED = -5,
        //
        // Summary:
        //     SDK failed to send the data to the DJI product. One possible reason is that the
        //     link between SDK and the product is not stable.
        SEND_PACK_FAILURE = -4,
        //
        // Summary:
        //     The request timeouted. Re-try later.
        REQUEST_TIMEOUT = -3,
        //
        // Summary:
        //     The request is not supported by the handler.
        REQUEST_NOT_SUPPORTED_BY_HANDLER = -2,
        //
        // Summary:
        //     The handler for the quest is not not found. One possible reason is that the component
        //     is not connected.
        REQUEST_HANDLER_NOT_FOUND = -1,
        //
        // Summary:
        //     No error. This code is returned when a command or a query succeeds.
        NO_ERROR = 0
    }

    //
    // Summary:
    //     A class that contains a string value.
    public struct StringMsg
    {
        //
        // Summary:
        //     The actual value of the class.
        public string value;
    }

    //
    // Summary:
    //     A class that contains a string value.
    public struct BoolMsg
    {
        //
        // Summary:
        //     The actual value of the class.
        public bool value;
    }

    //
    // Summary:
    //     A class that contains an int value.
    public struct IntMsg
    {
        //
        // Summary:
        //     The actual value of the class.
        public int value;
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
