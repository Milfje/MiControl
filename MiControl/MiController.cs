﻿using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace MiControl
{
    /// <summary>
    /// Class for controlling a MiLight WiFi enabled control box
    /// </summary>
    public class MiController
    {
        #region Properties

        UdpClient Controller; // Handles communication with the controller
        int RGBActiveGroup;
        int whiteActiveGroup;

        #endregion


        #region Constructor

        /// <summary>
        /// Constructs a new MiLight control class.
        /// </summary>
        /// <param name="ip">The IP-address of the MiLight controller</param>
        public MiController(string ip)
        {
            Controller = new UdpClient(ip, 8899);
            RGBActiveGroup = 0;
        }

        #endregion

        #region Static Methods

        // TODO: Method for finding MiLight WiFi controllers on the network.

        #endregion


        #region RGB Methods

        /// <summary>
        /// Switches a specified group of RGB bulbs on. Can be used to 
        /// link bulbs to a group (first time setup).
        /// </summary>
        /// <param name="group">The group to switch on. 1-4 or 0 for all groups.</param>
        public void RGBSwitchOn(int group)
        {
            CheckGroup(group);

            var groups = new byte[] { 0x42, 0x45, 0x47, 0x49, 0x4B };
            Controller.Send(new byte[] { groups[group], 0x00, 0x55 }, 3);
            RGBActiveGroup = group;
        }

        /// <summary>
        /// Switches a specified group of RGB bulbs off.
        /// </summary>
        /// <param name="group">The group to switch off. 1-4 or 0 for all groups.</param>
        public void RGBSwitchOff(int group)
        {
            CheckGroup(group);

            var groups = new byte[] { 0x41, 0x46, 0x48, 0x4A, 0x4C };
            Controller.Send(new byte[] { groups[group], 0x00, 0x55 }, 3);
            RGBActiveGroup = group;
        }

        /// <summary>
        /// Switches the specified group of RGB bulbs to white.
        /// </summary>
        /// <param name="group">The group to switch off. 1-4 or 0 for all groups.</param>
        public void RGBSwitchWhite(int group)
        {
            CheckGroup(group);

            var groups = new byte[] { 0xC2, 0xC5, 0xC7, 0xC9, 0xCB };
            Controller.Send(new byte[] { groups[group], 0x00, 0x55 }, 3);
        }

        /// <summary>
        /// Sets the brightness for a group or all groups.
        /// </summary>
        /// <param name="group">The group for which to set the brightness. 1-4 or 0 for all groups.</param>
        /// <param name="percentage">The percentage (0-100) of brightness to set.</param>
        public void RGBSetBrightness(int group, int percentage)
        {
            if (percentage < 0 || percentage > 100) {
                throw new Exception("Brightness must be between 0 and 100");
            }

            var brightness = new byte[]
            { 0x02,0x03,0x04,0x05,0x08,0x09,
              0x0A,0x0B,0x0D,0x0E,0x0F,0x10,
              0x11,0x12,0x13,0x14,0x15,0x17,
              0x18,0x19 };

            var index = (int)Math.Max(0, (Math.Ceiling((double)percentage / 100 * 19)) - 1);
            Controller.Send(new byte[] { 0x4E, brightness[index], 0x55 }, 3);
        }

        /// <summary>
        /// Sets a given group of RGB bulbs to the specified color.
        /// </summary>
        /// <param name="group">The group for which to set the color.</param>
        /// <param name="color">The color to set.</param>
        public void RGBSetColor(int group, Color color)
        {
            // Send 'on' to select correct group if it 
            // is not the currently selected group
            if (RGBActiveGroup != group) {
                RGBSwitchOn(group);
                RGBActiveGroup = group;
            }

            Controller.Send(new byte[] { 0x40, HueToMiLight(color.GetHue()), 0x55 }, 3);
        }

        #endregion

        #region White Methods
        // Oh, scheisse...

        #endregion


        #region Help Methods

        /// <summary>
        /// Checks if the specified group is between 0 and 4.
        /// Throws an Exception otherwise.
        /// </summary>
        /// <param name="group">The group to check.</param>
        private static void CheckGroup(int group)
        {
            if (group < 0 || group > 4) {
                throw new Exception("Specified group must be between 0 and 4.");
            }
        }

        /// <summary>
        /// Calculates the MiLight color value from a given Hue.
        /// </summary>
        /// <param name="hue">The Hue (in degrees) to convert.</param>
        /// <returns>Returns a byte for use in MiLight command.</returns>
        private static byte HueToMiLight(float hue)
        {
            return (byte)((256 + 176 - (int)(hue / 360.0 * 255.0)) % 256);
        }

        #endregion
    }
}
