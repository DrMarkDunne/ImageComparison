﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

// Created in 2012 by Jakob Krarup (www.xnafan.net).
// Use, alter and redistribute this code freely,
// but please leave this comment :)


namespace ImageComparison
{

    /// <summary>
    /// A class with extension methods for comparing images
    /// </summary>
    public static class ImageTool
    {
        private static readonly PathGrayscaleTupleComparer Comparer = new();

        /// <summary>
        /// Gets the difference between two images as a percentage
        /// </summary>
        /// <returns>The difference between the two images as a percentage</returns>
        /// <param name="image1Path">The path to the first image</param>
        /// <param name="image2Path">The path to the second image</param>
        /// <param name="threshold">How big a difference (out of 255) will be ignored - the default is 3.</param>
        /// <returns>The difference between the two images as a percentage</returns>
        public static float GetPercentageDifference(string image1Path, string image2Path, byte threshold = 3)
        {
            if (!CheckIfFileExists(image1Path) || !CheckIfFileExists(image2Path)) return -1;
            var img1 = Image.FromFile(image1Path);
            var img2 = Image.FromFile(image2Path);

            var difference = img1.PercentageDifference(img2, threshold);

            img1.Dispose();
            img2.Dispose();

            return difference;

        }

        /// <summary>
        /// Gets the difference between two images as a percentage
        /// </summary>
        /// <returns>The difference between the two images as a percentage</returns>
        /// <param name="image1Path">The path to the first image</param>
        /// <param name="image2Bitmap">The second image in Bitmap format</param>
        /// <param name="threshold">How big a difference (out of 255) will be ignored - the default is 3.</param>
        /// <returns>The difference between the two images as a percentage</returns>

        public static float GetPercentageDifference(string image1Path, Bitmap image2Bitmap, byte threshold = 3)
        {
            if (!CheckIfFileExists(image1Path)) return -1;
            var img1 = Image.FromFile(image1Path);
            var img2 = Image.FromHbitmap(image2Bitmap.GetHbitmap());

            var difference = img1.PercentageDifference(img2, threshold);

            img1.Dispose();
            img2.Dispose();

            return difference;

        }

        /// <summary>
        /// Gets the difference between two images as a percentage
        /// </summary>
        /// <returns>The difference between the two images as a percentage</returns>
        /// <param name="image1Bitmap">The first image in Bitmap format</param>
        /// <param name="image2Bitmap">The second image in Bitmap format</param>
        /// <param name="threshold">How big a difference (out of 255) will be ignored - the default is 3.</param>
        /// <returns>The difference between the two images as a percentage</returns>

        public static float GetPercentageDifference(Bitmap image1Bitmap, Bitmap image2Bitmap, byte threshold = 3)
        {

            var img1 = Image.FromHbitmap(image1Bitmap.GetHbitmap());
            var img2 = Image.FromHbitmap(image2Bitmap.GetHbitmap());

            var difference = img1.PercentageDifference(img2, threshold);

            img1.Dispose();
            img2.Dispose();

            return difference;

        }

        /// <summary>
        /// Gets the difference between two images as a percentage
        /// </summary>
        /// <returns>The difference between the two images as a percentage</returns>
        /// <param name="image1Path">The path to the first image</param>
        /// <param name="image2Path">The path to the second image</param>
        /// <returns>The difference between the two images as a percentage</returns>
        public static float GetBhattacharyyaDifference(string image1Path, string image2Path)
        {
            if (!CheckIfFileExists(image1Path) || !CheckIfFileExists(image2Path)) return -1;
            var img1 = Image.FromFile(image1Path);
            var img2 = Image.FromFile(image2Path);

            var difference = img1.BhattacharyyaDifference(img2);

            img1.Dispose();
            img2.Dispose();

            return difference;

        }

        /// <summary>
        /// Gets the difference between two images as a percentage
        /// </summary>
        /// <returns>The difference between the two images as a percentage</returns>
        /// <param name="image1Path">The path to the first image</param>
        /// <param name="image2Bitmap">The second image in Bitmap format</param>
        /// <returns>The difference between the two images as a percentage</returns>
        public static float GetBhattacharyyaDifference(string image1Path, Bitmap image2Bitmap)
        {
            if (!CheckIfFileExists(image1Path)) return -1;
            var img1 = Image.FromFile(image1Path);
            var img2 = Image.FromHbitmap(image2Bitmap.GetHbitmap());

            var difference = img1.BhattacharyyaDifference(img2);

            img1.Dispose();
            img2.Dispose();

            return difference;

        }

