using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Device.Gpio;
using Iot.Device.Card;
using Iot.Device.Card.Mifare;
using Iot.Device.Pn532;
using Iot.Device.Pn532.ListPassive;
using Iot.Device.Pn532.RfConfiguration;
using Iot.Device.Rfid;

namespace Write
{
    public class Program
    {
        public static void WriteCode()
        {
            // Creating the device class through serial: /dev/ttyS0
            try
            {
                string device = "/dev/ttyS0";
                var reader = new Pn532(device);
                byte[] writeUser = null;
                writeUser = reader.ListPassiveTarget(MaxTarget.One, TargetBaudRate.B106kbpsTypeA);
                Thread.Sleep(200);

                do
                {
                    var decrypted = reader.TryDecode106kbpsTypeA(writeUser.AsSpan().Slice(1));
                    var uid = BitConverter.ToString(decrypted.NfcId);   
                    
                    Console.WriteLine("Enter Username:");
                    var userName = Console.ReadLine();
                    byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(userName);
                    byte[] cardData = null;
                    reader.Transceive(1, byteArray, cardData);
                    
                    
                    MifareCard mifareCard = new MifareCard(reader, decrypted.TargetNumber)
                    {BlockNumber = 0, Command = MifareCardCommand.AuthenticationA};
                    mifareCard.SetCapacity(decrypted.Atqa, decrypted.Sak);
                    mifareCard.SerialNumber = decrypted.NfcId;
                    mifareCard.KeyA = new byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    mifareCard.KeyB = new byte[6] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

                     for (byte block = 0; block < 1; block++)
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
                                mifareCard.Command = MifareCardCommand.Write16Bytes;
                                
                                ret = mifareCard.RunMifiCardCommand();
                                Console.WriteLine(BitConverter.ToString(cardData));
                                continue;
                            }
                        }
                         
                    reader.ReleaseTarget(1);
                    break;
                }

                while(writeUser != null);
            }

            catch (System.Exception)
            {
                WriteCode();
            }  
        }

    }    
}