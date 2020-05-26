﻿
/*
 * Minimal Apple Homekit emulator with minimal dependencies
 * (c) Eric Sambell 2020 MIT License
 * 
 * SHA-512 based on Crhis Veness Java Script SHA-512 implementation (https://www.movable-type.co.uk/scripts/sha512.html)
 * Big integare arithmatic based on answer from: https://stackoverflow.com/questions/2207006/modular-exponentiation-for-high-numbers-in-c by clinux
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Makaretu.Dns;
using System.Numerics;
using System.Security.Policy;
using System.Runtime.Remoting.Messaging;

namespace HomeKit_Test
{
    public partial class Form1 : Form
    {

        struct UInt64c
        {
            public UInt32 lo;
            public UInt32 hi;
        };
        struct int32Array
        {
            public UInt32[] digits;
            public Boolean negative;
        }

        String[] k =
            { "428a2f98d728ae22", "7137449123ef65cd", "b5c0fbcfec4d3b2f", "e9b5dba58189dbbc",
            "3956c25bf348b538", "59f111f1b605d019", "923f82a4af194f9b", "ab1c5ed5da6d8118",
            "d807aa98a3030242", "12835b0145706fbe", "243185be4ee4b28c", "550c7dc3d5ffb4e2",
            "72be5d74f27b896f", "80deb1fe3b1696b1", "9bdc06a725c71235", "c19bf174cf692694",
            "e49b69c19ef14ad2", "efbe4786384f25e3", "0fc19dc68b8cd5b5", "240ca1cc77ac9c65",
            "2de92c6f592b0275", "4a7484aa6ea6e483", "5cb0a9dcbd41fbd4", "76f988da831153b5",
            "983e5152ee66dfab", "a831c66d2db43210", "b00327c898fb213f", "bf597fc7beef0ee4",
            "c6e00bf33da88fc2", "d5a79147930aa725", "06ca6351e003826f", "142929670a0e6e70",
            "27b70a8546d22ffc", "2e1b21385c26c926", "4d2c6dfc5ac42aed", "53380d139d95b3df",
            "650a73548baf63de", "766a0abb3c77b2a8", "81c2c92e47edaee6", "92722c851482353b",
            "a2bfe8a14cf10364", "a81a664bbc423001", "c24b8b70d0f89791", "c76c51a30654be30",
            "d192e819d6ef5218", "d69906245565a910", "f40e35855771202a", "106aa07032bbd1b8",
            "19a4c116b8d2d0c8", "1e376c085141ab53", "2748774cdf8eeb99", "34b0bcb5e19b48a8",
            "391c0cb3c5c95a63", "4ed8aa4ae3418acb", "5b9cca4f7763e373", "682e6ff3d6b2b8a3",
            "748f82ee5defb2fc", "78a5636f43172f60", "84c87814a1f0ab72", "8cc702081a6439ec",
            "90befffa23631e28", "a4506cebde82bde9", "bef9a3f7b2c67915", "c67178f2e372532b",
            "ca273eceea26619c", "d186b8c721c0c207", "eada7dd6cde0eb1e", "f57d4f7fee6ed178",
            "06f067aa72176fba", "0a637dc5a2c898a6", "113f9804bef90dae", "1b710b35131c471b",
            "28db77f523047d84", "32caab7b40c72493", "3c9ebe0a15c9bebc", "431d67c49c100d4c",
            "4cc5d4becb3e42b6", "597f299cfc657e2a", "5fcb6fab3ad6faec", "6c44198c4a475817"};
        UInt64c[] K = new UInt64c[80];


        String[] h =
            {  "6a09e667f3bcc908", "bb67ae8584caa73b", "3c6ef372fe94f82b", "a54ff53a5f1d36f1",
            "510e527fade682d1", "9b05688c2b3e6c1f", "1f83d9abfb41bd6b", "5be0cd19137e2179"};
        UInt64c[] HInitial = new UInt64c[80];

        UInt32[] N = {  0xFFFFFFFF, 0xFFFFFFFF, 0xC90FDAA2, 0x2168C234, 0xC4C6628B ,0x80DC1CD1, 0x29024E08, 0x8A67CC74,
                        0x020BBEA6, 0x3B139B22, 0x514A0879, 0x8E3404DD, 0xEF9519B3, 0xCD3A431B, 0x302B0A6D, 0xF25F1437,
                        0x4FE1356D, 0x6D51C245, 0xE485B576, 0x625E7EC6, 0xF44C42E9, 0xA637ED6B, 0x0BFF5CB6, 0xF406B7ED,
                        0xEE386BFB, 0x5A899FA5, 0xAE9F2411, 0x7C4B1FE6, 0x49286651, 0xECE45B3D, 0xC2007CB8, 0xA163BF05,
                        0x98DA4836, 0x1C55D39A, 0x69163FA8, 0xFD24CF5F, 0x83655D23, 0xDCA3AD96, 0x1C62F356, 0x208552BB,
                        0x9ED52907, 0x7096966D, 0x670C354E, 0x4ABC9804, 0xF1746C08, 0xCA18217C, 0x32905E46, 0x2E36CE3B,
                        0xE39E772C, 0x180E8603, 0x9B2783A2, 0xEC07A28F, 0xB5C55DF0, 0x6F4C52C9, 0xDE2BCBF6, 0x95581718,
                        0x3995497C, 0xEA956AE5, 0x15D22618, 0x98FA0510, 0x15728E5A, 0x8AAAC42D, 0xAD33170D, 0x04507A33,
                        0xA85521AB, 0xDF1CBA64, 0xECFB8504, 0x58DBEF0A, 0x8AEA7157, 0x5D060C7D, 0xB3970F85, 0xA6E1E4C7,
                        0xABF5AE8C, 0xDB0933D7, 0x1E8C94E0, 0x4A25619D, 0xCEE3D226, 0x1AD2EE6B, 0xF12FFA06, 0xD98A0864,
                        0xD8760273, 0x3EC86A64, 0x521F2B18, 0x177B200C, 0xBBE11757, 0x7A615D6C, 0x770988C0, 0xBAD946E2,
                        0x08E24FA0, 0x74E5AB31, 0x43DB5BFC, 0xE0FD108E, 0x4B82D120, 0xA93AD2CA, 0xFFFFFFFF, 0xFFFFFFFF
        };

        UInt32[] s = { 0xBEB25379, 0xD1A8581E, 0xB5A72767, 0x3A2441EE };

        UInt32[] a = {  0x60975527, 0x035CF2AD, 0x1989806F, 0x0407210B,
                        0xC81EDC04, 0xE2762A56, 0xAFD529DD, 0xDA2D4393
        };

        UInt32[] b = {  0xE487CB59, 0xD31AC550, 0x471E81F0, 0x0F6928E0,
            0x1DDA08E9, 0x74A004F4, 0x9E61F5D1, 0x05284D20
        };

        UInt32[] storedk;
        UInt32[] storedB;
        UInt32[] storedA;
        UInt32[] storedv;
        UInt32[] storedb;
        UInt32[] storedK;
        UInt32[] storedx;
        UInt32[] storedu;
        UInt32[] storedS;

        const int DevicePort = 5252;
        const string DeviceCode = "143-83-105";
        const Byte generator = 5;
        UInt32[] g = { generator };
        Random random = new Random();

        bool paired = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            K = initHexArray(k);
            HInitial = initHexArray(h);
            N = UInt32ArrayReverse(N);
            a = UInt32ArrayReverse(a);
            b = UInt32ArrayReverse(b);


            var mdns = new MulticastService(NetFilter);
            mdns.UseIpv4 = true;
            mdns.UseIpv6 = false;

            var sd = new ServiceDiscovery(mdns);

            var za = new ServiceProfile("Test HAP", "_hap._tcp", DevicePort);
            za.AddProperty("c#", "12");
            za.AddProperty("ff", "1");
            za.AddProperty("id", "12:13:12:12:12:13");
            za.AddProperty("md", "Ver 1");
            za.AddProperty("sf", "1");
            za.AddProperty("ci", "1");
            za.AddProperty("pv", "1.1");
            sd.Advertise(za);

            mdns.Start();
            TCPListenerTask.RunWorkerAsync();

            int32Array A, B, mod;
            A.digits = new uint[] { 0x06, 0xFFFFFFFF, 0xFFFF };
            A.digits = new uint[] { 0x5 };

            A.negative = false;
            BigInteger bigA = new BigInteger(fromUInt32Array(A.digits).Concat(new Byte[] { 0 }).ToArray());

            B.digits = new uint[] { 0xFFFFF };
            ////b.digits = s.Concat(new UInt32[] {  0x12345678, 0x12345678, 0x12345678, 0x12345678, 0x12345678, 0x12345678, 0x12345678, 0x12345678, 
            //                                    0x12345678, 0x12345678, 0x12345678, 0x12345678

            //}).ToArray();
            B.digits = new UInt32[] {   0xd1ff15eb , 0x16a1e0f1 , 0x0fb9ceaf , 0xb2b1db62 , 0x7b288f80 , 0xeb07c3ba , 0x5c182e5d , 0x141534d5,
                                        0x6693bac4 , 0xdccda4c6 , 0x577a8179 , 0xe520c965 , 0xa4907a1a , 0x81c38c8b , 0x544fa6e3 , 0xf36091f1
            };

            //b.digits = new UInt32[] {   0xd1ff15eb , 0x16a1e0f1 , 0x0fb9ceaf , 0xb2b1db62 , 0x7b288f80 , 0xeb07c3ba , 0x5c182e5d , 0x141534d5,
            //                            0x6693bac4 , 0xdccda4c6 , 0x577a8179 , 0xe520c965 , 0xa4907a1a , 0x81c38c8b , 0x8FFFFFFF
            //};

            //b.digits = new uint[] { 0x8FFFFFF, 0x8FFFFFFF, 0x8FFFFFFF };

            B.digits = a;
            B.negative = false;
            BigInteger bigB = new BigInteger(fromUInt32Array(B.digits).Concat(new Byte[] { 0 }).ToArray());

            mod.digits = new uint[] { 0xFFFFFFFF, 0xFFFFF };
            mod.digits = N;

            //mod.digits = new uint[] { 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000,
            //    0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000,
            //    0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000,
            //    0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000,
            //    0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000,
            //    0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000,
            //    0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000,
            //    0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000,
            //    0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000,
            //    0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000,
            //    0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000,
            //    0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000, 0x5000000

            //};
            mod.negative = false;
            BigInteger bigMod = new BigInteger(fromUInt32Array(mod.digits).Concat(new Byte[] { 0 }).ToArray());

            int32Array testResult;
            testResult.digits = new UInt32[] { 0 };
            //testResult.negative = false;
            // testResult = int32ArrayAddNoSign(a, b, mod);
            //testResult.digits = UInt32ArraySubSimple(a.digits, b.digits);
            //testResult.digits = UInt32ArrayMulNoSign(a.digits, b.digits, mod.digits);
            //testResult.digits = UInt32ArrayMod(a.digits, mod.digits);
            //testResult = int32ArraySHR(a);
            //testResult.digits = UInt32PowMod(A.digits, b.digits, mod.digits);


            UInt32[] q;

            //UInt32ArrayDiv(new uint[] { 0xFFFFFFF2, 0XFFF}, new uint[] { 0xF2342342, 0x12}, out q, out testResult.digits) ;

            //UInt32[] m = new uint[] { 7 };
            //UInt32[] R = new uint[] { 0x0,0x0, 0x1 };
            //UInt32[] ba = new uint[] { 0x0, 0x1 };

            //UInt32[] R_inv = UInt32ArrayGetInverse(R, m);

            //UInt32[] m_inv = UInt32ArrayGetInverse(m, ba);
            //UInt32[] m_prime = UInt32ArraySubSimple(ba, m_inv);
            //UInt32ArrayShrink(ref m_prime);
           
            UInt32[] temp = new uint[] { 0xffffffff, 0xf};
            UInt32[] temp1 = new uint[] { 0xfffffff };

            //UInt32ArraySHRWords(ref temp, 25);

            //testResult.digits = UInt32ArrayMulNoSign(temp, temp1);

            //foreach (UInt32 i in testResult.digits)
            //{
            //    //AddToLogBox(i.ToString() + " " + i.ToString("X8") + "\r\n");
            //    AddToLogBox(i.ToString("X8") + "\r\n");
            //}

            //Byte[] byteTest = new byte[] { 0x69, 0x69 };
            

            //appendTLVBytes(ref byteTest, 0x12, Enumerable.Repeat((Byte)0x61,300).ToArray());
            
            //foreach (Byte i in byteTest)
            //{
            //    //AddToLogBox(i.ToString() + " " + i.ToString("X8") + "\r\n");
            //    AddToLogBox(i.ToString("X2") + "\r\n");
            //}

            //       BigInteger bigIntTest = BigInteger.ModPow(bigA, bigB, bigMod);
            //   AddToLogBox(bigIntTest.ToString("X8") + "\r\n");

            //int testCmp = int32ArrayCmp(A, B);
            //AddToLogBox(testCmp.ToString() + "\r\n");

        }
        private IEnumerable<System.Net.NetworkInformation.NetworkInterface> NetFilter(IEnumerable<System.Net.NetworkInformation.NetworkInterface> InterfacesIn)
        {

            IEnumerable<System.Net.NetworkInformation.NetworkInterface> InterfacesOut;

            //foreach(NetworkInterface CurNetworkInterface in InterfacesIn)
            //{
            //    MessageBox.Show(CurNetworkInterface.Name);
            //}

            InterfacesOut = InterfacesIn.Where(x => x.Name == "Ethernet 4");

            //foreach (NetworkInterface CurNetworkInterface in InterfacesOut)
            //{
            //    MessageBox.Show(CurNetworkInterface.Name);
            //}

            return InterfacesOut;
        }

        private void TCPListenerTask_DoWork(object sender, DoWorkEventArgs e)
        {
            TcpListener server = new TcpListener(IPAddress.Any, DevicePort);
            server.Start();
            Byte[] bytes = new Byte[256];
            String data = null;
            int numConnects = 0;
            Byte[] received = new Byte[1];
            
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                data = null;

                int i;
                numConnects++;
                label1.BeginInvoke((MethodInvoker)delegate ()
                {
                    label1.Text = numConnects.ToString();
                });
                NetworkStream stream = client.GetStream();
                String request = null;
                bool isLineBlank = true;
                bool parsingHeader = true;
                int dataBytesReceived = 0;
                int contentLength = 0;
                string requestType = null;
                string uri = null;
                while (client.Connected)

                {

                    while ((i = stream.Read(received, 0, 1)) != 0)
                    {

                        if (parsingHeader)
                        {
                            // Translate data bytes to a ASCII string.
                            data = System.Text.Encoding.ASCII.GetString(received, 0, i);
                            request += data;

                            if (data == "\n" && isLineBlank)
                            {
                                AddToLogBox(request + "End" + "\r\n");

                                if (request.Substring(0, 4) == "POST")
                                {
                                    requestType = "POST";
                                }
                                int slashIndex = request.IndexOf('/');
                                int spaceIndex = request.IndexOf(' ', slashIndex);
                                uri = request.Substring(slashIndex, spaceIndex - slashIndex);
                                int contentLengthIndex;

                                if ((contentLengthIndex = request.IndexOf("Content-Length:") + 15) != 15)
                                {
                                    int lineEndIndex = request.IndexOf('\r', contentLengthIndex);
                                    contentLength = int.Parse(request.Substring(contentLengthIndex + 1, lineEndIndex - contentLengthIndex - 1));
                                }
                                else contentLength = 0;

                                AddToLogBox(requestType + "\r\n");
                                AddToLogBox(uri + "\r\n");
                                AddToLogBox(contentLength.ToString() + "\r\n");

                                parsingHeader = false;
                            }
                            if (data == "\n")
                            {
                                isLineBlank = true;
                            }
                            else if (data != "\r")
                            {
                                isLineBlank = false;
                            }
                        }
                        else if (!parsingHeader)
                        {
                            bytes[dataBytesReceived++] = received[0];

                        }

                        if (dataBytesReceived == contentLength && !parsingHeader) break;

                    }
                    if (dataBytesReceived == contentLength && !parsingHeader) break;
                }
                AddToLogBox(dataBytesReceived.ToString() + " Bytes Recevied\r\n");
                switch (requestType)
                {
                    case "POST":
                        switch (uri)
                        {
                            case "/pair-setup":

                                int stateIndex = findTLVIndex(bytes, dataBytesReceived, 0x6);
                                int state = bytes[stateIndex + 2];
                                AddToLogBox("State Index: " + stateIndex.ToString() + " State: " + state + "\r\n");
                                switch (state)
                                {
                                    case 1:

                                        {
                                            if (paired)
                                            {
                                                Byte[] responseBytes = new Byte[] { 0x06, 0x01, 0x02, 0x07, 0x01, 0x06 };
                                                sendTLVResponse(stream, responseBytes);
                                                
                                                
                                            }
                                            else
                                            {
                                                String userName = "Pair-Setup";

                                                Byte[] salt = byteGenRandom(16) ;
                                               
                                                storedx = generatePrivateKey(toUInt32ArrayLE(byteArrayReverse(salt)), userName, DeviceCode);
                                                storedv = generateVerifier(storedx);
                                                storedk = generateMultiplier();
                                                //storedA = generatePublicA(a);
                                                storedB = generatePublicB(storedk, storedv);
                                                //storedu = generateScrambling(storedA, storedB);
                                                //storedS = generateSessionSecret(storedA, storedv, storedu, b);
                                                //storedK = generateSessionKey(storedS);

                                                Byte[] TLVResponse = new byte[0];
                                                appendTLVBytes(ref TLVResponse, 0x06, new byte[] { 0x02 });
                                                appendTLVBytes(ref TLVResponse, 0x02, salt);
                                                appendTLVBytes(ref TLVResponse, 0x03, byteArrayReverse(fromUInt32Array(storedB)));

                                                sendTLVResponse(stream, TLVResponse);

                                            }

                                            break;
                                        }

                                }
                                break;

                        }

                        break;
                }
                AddToLogBox("Conection Dropped");
            }
        }

        void sendTLVResponse(NetworkStream stream, Byte[] responseBytes)
        {
            sendHTTPResponse(stream, "200", "application/pairing+tlv8", responseBytes, responseBytes.Length);
            AddToLogBox("Send Response: " + responseBytes.Length + "\r\n");
        }

        void appendTLVBytes(ref Byte[] bytesIn, Byte TLVkey, Byte[] TLVValue)
        {
            int curPos = 0;
            int n = TLVValue.Length;
            
            do
            {
                bytesIn = bytesIn.Append(TLVkey).ToArray();
                int curLength = n - curPos;
                if (curLength > 255) curLength = 255;
                bytesIn = bytesIn.Append((byte)curLength).ToArray();
                int curEnd = curPos + curLength;
                if (curLength != 0)
                {
                    Byte[] TLVData = new byte[curLength];
                    for (int i = 0; curPos + i < curEnd; i++)
                    {
                        TLVData[i] = TLVValue[curPos + i];
                    }
                    bytesIn = bytesIn.Concat(TLVData).ToArray();
                    curPos = curEnd;
                }

            } while (curPos < n) ;

            return;


        }
        
        Byte[] byteGenRandom(int length)
        {
            Byte[] x = new byte[length];
            for (int curByte = 0; curByte < 16; curByte++) { x[curByte] = (byte)random.Next(); }
            return x;
        }

        private void AddToLogBox(string TextIn)
        {
            label1.BeginInvoke((MethodInvoker)delegate ()
            {
                textBox1.AppendText(TextIn);
            });
        }
        private void updateStatusLbl(string TextIn)
        {
            Status1.BeginInvoke((MethodInvoker)delegate ()
            {
                Status1.Text = TextIn;
            });
        }
        private void sendHTTPResponse(NetworkStream senderStream, string status, string contentType = null, Byte[] data = null, int dataLength = 0)
        {

            String msg = "HTTP/1.1 " + status + " ";
            switch (status)
            {
                case "200":
                    msg += "OK";
                    break;
                case "204":
                    msg += "No Content";
                    break;
            }
            msg += "\r\n";
            if (!(contentType is null))
            {
                msg += "Content-Type: " + contentType + "\r\n";
            }
            if (dataLength != 0)
            {
                msg += "Content-Length: " + dataLength.ToString() + "\r\n";
            }
            msg += "\r\n";
            Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(msg);


            if (!(data is null))
            {
                if (dataLength != 0)
                {
                    bytes = bytes.Concat(data.Take(dataLength)).ToArray();
                }
                else
                {
                    bytes = bytes.Concat(data).ToArray();
                }
            }

            senderStream.Write(bytes, 0, bytes.Length);

            return;


        }
        private int findTLVIndex(Byte[] data, int dataLength, Byte typeIn)
        {
            int i = 0;
            while (data[i] != typeIn && i < dataLength)
            {
                i += data[i + 1] + 2;
            }

            return i;

        }
        private UInt64c[] initHexArray(String[] stringsIn)
        {
            UInt64c[] returnArray = new UInt64c[stringsIn.Length];

            int i = 0;
            foreach (string curString in stringsIn)
            {
                returnArray[i++] = UInt64cfromString(curString);
            }
            return returnArray;
        }
        private UInt64c UInt64cfromString(String stringIn)
        {
            UInt64c returnVal;
            returnVal.hi = UInt32.Parse(stringIn.Substring(0, 8), System.Globalization.NumberStyles.HexNumber);
            returnVal.lo = UInt32.Parse(stringIn.Substring(8, 8), System.Globalization.NumberStyles.HexNumber);
            return returnVal;
        }

        Byte[] genSHA512(Byte[] msg)
        {

            Byte[] returnVal = new Byte[64];
            UInt64c[] H = new UInt64c[HInitial.Length];
            HInitial.CopyTo(H, 0);

            msg = msg.Concat(new Byte[] { 0x80 }).ToArray(); //+= '\u0080'; //append 1 bit, padded with 0s

            int msgLength = (int)((float)msg.Length / 8) + 2;
            int numBlocks = (int)Math.Ceiling(((float)msgLength / 16));
            UInt64c[,] blocks = new UInt64c[numBlocks, 16];

            for (int i = 0; i < numBlocks; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    int basePosition = i * 128 + j * 8;
                    for (int k = 0; k < 8; k++)
                    {
                        int curPosition = basePosition + k;
                        if (curPosition >= msg.Length) break;
                        byte curByte = (byte)msg[curPosition];
                        if (k < 4)
                        {
                            blocks[i, j].hi = blocks[i, j].hi | (uint)curByte << ((3 - k) * 8);
                        }
                        else
                        {
                            blocks[i, j].lo = blocks[i, j].lo | (uint)curByte << ((7 - k) * 8);
                        }

                    }
                    //blocks[i, j].lo = (uint)msg[basePosition + 0] << 24 | (uint)msg[basePosition + 1] << 16 | (uint)msg[basePosition + 2] << 8 | (uint)msg[basePosition + 3] << 0;
                    //blocks[i, j].hi = (uint)msg[basePosition + 4] << 24 | (uint)msg[basePosition + 5] << 16 | (uint)msg[basePosition + 6] << 8 | (uint)msg[basePosition + 7] << 0;
                }
            }
            //          blocks[numBlocks - 1, 15].hi = (uint)((msg.Length - 1) * 8) / (uint)Math.Pow(2, 32); //ignore lengths above 2^32 bits, would overflow Arduino
            blocks[numBlocks - 1, 15].lo = (uint)((msg.Length - 1) * 8);

            for (int i = 0; i < numBlocks; i++)
            {
                UInt64c[] W = new UInt64c[80];

                for (int t = 0; t < 16; t++) W[t] = blocks[i, t];
                for (int t = 16; t < 80; t++) W[t] = UInt64cAdd(shaSig1(W[t - 2]), W[t - 7], shaSig0(W[t - 15]), (W[t - 16]));

                UInt64c a = H[0], b = H[1], c = H[2], d = H[3], e = H[4], f = H[5], g = H[6], h = H[7];
                for (int t = 0; t < 80; t++)
                {
                    UInt64c T1 = UInt64cAdd(h, shaSum1(e), shaCH(e, f, g), K[t], W[t]);
                    UInt64c T2 = UInt64cAdd(shaSum0(a), shaMaj(a, b, c));
                    h = g;
                    g = f;
                    f = e;
                    e = UInt64cAdd(d, T1);
                    d = c;
                    c = b;
                    b = a;
                    a = UInt64cAdd(T1, T2);
                }
                H[0] = UInt64cAdd(H[0], a);
                H[1] = UInt64cAdd(H[1], b);
                H[2] = UInt64cAdd(H[2], c);
                H[3] = UInt64cAdd(H[3], d);
                H[4] = UInt64cAdd(H[4], e);
                H[5] = UInt64cAdd(H[5], f);
                H[6] = UInt64cAdd(H[6], g);
                H[7] = UInt64cAdd(H[7], h);
            }

            //foreach (UInt64c i in H)
            //{
            //    AddToLogBox(i.hi.ToString("X8").ToLower());
            //    AddToLogBox(i.lo.ToString("X8").ToLower() + "\r\n");

            //}

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    returnVal[i * 8 + j] = UInt64CGetByte(H[i], j);
                }
            }

            return returnVal;

        }

        private void button1_Click(object sender, EventArgs e)
        {


            CryptoTest.RunWorkerAsync();




        }
        UInt64c ROTR(UInt64c x, int n)
        {
            UInt64c returnVal;

            if (n == 0) return x;
            if (n == 32) return new UInt64c { hi = x.lo, lo = x.hi };

            UInt32 hi = x.hi;
            UInt32 lo = x.lo;

            if (n > 32)
            {
                UInt32 temp = hi;
                hi = lo;
                lo = temp;
                n -= 32;
            }

            returnVal.hi = (hi >> n) | (lo << (32 - n));
            returnVal.lo = (lo >> n) | (hi << (32 - n));

            return returnVal;
        }

        UInt64c UInt64cAdd(UInt64c x, UInt64c y)
        {
            UInt64c returnVal;

            returnVal.lo = x.lo + y.lo;
            returnVal.hi = x.hi + y.hi;
            if (0xffffffff - x.lo < y.lo) //check if lo overflowed
            {
                returnVal.hi += 1;
            }

            return returnVal;
        }

        UInt64c UInt64cAdd(UInt64c x, UInt64c y, UInt64c z)
        {
            return UInt64cAdd(UInt64cAdd(x, y), z);
        }

        UInt64c UInt64cAdd(UInt64c x, UInt64c y, UInt64c z, UInt64c w)
        {
            return UInt64cAdd(UInt64cAdd(x, y, z), w);
        }
        UInt64c UInt64cAdd(UInt64c x, UInt64c y, UInt64c z, UInt64c w, UInt64c v)
        {
            return UInt64cAdd(UInt64cAdd(x, y, z, w), v);
        }
        UInt64c UInt64cAnd(UInt64c x, UInt64c y)
        {
            UInt64c returnVal;

            returnVal.lo = x.lo & y.lo;
            returnVal.hi = x.hi & y.hi;

            return returnVal;
        }
        UInt64c UInt64cXor(UInt64c x, UInt64c y)
        {
            UInt64c returnVal;

            returnVal.lo = x.lo ^ y.lo;
            returnVal.hi = x.hi ^ y.hi;

            return returnVal;
        }

        UInt64c UInt64cXor(UInt64c x, UInt64c y, UInt64c z)
        {
            UInt64c returnVal;

            returnVal.lo = x.lo ^ y.lo ^ z.lo;
            returnVal.hi = x.hi ^ y.hi ^ z.hi;

            return returnVal;
        }

        UInt64c UInt64cNot(UInt64c x)
        {
            UInt64c returnVal;

            returnVal.lo = ~x.lo;
            returnVal.hi = ~x.hi;

            return returnVal;
        }

        UInt64c UInt64cShr(UInt64c x, int n)
        {
            UInt64c returnVal = new UInt64c();

            if (n == 0) returnVal = x;
            else if (n == 32) returnVal.lo = x.hi;
            else if (n > 32) returnVal.lo = x.hi >> (n - 32);
            else
            {
                returnVal.hi = x.hi >> n;
                returnVal.lo = x.lo >> n | x.hi << (32 - n);
            }

            return returnVal;
        }

        UInt64c shaSum0(UInt64c x) { return UInt64cXor(ROTR(x, 28), ROTR(x, 34), ROTR(x, 39)); }
        UInt64c shaSum1(UInt64c x) { return UInt64cXor(ROTR(x, 14), ROTR(x, 18), ROTR(x, 41)); }
        UInt64c shaSig0(UInt64c x) { return UInt64cXor(ROTR(x, 1), ROTR(x, 8), UInt64cShr(x, 7)); }
        UInt64c shaSig1(UInt64c x) { return UInt64cXor(ROTR(x, 19), ROTR(x, 61), UInt64cShr(x, 6)); }
        UInt64c shaCH(UInt64c x, UInt64c y, UInt64c z) { return UInt64cXor(UInt64cAnd(x, y), UInt64cAnd(UInt64cNot(x), z)); }
        UInt64c shaMaj(UInt64c x, UInt64c y, UInt64c z) { return UInt64cXor(UInt64cAnd(x, y), UInt64cAnd(x, z), UInt64cAnd(y, z)); }

        Byte UInt64CGetByte(UInt64c x, int curByte)
        {
            Byte returnVal = 0;

            if (curByte < 4)
            {
                returnVal = (byte)(x.hi >> (3 - curByte) * 8);
            }
            else
            {
                returnVal = (byte)(x.lo >> (7 - curByte) * 8);
            }

            return returnVal;
        }

        Byte[] genVerifier(Byte[] salt, String pass)
        {
            // Byte[] x = genSHA512(Encoding.UTF8.GetString(salt) + pass);

            return null;
        }

        Byte[] modular_pow(uint b, Byte[] e, Byte[] m)
        {
            return null;

        }

        int32Array toUInt32Array(Byte[] byteArray)
        {
            int outputSize = (int)Math.Ceiling((decimal)byteArray.Length / 4);
            UInt32[] digits = new uint[outputSize];

            for (int i = 0; i < outputSize; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    digits[i] |= ((UInt32)byteArray[byteArray.Length - (i * 4 + j) - 1]) << (8 * j);

                }
            }

            int32Array returnVal;
            returnVal.digits = digits;
            returnVal.negative = false;

            return returnVal;
        }

        UInt32[] toUInt32ArrayLE(Byte[] byteArray)
        {
            int outputSize = (int)Math.Ceiling((decimal)byteArray.Length / 4);
            UInt32[] digits = new uint[outputSize];

            for (int i = 0; i < outputSize; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    digits[i] |= ((UInt32)byteArray[(i * 4 + j)]) << (8 * j);

                }
            }

            UInt32[] returnVal;
            returnVal = digits;


            return returnVal;
        }

        Byte[] fromUInt32Array(UInt32[] uint32Array)
        {
            Byte[] returnVal = new byte[uint32Array.Length * 4];

            for (int i = 0; i < uint32Array.Length; i++)
            {
                returnVal[i * 4] = (byte)uint32Array[i];
                returnVal[i * 4 + 1] = (byte)(uint32Array[i] >> 8);
                returnVal[i * 4 + 2] = (byte)(uint32Array[i] >> 16);
                returnVal[i * 4 + 3] = (byte)(uint32Array[i] >> 24);

            }
            return returnVal;
        }
        Byte[] fromUInt32ArrayBE(UInt32[] uint32Array)
        {
            Byte[] returnVal = new byte[uint32Array.Length * 4];

            for (int i = 0; i < uint32Array.Length; i++)
            {
                returnVal[i * 4 + 3] = (byte)uint32Array[i];
                returnVal[i * 4 + 2] = (byte)(uint32Array[i] >> 8);
                returnVal[i * 4 + 1] = (byte)(uint32Array[i] >> 16);
                returnVal[i * 4 + 0] = (byte)(uint32Array[i] >> 24);

            }
            return returnVal;
        }
        int32Array int32ArrayAddNoSign(int32Array a, int32Array b, int32Array mod)
        {
            int32Array returnVal;
            returnVal.digits = UInt32AddNoSign(a.digits, b.digits);
            returnVal.negative = false;
            return returnVal;

        }
        UInt32[] UInt32AddNoSign(UInt32[] a, UInt32[] b)
        {
            UInt32[] c;
            byte carry = 0;

            int n = a.Length;
            if (n < b.Length) n = b.Length;
            c = new UInt32[n + 1];

            for (int i = 0; i < n; ++i)
            {
                UInt32 aDigit = 0;
                UInt32 bDigit = 0;
                byte newCarry = 0;
                if (i < a.Length) aDigit = a[i];
                if (i < b.Length) bDigit = b[i];
                c[i] = aDigit + bDigit + carry;
                if (0xFFFFFFFF - aDigit < bDigit) newCarry = 1;
                if (carry == 1)
                {
                    if (aDigit + bDigit == 0xFFFFFFFF) newCarry = 1;
                }
                carry = newCarry;


            }
            c[n] = carry;

            return c;


        }

        int int32ArrayCmp(int32Array a, int32Array b)  //return 1 if a>b, -1 if b<a, 0 if a=b
        {

            int signDif = 0;

            if (a.negative & !b.negative) signDif = -1;
            if (!a.negative & b.negative) signDif = 1;

            int n = a.digits.Length;
            if (n < b.digits.Length) n = b.digits.Length;

            for (int i = n - 1; i >= 0; --i)
            {
                UInt32 aDigit = 0;
                UInt32 bDigit = 0;
                // byte newCarry = 0;
                if (i < a.digits.Length) aDigit = a.digits[i];
                if (i < b.digits.Length) bDigit = b.digits[i];
                if ((aDigit != 0 | bDigit != 0) & signDif != 0) return signDif;
                if (aDigit > bDigit) return (!a.negative ? 1 : -1);
                if (aDigit < bDigit) return (!a.negative ? -1 : 1);
            }
            return 0;

        }

        int int32ArrayCmpNoSign(int32Array a, int32Array b)  //return 1 if a>b, -1 if b<a, 0 if a=b
        {

            //  int signDif = 0;

            //  if (a.negative & !b.negative) signDif = -1;
            //  if (!a.negative & b.negative) signDif = 1;

            int n = a.digits.Length;
            if (n < b.digits.Length) n = b.digits.Length;

            for (int i = n - 1; i >= 0; --i)
            {
                UInt32 aDigit = 0;
                UInt32 bDigit = 0;
                // byte newCarry = 0;
                if (i < a.digits.Length) aDigit = a.digits[i];
                if (i < b.digits.Length) bDigit = b.digits[i];
                //     if ((aDigit != 0 | bDigit != 0) & signDif != 0) return signDif;
                if (aDigit > bDigit) return 1;
                if (aDigit < bDigit) return -1;
            }
            return 0;

        }

        int UInt32ArrayCmpNoSign(UInt32[] a, UInt32[] b)
        {
            int32Array A;
            A.digits = a;
            A.negative = false;

            int32Array B;
            B.digits = b;
            B.negative = false;

            return int32ArrayCmpNoSign(A, B);

        }

        int32Array int32ArrayModSlow(int32Array a, int32Array mod)
        {
            int32Array c;
            c.digits = new uint[mod.digits.Length];
            c.negative = false;

            int cmpResult = int32ArrayCmp(a, mod);
            if (cmpResult == 0) return c;
            if (cmpResult == -1) return a;

            int n = mod.digits.Length;



            int32Array curDigits;
            curDigits.digits = new UInt32[n + 1];
            curDigits.negative = false;
            int32Array curRemainder;
            curRemainder.digits = new UInt32[n + 1];
            curRemainder.negative = false;


            for (int i = a.digits.Length - 1; i >= 0; --i)
            {

                for (int j = n; j > 0; --j)
                {
                    curDigits.digits[j] = curRemainder.digits[j - 1];
                }
                curDigits.digits[0] = a.digits[i];
                while (int32ArrayCmp(curDigits, mod) == 1)
                {
                    curDigits.digits = UInt32ArraySubSimple(curDigits.digits, mod.digits);
                    // updateStatusLbl(curDigits.digits[0].ToString("X8"));
                }
                curRemainder = curDigits;
            }

            c.digits = curRemainder.digits;

            return c;

        }

        UInt32[] UInt32ArrayMod(UInt32[] aIn, UInt32[] modIn)
        {
            int n = aIn.Length;
            int32Array c;
            c.digits = new uint[modIn.Length];
            c.negative = false;

            int32Array a;
            a.digits = aIn;
            a.negative = false;

            int32Array mod;
            mod.digits = modIn;
            mod.negative = false;



            int cmpResult = int32ArrayCmp(a, mod);
            if (cmpResult == 0) return c.digits;
            if (cmpResult == -1)
            {
                for (int i = 0; i < c.digits.Length && i < a.digits.Length; i++)
                {
                    c.digits[i] = a.digits[i];
                }
                return c.digits;
            }

            int32Array curDenominator;
            curDenominator.digits = new UInt32[n * 2];
            curDenominator.negative = false;
            for (int i = 0; i < n; i++)
            {
                if (i < mod.digits.Length)
                {
                    curDenominator.digits[i + n] = mod.digits[i];
                }

            }
            int32Array curRemainder;
            curRemainder.digits = new UInt32[n * 2];
            curRemainder.negative = false;
            for (int i = 0; i < n; i++)
            {
                curRemainder.digits[i] = a.digits[i];
            }

            for (int i = (n * 32) - 1; i >= 0; i--)
            {
                curRemainder = int32ArraySHL(curRemainder);
                if (int32ArrayCmp(curRemainder, curDenominator) >= 0)
                {
                    curRemainder.digits = UInt32ArraySubSimple(curRemainder.digits, curDenominator.digits);
                }
            }

            for (int i = 0; i < mod.digits.Length; i++)
            {
                c.digits[i] = curRemainder.digits[i + n];
            }


            return c.digits;

        }


        void UInt32ArraySHLWords(ref UInt32[] a, int n)
        {
            int len = a.Length;
            if (n > len) n = len;

            for (int i = len - 1; i > (n - 1); --i)
            {
                a[i] = a[i - n];
            }
            for (int i = 0; i < n; i++) a[i] = 0;

            return;
        }

        void UInt32ArraySHRWords(ref UInt32[] a, int n)
        {
            int len = a.Length;
            if (n > len) n = len;

            for (int i = 0; i < (len - n); i++)
            {
                a[i] = a[i + n];
            }
            for (int i = len - n; i < len; i++) a[i] = 0;

            return;
        }
        UInt32[] UInt32ArraySubSimple(UInt32[] a, UInt32[] b) //assumes both positive and a > b
        {
            int n = a.Length;

            UInt32[] c;
            c = new UInt32[n];
            // c.negative = false;

            Byte borrow = 0;

            for (int i = 0; i < n; ++i)
            {
                UInt32 aDigit = 0;
                UInt32 bDigit = 0;
                byte newBorrow = 0;
                if (i < a.Length) aDigit = a[i];
                if (i < b.Length) bDigit = b[i];
                c[i] = aDigit - bDigit - borrow;
                if (aDigit < bDigit) newBorrow = 1;
                if (borrow == 1)
                {
                    if (aDigit - bDigit == 0) newBorrow = 1;
                }
                borrow = newBorrow;


            }
            return c;

        }

        int32Array int32ArraySHL(int32Array a)
        {
            int n = a.digits.Length;
            for (int i = n - 1; i > 0; i--)
            {
                a.digits[i] = (a.digits[i] << 1) | (a.digits[i - 1] >> 31);
            }
            a.digits[0] = a.digits[0] << 1;
            return a;

        }

        UInt32[] UInt32ArraySHL(UInt32[] a)
        {
            int32Array A;
            A.digits = a;
            A.negative = false;

            return int32ArraySHL(A).digits;
        }

        int32Array int32ArraySHR(int32Array a)
        {
            //int n = a.digits.Length;
            //for (int i = 0; i < n; i++)
            //{
            //    a.digits[i] = a.digits[i] >> 1;
            //    if (i != n - 1)
            //    {
            //        a.digits[i] |= a.digits[i + 1] << 31;
            //    }
            //}

            a.digits = UInt32ArraySHR(a.digits);

            return a;

        }

        UInt32[] UInt32ArraySHR(UInt32[] a)
        {
            int n = a.Length;
            for (int i = 0; i < n; i++)
            {
                a[i] = a[i] >> 1;
                if (i != n - 1)
                {
                    a[i] |= a[i + 1] << 31;
                }
            }

            return a;

        }

        UInt32[] UInt32ArrayMulNoSign(UInt32[] a, UInt32[] b)
        {

            int n = a.Length + b.Length;
            UInt32[] c = new uint[n];
            UInt64c[] p64 = new UInt64c[4];
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < b.Length; j++)
                {
                    //UInt64c pA64, pB64, pC64, pD64;
                    

                    p64[0].lo = (a[i] & 0xFFFF) * (b[j] & 0xFFFF);
                    p64[0].hi = 0;
                    UInt32 pB = (a[i] & 0xFFFF) * (b[j] >> 16);
                    p64[1].lo = pB << 16;
                    p64[1].hi = pB >> 16;
                    UInt32 pC = (a[i] >> 16) * (b[j] & 0xFFFF);
                    p64[2].lo = pC << 16;
                    p64[2].hi = pC >> 16;
                    p64[3].lo = 0;
                    p64[3].hi = (a[i] >> 16) * (b[j] >> 16);

                    //pA64 = UInt64cAdd(pA64, pB64, pC64, pD64);


                    for (int k = 1; k < 4; k++)
                    {
                        if (0xFFFFFFFF - p64[0].lo < p64[k].lo) p64[0].hi++;
                        p64[0].lo += p64[k].lo;
                        
                    }

                    for (int k = 1; k<4; k++)
                    {
                        p64[0].hi += p64[k].hi;
                    }


                    byte carry = 0;
                    
                    if (0xFFFFFFFF - p64[0].lo < c[i + j]) carry = 1;
                    c[i + j] += p64[0].lo;

                    c[i + j + 1] += carry;
                    if (c[i + j + 1] == 0 && carry == 1)
                    {
                        carry = 1;
                    }
                    else carry = 0;

                    if (0xFFFFFFFF - p64[0].hi < c[i + j + 1]) carry = 1;

                    c[i + j + 1] += p64[0].hi;

                    for (int k = 2; carry == 1; k++)
                    {
                        carry = 0;
                        if (++c[i + j + k] == 0) carry = 1;
                    }

                }
            }

            return c;
        }

        UInt32[] UInt32ArrayMulMod(UInt32[] a, UInt32[] b, UInt32[] mod)
        {
            UInt32[] c = UInt32ArrayMulNoSign(a, b);
            return UInt32ArrayMod(c, mod);
        }

        UInt32[] UInt32PowMod(UInt32[] a, in UInt32[] b, UInt32[] mod)
        {

            int n = b.Length * 32;
            UInt32[] curPower;

            UInt32[] powShift = new UInt32[b.Length];
            b.CopyTo(powShift, 0);

            UInt32[] returnVal = new UInt32[mod.Length];
            returnVal[0] = 1;

            UInt32[][] powers = new UInt32[n][];
            powers[0] = a;



            curPower = a;
            for (int i = 0; i < n; i++)
            {

                if ((powShift[0] & 0x1) == 1)
                {
                    returnVal = UInt32ArrayMulMod(curPower, returnVal, mod);
                }
                curPower = UInt32ArrayMulMod(curPower, curPower, mod);
                //for (int j=curPower.Length -1; j>=0; j--)
                //{
                //    if (curPower[j] > 0)
                //    {
                //        AddToLogBox((j+1).ToString() + "\r\n");
                //        break;
                //    }
                //}
                UInt32ArraySHR(powShift);
                updateStatusLbl(i.ToString());
            }

            return returnVal;


        }

        

        UInt32[] UInt32ArrayReverse(UInt32[] a)
        {
            int n = a.Length;
            UInt32[] c = new UInt32[n];

            for (int i = 0; i < n; i++)
            {
                c[i] = a[n - i - 1];
            }
            return c;
        }

        private void CryptoTest_DoWork(object sender, DoWorkEventArgs e)
        {


            storedx = generatePrivateKey(s, "alice", "password123");
            storedv = generateVerifier(storedx);
            storedk = generateMultiplier();
            storedA = generatePublicA(a);
            storedB = generatePublicB(storedk, storedv);
            storedu = generateScrambling(storedA, storedB);
            storedS = generateSessionSecret(storedA, storedv, storedu, b);
            storedK = generateSessionKey(storedS);

            foreach (UInt32 i in storedK)
            {
                AddToLogBox(i.ToString("X8") + "\r\n");
            }



            //Byte[] zeroByte = { 0x0 };


            //BigInteger bigN = new BigInteger(fromUInt32Array(N).Concat(new Byte[] { 0 }).ToArray());
            //BigInteger bigx = new BigInteger(hashBytes.Concat(zeroByte).ToArray());
            //BigInteger bigG = new BigInteger(5);
            //BigInteger bigv = BigInteger.ModPow(bigG, bigx, bigN);

            //AddToLogBox(bigv.ToString("X8") + "\r\n");
        }

        Byte[] byteArrayReverse(Byte[] a)
        {
            Byte[] returnVal = new byte[a.Length];

            for (int i = 0; i < a.Length; i++)
            {
                returnVal[i] = a[a.Length - i - 1];
            }
            return returnVal;
        }


        UInt32[] generateVerifier(UInt32[] salt, String authString)
        {
            UInt32[] x = generatePrivateKey(salt, authString);



            return generateVerifier(x);
        }

        UInt32[] generateVerifier(UInt32[] x)
        {
            //UInt32[] g = { generator };
            return UInt32PowModMonty(g, x, N);

        }


        UInt32[] generatePrivateKey(UInt32[] salt, String authString)
        {
            Byte[] saltBytes = fromUInt32ArrayBE(salt);
            //salt = byteArrayReverse(salt);
            Byte[] authBytes = Encoding.ASCII.GetBytes(authString);
            Byte[] pwHash = genSHA512(authBytes);
            Byte[] toHash = saltBytes.Concat(pwHash).ToArray();

            Byte[] hashBytes = genSHA512(toHash);

            hashBytes = byteArrayReverse(hashBytes);

            return toUInt32ArrayLE(hashBytes);
        }

        UInt32[] generatePrivateKey(UInt32[] salt, String userName, String password)
        {
            String authString = userName + ":" + password;
            return generatePrivateKey(salt, authString);
        }

        UInt32[] generateMultiplier()
        {
            UInt32[] gPad = new uint[N.Length];
            gPad[gPad.Length - 1] = generator;
            Byte[] gBytes = fromUInt32ArrayBE(gPad);

            Byte[] NBytes = fromUInt32Array(N);
            NBytes = byteArrayReverse(NBytes);
            Byte[] toHash = NBytes.Concat(gBytes).ToArray();
            byte[] hash = genSHA512(toHash);
            Byte[] hashRev = byteArrayReverse(hash);
            UInt32[] returnVal = toUInt32ArrayLE(hashRev);

            return returnVal;
        }

        UInt32[] generatePublicB(UInt32[] k, UInt32[] v)
        {

            UInt32[] B = UInt32AddNoSign(UInt32ArrayMulMod(k, v, N), UInt32PowModMonty(g, b, N));
            B = UInt32ArrayMod(B, N);

            return B;
        }

        UInt32[] generateScrambling(UInt32[] A, UInt32[] B)
        {
            Byte[] bytesA = byteArrayReverse(fromUInt32Array(A));
            Byte[] bytesB = byteArrayReverse(fromUInt32Array(B));

            Byte[] toHash = bytesA.Concat(bytesB).ToArray();
            Byte[] hash = genSHA512(toHash);

            Byte[] hashRev = byteArrayReverse(hash);

            UInt32[] u = toUInt32ArrayLE(hashRev);
            return u;
        }

        UInt32[] generatePublicA(UInt32[] a)
        {
            UInt32[] A = UInt32PowModMonty(g, a, N);

            return A;
        }

        UInt32[] generateSessionSecret(UInt32[] A, UInt32[] v, UInt32[] u, UInt32[] b)
        {
            UInt32[] S;

            S = UInt32PowModMonty(v, u, N);
            S = UInt32ArrayMulMod(A, S, N);

            S = UInt32PowModMonty(S, b, N);

            return S;

        }

        UInt32[] generateSessionKey(UInt32[] S)
        {
            Byte[] toHash = byteArrayReverse(fromUInt32Array(storedS));

            Byte[] hash = byteArrayReverse(genSHA512(toHash));

            return toUInt32ArrayLE(hash);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Byte[] toHash = Encoding.UTF8.GetBytes(textBox2.Text);
            Byte[] hash = genSHA512(toHash);

            foreach (Byte i in hash)
            {
                AddToLogBox(i.ToString("X2") + " ");
            }
            AddToLogBox("\r\n");

        }

        void UInt32ArrayDiv(UInt32[] A, UInt32[] B, out UInt32[] q, out UInt32[] r)
        {
            int n = A.Length;
            UInt32[] c;
            c = new uint[B.Length];


            UInt32[] a = new UInt32[A.Length];
            A.CopyTo(a, 0);


            int cmpResult = UInt32ArrayCmpNoSign(a, B);
            if (cmpResult == 0)
            {
                q = new UInt32[] { 1 };
                r = new uint[] { 0 };
            }
            if (cmpResult == -1)
            {
                q = new UInt32[] { 0 };
                r = new UInt32[B.Length];

                for (int i = 0; i < r.Length && i < a.Length; i++)
                {
                    r[i] = a[i];
                }
                return;
            }

            UInt32[] curDenominator = new UInt32[n * 2];

            for (int i = 0; i < n; i++)
            {
                if (i < B.Length)
                {
                    curDenominator[i + n] = B[i];
                }

            }
            UInt32[] curRemainder = new UInt32[n * 2];
            for (int i = 0; i < n; i++)
            {
                curRemainder[i] = a[i];
            }
            q = new UInt32[A.Length + B.Length];
            for (int i = (n * 32) - 1; i >= 0; i--)
            {
                curRemainder = UInt32ArraySHL(curRemainder);
                if (UInt32ArrayCmpNoSign(curRemainder, curDenominator) >= 0)
                {
                    curRemainder = UInt32ArraySubSimple(curRemainder, curDenominator);
                    q[i / 32] |= (UInt32)0x1 << (i - i / 32); 
                }
            }

            
            r = new UInt32[B.Length];

            for (int i = 0; i < B.Length; i++)
            {
                r[i] = curRemainder[i + n];
            }

            return;
        }

        UInt32[] UInt32ArrayGetInverse(UInt32[] u, UInt32[] v)
        {
            UInt32[] u1, v1, t1, u3, v3, q, r, inv;

            u3 = new uint[u.Length];
            u.CopyTo(u3, 0);

            v3 = new uint[v.Length];
            v.CopyTo(v3, 0);

            u1 = new uint[] { 1 };
            v1 = new uint[] { 0 };

            bool iter = true;

            while (!UInt32ArrayIsZero(v3))
            {
                UInt32ArrayDiv(u3, v3, out q, out r);
                t1 = UInt32AddNoSign(u1, UInt32ArrayMulNoSign(q, v1));
                UInt32ArrayShrink(ref t1);
                u1 = v1;
                v1 = t1;
                u3 = v3;
                v3 = r;

                iter = !iter;
            }

            if (!iter) inv = UInt32ArraySubSimple(v1, u1);
            else inv = u1;

            return inv;


        }

        bool UInt32ArrayIsZero (UInt32[] x) 
        {
            foreach (UInt32 i in x) if (i != 0) return false;
            return true;
        }
        
        void UInt32ArrayShrink (ref UInt32[] x)
        {
            int msw;
            UInt32[] y;

            for (msw = x.Length - 1; msw > 0; msw--)
            {
                if (x[msw] != 0) break;
            }
            if (msw < x.Length -1)
            {
                y = new UInt32[msw + 1];
                for (int i = 0; i < y.Length; i++)
                {
                    y[i] = x[i];
                }
                x = y;
            }
            return;
        }
        UInt32[] UInt32ArrayMulMonty(UInt32[] x, UInt32[] y, UInt32[] m, UInt32[] m_prime)
        {

            int n = m.Length;
            UInt32[] A = new UInt32[m.Length];

            for (int i = 0; i < n; i++)
            {
                UInt32 xi = (i < x.Length) ? x[i] : 0;
                UInt32[] u = new uint[] { (A[0] + xi * y[0]) * m_prime[0] };
                UInt32[] xiy = UInt32ArrayMulNoSign(new uint[] { xi }, y);
                UInt32[] uim = UInt32ArrayMulNoSign(u, m);
                A = UInt32AddNoSign(A, UInt32AddNoSign(xiy, uim));
                UInt32ArraySHRWords(ref A, 1);
                UInt32ArrayShrink(ref A);

            }
            if (UInt32ArrayCmpNoSign(A, m) >= 0) A = UInt32ArraySubSimple(A, m);
            UInt32ArrayShrink(ref A);
            return A;
        }
        UInt32[] UInt32PowModMonty(UInt32[] x, UInt32[] e, UInt32[] m)
        {

            updateStatusLbl("Started");
            UInt32[] R = new UInt32[m.Length + 1];
            R[m.Length] = 0x1;
            //UInt32[] R_inv = UInt32ArrayGetInverse(R, m);
            UInt32[] ba = new UInt32[] { 0x0, 0x1 };
            UInt32[] m_inv = UInt32ArrayGetInverse(m, ba);
            UInt32[] m_prime = UInt32ArraySubSimple(R, m_inv);
            UInt32ArrayShrink(ref m_prime);
            
            UInt32[] A = UInt32ArrayMod(R, m);
            UInt32[] R2 = UInt32ArrayMulMod(A, A, m);
            UInt32[] x_bar = UInt32ArrayMulMonty(x, R2, m, m_prime);
            int loopCount = 0;


            for (int i = e.Length - 1; i >= 0; i--)
            {
                UInt32 eWord = e[i];
                for (int j = 31; j >= 0; j--)
                {
                    A = UInt32ArrayMulMonty(A, A, m, m_prime);
                    if ((eWord & 0x80000000) == 0x80000000)
                    {
                        A = UInt32ArrayMulMonty(A, x_bar, m, m_prime);
                    }
                    eWord <<= 1;
                    updateStatusLbl((++loopCount).ToString());

                }
            }
            A = UInt32ArrayMulMonty(A, new uint[] { 0x1 }, m, m_prime);

            return A;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MontyTest.RunWorkerAsync();
        }

        private void MontyTest_DoWork(object sender, DoWorkEventArgs e)
        {
            UInt32[] testResult = UInt32PowModMonty(g, a, N);

            foreach (UInt32 i in testResult)
            {
                //AddToLogBox(i.ToString() + " " + i.ToString("X8") + "\r\n");
                AddToLogBox(i.ToString("X8") + "\r\n");
            }
        }
    }
}

