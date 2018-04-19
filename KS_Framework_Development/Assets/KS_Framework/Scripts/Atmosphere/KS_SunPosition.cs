using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS_Core.Atmosphere.SunCalc
{
    public static class KS_SunCalc
    {
        private const double Deg2Rad = Math.PI / 180.0;
        private const double Rad2Deg = 180.0 / Math.PI;

        /// <summary>
        /// Calculate the suns position for any given time and Latitude and longitude
        /// </summary>
        /// <param name="hour">Hour of the day 0-23</param>
        /// <param name="min">Minute 0-59</param>
        /// <param name="sec">Seconds 0-59</param>
        /// <param name="latitude">Position on earth as latitude</param>
        /// <param name="longitude">Position on the earth as longitude</param>
        /// <returns>Vector2 with sun position, X = Latitude, Y = Longitude</returns>
        public static Vector2 CalculateSunPosition(
            int hour,
            int min,
            int sec,
            double latitude,
            double longitude)
        {
            DateTime dateTime = new DateTime(DateTime.Now.Year,
                                             DateTime.Now.Month,
                                             DateTime.Now.Day,
                                             hour,
                                             min,
                                             sec);

            // Get Julian Date and Centuries
            double JD = ToJulianDate(dateTime);
            double JC = ToJulianCenturies(JD);

            //Debug.Log("Julian Date: " + JD + " - Julian Centuries: " + JC);

            // SideReal time
            double sideRealTime = GetLST(dateTime, JD, JC, longitude);

            //Debug.Log("Sidereal Time: " + sideRealTime);

            // Refine Time to add current percentage of day
            JD += (double)dateTime.TimeOfDay.TotalHours / 24.0;
            JC = JD / 36525.0;

            //Debug.Log("Refined Julian Date: " + JD + " - refined Julian Centuries: " + JC);

            // Solar Coordinates 
            double meanLongitude = GetMeanLongitude(JC);
            double meanAnomaly = GetMeanAnomaly(JC);
            double equationOfCenter = GetEquationOfCenter(JC, meanAnomaly);
            double elipticalLongitude = GetElipticalLongitude(meanLongitude, equationOfCenter);
            double obliquity = GetObliquity(JC);
            double rightAscension = GetRightAscension(obliquity, elipticalLongitude);
            double declination = GetDeclination(rightAscension, obliquity);

            // Terestial Coordinates

            double hourAngle = GetHorizontalCoordinates(sideRealTime, rightAscension);
            double altitude = GetAltitude(declination, hourAngle, latitude);
            double azimuth = GetAzimuth(hourAngle, declination, latitude);


            return new Vector2(Convert.ToSingle(altitude * Rad2Deg),
                               Convert.ToSingle(azimuth * Rad2Deg));

        }

        private static double GetMeanLongitude(double julianCenturies)
        {
            return CorrectAngle(Deg2Rad * (280.466 + 36000.77 * julianCenturies));
        }

        private static double GetMeanAnomaly(double julianCenturies)
        {
            return CorrectAngle(Deg2Rad * (357.529 + 35999.05 * julianCenturies));
        }

        private static double GetEquationOfCenter(double julianCenturies, double meanAnomaly)
        {
            return Deg2Rad * ((1.915 - 0.005 * julianCenturies) *
                   Math.Sin(meanAnomaly) + 0.02 * Math.Sin(2 * meanAnomaly));
        }

        private static double GetElipticalLongitude(double meanLongitude, double equationOfCenter)
        {
            return CorrectAngle(meanLongitude + equationOfCenter);
        }

        private static double GetObliquity(double julianCenturies)
        {
            return (23.439 - 0.013 * julianCenturies) * Deg2Rad;
        }

        private static double GetRightAscension(double obliquity, double elipticalLongitude)
        {
            return Math.Atan2(
                Math.Cos(obliquity) * Math.Sin(elipticalLongitude),
                Math.Cos(elipticalLongitude)
                );
        }

        private static double GetDeclination(double rightAscension, double obliquity)
        {
            return Math.Sin(rightAscension) * Math.Sin(obliquity);
        }

        private static double GetHorizontalCoordinates(double sideRealTime, double rightAscension)
        {
            double hAngle = CorrectAngle(sideRealTime * Deg2Rad) - rightAscension;

            if (hAngle > Math.PI)
            {
                hAngle -= 2 * Math.PI;
            }

            return hAngle;
        }

        private static double GetAltitude(double declination, double hourAngle, double latitude)
        {
            return Math.Asin(
                Math.Sin(latitude * Deg2Rad) *
                Math.Sin(declination) + Math.Cos(latitude * Deg2Rad) *
                Math.Cos(declination) * Math.Cos(hourAngle));
        }

        private static double GetAzimuth(double hourAngle, double declination, double latitude)
        {
            double aN = -Math.Sin(hourAngle);
            double aD = Math.Tan(declination) * Math.Cos(latitude * Deg2Rad) -
                        Math.Sin(latitude * Deg2Rad) * Math.Cos(hourAngle);

            double azimuth = Math.Atan(aN / aD);

            if (aD < 0)
            {
                azimuth += Math.PI;
            }
            else if (aN < 0)
            {
                azimuth += 2 * Math.PI;
            }

            return azimuth;
        }

        private static double ToJulianDate(DateTime date)
        {
            // Convert to UTC  
            date = date.ToUniversalTime();

            return 367 * date.Year -
                (int)((7.0 / 4.0) * (date.Year +
                (int)((date.Month + 9.0) / 12.0))) +
                (int)((275.0 * date.Month) / 9.0) +
                date.Day - 730531.5;
        }

        private static double ToJulianCenturies(double julianDate)
        {
            return julianDate / 36525.0;
        }

        private static double GetLST(DateTime date, double JulianDay, double JulianCentuires, double longitude)
        {
            double siderealTimeHours = 6.6974 + 2400.0513 * JulianCentuires;

            double siderealTimeUT = siderealTimeHours +
                (366.2422 / 365.2422) * (double)date.TimeOfDay.TotalHours;

            return siderealTimeUT * 15 + longitude;
        }

        private static double CorrectAngle(double angleInRadians)
        {
            if (angleInRadians < 0)
            {
                return 2 * Math.PI - (Math.Abs(angleInRadians) % (2 * Math.PI));
            }
            else if (angleInRadians > 2 * Math.PI)
            {
                return angleInRadians % (2 * Math.PI);
            }
            else
            {
                return angleInRadians;
            }
        }
    }
}