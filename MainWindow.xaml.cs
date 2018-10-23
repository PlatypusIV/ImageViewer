using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace ImageViewerTwo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Classes.FileIO fairu;
        Classes.EfficiencyFunctions efficiency;
        int imageIndex;
        string[] imageFileNameArray;
        bool isFullScreen;
        OpenFileDialog ofd;
        bool isAnimated;
        bool animationIsPlaying = false;
        string initialDirectory;
        

        public MainWindow()
        {
            InitializeComponent();
            imageIndex = 0;
            fairu = new Classes.FileIO();
            efficiency = new Classes.EfficiencyFunctions();
            isFullScreen = false;
            isAnimated = false;
            animatedFilesPlayer.Visibility = Visibility.Hidden;
            ofd = new OpenFileDialog();
            ofd.Filter = "Image files|*.jpg;*.png;*.jpeg;*.bmap;*.gif;" ;
            setInitialDirectory();
            
        }

        async void setInitialDirectory()
        {
            try
            {
                initialDirectory = await fairu.GetInitialDirectoryTask();
                if (initialDirectory != null || initialDirectory != "")
                {
                    ofd.InitialDirectory = initialDirectory;
                }
                else
                {
                    ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop).ToString();
                }
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
            
        }
        async void writeInitialDirectoryToFile()
        {
            try
            {
                if(imageFileNameArray != null)
                {
                    await fairu.writeInitialDirectoryTask(imageFileNameArray[1]);
                }

            }catch(Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private async void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {                
                if (ofd.ShowDialog() != null)
                {
                    isAnimated = await efficiency.CheckIfAnimatedTask(ofd.FileName);
                    imageFileNameArray = await fairu.getAllFilesFromFolderAlterTask(ofd.FileName);
                    imageIndex = await efficiency.getIndexPositionTask(imageFileNameArray, ofd.FileName);
                    writeInitialDirectoryToFile();
                    changeImage(imageIndex);
                }
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }            
        }

        private void forwardBtn_Click(object sender, RoutedEventArgs e)
        {           
            changeImage(++imageIndex);           
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {            
            changeImage(--imageIndex);
        }

        private async void changeImage(int index)
        {
            if(imageFileNameArray != null)
            {
                try
                {
                
                    if (index < 0)
                    {
                        imageIndex = imageFileNameArray.Length - 1;
                        index = imageIndex;

                    }
                    else if (index > imageFileNameArray.Length - 1)
                    {
                        imageIndex = 0;
                        index = imageIndex;
                    }
                    isAnimated = await efficiency.CheckIfAnimatedTask(imageFileNameArray[index]);

                    if (isAnimated)
                    {
                        ShowAnimation(index);
                    }
                    else
                    {
                        ShowImage(index);
                    }
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.ToString());
                }
            }
        }

        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.Key)
                {
                    case Key.Left:
                        changeImage(--imageIndex);
                        break;
                    case Key.Right:
                        changeImage(++imageIndex);
                        break;
                    case Key.F:
                        fullScreen();
                        break;
                    case Key.Escape:
                        Close();
                        break;
                    case Key.R:
                        if(imageFileNameArray != null)
                        {
                            imageIndex = await efficiency.trueRandomnessTask(0, imageFileNameArray.Length - 1);
                            changeImage(imageIndex);
                        }
                        break;
                    /*case Key.S:
                        if (isAnimated)
                        {           
                            switch (animationIsPlaying)
                            {
                                case true:
                                    animatedFilesPlayer.Pause();
                                    animationIsPlaying = false;
                                    break;
                                case false:
                                    animatedFilesPlayer.Play();
                                    animationIsPlaying = true;
                                    break;
                            }
                        }
                        break;*/
                }
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }          
        }

        private void fullScreen()
        {
            try
            {
                if (isFullScreen)
                {
                    this.WindowState = WindowState.Normal;
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                    isFullScreen = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    this.WindowStyle = WindowStyle.None;
                    isFullScreen = true;
                }
            }catch(Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        private async void randomBtn_Click(object sender, RoutedEventArgs e)
        {
            if(imageFileNameArray != null)
            {
                try
                {
                    imageIndex = await efficiency.trueRandomnessTask(0, imageFileNameArray.Length - 1);
                    changeImage(imageIndex);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.ToString());
                }
            }
        }

        private void ShowImage(int input)
        {
            if (mainImage.Visibility == Visibility.Hidden)
            {
                mainImage.Visibility = Visibility.Visible;
                animatedFilesPlayer.Visibility = Visibility.Hidden;
            }
            animationIsPlaying = false;
            mainImage.Source = new BitmapImage(new Uri(imageFileNameArray[input]));
        }

        private void ShowAnimation(int input)
        {
            if (animatedFilesPlayer.Visibility == Visibility.Hidden)
            {
                mainImage.Visibility = Visibility.Hidden;
                animatedFilesPlayer.Visibility = Visibility.Visible;
            }
            animationIsPlaying = true;
            animatedFilesPlayer.Source = new Uri(imageFileNameArray[input]);
        }
    }
}