        /// <summary>
        /// Gets the difference between two images as a percentage
        /// </summary>
        /// <returns>The difference between the two images as a percentage</returns>
        /// <param name="image1Bitmap">The first image in Bitmap format</param>
        /// <param name="image2Bitmap">The second image in Bitmap format</param>
        /// <returns>The difference between the two images as a percentage</returns>
        public static float GetBhattacharyyaDifference(Bitmap image1Bitmap, Bitmap image2Bitmap)
        {
            var img1 = Image.FromHbitmap(image1Bitmap.GetHbitmap());
            var img2 = Image.FromHbitmap(image2Bitmap.GetHbitmap());

            var difference = img1.BhattacharyyaDifference(img2);

            img1.Dispose();
            img2.Dispose();

            return difference;

        }


        /// <summary>
        /// Find all duplicate images in a folder, and possibly subfolders
        /// IMPORTANT: this method assumes that all files in the folder(s) are images!
        /// </summary>
        /// <param name="folderPath">The folder to look for duplicates in</param>
        /// <param name="checkSubfolders">Whether to look in subfolders too</param>
        /// <returns>A list of all the duplicates found, collected in separate Lists (one for each distinct image found)</returns>
        public static IEnumerable<IEnumerable<string>> GetDuplicateImages(string folderPath, bool checkSubfolders)
        {
            var imagePaths = Directory.GetFiles(folderPath, "*.*", checkSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).ToList();
            return GetDuplicateImages(imagePaths);
        }

        /// <summary>
        /// Find all duplicate images from in list
        /// </summary>
        /// <param name="pathsOfPossibleDuplicateImages">The paths to the images to check for duplicates</param>
        /// <returns>A list of all the duplicates found, collected in separate Lists (one for each distinct image found)</returns>
        public static IEnumerable<IEnumerable<string>> GetDuplicateImages(IEnumerable<string> pathsOfPossibleDuplicateImages)
        {
            var imagePathsAndGrayValues = GetSortedGrayscaleValues(pathsOfPossibleDuplicateImages);
            var duplicateGroups = GetDuplicateGroups(imagePathsAndGrayValues);

            return duplicateGroups.Select(list => list.Select(tuple => tuple.Item1).ToList()).ToList();
        }

        #region Helpermethods

        private static IEnumerable<Tuple<string, byte[,]>> GetSortedGrayscaleValues(IEnumerable<string> pathsOfPossibleDuplicateImages)
        {
            var imagePathsAndGrayValues = new List<Tuple<string, byte[,]>>();
            foreach (var imagePath in pathsOfPossibleDuplicateImages)
            {
                using var image = Image.FromFile(imagePath);
                var grayValues = image.GetGrayScaleValues();
                var tuple = new Tuple<string, byte[,]>(imagePath, grayValues);
                imagePathsAndGrayValues.Add(tuple);
            }

            imagePathsAndGrayValues.Sort(Comparer);
            return imagePathsAndGrayValues;
        }

        private static IEnumerable<IEnumerable<Tuple<string, byte[,]>>> GetDuplicateGroups(IEnumerable<Tuple<string, byte[,]>> imagePathsAndGrayValues)
        {
            var duplicateGroups = new List<IEnumerable<Tuple<string, byte[,]>>>();
            var currentDuplicates = new List<Tuple<string, byte[,]>>();

            foreach (var tuple in imagePathsAndGrayValues)
            {
                if (currentDuplicates.Any() && Comparer.Compare(currentDuplicates.First(), tuple) != 0)
                {
                    if (currentDuplicates.Count > 1)
                    {
                        duplicateGroups.Add(currentDuplicates);
                        currentDuplicates = new List<Tuple<string, byte[,]>>();
                    }
                    else
                    {
                        currentDuplicates.Clear();
                    }
                }

                currentDuplicates.Add(tuple);
            }
            if (currentDuplicates.Count > 1)
            {
                duplicateGroups.Add(currentDuplicates);
            }
            return duplicateGroups;
        }

        private static bool CheckIfFileExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File '" + filePath + "' not found!");
            }
            return true;
        }
        #endregion

    }
}
