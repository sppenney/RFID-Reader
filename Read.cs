﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Device.Gpio;
using Iot.Device.Card;
using Iot.Device.Card.Mifare;
using Iot.Device.Pn532;
using Iot.Device.Pn532.ListPassive;

namespace Read
{
    public class Reader
    {
        private static Pn532 rfidHat;
        public static void init()
        {
            try
            {
                string reader = "/dev/ttyS0";
                rfidHat = new Pn532(reader);
                byte[] readUid = null;
                readUid = rfidHat.ListPassiveTarget(MaxTarget.One, TargetBaudRate.B106kbpsTypeA);
                Thread.Sleep(200);

                do
                {
                    var decrypted = rfidHat.TryDecode106kbpsTypeA(readUid.AsSpan().Slice(1));
                    var uid = BitConverter.ToString(decrypted.NfcId);
                    Console.WriteLine("Unique User ID:  " + uid);
                    rfidHat.ReleaseTarget(1);
                    close();
                    break; 
                }

                while(readUid != null);
                
                close();

            }
            
            catch(System.Exception)
            {init();}
        }
        public static void close()
        {
            if (rfidHat != null)
            {
                Thread.Sleep(500);
                rfidHat.Dispose();
                RFID.Menu.Main();
            }
        }
        //public static void ReadCode()
        //{
            // Creating the device class through serial: /dev/ttyS0
            //try
            //{
                //do
                //{
                    //var decrypted = reader.TryDecode106kbpsTypeA(readUid.AsSpan().Slice(1));
                    //var uid = BitConverter.ToString(decrypted.NfcId);
                    //Console.WriteLine("Unique User ID:  " + uid);                    
                    
                    /* Use the following code to read specific blocks

                    MifareCard mifareCard = new MifareCard(reader, decrypted.TargetNumber)
                    {BlockNumber = 0, Command = MifareCardCommand.AuthenticationA};
                    mifareCard.SetCapacity(decrypted.Atqa, decrypted.Sak);
                    mifareCard.SerialNumber = decrypted.NfcId;
                    mifareCard.KeyA = new byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    mifareCard.KeyB = new byte[6] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

                     for (byte block = 1; block < 2; block++)
                        {
                            mifareCard.BlockNumber = block;
                            mifareCard.Command = MifareCardCommand.AuthenticationB;
                            var ret = mifareCard.RunMifiCardCommand();
                            if (ret < 0)
                            {
                                // Try another one
                                mifareCard.Command = MifareCardCommand.AuthenticationA;
                                ret = mifareCard.RunMifiCardCommand();
                            }

                            if (ret == 1)
                            {
                                mifareCard.BlockNumber = block;
                                mifareCard.Command = MifareCardCommand.Read16Bytes;
                                ret = mifareCard.RunMifiCardCommand();
                                Console.WriteLine();
                                Console.WriteLine("Block " + block + ": " + BitConverter.ToString(mifareCard.Data));
                                continue;
                            }
                        }

                    End codeblock */


                //}

                //while(readUid != null);

                //Thread.Sleep(500);
                //reader.Dispose();
                //ReadCode();
            //}

            //catch (System.Exception)
            //{ 
            //    ReadCode();
            //}        
        //}


    }    

}