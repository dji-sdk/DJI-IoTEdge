using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace DJI_IoTEdge
{

    class VisionAI
    {
        public delegate void _Outputlog(string text);
        _Outputlog WriteLogFunction = null;
        string AIEndPointIP;
        string AIEndPointPort;

        public VisionAI(string _AIEndPointIP, string _AIEndPointPort)
        {
            AIEndPointIP = _AIEndPointIP;
            AIEndPointPort = _AIEndPointPort;
        }


        public async Task<string> ProcessStreamWithMLEdgeAsync(byte[] data, int width, int height)
        {
            try
            {
                using (var stream = new InMemoryRandomAccessStream())
                {
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                    encoder.SetPixelData(BitmapPixelFormat.Rgba8, BitmapAlphaMode.Premultiplied, (uint)width, (uint)height, 96, 96, data);
                    await encoder.FlushAsync().AsTask();
                    return await ProcessStreamWithMLEdgeAsync(stream);


                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }

            return null;
        }


        private async Task<string> ProcessStreamWithMLEdgeAsync(InMemoryRandomAccessStream stream)
        {
            string sret = null;
            try
            {

                HttpContent httpContent = new StreamContent(WindowsRuntimeStreamExtensions.AsStreamForRead(stream.GetInputStreamAt(0)));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                HttpRequestMessage RequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri("http://" + AIEndPointIP + ":" + AIEndPointPort + "/image"));
                RequestMessage.Content = httpContent;
                var httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.SendAsync(RequestMessage);
                if (response.IsSuccessStatusCode)
                {
                    sret = await response.Content.ReadAsStringAsync();
                    //WriteLog(sret);
                    //WriteLog("Connected!");
                    return sret;
                }
                else
                {
                    WriteLog(string.Format("The request failed with status code: {0}", response.StatusCode));

                    WriteLog(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    WriteLog(responseContent);
                    WriteLog("Disconnect!");

                }
            }
            catch (Exception ex)
            {
                //WriteLog(ex.Message);
                //WriteLog( "Disconnect!");

            }
            return sret;
        }



        public void SetLogFuntion(MainPageViewModel viewModel)
        {
            WriteLogFunction = viewModel.OutputLog;
        }

        void WriteLog(string text)
        {
            WriteLogFunction?.Invoke("VisionAI Log: " + text);
        }
    }
}
