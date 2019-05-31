using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DJI_IoTEdge
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
       
        public MainPage()
        {

            this.InitializeComponent();
            viewModel = new MainPageViewModel(Dispatcher, this);
        }

        private MainPageViewModel viewModel;
        public MainPageViewModel ViewModel
        {
            get { return viewModel; }
        }

        public SwapChainPanel GetSwapChainPanel()
        {


            return swapChainPanel;
        }


        public Canvas GetAIResultCanvas()
        {
            return AIResultCanvas;
        }


        public async Task<double> GetSwapChainPanelActualWidthAsync()
        {
            double value = 0;
            if (Dispatcher.HasThreadAccess)
            {
                value = swapChainPanel.ActualWidth;
                //value = videoElement.ActualWidth;
            }
            else
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {

                    value = swapChainPanel.ActualWidth;
                    //value = videoElement.ActualWidth;
                });
            }

            return value;
        }
        public async Task<double> GetSwapChainPanelActualHeightAsync()
        {
            double value = 0;
            if (Dispatcher.HasThreadAccess)
            {
                value = swapChainPanel.ActualHeight;
            }
            else
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {

                    value = swapChainPanel.ActualHeight;
                });
            }

            return value;
        }

    }
}
