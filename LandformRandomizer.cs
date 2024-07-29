/**************************************************************************************************************************
* Copyright 2024, Peter R. Nelson
*
* This file is part of the MapCreator application. The MapCreator application is intended
* for creating fantasy maps for gaming and world building.
*
* MapCreator is free software: you can redistribute it and/or modify it under the terms
* of the GNU General Public License as published by the Free Software Foundation,
* either version 3 of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
* without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
* See the GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License along with this program.
* The text of the GNU General Public License (GPL) is found in the LICENSE file.
* If the LICENSE file is not present or the text of the GNU GPL is not present in the LICENSE file,
* see https://www.gnu.org/licenses/.
*
* For questions about the MapCreator application or about licensing, please email
* contact@brookmonte.com
*
***************************************************************************************************************************/
using SkiaSharp;
using System.ComponentModel;

namespace MapCreator
{
    internal class LandformRandomizer
    {
        private BackgroundWorker randomizerBackgroundWorker = new();
        readonly MainForm main;
        private readonly MapCreatorMap map;
        private MapLandformType2? mapLandform;

        public LandformRandomizer(MainForm main, MapCreatorMap map)
        {
            this.main = main;
            this.map = map;
        }

        public void RandomizeLandform(ref MapLandformType2 mapLandform)
        {
            this.mapLandform = mapLandform;

            randomizerBackgroundWorker = new()
            {
                WorkerReportsProgress = true
            };

            randomizerBackgroundWorker.DoWork += LandformRandomizerBackgroundWorker_DoWork;
            randomizerBackgroundWorker.ProgressChanged += LandformRandomizerBackgroundWorker_ProgressChanged;
            randomizerBackgroundWorker.RunWorkerCompleted += LandformRandomizerBackgroundWorker_RunWorkerCompleted;

            randomizerBackgroundWorker.RunWorkerAsync();
        }

        private void LandformRandomizerBackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            if (mapLandform == null) return;

            // TODO: randomization doesn't work very well
            Random r = new();

            SKPath newPath = new(mapLandform.LandformContourPath)
            {
                FillType = SKPathFillType.Winding
            };

            randomizerBackgroundWorker.ReportProgress(0);

#pragma warning disable CS8629 // Nullable value type may be null.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            float coastEffectPathWidth = (float)(mapLandform.CoastlineEffectDistance / 8.0F);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8629 // Nullable value type may be null.


            int progressPercent = 10;


            randomizerBackgroundWorker.ReportProgress(5, "Collecting " + newPath.PointCount + " Points...");

            mapLandform.LandformPath.GetTightBounds(out SKRect bounds);

            List<SKPoint> addPoints = [];

            // how often to add a new circle to the path?
            for (int j = 1; j < newPath.PointCount; j += 4)
            {
                progressPercent = (int)(100 * (float)j / newPath.PointCount);
                addPoints.Add(newPath.GetPoint(j));
            }

            randomizerBackgroundWorker.ReportProgress(10, "Collected Points. Randomizing " + addPoints.Count + " Points...");

            int k = 0;
            string statusText = "Collected Points. Randomizing " + addPoints.Count + " Points.";
            string periods = "";

            foreach (SKPoint p in addPoints)
            {
                k++;
                progressPercent = (int)(100 * (float)k / addPoints.Count);
                randomizerBackgroundWorker.ReportProgress(progressPercent, statusText);
                statusText += periods;

                periods += ".";

                if (k % 8 == 0)
                {
                    statusText = "Collected Points. Randomizing " + addPoints.Count + " Points.";
                    periods = "";
                }

                int rndRadius = r.Next(4, 15);

                using SKPath shapePath = new()
                {
                    FillType = SKPathFillType.Winding
                };

                shapePath.Reset();

                int addRemove = r.Next(0, 2);
                //float radiusOffset = 1.5708F; //1.5708 = Pi / 2
                float radiusOffset = 1;

                // add a circle at the point, shifted toward the middle of the path by the radius of the circle times Pi / 2
                if (p.X < bounds.MidX)
                {
                    

                    if (p.Y < bounds.MidY)
                    {
                        shapePath.AddCircle(p.X + rndRadius * radiusOffset, p.Y + rndRadius * radiusOffset, rndRadius);
                    }
                    else
                    {
                        shapePath.AddCircle(p.X + rndRadius * radiusOffset, p.Y - rndRadius * radiusOffset, rndRadius);
                    }
                }
                else
                {
                    if (p.Y < bounds.MidY)
                    {
                        shapePath.AddCircle(p.X - rndRadius * radiusOffset, p.Y + rndRadius * radiusOffset, rndRadius);
                    }
                    else
                    {
                        shapePath.AddCircle(p.X - rndRadius * radiusOffset, p.Y - rndRadius * radiusOffset, rndRadius);
                    }
                }

                if (addRemove == 0)
                {
                    using SKPath diffPath = newPath.Op(shapePath, SKPathOp.Difference);
                    newPath.Dispose();
                    newPath = new(diffPath);
                }
                else if (addRemove == 1)
                {
                    using SKPath diffPath = newPath.Op(shapePath, SKPathOp.Union);
                    newPath.Dispose();
                    newPath = new(diffPath);
                }

                //mapLandform.LandformPath = newPath;
                //LandformType2Methods.CreateType2LandformPaths(map, mapLandform);

                //newPath = new(mapLandform.LandformContourPath)
                //{
                //    FillType = SKPathFillType.Winding
                //};

            }

            randomizerBackgroundWorker.ReportProgress(95, "Creating landform paths");

            mapLandform.LandformPath = newPath;
            LandformType2Methods.CreateType2LandformPaths(map, mapLandform);


            randomizerBackgroundWorker.ReportProgress(99, "Merging Complete.");
        }

        private void LandformRandomizerBackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            main.UpdateProgressBar(e.ProgressPercentage, e.UserState as string);
        }

        private void LandformRandomizerBackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDFORMLAYER)?.Clear();
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.Clear();



            main.RenderDrawingPanel();
            main.UpdateProgressBar(0, "Randomization Complete.");
        }
    }
}
