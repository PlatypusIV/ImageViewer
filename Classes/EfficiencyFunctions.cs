using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewerTwo.Classes
{
    class EfficiencyFunctions
    {
        private int trueRandomness(int min, int max)
        {
            Random rng = new Random();
            int numberToReturn = rng.Next(min, max);
            return numberToReturn;
        }

        public Task<int> trueRandomnessTask(int minTask, int maxTask)
        {
            return Task.Factory.StartNew(() => trueRandomness(minTask, maxTask));
        }

        private int getIndexPosition(string[] inputArray, string fileName)
        {
            int indexToReturn = 0;
            for (int i = 0; i < inputArray.Length; i++)
            {
                if (inputArray[i] == fileName)
                {
                    indexToReturn = i;
                }
            }
            return indexToReturn;
        }

        public Task<int> getIndexPositionTask(string[] inputArrayTask, string fileNameTask)
        {
            return Task.Factory.StartNew(() => getIndexPosition(inputArrayTask, fileNameTask));
        }

        private bool CheckIfAnimated(string fileName)
        {
            bool animated = false;
            if(fileName.Contains(".gif"))
            {
                animated = true;
            }
            return animated;
        }

        public Task<bool> CheckIfAnimatedTask(string fileNameTask)
        {
            return Task.Factory.StartNew(() => CheckIfAnimated(fileNameTask));
        }
    }
}
