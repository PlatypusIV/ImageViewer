using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewerTwo.Classes
{
    class FileIO
    {
        FileInfo[] getAllFiles(string inputFileName)
        {
            string[] temp = inputFileName.Split('\\');
            string[] imageExtensions = { ".bmp", ".jpg", ".png", ".jpeg",".gif" };

            string folderpath = "";
            for(int i = 0;i < temp.Length - 1; i++)
            {
                folderpath += temp[i] + "\\";
            }

            DirectoryInfo dirInfo = new DirectoryInfo(folderpath);

            FileInfo[] filesFromFolder = dirInfo.GetFiles();

            List<FileInfo> imageList = new List<FileInfo>();
            foreach(FileInfo file in filesFromFolder)
            {
                foreach(string extension in imageExtensions)
                {
                    if (file.FullName.Contains(extension))
                    {
                        imageList.Add(file);
                    }
                }
            }

            return filesFromFolder;
        }

        public Task<FileInfo[]> getAllFilesTask(string inputFileNameTask)
        {
            return Task.Factory.StartNew(() => getAllFiles(inputFileNameTask));
        }

        string[] getAllFilesFromFolderAlter(string input)
        {
            string[] imageExtensions = { ".bmp", ".jpg", ".png", ".jpeg",".gif"};
            FileInfo chosenFile = new FileInfo(input);

            string[] allFilesFromFolder = Directory.GetFiles(chosenFile.Directory.ToString());
            List<string> tempFileNameList = new List<string>();

            foreach(string fileName in allFilesFromFolder)
            {
                foreach(string extension in imageExtensions)
                {
                    if (fileName.Contains(extension))
                    {
                        tempFileNameList.Add(fileName);
                    }
                }
            }
            string[] imagesFromFolder = tempFileNameList.ToArray();
            return imagesFromFolder;
        }

        public Task<string[]> getAllFilesFromFolderAlterTask(string inputTask)
        {
            return Task.Factory.StartNew(() => getAllFilesFromFolderAlter(inputTask));
        }

        private string GetInitialDirectory()
        {
            string contentToReturn = "";
            try
            {
                using (FileStream fs = new FileStream("Textfiles\\DirectoryInit.txt", FileMode.Open, FileAccess.Read))
                using (StreamReader sr = new StreamReader(fs))
                {
                    contentToReturn = sr.ReadToEnd();
                }
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
            return contentToReturn;
        }

        public Task<string> GetInitialDirectoryTask()
        {
            return Task.Factory.StartNew(() => GetInitialDirectory());
        }

        private void writeInitialDirectory(string fileName)
        {
            string[] temp = fileName.Split('\\');
            string directoryName = "";
            for(int i = 0; i< temp.Length - 1;i++)
            {
                directoryName = temp[i] + @"\";
            }

            byte[] bytesToWrite = Encoding.ASCII.GetBytes(directoryName);
            using (FileStream fs = new FileStream("Textfiles\\DirectoryInit.txt", FileMode.Truncate, FileAccess.Write))
            {
                fs.Write(bytesToWrite, 0, bytesToWrite.Length);
            }
        }

        public Task writeInitialDirectoryTask(string fileNameTask)
        {
            return Task.Factory.StartNew(() => writeInitialDirectory(fileNameTask));
        }

    }
}
