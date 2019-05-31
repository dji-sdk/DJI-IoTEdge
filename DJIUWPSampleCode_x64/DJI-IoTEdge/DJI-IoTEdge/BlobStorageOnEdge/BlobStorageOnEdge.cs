using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace DJI_IoTEdge
{
    class BlobStorageOnEdge
    {
        public delegate void _Outputlog(string text);
        _Outputlog WriteLogFunction = null;
        CloudStorageAccount storageAccount = null;
        CloudBlobContainer cloudBlobContainer = null;
        bool BlobStorageInitFinished = false;
        string StorageAcount;
        string StorageAccessKey ;
        string IoTEdgeDeviceIP ;


        public BlobStorageOnEdge(string _StorageAcount, string _StorageAccessKey, string _IoTEdgeDeviceIP = "127.0.0.1")
        {
            StorageAcount = _StorageAcount;
            StorageAccessKey = _StorageAccessKey;
            IoTEdgeDeviceIP = _IoTEdgeDeviceIP;
        }


        public void SetLogFuntion(MainPageViewModel viewModel)
        {
            WriteLogFunction = viewModel.OutputLog;
        }

        void WriteLog(string text)
        {
            WriteLogFunction?.Invoke("StorageOnEdge Log: " + text);
        }

        public async Task BlobStorageInitAsync()
        {
           
            if (CloudStorageAccount.TryParse("DefaultEndpointsProtocol=https;BlobEndpoint=http://" + IoTEdgeDeviceIP + ":11002/" + StorageAcount + ";AccountName=" + StorageAcount + ";AccountKey=" + StorageAccessKey, out storageAccount))
            {




                try
                {
                    WriteLog( "Bob Storage Init!");
                    // Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
                    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                    // Create a container called 'quickstartblobs' and append a GUID value to it to make the name unique. 
                    cloudBlobContainer = cloudBlobClient.GetContainerReference("pictures");
                    await cloudBlobContainer.CreateIfNotExistsAsync();
                    WriteLog("Created container " + cloudBlobContainer.Name);
                    //Console.WriteLine();

                    // Set the permissions so the blobs are public. 
                    BlobContainerPermissions permissions = new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    };
                    await cloudBlobContainer.SetPermissionsAsync(permissions);
                    BlobStorageInitFinished = true;
                    WriteLog("connected!");
                }
                catch (StorageException )
                {
                    BlobStorageInitFinished = false;
                   
                }


            }
            else
            {
                BlobStorageInitFinished = false;
                WriteLog(
                   "A connection string has not been defined in the system environment variables. " +
                   "Add a environment variable named 'storageconnectionstring' with your storage " +
                   "connection string as a value.");
              

            }
        }

        public async Task SaveToBlobStorageAsync(byte[] data, int width, int height, string filename =null )
        {
            while (BlobStorageInitFinished == false)
            {
                await BlobStorageInitAsync();
            }

            try
            {
                using (var stream = new InMemoryRandomAccessStream())
                {
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                    encoder.SetPixelData(BitmapPixelFormat.Rgba8, BitmapAlphaMode.Premultiplied, (uint)width, (uint)height, 96, 96, data);
                    await encoder.FlushAsync().AsTask();
                    try
                    {
                        filename = filename == null ? GetStandTimeStr() : filename;
                        string blobname = "Pic" + filename + ".jpg";
                        CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blobname);
                        using (var reader = new DataReader(stream))
                        {
                            await reader.LoadAsync((uint)stream.Size);
                            var buffer = new byte[(int)stream.Size];
                            reader.ReadBytes(buffer);
                            await cloudBlockBlob.UploadFromByteArrayAsync(buffer, 0, buffer.Length);
                            WriteLog("Save Successfully," + blobname);

                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog(ex.Message);
                        WriteLog( "Disconnect!");
                    }



                }
            }
            catch (Exception ex)
            {
               
                WriteLog( ex.Message);
            }
        }
                


       


    public static string GetStandTimeStr()
    {
        DateTime now = DateTime.UtcNow;
        var ss = now.GetDateTimeFormats();
        string ret = String.Format
            (
            "{0,4:D4}{1,2:D2}{2,2:D2}T{3,2:D2}{4,2:D2}{5,2:D2}{6}Z",
            now.Year,
            now.Month,
            now.Day,
            now.Hour,
            now.Minute,
            now.Second,
            (new Random()).Next(1, 100)
            );
        return ret.ToLower();
    }
}
}
